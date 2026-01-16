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
    /// CreatedBy: DDTOAN (15/01/2026)
    /// EditedBy: DDTOAN (20/06/2024) - Thêm các trường Hao mòn lũy kế, Giá trị còn lại
    /// </summary>
    public class FixedAssetDto : FixedAsset
        {
            public string DepartmentName { get; set; }
            public string FixedAssetCategoryName { get; set; }

        /// <summary>
        /// Hao mòn/Khấu hao lũy kế
        /// </summary>
        public decimal AccumulatedDepreciation { get; set; }

        /// <summary>
        /// Giá trị còn lại
        /// </summary>
        public decimal RemainingValue { get; set; }
    }
    
}
