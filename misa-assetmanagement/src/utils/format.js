/*
  Mô tả: Utility functions cho format dữ liệu (số, ngày, tiền tệ)
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
*/

/**
 * Format số với dấu chấm (.) ngăn cách hàng nghìn
 * @param {number|string} value - Giá trị cần format
 * @returns {string} - Chuỗi đã format (VD: 1000000 → "1.000.000")
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const formatNumber = (value) => {
  if (value === null || value === undefined || value === '') {
    return '0'
  }
  
  // Convert to number nếu là string
  const numValue = typeof value === 'string' ? parseFloat(value.replace(/\./g, '')) : Number(value)
  
  if (isNaN(numValue)) {
    return '0'
  }
  
  // Format với dấu chấm ngăn cách hàng nghìn
  return numValue.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.')
}

/**
 * Parse số từ string có dấu chấm
 * @param {string} value - Chuỗi cần parse (VD: "1.000.000")
 * @returns {number} - Số đã parse (VD: 1000000)
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const parseNumber = (value) => {
  if (!value || value === '') {
    return 0
  }
  
  // Remove dấu chấm và convert to number
  const cleanedValue = value.toString().replace(/\./g, '')
  const numValue = parseFloat(cleanedValue)
  
  return isNaN(numValue) ? 0 : numValue
}

/**
 * Format tiền tệ với đơn vị (tùy chọn)
 * @param {number|string} value - Giá trị cần format
 * @param {string} unit - Đơn vị tiền tệ (mặc định: '')
 * @returns {string} - Chuỗi đã format (VD: 1000000 → "1.000.000 VNĐ")
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const formatCurrency = (value, unit = '') => {
  const formatted = formatNumber(value)
  return unit ? `${formatted} ${unit}` : formatted
}

/**
 * Format ngày tháng theo định dạng dd/mm/YYYY
 * @param {Date|string} date - Ngày cần format
 * @returns {string} - Chuỗi đã format (VD: "25/08/2021")
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const formatDate = (date) => {
  if (!date) {
    return ''
  }
  
  let dateObj
  
  // Nếu là string, parse thành Date
  if (typeof date === 'string') {
    // Hỗ trợ format dd/mm/YYYY
    if (date.includes('/')) {
      const [day, month, year] = date.split('/')
      dateObj = new Date(year, month - 1, day)
    } else {
      dateObj = new Date(date)
    }
  } else {
    dateObj = date
  }
  
  if (!(dateObj instanceof Date) || isNaN(dateObj.getTime())) {
    return ''
  }
  
  const day = String(dateObj.getDate()).padStart(2, '0')
  const month = String(dateObj.getMonth() + 1).padStart(2, '0')
  const year = dateObj.getFullYear()
  
  return `${day}/${month}/${year}`
}

/**
 * Parse ngày từ string định dạng dd/mm/YYYY
 * @param {string} dateString - Chuỗi ngày (VD: "25/08/2021")
 * @returns {Date|null} - Date object hoặc null nếu không hợp lệ
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const parseDate = (dateString) => {
  if (!dateString || dateString === '') {
    return null
  }
  
  // Hỗ trợ format dd/mm/YYYY
  if (dateString.includes('/')) {
    const parts = dateString.split('/')
    if (parts.length === 3) {
      const day = parseInt(parts[0], 10)
      const month = parseInt(parts[1], 10) - 1 // Month is 0-indexed
      const year = parseInt(parts[2], 10)
      
      const date = new Date(year, month, day)
      
      // Validate date
      if (
        date.getDate() === day &&
        date.getMonth() === month &&
        date.getFullYear() === year
      ) {
        return date
      }
    }
  }
  
  // Fallback: try to parse as ISO string
  const date = new Date(dateString)
  if (!isNaN(date.getTime())) {
    return date
  }
  
  return null
}

/**
 * Lấy năm từ ngày
 * @param {Date|string} date - Ngày cần lấy năm
 * @returns {number|null} - Năm hoặc null nếu không hợp lệ
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const getYearFromDate = (date) => {
  if (!date) {
    return null
  }
  
  const dateObj = typeof date === 'string' ? parseDate(date) : date
  
  if (!dateObj || !(dateObj instanceof Date) || isNaN(dateObj.getTime())) {
    return null
  }
  
  return dateObj.getFullYear()
}

/**
 * Format số thập phân với số chữ số sau dấu phẩy
 * @param {number|string} value - Giá trị cần format
 * @param {number} decimals - Số chữ số sau dấu phẩy (mặc định: 2)
 * @returns {string} - Chuỗi đã format (VD: 6.67 → "6,67")
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const formatDecimal = (value, decimals = 2) => {
  if (value === null || value === undefined || value === '') {
    return '0'
  }
  
  const numValue = typeof value === 'string' ? parseFloat(value.replace(',', '.')) : Number(value)
  
  if (isNaN(numValue)) {
    return '0'
  }
  
  // Format với dấu phẩy cho phần thập phân
  return numValue.toFixed(decimals).replace('.', ',')
}

/**
 * Parse số thập phân từ string có dấu phẩy
 * @param {string} value - Chuỗi cần parse (VD: "6,67")
 * @returns {number} - Số đã parse (VD: 6.67)
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const parseDecimal = (value) => {
  if (!value || value === '') {
    return 0
  }
  
  // Replace dấu phẩy bằng dấu chấm và parse
  const cleanedValue = value.toString().replace(',', '.')
  const numValue = parseFloat(cleanedValue)
  
  return isNaN(numValue) ? 0 : numValue
}
