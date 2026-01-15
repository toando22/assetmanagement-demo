using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Interface.Repository
{
    /// <summary>
    /// Interface Repository Tài sản
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    public interface IFixedAssetRepository : IBaseRepository<FixedAsset>
    {
        Task<string> GetNewCodeAsync();

        /// <summary>
        /// Lấy danh sách phân trang và lọc
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy:  DDTOAN (15/01/2026) - Sửa kiểu trả về thành FixedAssetDto
        /// </summary>
        Task<PagingResult<FixedAssetDto>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId);

        /// <summary>
        /// Sửa tài sản
        /// CreatedBy: DDTOAN (15/01/2026)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assetDto"></param>
        /// <returns></returns>
        // Thêm hàm này vào Interface
        Task<int> UpdateAssetAsync(Guid id, FixedAssetUpdateDto assetDto);

       
    }
}
