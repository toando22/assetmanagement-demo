using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Repository;
using MISA.AssetManagement.Core.Interface.Service;

namespace MISA.AssetManagement.Api.Controllers
{
        /// <summary>
        /// API Tài sản
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy:
        /// </summary>
        [Route("api/v1/fixed-assets")]
        [ApiController]
        public class FixedAssetsController : ControllerBase
        {
            private readonly IFixedAssetService _service;
            private readonly IFixedAssetRepository _repo; // Để gọi get new code nhanh

            public FixedAssetsController(IFixedAssetService service, IFixedAssetRepository repo)
            {
                _service = service;
                _repo = repo;
            }

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
                var code = await _repo.GetNewCodeAsync();
                return Ok(code);
            }

            [HttpPost]
            public async Task<IActionResult> Insert(FixedAsset entity)
            {
                var result = await _service.InsertServiceAsync(entity);
                return StatusCode(201, result);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(Guid id, FixedAsset entity)
            {
                var result = await _service.UpdateServiceAsync(id, entity);
                return Ok(result);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(Guid id)
            {
                var result = await _service.DeleteServiceAsync(id);
                return Ok(result);
            }
        }
   


}
