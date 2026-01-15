using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.AssetManagement.Core.DTOs;
using MISA.AssetManagement.Core.Entities;
using MISA.AssetManagement.Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        /// <summary>
        /// Mã tự sinh và sinh mã dựa trên mã chuẩn có tiền tố "TS"
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy: DDTOAN (16/01/2026) - Sửa logic sinh mã để đúng yêu cầu
        /// </summary>
        /// <summary>
        /// Sinh mã tài sản mới tự động theo quy tắc TS + số tăng dần.
        /// Logic: Chỉ dựa trên các mã đúng chuẩn hệ thống (TSxxxxx).
        /// Các mã do người dùng tự nhập (không đúng chuẩn) sẽ bị bỏ qua khi tính toán,
        /// nhưng vẫn được lưu bình thường ở hàm Insert.
        /// </summary>
        public async Task<string> GetNewCodeAsync()
        {
            using (var conn = CreateConnection())
            {
                // 1. Lọc sơ bộ bằng SQL
                // Tăng LIMIT lên 500 để "bất chấp" việc có nhiều mã rác
                var sql = @"SELECT fixed_asset_code 
                    FROM fixed_asset 
                    WHERE fixed_asset_code LIKE 'TS%' 
                    ORDER BY LENGTH(fixed_asset_code) DESC, fixed_asset_code DESC 
                    LIMIT 500";

                var codes = await conn.QueryAsync<string>(sql);

                foreach (var code in codes)
                {
                    if (string.IsNullOrWhiteSpace(code)) continue;

                    var cleanCode = code.Trim();

                    // 2. BỘ LỌC THÉP (STRICT REGEX)
                    // Chỉ chấp nhận: Bắt đầu TS + Theo sau là Số nguyên hoàn toàn
                    // TS00002fhgdsfhd -> CÓ CHỮ -> TRƯỢT !!!
                    if (!System.Text.RegularExpressions.Regex.IsMatch(cleanCode, @"^TS[0-9]+$"))
                    {
                        continue;
                    }

                    // 3. Xử lý mã chuẩn
                    var numberPart = cleanCode.Substring(2);
                    if (long.TryParse(numberPart, out long number))
                    {
                        number++;
                        return $"TS{number.ToString().PadLeft(5, '0')}";
                    }
                }

                return "TS00001";
            }
        }


        /// <summary>
        /// Sửa tài sản 
        /// CreatedBy: DDTOAN (15/01/2026)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assetDto"></param>
        /// <returns></returns>

        // Hàm này nhận vào DTO, map từng cái sang tham số SQL -> Tránh lỗi 500 tuyệt đối
        public async Task<int> UpdateAssetAsync(Guid id, FixedAssetUpdateDto assetDto)
        {
            using (var conn = CreateConnection())
            {
                var procName = "Proc_UpdateFixedAsset";
                var parameters = new DynamicParameters();

                // Thêm tiền tố p_ vào trước các tham số
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

                // Không gửi created_date nữa -> Hết lỗi 500
                parameters.Add("p_modified_date", DateTime.Now);
                parameters.Add("p_modified_by", assetDto.ModifiedBy ?? "User");

                var rowsAffected = await conn.ExecuteAsync(procName, parameters, commandType: CommandType.StoredProcedure);
                return rowsAffected;
            }
        }
        private string ConvertToSnakeCase(string str)
        {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
        }

        /// <summary>
        /// Mapping hàm lấy danh sách phân trang và lọc
        /// CreatedBy: DDTOAN (14/01/2026)
        /// EditedBy:  DDTOAN (15/01/2026) - Sửa kiểu trả về thành FixedAssetDto
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="keyword"></param>
        /// <param name="departmentId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
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

                // Dapper sẽ tự động map các cột department_name, fixed_asset_category_name vào DTO
                var data = await conn.QueryAsync<FixedAssetDto>("Proc_GetFixedAssetsPaging", parameters, commandType: CommandType.StoredProcedure);
                var totalRecords = parameters.Get<int>("p_total_records");

                return new PagingResult<FixedAssetDto>
                {
                    Data = data,
                    TotalRecords = totalRecords
                };
            }
        }

    }
    
}
