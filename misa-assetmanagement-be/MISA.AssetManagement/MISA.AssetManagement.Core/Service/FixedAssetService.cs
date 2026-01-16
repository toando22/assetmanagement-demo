using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Exception;
using MISA.AssetManagement.Core.Interface.Repository;
using MISA.AssetManagement.Core.Interface.Service;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.ComponentModel;

namespace MISA.AssetManagement.Core.Service
{
    /// <summary>
    /// Service Tài sản
    /// CreatedBy: DDTOAN (14/01/2026)
    /// </summary>
    public class FixedAssetService : BaseService<FixedAsset>, IFixedAssetService
    {
        private readonly IFixedAssetRepository _fixedAssetRepository;

        public FixedAssetService(IFixedAssetRepository fixedAssetRepository) : base(fixedAssetRepository)
        {
            _fixedAssetRepository = fixedAssetRepository;
        }

        public async Task<string> GetNewCodeAsync()
        {
            return await _fixedAssetRepository.GetNewCodeAsync();
        }

        public async Task<PagingResult<FixedAssetDto>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId, int trackingYear)
        {
            return await _fixedAssetRepository.GetPagingAsync(pageIndex, pageSize, keyword, departmentId, categoryId, trackingYear);
        }

        /// <summary>
        /// Override hàm Insert để xử lý nghiệp vụ riêng
        /// EditBy: DDTOAN - 15/01/2026 - Bổ sung logic tự sinh mã nếu người dùng để trống
        /// </summary>
        public override async Task<int> InsertServiceAsync(FixedAsset entity)
        {
            // 1. Kiểm tra nếu mã trống thì tự sinh
            if (string.IsNullOrEmpty(entity.FixedAssetCode))
            {
                entity.FixedAssetCode = await _fixedAssetRepository.GetNewCodeAsync();
            }

            // 2. Gọi lại logic validate và insert của cha (BaseService)
            return await base.InsertServiceAsync(entity);
        }

        /// <summary>
        /// Sửa tài sản
        /// CreatedBy: DDTOAN (15/01/2026)
        /// </summary>
        public async Task<int> UpdateAssetDtoAsync(Guid id, FixedAssetUpdateDto assetDto)
        {
            // Validate nghiệp vụ nếu cần 
            // Gọi xuống Repo
            return await _fixedAssetRepository.UpdateAssetAsync(id, assetDto);
        }

        /// <summary>
        /// Xóa nhiều tài sản
        /// </summary>
        public async Task<int> DeleteMultiAsync(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0) return 0;
            return await _fixedAssetRepository.DeleteMultiAsync(ids);
        }

        protected override async Task ValidateBusinessAsync(FixedAsset entity, bool isInsert, Guid? id = null)
        {
            var errors = new Dictionary<string, string>();

            // 1. Validate Ngày sử dụng >= Ngày mua
            if (entity.FixedAssetStartUseDate < entity.FixedAssetPurchaseDate)
            {
                errors.Add("FixedAssetStartUseDate", "Ngày bắt đầu sử dụng phải lớn hơn hoặc bằng ngày mua.");
            }

            // 2. Validate Trùng mã
            if (isInsert)
            {
                var isDup = await _fixedAssetRepository.CheckDuplicateCodeAsync(entity.FixedAssetCode);
                if (isDup) errors.Add("FixedAssetCode", $"Mã tài sản <{entity.FixedAssetCode}> đã tồn tại.");
            }

            // 3. Logic khấu hao: Tỷ lệ hao mòn = (1/Số năm sử dụng) * 100
            if (entity.FixedAssetYearsOfUse > 0)
            {
                decimal expectedRate = (1.0m / entity.FixedAssetYearsOfUse) * 100;
                decimal diff = Math.Abs(entity.FixedAssetDepreciationRate - expectedRate);

                // Cho phép sai số 0.05 do làm tròn (Tùy chỉnh nếu cần thiết)
                // if (diff > 0.05m) { ... }
            }

            if (entity.FixedAssetAnnualDepreciationValue > entity.FixedAssetOriginalCost)
            {
                errors.Add("FixedAssetAnnualDepreciationValue", "Giá trị hao mòn năm không được lớn hơn nguyên giá.");
            }

            if (errors.Count > 0)
            {
                throw new MISAValidateException("Dữ liệu không hợp lệ.", errors);
            }
        }
        /// <summary>
        /// Chuẩn bị dữ liệu nhân bản
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        public async Task<FixedAsset> PrepareCloneAsync(Guid sourceId)
        {
            // 1. Lấy thông tin tài sản gốc từ DB
            var oldAsset = await _fixedAssetRepository.GetByIdAsync(sourceId);

            // Kiểm tra nếu tài sản không tồn tại (đã bị xóa bởi người khác)
            if (oldAsset == null)
            {
                throw new MISAValidateException("Tài sản nguồn không tồn tại hoặc đã bị xóa.", null) ;
            }

            // 2. Lấy mã tài sản mới tự tăng
            var newCode = await _fixedAssetRepository.GetNewCodeAsync();

            // 3. Gán thông tin mới vào đối tượng cũ
            oldAsset.FixedAssetId = Guid.Empty; // Reset ID để Frontend hiểu là Thêm mới
            oldAsset.FixedAssetCode = newCode;  // Gán mã mới tự tăng

            // Reset các thông tin Audit (tùy chọn, vì khi Insert sẽ tự gán lại)
            oldAsset.CreatedDate = DateTime.Now;
            oldAsset.ModifiedDate = DateTime.Now;
            oldAsset.CreatedBy = null;
            oldAsset.ModifiedBy = null;

            // 4. Trả về đối tượng đã xử lý cho Frontend bind vào Form
            return oldAsset;
        }
        /// <summary>
        /// Lọc dữ liệu phục vụ xuất khẩu Excel
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        public async Task<byte[]> ExportExcelAsync(string keyword, Guid? departmentId, Guid? categoryId, int trackingYear)
        {
            // 1. Lấy dữ liệu từ Repo
            var assets = await _fixedAssetRepository.GetExportDataAsync(keyword, departmentId, categoryId, trackingYear);

            

            using (var package = new ExcelPackage())
            {
                // 3. Tạo Sheet
                var worksheet = package.Workbook.Worksheets.Add("Danh sách tài sản");

                // 4. Tạo Header (Tiêu đề cột)
                worksheet.Cells[1, 1].Value = "STT";
                worksheet.Cells[1, 2].Value = "Mã tài sản";
                worksheet.Cells[1, 3].Value = "Tên tài sản";
                worksheet.Cells[1, 4].Value = "Loại tài sản";
                worksheet.Cells[1, 5].Value = "Bộ phận sử dụng";
                worksheet.Cells[1, 6].Value = "Số lượng";
                worksheet.Cells[1, 7].Value = "Nguyên giá";
                worksheet.Cells[1, 8].Value = "HM/KH lũy kế";
                worksheet.Cells[1, 9].Value = "Giá trị còn lại";

                // Style cho Header (Đậm, nền xám, căn giữa)
                using (var range = worksheet.Cells["A1:I1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                // 5. Đổ dữ liệu vào các dòng
                int row = 2;
                int stt = 1;
                foreach (var item in assets)
                {
                    worksheet.Cells[row, 1].Value = stt;
                    worksheet.Cells[row, 2].Value = item.FixedAssetCode;
                    worksheet.Cells[row, 3].Value = item.FixedAssetName;
                    worksheet.Cells[row, 4].Value = item.FixedAssetCategoryName;
                    worksheet.Cells[row, 5].Value = item.DepartmentName;
                    worksheet.Cells[row, 6].Value = item.FixedAssetQuantity;
                    worksheet.Cells[row, 7].Value = item.FixedAssetOriginalCost;

                    // Tính toán sơ bộ nếu DB chưa tính
                    // Ví dụ: Giá trị còn lại = Nguyên giá - Hao mòn (Logic này nên để DB tính thì tốt hơn, ở đây demo)
                    // double originalCost = (double)item.FixedAssetOriginalCost;
                    // worksheet.Cells[row, 8].Value = ...; 

                    // Format số tiền (VD: 19.000.000)
                    worksheet.Cells[row, 7].Style.Numberformat.Format = "#,##0";

                    // Lấy giá trị từ DTO (SQL đã tính sẵn)
                    decimal accumulated = item.AccumulatedDepreciation;
                    decimal remaining = item.RemainingValue;

                    // LOGIC NGHIỆP VỤ AN TOÀN:
                    // 1. Lũy kế không được < 0 (Trường hợp ngày sử dụng > năm hiện tại)
                    if (accumulated < 0) accumulated = 0;

                    // 2. Lũy kế không được vượt quá Nguyên giá
                    if (accumulated > item.FixedAssetOriginalCost) accumulated = item.FixedAssetOriginalCost;

                    // 3. Tính lại giá trị còn lại cho khớp
                    remaining = item.FixedAssetOriginalCost - accumulated;

                    // Gán vào Excel
                    worksheet.Cells[row, 8].Value = accumulated;
                    worksheet.Cells[row, 9].Value = remaining;

                    // Căn phải cho cột số
                    worksheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[row, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    worksheet.Cells[row, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                    row++;
                    stt++;
                }

                // 6. Tự động giãn độ rộng cột cho đẹp
                worksheet.Cells.AutoFitColumns();

                // 7. Trả về mảng byte
                return package.GetAsByteArray();
            }
        }
    }
}