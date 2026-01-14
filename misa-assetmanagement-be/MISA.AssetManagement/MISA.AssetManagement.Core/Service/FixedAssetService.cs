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

            public async Task<PagingResult<FixedAsset>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId)
            {
                return await _fixedAssetRepository.GetPagingAsync(pageIndex, pageSize, keyword, departmentId, categoryId);
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
                    if (isDup) errors.Add("FixedAssetCode", "Mã tài sản đã tồn tại.");
                }
                // (Lưu ý: Logic update check trùng mã cần loại trừ ID hiện tại - bổ sung sau ở Repo nếu cần)

                // 3. Logic khấu hao (Ví dụ)
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
