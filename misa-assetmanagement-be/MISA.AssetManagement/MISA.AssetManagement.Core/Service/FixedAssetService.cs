using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Exception;
using MISA.AssetManagement.Core.Interface.Repository;
using MISA.AssetManagement.Core.Interface.Service;

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

        public async Task<PagingResult<FixedAssetDto>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId)
        {
            return await _fixedAssetRepository.GetPagingAsync(pageIndex, pageSize, keyword, departmentId, categoryId);
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
    }
}