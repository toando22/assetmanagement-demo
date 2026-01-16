using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AssetManagement.Core.Interface.Repository;
using MISA.AssetManagement.Core.MisaAttribute;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Infrastructure.Repository
{
    /// <summary>
    /// Repository cơ sở Dapper
    /// CreatedBy: DDTOAN (14/01/2026)
    /// </summary>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly string _connectionString;
        protected string _tableName;

        public BaseRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tableName = ConvertToSnakeCase(typeof(T).Name);
            // Cấu hình Dapper map snake_case sang PascalCase tự động
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }

        protected IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using (var conn = CreateConnection())
            {
                // Lưu ý: Đảm bảo mọi bảng đều có cột created_date, nếu không sẽ lỗi SQL
                var sql = $"SELECT * FROM {_tableName} ORDER BY created_date DESC";
                return await conn.QueryAsync<T>(sql);
            }
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            using (var conn = CreateConnection())
            {
                var sql = $"SELECT * FROM {_tableName} WHERE {_tableName}_id = @Id";
                return await conn.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
            }
        }

        /// <summary>
        /// Thêm mới bản ghi dùng Stored Procedure
        /// </summary>
        public virtual async Task<int> InsertAsync(T entity)
        {
            using (var conn = CreateConnection())
            {
                var procName = $"Proc_Insert{typeof(T).Name}";
                var props = typeof(T).GetProperties();

                // 1. Tự sinh ID nếu chưa có
                var pkProp = props.FirstOrDefault(p => p.GetCustomAttributes(typeof(MISAKey), true).Length > 0);
                if (pkProp != null && (Guid)pkProp.GetValue(entity) == Guid.Empty)
                {
                    pkProp.SetValue(entity, Guid.NewGuid());
                }

                // 2. Map tham số
                var parameters = new DynamicParameters();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(entity);
                    var name = ConvertToSnakeCase(prop.Name);
                    parameters.Add(name, value);
                }

                return await conn.ExecuteAsync(procName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Cập nhật bản ghi dùng Stored Procedure
        /// </summary>
        public virtual async Task<int> UpdateAsync(Guid id, T entity)
        {
            using (var conn = CreateConnection())
            {
                var procName = $"Proc_Update{typeof(T).Name}";
                var parameters = new DynamicParameters();
                var props = typeof(T).GetProperties();

                foreach (var prop in props)
                {
                    var name = ConvertToSnakeCase(prop.Name);
                    var value = prop.GetValue(entity);

                    // Logic: Luôn lấy ID từ URL để đảm bảo an toàn
                    if (prop.IsDefined(typeof(MISAKey), true))
                    {
                        value = id;
                    }

                    parameters.Add(name, value);
                }

                return await conn.ExecuteAsync(procName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public virtual async Task<int> DeleteAsync(Guid id)
        {
            using (var conn = CreateConnection())
            {
                var sql = $"DELETE FROM {_tableName} WHERE {_tableName}_id = @Id";
                return await conn.ExecuteAsync(sql, new { Id = id });
            }
        }

        public virtual async Task<bool> CheckDuplicateCodeAsync(string code)
        {
            using (var conn = CreateConnection())
            {
                var sql = $"SELECT {_tableName}_code FROM {_tableName} WHERE {_tableName}_code = @Code";
                var res = await conn.QueryFirstOrDefaultAsync<string>(sql, new { Code = code });
                return res != null;
            }
        }

        private string ConvertToSnakeCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
        }
    }
}