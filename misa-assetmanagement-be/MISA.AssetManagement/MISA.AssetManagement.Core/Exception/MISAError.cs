using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Exception
{
    /// <summary>
    /// Model lỗi trả về cho Client
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    public class MISAError
    {
        public string? UserMsg { get; set; }
        public string? DevMsg { get; set; }
        public string? ErrorCode { get; set; }
        public string? TraceId { get; set; }
        public string? MoreInfo { get; set; }

        /// <summary>
        /// Danh sách lỗi chi tiết (Dictionary)
        /// </summary>
        public object? Errors { get; set; }
    }
}
