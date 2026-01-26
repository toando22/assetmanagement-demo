using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AssetManagement.Core.Interface.Service;

namespace MISA.AssetManagement.Api.Controllers
{
        [Route("api/v1/[controller]")]
        [ApiController]
        public class BaseController<T> : ControllerBase
        {
            #region Fields
            protected readonly IBaseService<T> _baseService;
            #endregion

            #region Constructor
            public BaseController(IBaseService<T> baseService)
            {
                _baseService = baseService;
            }
        #endregion

        #region Methods
        /// <summary>
        /// Lấy toàn bộ danh sách
        /// CreatedBy: DDTOAN (14/01/2026)
        /// </summary>
        [HttpGet]
            public virtual async Task<IActionResult> GetAllAsync()
            {
                var entities = await _baseService.GetAllAsync();
                return Ok(entities);
            }

        /// <summary>
        /// Lấy bản ghi theo ID
        /// CreatedBy: DDTOAN (14/01/2026)
        /// </summary>
        [HttpGet("{id}")]
            public virtual async Task<IActionResult> GetByIdAsync(Guid id)
            {
                var entity = await _baseService.GetByIdAsync(id);
                return Ok(entity);
            }

        /// <summary>
        /// Thêm mới bản ghi
        /// CreatedBy: DDTOAN (14/01/2026)
        /// </summary>
        [HttpPost]
            public virtual async Task<IActionResult> InsertAsync([FromBody] T entity)
            {
                var result = await _baseService.InsertServiceAsync(entity);
                // Trả về 201 Created và số bản ghi thêm được
                return StatusCode(StatusCodes.Status201Created, result);
            }

        /// <summary>
        /// Cập nhật bản ghi
        /// CreatedBy: DDTOAN (14/01/2026)
        /// </summary>
        [HttpPut("{id}")]
            public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] T entity)
            {
                var result = await _baseService.UpdateServiceAsync(id, entity);
                return Ok(result);
            }

        /// <summary>
        /// Xóa bản ghi
        /// CreatedBy: DDTOAN (14/01/2026)
        /// </summary>
        [HttpDelete("{id}")]
            public virtual async Task<IActionResult> DeleteAsync(Guid id)
            {
                var result = await _baseService.DeleteAsync(id);
                return Ok(result);
            }
            #endregion
        }
    
}
