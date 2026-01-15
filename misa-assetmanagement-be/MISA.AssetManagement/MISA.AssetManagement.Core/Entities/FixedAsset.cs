using MISA.AssetManagement.Core.MisaAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Entities
{
    /// <summary>
    /// Thực thể Tài sản
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    public class FixedAsset
    {
        [MISAKey]
        public Guid FixedAssetId { get; set; }

        [MISARequired("Mã tài sản không được để trống")]
        [MISAPropertyName("Mã tài sản")]
        public string FixedAssetCode { get; set; }

        [MISARequired("Tên tài sản không được để trống")]
        [MISAPropertyName("Tên tài sản")]
        public string FixedAssetName { get; set; }

        [MISARequired("Bộ phận không được để trống")]
        [MISAPropertyName("Bộ phận sử dụng")]
        public Guid DepartmentId { get; set; }

        [MISARequired("Loại tài sản không được để trống")]
        [MISAPropertyName("Loại tài sản")]
        public Guid FixedAssetCategoryId { get; set; }

        [MISARequired("Ngày mua không được để trống")]
        public DateTime FixedAssetPurchaseDate { get; set; }

        [MISARequired("Ngày bắt đầu sử dụng không được để trống")]
        public DateTime FixedAssetStartUseDate { get; set; }

        [MISARequired("Nguyên giá không được để trống")]
        public decimal FixedAssetOriginalCost { get; set; }

        [MISARequired("Số lượng không được để trống")]
        public int FixedAssetQuantity { get; set; }

        [MISARequired("Tỷ lệ hao mòn không được để trống")]
        public decimal FixedAssetDepreciationRate { get; set; }

        public int FixedAssetTrackingYear { get; set; }

        [MISARequired("Số năm sử dụng không được để trống")]
        public int FixedAssetYearsOfUse { get; set; }

        public int FixedAssetProductionYear { get; set; }

        [MISARequired("Giá trị hao mòn năm không được để trống")]
        public decimal FixedAssetAnnualDepreciationValue { get; set; }

        #region Audit
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        #endregion
    }
}
