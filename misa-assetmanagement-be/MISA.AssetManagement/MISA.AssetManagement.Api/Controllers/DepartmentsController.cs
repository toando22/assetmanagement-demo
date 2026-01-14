using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Service;

namespace MISA.AssetManagement.Api.Controllers
{

  
        /// <summary>
        /// API Danh mục Bộ phận
        /// CreatedBy: DDTOAN (14/01/2026)
        /// </summary>
        [Route("api/v1/departments")]
        [ApiController]
        public class DepartmentsController : BaseController<Department>
        {
            public DepartmentsController(IDepartmentService service) : base(service)
            {
            }
        }
    
}
