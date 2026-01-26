/*
  Mô tả: Composable để quản lý keyboard navigation (↑↓) cho table
  Features:
  - Di chuyển lên/xuống bằng phím mũi tên trong trang hiện tại
  - Enter/Space để chọn row
  - Tự động scroll khi di chuyển
  - Tái sử dụng cho nhiều màn hình
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
*/

import { ref, onMounted, onUnmounted, nextTick } from 'vue'

/**
 * Composable để quản lý keyboard navigation
 * @param {Object} options - Các tùy chọn
 * @param {import('vue').Ref<Array>|import('vue').ComputedRef<Array>} options.items - Danh sách items trong trang hiện tại (ref/computed)
 * @param {Function} options.onSelect - Callback khi chọn row (Enter/Space)
 * @param {Function} options.getItemId - Function để lấy ID của item
 * @returns {Object} - Object chứa state và methods
 */
export function useKeyboardNavigation(options = {}) {
  const {
    items = ref([]),
    onSelect = () => { },
    getItemId = (item) => item.id,
  } = options

  // State
  const focusedRowIndex = ref(-1)
  const isEnabled = ref(true)

  /**
   * Xử lý keydown event
   * @param {KeyboardEvent} event - Keyboard event
   */
  const handleKeyDown = (event) => {
    if (!isEnabled.value || items.value.length === 0) return

    // Chỉ xử lý khi đang focus vào table, không phải input/textarea
    const target = event.target
    if (
      target.tagName === 'INPUT' ||
      target.tagName === 'TEXTAREA' ||
      target.isContentEditable
    ) {
      return
    }

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault()
        moveDown()
        break
      case 'ArrowUp':
        event.preventDefault()
        moveUp()
        break
      case 'Enter':
      case ' ':
        event.preventDefault()
        if (focusedRowIndex.value >= 0 && focusedRowIndex.value < items.value.length) {
          const item = items.value[focusedRowIndex.value]
          onSelect(item, focusedRowIndex.value)
        }
        break
      case 'Home':
        event.preventDefault()
        focusRow(0)
        break
      case 'End':
        event.preventDefault()
        focusRow(items.value.length - 1)
        break
    }
  }

  /**
   * Di chuyển xuống
   */
  const moveDown = () => {
    if (focusedRowIndex.value < items.value.length - 1) {
      focusRow(focusedRowIndex.value + 1)
    } else if (focusedRowIndex.value === -1) {
      // Nếu chưa focus, focus vào row đầu tiên
      focusRow(0)
    }
  }

  /**
   * Di chuyển lên
   */
  const moveUp = () => {
    if (focusedRowIndex.value > 0) {
      focusRow(focusedRowIndex.value - 1)
    } else if (focusedRowIndex.value === -1) {
      // Nếu chưa focus, focus vào row cuối
      focusRow(items.value.length - 1)
    }
  }

  /**
   * Focus vào row cụ thể
   * @param {number} index - Index của row
   */
  const focusRow = (index) => {
    if (index < 0 || index >= items.value.length) return

    focusedRowIndex.value = index
    scrollToRow(index)
  }

  /**
   * Scroll đến row được focus
   * @param {number} index - Index của row
   */
  const scrollToRow = (index) => {
    nextTick(() => {
      const rowElement = document.querySelector(
        `[data-row-index="${index}"]`
      )
      if (rowElement) {
        rowElement.scrollIntoView({
          behavior: 'smooth',
          block: 'nearest',
        })
      }
    })
  }

  /**
   * Reset focus
   */
  const resetFocus = () => {
    focusedRowIndex.value = -1
  }

  /**
   * Enable/Disable keyboard navigation
   * @param {boolean} enabled - Trạng thái enable
   */
  const setEnabled = (enabled) => {
    isEnabled.value = enabled
    if (!enabled) {
      resetFocus()
    }
  }

  // Mount/Unmount lắng nghe sự kiện
  onMounted(() => {
    document.addEventListener('keydown', handleKeyDown)
  })

  onUnmounted(() => {
    document.removeEventListener('keydown', handleKeyDown)
  })

  return {
    // State
    focusedRowIndex,
    isEnabled,

    // Methods
    handleKeyDown,
    moveDown,
    moveUp,
    focusRow,
    resetFocus,
    setEnabled,
  }
}
