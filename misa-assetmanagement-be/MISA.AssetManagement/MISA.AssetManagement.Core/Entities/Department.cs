using MISA.AssetManagement.Core.MisaAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Entities
{
    /// <summary>
    /// Thực thể Bộ phận
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    public class Department
    {
        [MISAKey]
        public Guid DepartmentId { get; set; }

        [MISARequired]
        public string DepartmentCode { get; set; }

        [MISARequired]
        public string DepartmentName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
