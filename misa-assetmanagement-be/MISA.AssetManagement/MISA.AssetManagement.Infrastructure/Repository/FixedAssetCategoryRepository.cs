using Microsoft.Extensions.Configuration;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Infrastructure.Repository
{
    public class FixedAssetCategoryRepository : BaseRepository<FixedAssetCategory>, IFixedAssetCategoryRepository
    {
        public FixedAssetCategoryRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
