using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Interface.Service
{
    /// <summary>
    /// Interface Service cơ sở
    /// CreatedBy: DDTOAN (14/01/2026)
    /// EditedBy:
    /// </summary>
    public interface IBaseService<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task<int> InsertServiceAsync(T entity);
        Task<int> UpdateServiceAsync(Guid id, T entity);
        Task<int> DeleteAsync(Guid id);
    }
}
