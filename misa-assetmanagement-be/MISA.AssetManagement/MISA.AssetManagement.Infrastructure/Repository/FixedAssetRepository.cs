using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.AssetManagement.Infrastructure.Repository
{
        /// <summary>
        /// Repository Tài sản
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy:
        /// </summary>
        public class FixedAssetRepository : BaseRepository<FixedAsset>, IFixedAssetRepository
        {
            public FixedAssetRepository(IConfiguration configuration) : base(configuration) { }

            public async Task<string> GetNewCodeAsync()
            {
                using (var conn = CreateConnection())
                {
                    // Lấy mã lớn nhất: Order by độ dài rồi đến giá trị
                    var sql = "SELECT fixed_asset_code FROM fixed_asset ORDER BY LENGTH(fixed_asset_code) DESC, fixed_asset_code DESC LIMIT 1";
                    var maxCode = await conn.QueryFirstOrDefaultAsync<string>(sql);

                    if (string.IsNullOrEmpty(maxCode)) return "TS00001";

                    // Tách số (TS00001 -> 1)
                    var numberPart = maxCode.Substring(2);
                    if (long.TryParse(numberPart, out long number))
                    {
                        number++;
                        return $"TS{number.ToString().PadLeft(5, '0')}";
                    }

                    return "TS00001";
                }
            }

            public async Task<PagingResult<FixedAsset>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId)
            {
                using (var conn = CreateConnection())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("p_page_index", pageIndex);
                    parameters.Add("p_page_size", pageSize);
                    parameters.Add("p_keyword", keyword);
                    parameters.Add("p_department_id", departmentId);
                    parameters.Add("p_category_id", categoryId);
                    parameters.Add("p_total_records", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    var data = await conn.QueryAsync<FixedAsset>("Proc_GetFixedAssetsPaging", parameters, commandType: CommandType.StoredProcedure);
                    var totalRecords = parameters.Get<int>("p_total_records");

                    return new PagingResult<FixedAsset>
                    {
                        Data = data,
                        TotalRecords = totalRecords
                    };
                }
            }
        }
    
}
