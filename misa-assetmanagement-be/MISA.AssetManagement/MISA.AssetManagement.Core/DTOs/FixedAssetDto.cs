using MISA.AssetManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.DTOs
{
   
        /// <summary>
        /// DTO dùng để trả dữ liệu ra Frontend (có kèm tên Bộ phận, Loại)
        /// </summary>
        public class FixedAssetDto : FixedAsset
        {
            public string DepartmentName { get; set; }
            public string FixedAssetCategoryName { get; set; }
        }
    
}
