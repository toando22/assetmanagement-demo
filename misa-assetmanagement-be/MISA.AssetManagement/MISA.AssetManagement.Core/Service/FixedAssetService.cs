using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Exception;
using MISA.AssetManagement.Core.Interface.Repository;
using MISA.AssetManagement.Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Service
{
        /// <summary>
        /// Service Tài sản
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy:
        /// </summary>
        public class FixedAssetService : BaseService<FixedAsset>, IFixedAssetService
        {
            private readonly IFixedAssetRepository _fixedAssetRepository;

            public FixedAssetService(IFixedAssetRepository fixedAssetRepository) : base(fixedAssetRepository)
            {
                _fixedAssetRepository = fixedAssetRepository;
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
            // Lưu ý: BaseService sẽ gọi ValidateData -> ValidateBusinessAsync -> Repository.InsertAsync
            return await base.InsertServiceAsync(entity);
        }

        /// <summary>
        /// Sửa tài sản
        /// CreatedBy: DDTOAN (15/01/2026)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assetDto"></param>
        /// <returns></returns>
        public async Task<int> UpdateAssetDtoAsync(Guid id, FixedAssetUpdateDto assetDto)
        {
            // Validate nghiệp vụ nếu cần (ví dụ check trùng mã trừ chính nó...)

            // Gọi xuống Repo
            return await _fixedAssetRepository.UpdateAssetAsync(id, assetDto);
        }

        protected override async Task ValidateBusinessAsync(FixedAsset entity, bool isInsert, Guid? id = null)
            {
                var errors = new Dictionary<string, string>();

                // 1. Validate Ngày sử dụng >= Ngày mua
                if (entity.FixedAssetStartUseDate < entity.FixedAssetPurchaseDate)
                {
                    errors.Add("FixedAssetStartUseDate", "Ngày bắt đầu sử dụng phải lớn hơn hoặc bằng ngày mua.");
                }

            // 2. Validate Trùng mã (Chỉ check nếu mã do người dùng nhập hoặc mã tự sinh bị trùng hy hữu)
            if (isInsert)
            {
                var isDup = await _fixedAssetRepository.CheckDuplicateCodeAsync(entity.FixedAssetCode);
                if (isDup) errors.Add("FixedAssetCode", $"Mã tài sản <{entity.FixedAssetCode}> đã tồn tại.");
            }
            // (Lưu ý: Logic update check trùng mã cần loại trừ ID hiện tại - bổ sung sau ở Repo nếu cần)

            // =========================================================
            // Cập nhật logic tính hao mòn theo tên biến mới
            // EditBy: DDTOAN - 15/01/2026 - Cập nhật tên biến FixedAssetYearsOfUse
            // =========================================================

            // 3. Logic khấu hao: Tỷ lệ hao mòn = (1/Số năm sử dụng) * 100
            if (entity.FixedAssetYearsOfUse > 0)
            {
                decimal expectedRate = (1.0m / entity.FixedAssetYearsOfUse) * 100;
                decimal diff = Math.Abs(entity.FixedAssetDepreciationRate - expectedRate);

                // Cho phép sai số 0.05 do làm tròn
                if (diff > 0.05m)
                {
                    // errors.Add("FixedAssetDepreciationRate", "Tỷ lệ hao mòn phải bằng (1/Số năm sử dụng) x 100.");
                    // Tạm comment lại nếu Frontend gửi lên bị lệch số lẻ nhiều quá, hoặc mở ra nếu muốn chặt chẽ
                }
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
        }
    
}
