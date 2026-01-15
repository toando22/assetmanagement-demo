using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.DTOs
{
    /// <summary>
    /// DTO dùng để cập nhật Tài sản
    /// CreatedBy: DDTOAN (15/01/2026)
    /// </summary>
    public class FixedAssetUpdateDto
    {
        // Không cần ID ở đây vì ID sẽ lấy từ URL
        public string FixedAssetCode { get; set; }
        public string FixedAssetName { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid FixedAssetCategoryId { get; set; }
        public int FixedAssetQuantity { get; set; }
        public decimal FixedAssetOriginalCost { get; set; }
        public decimal FixedAssetDepreciationRate { get; set; }
        public DateTime FixedAssetPurchaseDate { get; set; }
        public DateTime FixedAssetStartUseDate { get; set; }
        public int FixedAssetTrackingYear { get; set; }
        public int FixedAssetYearsOfUse { get; set; }
        public decimal FixedAssetAnnualDepreciationValue { get; set; }

        // Có thể thêm ModifiedBy nếu cần
        public string? ModifiedBy { get; set; }
       // public DateTime? ModifiedDate { get; set; } = DateTime.Now;
    }
}
