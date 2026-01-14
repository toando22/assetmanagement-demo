using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Service;

namespace MISA.AssetManagement.Api.Controllers
{

        [Route("api/v1/fixed-asset-categories")]
        public class FixedAssetCategoriesController : BaseController<FixedAssetCategory>
        {
            public FixedAssetCategoriesController(IFixedAssetCategoryService service) : base(service)
            {
            }
        }
    
}
