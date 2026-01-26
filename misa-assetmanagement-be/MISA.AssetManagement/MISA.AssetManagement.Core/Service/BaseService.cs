using MISA.AssetManagement.Core.Exception;
using MISA.AssetManagement.Core.Interface.Repository;
using MISA.AssetManagement.Core.Interface.Service;
using MISA.AssetManagement.Core.MisaAttribute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Core.Service
{
    /// <summary>
    /// Service cơ sở xử lý logic chung
    /// CreatedBy: DDTOAN (14/01/2026)
    /// </summary>
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected readonly IBaseRepository<T> _baseRepository;

        public BaseService(IBaseRepository<T> baseRepository)
        {
            _baseRepository = baseRepository;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync() => await _baseRepository.GetAllAsync();

        public virtual async Task<T> GetByIdAsync(Guid id) => await _baseRepository.GetByIdAsync(id);

        public virtual async Task<int> InsertServiceAsync(T entity)
        {
            ValidateData(entity);
            await ValidateBusinessAsync(entity, true);
            return await _baseRepository.InsertAsync(entity);
        }

        public virtual async Task<int> UpdateServiceAsync(Guid id, T entity)
        {
            ValidateData(entity);
            await ValidateBusinessAsync(entity, false, id);
            return await _baseRepository.UpdateAsync(id, entity);
        }

        // Đã thêm từ khóa 'virtual' để lớp con có thể override
        public virtual async Task<int> DeleteAsync(Guid id)
        {
            var result = await _baseRepository.DeleteAsync(id);
            return result;
        }

        /// <summary>
        /// Hàm validate dữ liệu đầu vào dựa trên Attribute (Required, Length...)
        /// CreatedBy: DDTOAN (14/01/2026)
        /// </summary>
        private void ValidateData(T entity)
        {
            var props = typeof(T).GetProperties();
            var validateErrors = new Dictionary<string, string>();

            foreach (var prop in props)
            {
                var requiredAttrs = prop.GetCustomAttributes(typeof(MISARequired), true);
                if (requiredAttrs.Length > 0)
                {
                    var propValue = prop.GetValue(entity);
                    if (propValue == null || string.IsNullOrEmpty(propValue.ToString()))
                    {
                        var msg = (requiredAttrs[0] as MISARequired).ErrorMsg ?? $"{prop.Name} không được để trống.";
                        validateErrors.Add(prop.Name, msg);
                    }
                }
            }

            if (validateErrors.Count > 0)
            {
                throw new MISAValidateException("Dữ liệu đầu vào không hợp lệ", validateErrors);
            }
        }

        /// <summary>
        /// Hàm hook để lớp con viết logic nghiệp vụ riêng (Check trùng, check ngày tháng...)
        /// CreatedBy: DDTOAN (14/01/2026)
        /// </summary>
        protected virtual Task ValidateBusinessAsync(T entity, bool isInsert, Guid? id = null)
        {
            return Task.CompletedTask;
        }
    }
}