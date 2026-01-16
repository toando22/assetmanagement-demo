using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;

namespace MISA.AssetManagement.Core.Interface.Repository
{
    /// <summary>
    /// Interface Repository Tài sản
    /// CreatedBy: DDTOAN (14/01/2026)
    /// </summary>
    public interface IFixedAssetRepository : IBaseRepository<FixedAsset>
    {
        Task<string> GetNewCodeAsync();

        /// <summary>
        /// Kiểm tra trùng mã tài sản
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        Task<bool> CheckDuplicateCodeAsync(string code);

        /// <summary>
        /// Lấy danh sách phân trang và lọc
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy: DDTOAN (15/01/2026) - Sửa kiểu trả về thành FixedAssetDto
        /// </summary>
        Task<PagingResult<FixedAssetDto>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId, int trackingYear);

        /// <summary>
        /// Sửa tài sản
        /// CreatedBy: DDTOAN (15/01/2026)
        /// </summary>
        Task<int> UpdateAssetAsync(Guid id, FixedAssetUpdateDto assetDto);

        /// <summary>
        /// Hàm xóa nhiều tài sản
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        Task<int> DeleteMultiAsync(List<Guid> ids);

        /// <summary>
        /// Lấy thông tin tài sản theo ID (Phục vụ nhân bản/sửa)
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        Task<FixedAsset> GetByIdAsync(Guid fixedAssetId);

        
        /// <summary>
        /// Lấy dữ liệu phục vụ xuất khẩu Excel
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        Task<IEnumerable<FixedAssetDto>> GetExportDataAsync(string keyword, Guid? departmentId, Guid? categoryId, int trackingYear);
    }
}