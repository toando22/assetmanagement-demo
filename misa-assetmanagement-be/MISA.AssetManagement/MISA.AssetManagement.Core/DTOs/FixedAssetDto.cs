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
    public class FixedAssetDto 
        {
        public Guid FixedAssetId { get; set; }

        public string FixedAssetCode { get; set; }

        public string FixedAssetName { get; set; }

        public Guid DepartmentId { get; set; }

        public Guid FixedAssetCategoryId { get; set; }

        public DateTime FixedAssetPurchaseDate { get; set; }

        public DateTime FixedAssetStartUseDate { get; set; }

        public decimal FixedAssetOriginalCost { get; set; }

        public int FixedAssetQuantity { get; set; }

        public decimal FixedAssetDepreciationRate { get; set; }

        public int FixedAssetTrackingYear { get; set; }

        public int FixedAssetYearsOfUse { get; set; }

        public int FixedAssetProductionYear { get; set; }

        public decimal FixedAssetAnnualDepreciationValue { get; set; }
        public required string DepartmentName { get; set; }
            public required string FixedAssetCategoryName { get; set; }

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
