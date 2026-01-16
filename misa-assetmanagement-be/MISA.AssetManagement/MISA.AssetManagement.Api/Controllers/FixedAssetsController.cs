using Microsoft.AspNetCore.Mvc;
using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Service;

namespace MISA.AssetManagement.Api.Controllers
{
    /// <summary>
    /// API Tài sản
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy: DDTOAN (16/01/2026) - Refactor Clean Architecture
    /// </summary>
    [Route("api/v1/fixed-assets")]
    [ApiController]
    public class FixedAssetsController : ControllerBase
    {
        private readonly IFixedAssetService _service;

        public FixedAssetsController(IFixedAssetService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lấy danh sách phân trang
        /// EditedBy: DDTOAN (15/01/2026) - Sửa kiểu trả về thành FixedAssetDto
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? keyword = null,
            [FromQuery] Guid? departmentId = null,
            [FromQuery] Guid? categoryId = null)
        {
            var result = await _service.GetPagingAsync(pageIndex, pageSize, keyword, departmentId, categoryId);
            return Ok(result);
        }

        [HttpGet("new-code")]
        public async Task<IActionResult> GetNewCode()
        {
            // Gọi Service thay vì gọi trực tiếp Repo
            var code = await _service.GetNewCodeAsync();
            return Ok(code);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(FixedAsset entity)
        {
            var result = await _service.InsertServiceAsync(entity);
            return StatusCode(201, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] FixedAssetUpdateDto assetDto)
        {
            var result = await _service.UpdateAssetDtoAsync(id, assetDto);

            // Xử lý kết quả trả về
            if (result > 0) return Ok(result);

            // Nếu không update được dòng nào
            return StatusCode(500, new { userMsg = "Cập nhật thất bại", devMsg = "Update failed" });
        }

        /// <summary>
        /// API Xóa 1 tài sản (Xóa mềm)
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteAsync(id);
            if (result > 0) return Ok(result);
            return StatusCode(500, new { userMsg = "Xóa thất bại", devMsg = "Delete failed" });
        }

        /// <summary>
        /// API Xóa nhiều tài sản
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        [HttpDelete("batch-delete")]
        public async Task<IActionResult> DeleteMulti([FromBody] List<Guid> ids)
        {
            try
            {
                var result = await _service.DeleteMultiAsync(ids);

                if (result > 0)
                {
                    return Ok(result);
                }
                return StatusCode(500, new { userMsg = "Không thể xóa các bản ghi này.", devMsg = "Delete failed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { userMsg = "Lỗi hệ thống", devMsg = ex.Message });
            }
        }
        /// <summary>
        /// API Lấy dữ liệu nhân bản từ một tài sản có sẵn
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        /// <param name="sourceId">ID của tài sản gốc</param>
        /// <returns>Đối tượng tài sản chứa thông tin cũ + Mã mới</returns>
        [HttpGet("{sourceId}/clone")]
        public async Task<IActionResult> Clone(Guid sourceId)
        {
            try
            {
                var result = await _service.PrepareCloneAsync(sourceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu không tìm thấy sourceId
                return StatusCode(500, new { userMsg = "Không thể nhân bản tài sản này.", devMsg = ex.Message });
            }
        }
    }
}   