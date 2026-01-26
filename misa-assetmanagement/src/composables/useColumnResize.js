/*
  Mô tả: Composable để quản lý logic resize độ rộng cột trong bảng
  Features:
  - Quản lý state độ rộng các cột
  - Xử lý logic resize (mousedown, mousemove, mouseup)
  - Lưu/load từ localStorage
  - Tính toán độ rộng động cho các cột
  
  CreatedBy: DDTOAN - (18/1/2026)
  
  EditBy: 
*/

import { ref, onMounted, onUnmounted } from 'vue'

/**
 * Composable để quản lý logic resize độ rộng cột
 * @param {Object} defaultWidths - Object chứa độ rộng mặc định của các cột
 * @param {string} [storageKey='column-widths'] - Key để lưu vào localStorage
 * @returns {Object} - Object chứa state, computed và methods
 */
export function useColumnResize(defaultWidths = {}, storageKey = 'column-widths') {
  // State: Độ rộng các cột
  const columnWidths = ref({ ...defaultWidths })

  // State: Đang resize hay không
  const isResizing = ref(false)

  // State: Thông tin resize hiện tại
  const resizeState = ref({
    columnKey: null,
    startX: 0,
    startWidth: 0
  })

  /**
   * Load độ rộng từ localStorage
   * CreatedBy: DDTOAN - (18/1/2026)
   */
  const loadColumnWidths = () => {
    try {
      const stored = localStorage.getItem(storageKey)
      if (stored) {
        const parsed = JSON.parse(stored)
        // Merge với defaultWidths để đảm bảo có đủ tất cả các cột
        columnWidths.value = {
          ...defaultWidths,
          ...parsed
        }
        // Validate: đảm bảo độ rộng không nhỏ hơn min-width (nếu có)
        Object.keys(columnWidths.value).forEach((key) => {
          if (defaultWidths[key] && columnWidths.value[key] < defaultWidths[key]) {
            columnWidths.value[key] = defaultWidths[key]
          }
        })
      }
    } catch (error) {
      console.warn('Failed to load column widths from localStorage:', error)
      // Fallback về default widths
      columnWidths.value = { ...defaultWidths }
    }
  }

  /**
   * Lưu độ rộng vào localStorage
   * CreatedBy: DDTOAN - (18/1/2026)
   */
  const saveColumnWidths = () => {
    try {
      localStorage.setItem(storageKey, JSON.stringify(columnWidths.value))
    } catch (error) {
      console.warn('Failed to save column widths to localStorage:', error)
    }
  }

  /**
   * Bắt đầu resize
   * @param {string} columnKey - Key của cột cần resize
   * @param {MouseEvent} event - Mouse event
   * CreatedBy: DDTOAN - (18/1/2026)
   */
  const startResize = (columnKey, event) => {
    if (!columnWidths.value[columnKey]) {
      console.warn(`Column key "${columnKey}" not found in columnWidths`)
      return
    }

    isResizing.value = true
    resizeState.value = {
      columnKey,
      startX: event.clientX,
      startWidth: columnWidths.value[columnKey]
    }

    // Prevent text selection while resizing
    document.body.style.userSelect = 'none'
    document.body.style.cursor = 'col-resize'
  }

  /**
   * Xử lý khi đang kéo để resize
   * @param {MouseEvent} event - Mouse event
   * CreatedBy: DDTOAN - (18/1/2026)
   */
  const handleResize = (event) => {
    if (!isResizing.value || !resizeState.value.columnKey) {
      return
    }

    const { columnKey, startX, startWidth } = resizeState.value
    const diff = event.clientX - startX
    const newWidth = startWidth + diff

    // Lấy min-width từ defaultWidths (nếu có)
    const minWidth = defaultWidths[columnKey] || 50

    // Đảm bảo không nhỏ hơn min-width
    if (newWidth >= minWidth) {
      columnWidths.value[columnKey] = Math.round(newWidth)
    } else {
      columnWidths.value[columnKey] = minWidth
    }
  }

  /**
   * Kết thúc resize và lưu
   * CreatedBy: DDTOAN - (18/1/2026)
   */
  const endResize = () => {
    if (isResizing.value) {
      isResizing.value = false
      saveColumnWidths()
      resizeState.value = {
        columnKey: null,
        startX: 0,
        startWidth: 0
      }

      // Restore text selection
      document.body.style.userSelect = ''
      document.body.style.cursor = ''
    }
  }

  /**
   * Reset về độ rộng mặc định
   * CreatedBy: DDTOAN - (18/1/2026)
   */
  const resetColumnWidths = () => {
    columnWidths.value = { ...defaultWidths }
    saveColumnWidths()
  }

  // Load độ rộng khi mounted
  onMounted(() => {
    loadColumnWidths()

    // Thêm event listeners cho resize
    document.addEventListener('mousemove', handleResize)
    document.addEventListener('mouseup', endResize)
  })

  // Cleanup khi unmounted
  onUnmounted(() => {
    document.removeEventListener('mousemove', handleResize)
    document.removeEventListener('mouseup', endResize)
    // Restore text selection nếu đang resize
    if (isResizing.value) {
      document.body.style.userSelect = ''
      document.body.style.cursor = ''
    }
  })

  return {
    // State
    columnWidths,
    isResizing,
    resizeState,

    // Methods
    startResize,
    handleResize,
    endResize,
    resetColumnWidths,
    loadColumnWidths,
    saveColumnWidths
  }
}
