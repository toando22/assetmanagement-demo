using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;

namespace MISA.AssetManagement.Core.Interface.Service
{
    /// <summary>
    /// Interface Service Tài sản
    /// CreatedBy: DDTOAN (14/01/2026)
    /// </summary>
    public interface IFixedAssetService : IBaseService<FixedAsset>
    {
        /// <summary>
        /// Lấy danh sách phân trang
        /// EditedBy: DDTOAN (15/01/2026) - Sửa kiểu trả về thành FixedAssetDto
        /// </summary>
        Task<PagingResult<FixedAssetDto>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId);

        /// <summary>
        /// Sửa tài sản 
        /// CreatedBy: DDTOAN (15/01/2026)
        /// </summary>
        Task<int> UpdateAssetDtoAsync(Guid id, FixedAssetUpdateDto assetDto);

        /// <summary>
        /// Xóa nhiều bản ghi
        /// CreatedBy: DDTOAN (15/01/2026)
        /// </summary>
        Task<int> DeleteMultiAsync(List<Guid> ids);

        /// <summary>
        /// Lấy mã tài sản mới tự động
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        Task<string> GetNewCodeAsync();

        /// <summary>
        /// Chuẩn bị dữ liệu cho tính năng Nhân bản (Lấy data cũ + Mã mới)
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        Task<FixedAsset> PrepareCloneAsync(Guid sourceId);
    }
}