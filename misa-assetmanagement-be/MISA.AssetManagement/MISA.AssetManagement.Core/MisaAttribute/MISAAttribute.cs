using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.MisaAttribute
{
    /// <summary>
    /// Attribute đánh dấu khóa chính
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAKey : Attribute
    {
    }

    /// <summary>
    /// Attribute đánh dấu trường bắt buộc nhập
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MISARequired : Attribute
    {
        public string ErrorMsg { get; set; }
        public MISARequired(string errorMsg = "")
        {
            ErrorMsg = errorMsg;
        }
    }

    /// <summary>
    /// Attribute đặt tên hiển thị cho Property (dùng khi báo lỗi)
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MISAPropertyName : Attribute
    {
        public string Name { get; set; }
        public MISAPropertyName(string name)
        {
            Name = name;
        }
    }
}
