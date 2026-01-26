/*
  Mô tả: Composable để quản lý state năm filter
  Features:
  - Quản lý selectedYear state (mặc định: năm hiện tại)
  - Generate yearOptions (từ năm hiện tại trở về 10 năm trước)
  - Methods để set/get năm
  - Có thể sử dụng với provide/inject hoặc standalone
  
  CreatedBy: DDToan - (24/1/2026)
  
  EditBy: 
*/

import { ref, computed } from 'vue'

/**
 * Composable để quản lý state năm filter
 * @param {number} initialYear - Năm khởi tạo (mặc định: năm hiện tại)
 * @returns {object} - Object chứa state và methods
 */
export function useYearFilter(initialYear = null) {
  // Lấy năm hiện tại làm mặc định nếu không có initialYear
  const currentYear = new Date().getFullYear()
  const defaultYear = initialYear !== null ? initialYear : currentYear
  
  // State: Năm được chọn
  const selectedYear = ref(defaultYear)
  
  // Computed: Danh sách năm (từ năm hiện tại trở về trước 10 năm)
  const years = computed(() => {
    const currentYear = new Date().getFullYear()
    const yearsList = []
    for (let i = 0; i <= 10; i++) {
      yearsList.push(currentYear - i)
    }
    return yearsList
  })
  
  // Computed: Options cho dropdown năm - format { value, label }
  const yearOptions = computed(() => {
    return years.value.map(year => ({
      value: year,
      label: year.toString()
    }))
  })
  
  /*
    Mô tả: Set năm được chọn
    @param {number} year - Năm cần set
    CreatedBy: DDToan - (24/1/2026)
  */
  const setYear = (year) => {
    // Validate: năm phải là number và trong khoảng hợp lệ
    if (typeof year === 'number' && year >= 1900 && year <= 2100) {
      selectedYear.value = year
    } else {
      console.warn('useYearFilter: Invalid year value:', year, '- Using current year')
      selectedYear.value = currentYear
    }
  }
  
  /*
    Mô tả: Lấy năm hiện tại được chọn
    @returns {number} - Năm được chọn
    CreatedBy: DDToan - (24/1/2026)
  */
  const getYear = () => {
    return selectedYear.value
  }
  
  /*
    Mô tả: Reset về năm hiện tại
    CreatedBy: DDToan - (24/1/2026)
  */
  const resetToCurrentYear = () => {
    selectedYear.value = new Date().getFullYear()
  }
  
  return {
    // State
    selectedYear,
    
    // Computed
    years,
    yearOptions,
    
    // Methods
    setYear,
    getYear,
    resetToCurrentYear
  }
}
