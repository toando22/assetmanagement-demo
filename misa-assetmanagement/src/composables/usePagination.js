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

import { ref, computed, watch, unref } from 'vue'

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
    // Sử dụng unref để lấy giá trị từ ref hoặc computed
    const totalValue = unref(total)
    const result = Math.ceil(totalValue / pageSize.value) || 1
    return result
  })

  // Computed: Chỉ số bắt đầu (0-based)
  const startIndex = computed(() => {
    return (currentPage.value - 1) * pageSize.value
  })

  // Computed: Chỉ số kết thúc (0-based, exclusive)
  const endIndex = computed(() => {
    return startIndex.value + pageSize.value
  })

  // Computed: Danh sách các trang hiển thị với ellipsis
  const visiblePages = computed(() => {
    const pages = []
    const totalPagesValue = totalPages.value
    const current = currentPage.value
    
    if (totalPagesValue <= 1) {
      return [1]
    }
    
    // Nếu chỉ có ít trang (<= maxVisiblePages), hiển thị tất cả
    if (totalPagesValue <= maxVisiblePages) {
      for (let i = 1; i <= totalPagesValue; i++) {
        pages.push(i)
      }
      return pages
    }
    
    // Luôn hiển thị trang đầu
    pages.push(1)
    
    // Nếu currentPage gần đầu (trang 1, 2, 3)
    if (current <= 3) {
      // Hiển thị: 1, 2, 3, ..., totalPages
      if (current === 1) {
        if (totalPagesValue > 1) {
          pages.push(2)
        }
      } else if (current === 2) {
        pages.push(2, 3)
      } else {
        // current === 3
        pages.push(2, 3, 4)
      }
      // Thêm ellipsis và trang cuối nếu cần
      if (totalPagesValue > 4) {
        pages.push('ellipsis-end')
      }
      // Luôn hiển thị trang cuối
      pages.push(totalPagesValue)
    }
    // Nếu currentPage gần cuối (>= totalPagesValue - 2)
    else if (current >= totalPagesValue - 2) {
      // Hiển thị: 1, ..., n-2, n-1, n (với n = totalPagesValue)
      pages.push('ellipsis-start')
      if (current === totalPagesValue) {
        // Ở trang cuối: hiển thị n-1, n
        pages.push(totalPagesValue - 1, totalPagesValue)
      } else if (current === totalPagesValue - 1) {
        // Ở trang n-1: hiển thị n-2, n-1, n
        pages.push(totalPagesValue - 2, totalPagesValue - 1, totalPagesValue)
      } else {
        // current === totalPagesValue - 2: hiển thị n-3, n-2, n-1, n
        pages.push(totalPagesValue - 3, totalPagesValue - 2, totalPagesValue - 1, totalPagesValue)
      }
    }
    // Nếu currentPage ở giữa
    else {
      // Hiển thị: 1, ..., current-1, current, current+1, ..., totalPages
      pages.push('ellipsis-start')
      pages.push(current - 1, current, current + 1)
      pages.push('ellipsis-end')
      // Luôn hiển thị trang cuối
      pages.push(totalPagesValue)
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

  // Watch: Khi totalPages thay đổi, đảm bảo currentPage không vượt quá totalPages
  watch(
    totalPages,
    (newTotalPages) => {
      if (newTotalPages > 0 && currentPage.value > newTotalPages) {
        currentPage.value = newTotalPages
      } else if (newTotalPages === 0) {
        // Nếu không còn trang nào, reset về trang 1
        currentPage.value = 1
      }
    },
    { immediate: true }
  )

  // Watch: Khi pageSize thay đổi, đảm bảo currentPage không vượt quá totalPages
  watch(
    pageSize,
    () => {
      // setPageSize đã reset về trang 1, nhưng kiểm tra lại để đảm bảo
      if (totalPages.value > 0 && currentPage.value > totalPages.value) {
        currentPage.value = totalPages.value
      }
    }
  )

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
