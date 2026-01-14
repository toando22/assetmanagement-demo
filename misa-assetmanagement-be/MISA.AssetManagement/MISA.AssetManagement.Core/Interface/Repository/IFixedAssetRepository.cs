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
        /// </summary>
        Task<PagingResult<FixedAsset>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId);
    }
}
