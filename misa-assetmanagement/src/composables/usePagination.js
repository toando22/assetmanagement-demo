/*
  Mô tả: Composable để quản lý logic phân trang, có thể tái sử dụng cho nhiều màn hình
  Features:
  - Quản lý state: currentPage, pageSize
  - Tính toán: totalPages, startIndex, endIndex, visiblePages
  - Methods: goToPage, nextPage, prevPage, setPageSize, reset
  - Hỗ trợ tùy chỉnh pageSize options và maxVisiblePages
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy:
*/

import { ref, computed, watch } from 'vue'

/**
 * Composable để quản lý logic phân trang
 * @param {Object} options - Các tùy chọn cho pagination
 * @param {number|import('vue').ComputedRef<number>} options.total - Tổng số bản ghi
 * @param {number} [options.initialPageSize=20] - Số bản ghi mỗi trang mặc định
 * @param {number[]} [options.pageSizeOptions=[20, 50, 100]] - Các tùy chọn pageSize
 * @param {number} [options.maxVisiblePages=5] - Số trang tối đa hiển thị trong pagination
 * @param {number} [options.initialPage=1] - Trang khởi tạo
 * @returns {Object} - Object chứa state, computed và methods
 */
export function usePagination(options = {}) {
  const {
    total = 0,
    initialPageSize = 20,
    pageSizeOptions = [20, 50, 100],
    maxVisiblePages = 5,
    initialPage = 1
  } = options

  // State
  const currentPage = ref(initialPage)
  const pageSize = ref(initialPageSize)

  // Computed: Tổng số trang
  const totalPages = computed(() => {
    const totalValue = typeof total === 'function' ? total.value : total
    return Math.ceil(totalValue / pageSize.value) || 1
  })

  // Computed: Chỉ số bắt đầu (0-based)
  const startIndex = computed(() => {
    return (currentPage.value - 1) * pageSize.value
  })

  // Computed: Chỉ số kết thúc (0-based, exclusive)
  const endIndex = computed(() => {
    return startIndex.value + pageSize.value
  })

  // Computed: Danh sách các trang hiển thị
  const visiblePages = computed(() => {
    const pages = []
    let startPage = Math.max(1, currentPage.value - Math.floor(maxVisiblePages / 2))
    let endPage = Math.min(totalPages.value, startPage + maxVisiblePages - 1)

    // Điều chỉnh startPage nếu endPage quá gần cuối
    if (endPage - startPage < maxVisiblePages - 1) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1)
    }

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i)
    }

    return pages
  })

  /*
    Mô tả: Chuyển đến trang cụ thể
    
    CreatedBy: DDToan - (11/1/2026)
  */
  const goToPage = (page) => {
    const pageNum = Number(page)
    if (pageNum >= 1 && pageNum <= totalPages.value) {
      currentPage.value = pageNum
    }
  }

  /*
    Mô tả: Chuyển đến trang tiếp theo
    
    CreatedBy: DDToan - (11/1/2026)
  */
  const nextPage = () => {
    if (currentPage.value < totalPages.value) {
      currentPage.value++
    }
  }

  /*
    Mô tả: Chuyển đến trang trước
    
    CreatedBy: DDToan - (11/1/2026)
  */
  const prevPage = () => {
    if (currentPage.value > 1) {
      currentPage.value--
    }
  }

  /*
    Mô tả: Thay đổi số bản ghi mỗi trang
    
    CreatedBy: DDToan - (11/1/2026)
  */
  const setPageSize = (size) => {
    const sizeNum = Number(size)
    if (pageSizeOptions.includes(sizeNum) && sizeNum > 0) {
      pageSize.value = sizeNum
      // Reset về trang 1 khi thay đổi pageSize
      currentPage.value = 1
    }
  }

  /*
    Mô tả: Reset về trang 1
    
    CreatedBy: DDToan - (11/1/2026)
  */
  const reset = () => {
    currentPage.value = initialPage
  }

  // Watch: Khi total thay đổi, đảm bảo currentPage không vượt quá totalPages
  watch(
    [totalPages, currentPage],
    ([newTotalPages, newCurrentPage]) => {
      if (newCurrentPage > newTotalPages && newTotalPages > 0) {
        currentPage.value = newTotalPages
      }
    }
  )

  // Watch: Khi pageSize thay đổi, reset về trang 1 nếu cần
  watch(pageSize, () => {
    if (currentPage.value > totalPages.value && totalPages.value > 0) {
      currentPage.value = totalPages.value
    }
  })

  return {
    // State
    currentPage,
    pageSize,

    // Computed
    totalPages,
    startIndex,
    endIndex,
    visiblePages,

    // Methods
    goToPage,
    nextPage,
    prevPage,
    setPageSize,
    reset,

    // Options
    pageSizeOptions
  }
}
