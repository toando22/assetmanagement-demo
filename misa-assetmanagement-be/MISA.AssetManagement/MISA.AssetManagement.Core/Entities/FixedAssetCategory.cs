using MISA.AssetManagement.Core.MisaAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Entities
{
    /// <summary>
    /// Thực thể Loại tài sản
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    public class FixedAssetCategory
    {
        [MISAKey]
        public Guid FixedAssetCategoryId { get; set; }

        [MISARequired]
        public string FixedAssetCategoryCode { get; set; }

        [MISARequired]
        public string FixedAssetCategoryName { get; set; }

        public int FixedAssetCategoryLifeYears { get; set; }
        public decimal FixedAssetCategoryDepreciationRate { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
