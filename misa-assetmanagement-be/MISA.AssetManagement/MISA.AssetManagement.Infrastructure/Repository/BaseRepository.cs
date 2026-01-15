using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AssetManagement.Core.Interface.Repository;
using MISA.AssetManagement.Core.MisaAttribute;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Infrastructure.Repository
{
        /// <summary>
        /// Repository cơ sở Dapper
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy:
        /// </summary>
        public class BaseRepository<T> : IBaseRepository<T> where T : class
        {
            protected readonly string _connectionString;
            protected string _tableName;

            public BaseRepository(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
                _tableName = ConvertToSnakeCase(typeof(T).Name);
                // Cấu hình Dapper map snake_case
                Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            }

            protected IDbConnection CreateConnection() => new MySqlConnection(_connectionString);

            public virtual async Task<IEnumerable<T>> GetAllAsync()
            {
                using (var conn = CreateConnection())
                {
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
        /// Thêm mới bản ghi
        /// EditBy: DDTOAN - 15/01/2026 - Fix lỗi mapping tham số Dapper (PascalCase -> snake_case) và tự sinh ID
        /// </summary>
        public virtual async Task<int> InsertAsync(T entity)
        {
            using (var conn = CreateConnection())
            {
                var procName = $"Proc_Insert{typeof(T).Name}";

                // 1. Tự động sinh ID mới nếu chưa có (Guid.Empty)
                var props = typeof(T).GetProperties();
                var pkProp = props.FirstOrDefault(p => p.GetCustomAttributes(typeof(MISAKey), true).Length > 0);

                if (pkProp != null && (Guid)pkProp.GetValue(entity) == Guid.Empty)
                {
                    pkProp.SetValue(entity, Guid.NewGuid());
                }

                // 2. Map tham số thủ công sang snake_case để khớp với Stored Procedure
                var parameters = new DynamicParameters();
                foreach (var prop in props)
                {
                    var value = prop.GetValue(entity);
                    var name = ConvertToSnakeCase(prop.Name); // Chuyển FixedAssetId -> fixed_asset_id
                    parameters.Add(name, value);
                }

                return await conn.ExecuteAsync(procName, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Cập nhật bản ghi
        /// EditBy: DDTOAN - 15/01/2026 - Fix lỗi mapping tham số Dapper (PascalCase -> snake_case)
        /// EditBy: DDTOAN - 16/01/2026 - Sửa logic lấy giá trị Khóa Chính từ tham số 'id' thay vì từ Entity
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
                    // 1. Map tên tham số sang Snake_Case (VD: FixedAssetCode -> fixed_asset_code)
                    var name = ConvertToSnakeCase(prop.Name);

                    // 2. Lấy giá trị từ Entity (Dữ liệu từ Body JSON)
                    var value = prop.GetValue(entity);

                    // 3. LOGIC XỬ LÝ ID CHUYÊN NGHIỆP:
                    // Nếu property hiện tại là Khóa Chính (có attribute [MISAKey])
                    // -> Ta BẮT BUỘC lấy giá trị từ tham số 'id' (trên URL)
                    // -> Điều này đảm bảo tính nhất quán và sửa đúng bản ghi cần sửa.
                    if (prop.IsDefined(typeof(MISAKey), true))
                    {
                        value = id;
                    }

                    parameters.Add(name, value);
                }

                // Thực thi Stored Procedure
                var rowsAffected = await conn.ExecuteAsync(procName, parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected;
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
