using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Repository;
using System.Data;

namespace MISA.AssetManagement.Infrastructure.Repository
{
    /// <summary>
    /// Repository Tài sản
    /// CreatedBy: DDTOAN (14/01/2026)
    /// </summary>
    public class FixedAssetRepository : BaseRepository<FixedAsset>, IFixedAssetRepository
    {
        public FixedAssetRepository(IConfiguration configuration) : base(configuration) { }

        /// <summary>
        /// Sinh mã tài sản mới tự động theo quy tắc TS + số tăng dần.
        /// Logic: Tối ưu hóa bằng SQL, không tải thừa dữ liệu về Server.
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy: DDTOAN (16/01/2026) - Fix performance
        /// </summary>
        public async Task<string> GetNewCodeAsync()
        {
            using (var conn = CreateConnection())
            {
                // Regex lọc các mã bắt đầu bằng TS và theo sau là số
                var sql = @"SELECT fixed_asset_code 
                            FROM fixed_asset 
                            WHERE fixed_asset_code REGEXP '^TS[0-9]+$' 
                            ORDER BY CAST(SUBSTRING(fixed_asset_code, 3) AS UNSIGNED) DESC 
                            LIMIT 1;";

                var maxCode = await conn.QueryFirstOrDefaultAsync<string>(sql);

                if (string.IsNullOrEmpty(maxCode))
                {
                    return "TS00001";
                }

                // Tách phần số và tăng lên 1
                var numberPart = maxCode.Substring(2);
                if (long.TryParse(numberPart, out long number))
                {
                    number++;
                    return $"TS{number.ToString().PadLeft(5, '0')}";
                }

                return "TS00001";
            }
        }

        /// <summary>
        /// Kiểm tra trùng mã
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        public async Task<bool> CheckDuplicateCodeAsync(string code)
        {
            using (var conn = CreateConnection())
            {
                var sql = "SELECT fixed_asset_code FROM fixed_asset WHERE fixed_asset_code = @Code LIMIT 1";
                var result = await conn.QueryFirstOrDefaultAsync<string>(sql, new { Code = code });
                return !string.IsNullOrEmpty(result);
            }
        }

        /// <summary>
        /// Sửa tài sản 
        /// CreatedBy: DDTOAN (15/01/2026)
        /// </summary>
        public async Task<int> UpdateAssetAsync(Guid id, FixedAssetUpdateDto assetDto)
        {
            using (var conn = CreateConnection())
            {
                var procName = "Proc_UpdateFixedAsset";
                var parameters = new DynamicParameters();

                parameters.Add("p_fixed_asset_id", id);
                parameters.Add("p_fixed_asset_code", assetDto.FixedAssetCode);
                parameters.Add("p_fixed_asset_name", assetDto.FixedAssetName);
                parameters.Add("p_department_id", assetDto.DepartmentId);
                parameters.Add("p_fixed_asset_category_id", assetDto.FixedAssetCategoryId);
                parameters.Add("p_fixed_asset_quantity", assetDto.FixedAssetQuantity);
                parameters.Add("p_fixed_asset_original_cost", assetDto.FixedAssetOriginalCost);
                parameters.Add("p_fixed_asset_depreciation_rate", assetDto.FixedAssetDepreciationRate);
                parameters.Add("p_fixed_asset_purchase_date", assetDto.FixedAssetPurchaseDate);
                parameters.Add("p_fixed_asset_start_use_date", assetDto.FixedAssetStartUseDate);
                parameters.Add("p_fixed_asset_tracking_year", assetDto.FixedAssetTrackingYear);
                parameters.Add("p_fixed_asset_years_of_use", assetDto.FixedAssetYearsOfUse);
                parameters.Add("p_fixed_asset_annual_depreciation_value", assetDto.FixedAssetAnnualDepreciationValue);
                parameters.Add("p_modified_date", DateTime.Now);
                parameters.Add("p_modified_by", assetDto.ModifiedBy ?? "User");

                var rowsAffected = await conn.ExecuteAsync(procName, parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected;
            }
        }

        /// <summary>
        /// Mapping hàm lấy danh sách phân trang và lọc
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy: DDTOAN (15/01/2026) - Sửa kiểu trả về thành FixedAssetDto
        /// </summary>
        public async Task<PagingResult<FixedAssetDto>> GetPagingAsync(int pageIndex, int pageSize, string keyword, Guid? departmentId, Guid? categoryId)
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

                var data = await conn.QueryAsync<FixedAssetDto>("Proc_GetFixedAssetsPaging", parameters, commandType: CommandType.StoredProcedure);
                var totalRecords = parameters.Get<int>("p_total_records");

                return new PagingResult<FixedAssetDto>
                {
                    Data = data,
                    TotalRecords = totalRecords
                };
            }
        }

        /// <summary>
        /// Override hàm Xóa của cha (BaseRepository) để chuyển thành Xóa mềm
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        public override async Task<int> DeleteAsync(Guid id)
        {
            using (var conn = CreateConnection())
            {
                var sql = @"UPDATE fixed_asset 
                            SET is_active = 0, 
                                modified_date = NOW(),
                                modified_by = @ModifiedBy 
                            WHERE fixed_asset_id = @id";

                var parameters = new { id = id, ModifiedBy = "Admin" };
                return await conn.ExecuteAsync(sql, parameters);
            }
        }

        /// <summary>
        /// Hàm Xóa nhiều bản ghi cùng lúc
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        public async Task<int> DeleteMultiAsync(List<Guid> ids)
        {
            using (var conn = CreateConnection())
            {
                var sql = @"UPDATE fixed_asset 
                            SET is_active = 0, 
                                modified_date = NOW(), 
                                modified_by = @ModifiedBy
                            WHERE fixed_asset_id IN @Ids";

                var parameters = new { Ids = ids, ModifiedBy = "Admin" };
                return await conn.ExecuteAsync(sql, parameters);
            }
        }

        /// <summary>
        /// Lấy thông tin tài sản theo ID
        /// CreatedBy: DDTOAN (16/01/2026)
        /// </summary>
        public async Task<FixedAsset> GetByIdAsync(Guid fixedAssetId)
        {
            using (var conn = CreateConnection())
            {
                var sql = "SELECT * FROM fixed_asset WHERE fixed_asset_id = @Id";

                // Dùng QueryFirstOrDefaultAsync để trả về null nếu không tìm thấy
                var result = await conn.QueryFirstOrDefaultAsync<FixedAsset>(sql, new { Id = fixedAssetId });
                return result;
            }
        }
    }
}