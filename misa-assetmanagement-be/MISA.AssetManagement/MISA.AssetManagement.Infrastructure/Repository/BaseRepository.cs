using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AssetManagement.Core.Interface.Repository;
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

            public virtual async Task<int> InsertAsync(T entity)
            {
                using (var conn = CreateConnection())
                {
                    var procName = $"Proc_Insert{typeof(T).Name}";
                    return await conn.ExecuteAsync(procName, entity, commandType: CommandType.StoredProcedure);
                }
            }

            public virtual async Task<int> UpdateAsync(Guid id, T entity)
            {
                using (var conn = CreateConnection())
                {
                    var procName = $"Proc_Update{typeof(T).Name}";
                    return await conn.ExecuteAsync(procName, entity, commandType: CommandType.StoredProcedure);
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
