/*
  Mô tả: Constants cho dữ liệu tài sản (Bộ phận sử dụng, Loại tài sản)
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

/**
 * Danh sách Bộ phận sử dụng
 */
export const DEPARTMENTS = [
  { code: '01', name: 'Ban Giám hiệu' },
  { code: '02', name: 'Phòng Hành chính - Quản trị' },
  { code: '03', name: 'Phòng Tài vụ' },
  { code: '04', name: 'Phòng Đào tạo' },
  { code: '05', name: 'Tổ chuyên môn (Giáo viên bộ môn)' },
]

/**
 * Danh sách Loại tài sản cố định
 */
export const ASSET_TYPES = [
  { 
    code: '1', 
    name: 'Nhà, công trình xây dựng', 
    lifeYears: 80, 
    depreciationRate: 1.25 
  },
  { 
    code: '2', 
    name: 'Vật kiến trúc', 
    lifeYears: 20, 
    depreciationRate: 5.00 
  },
  { 
    code: '3', 
    name: 'Xe ô tô', 
    lifeYears: 15, 
    depreciationRate: 6.67 
  },
  { 
    code: '4', 
    name: 'Phương tiện vận tải khác (ngoài xe ô tô)', 
    lifeYears: 20, 
    depreciationRate: 5.00 
  },
  { 
    code: '5', 
    name: 'Máy móc, thiết bị', 
    lifeYears: 5, 
    depreciationRate: 20.00 
  },
  { 
    code: '6', 
    name: 'Cây lâu năm, súc vật làm việc và/hoặc cho sản phẩm', 
    lifeYears: 4, 
    depreciationRate: 25.00 
  },
  { 
    code: '7', 
    name: 'Tài sản cố định hữu hình khác', 
    lifeYears: 10, 
    depreciationRate: 10.00 
  },
]

/**
 * Lấy thông tin bộ phận theo code
 * @param {string} code - Mã bộ phận
 * @returns {object|null} - Thông tin bộ phận hoặc null
 */
export const getDepartmentByCode = (code) => {
  return DEPARTMENTS.find(dept => dept.code === code) || null
}

/**
 * Lấy thông tin loại tài sản theo code
 * @param {string} code - Mã loại tài sản
 * @returns {object|null} - Thông tin loại tài sản hoặc null
 */
export const getAssetTypeByCode = (code) => {
  return ASSET_TYPES.find(type => type.code === code) || null
}

/**
 * Mock data - Danh sách tài sản
 */
export const MOCK_ASSETS = [
  {
    id: 1,
    code: '55H7WN72/2022',
    name: 'Dell Inspiron 3467',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Hành chính Kế toán',
    quantity: 1,
    cost: 20000000,
    depreciation: 894000,
    remainingValue: 19106000
  },
  {
    id: 2,
    code: 'MXT88618',
    name: 'Máy tính xách tay Fujitsu',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Hành chính Kế toán',
    quantity: 1,
    cost: 10000000,
    depreciation: 1225000,
    remainingValue: 8775000
  },
  {
    id: 3,
    code: '37H7WN72/2022',
    name: 'Dell Inspiron 3467',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Hành chính Kế toán',
    quantity: 4,
    cost: 40000000,
    depreciation: 1730000,
    remainingValue: 38270000
  },
  {
    id: 4,
    code: 'MXT8866',
    name: 'Máy tính xách tay Fujitsu',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Thư ký',
    quantity: 1,
    cost: 5000000,
    depreciation: 1646000,
    remainingValue: 3354000
  },
  {
    id: 5,
    code: '14H7WN72/2019',
    name: 'Dell Latitude E 5450',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Hành chính Kế toán',
    quantity: 1,
    cost: 10000000,
    depreciation: 2456000,
    remainingValue: 7544000
  },
  {
    id: 6,
    code: 'D0PQ3F2/2017',
    name: 'DELL Inspiron 3467',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Hành chính Kế toán',
    quantity: 20,
    cost: 50000000,
    depreciation: 913000,
    remainingValue: 49087000
  },
  {
    id: 7,
    code: 'MXT8869',
    name: 'Máy tính xách tay Fujitsu',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Hành chính Kế toán',
    quantity: 1,
    cost: 50000000,
    depreciation: 3929000,
    remainingValue: 46071000
  },
  {
    id: 8,
    code: '49H7WN72/2022',
    name: 'Dell Inspiron 3467',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Tài chính Tổng hợp',
    quantity: 1,
    cost: 4000000,
    depreciation: 432000,
    remainingValue: 3568000
  },
  {
    id: 9,
    code: '33H7WN72/2022',
    name: 'Dell Inspiron 3467',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Tài chính Tổng hợp',
    quantity: 1,
    cost: 20000000,
    depreciation: 3400000,
    remainingValue: 16600000
  },
  {
    id: 10,
    code: '22H7WN72/2019',
    name: 'Dell Latitude E 5450',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Tài chính Tổng hợp',
    quantity: 1,
    cost: 40000000,
    depreciation: 3091000,
    remainingValue: 36909000
  },
  {
    id: 11,
    code: 'MXT88617',
    name: 'Máy tính xách tay Fujitsu',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Tài chính Tổng hợp',
    quantity: 1,
    cost: 40000000,
    depreciation: 1789000,
    remainingValue: 38211000
  },
  {
    id: 12,
    code: '50H7WN72/2022',
    name: 'Dell Inspiron 3467',
    type: 'Máy vi tính xách tay',
    department: 'Phòng Tài chính Tổng hợp',
    quantity: 1,
    cost: 20000000,
    depreciation: 1521000,
    remainingValue: 18479000
  }
]

// Tạo thêm data để có tổng 200 bản ghi
const additionalAssets = []
for (let i = 13; i <= 200; i++) {
  const types = ['Máy vi tính xách tay', 'Máy tính để bàn', 'Thiết bị văn phòng']
  const departments = ['Phòng Hành chính Kế toán', 'Phòng Thư ký', 'Phòng Tài chính Tổng hợp']
  const brands = ['Dell Inspiron 3467', 'Dell Latitude E 5450', 'Máy tính xách tay Fujitsu', 'HP ProBook 450', 'Lenovo ThinkPad']
  
  const randomType = types[Math.floor(Math.random() * types.length)]
  const randomDept = departments[Math.floor(Math.random() * departments.length)]
  const randomBrand = brands[Math.floor(Math.random() * brands.length)]
  const randomQuantity = Math.floor(Math.random() * 20) + 1
  const randomCost = (Math.floor(Math.random() * 10) + 1) * 5000000
  const randomDepreciation = Math.floor(Math.random() * randomCost * 0.3)
  
  additionalAssets.push({
    id: i,
    code: `${Math.random().toString(36).substring(2, 8).toUpperCase()}${i}`,
    name: randomBrand,
    type: randomType,
    department: randomDept,
    quantity: randomQuantity,
    cost: randomCost,
    depreciation: randomDepreciation,
    remainingValue: randomCost - randomDepreciation
  })
}

MOCK_ASSETS.push(...additionalAssets)
