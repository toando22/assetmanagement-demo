using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Exception
{
    /// <summary>
    /// Exception tùy chỉnh cho lỗi Validate
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    public class MISAValidateException : System.Exception
    {
        public string? ValidateErrorMsg { get; set; }
        public IDictionary Errors { get; set; }

        public MISAValidateException(string msg, IDictionary errors) : base(msg)
        {
            ValidateErrorMsg = msg;
            Errors = errors;
        }
    }
}
