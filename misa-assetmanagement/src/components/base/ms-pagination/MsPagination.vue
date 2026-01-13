<!--
  Mô tả: Pagination component để hiển thị và điều khiển phân trang
  Features:
  - Hiển thị tổng số bản ghi
  - Điều hướng trang (prev, next, go to page)
  - Chọn số bản ghi mỗi trang
  - Hiển thị totals (optional)
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy:
-->

<template>
  <div class="ms-pagination">
    <!-- Left: Total records + Page size selector -->
    <div class="ms-pagination__left">
      <div class="ms-pagination__info">
        Tổng số: <strong>{{ total }} bản ghi</strong>
      </div>
      <div class="ms-pagination__size">
        <MsDropdown
          v-model="pageSizeModel"
          :options="pageSizeDropdownOptions"
          class="ms-pagination__dropdown"
        />
      </div>
    </div>

    <!-- Center: Page navigation -->
    <div class="ms-pagination__nav">
      <button 
        class="ms-pagination__btn" 
        :disabled="currentPage === 1"
        @click="handleGoToPage(currentPage - 1)"
      >
        &lt;
      </button>
      
      <template v-for="page in displayPages" :key="page">
        <button 
          v-if="page === 'ellipsis-start' || page === 'ellipsis-end'"
          class="ms-pagination__btn ms-pagination__btn--ellipsis"
          disabled
        >
          ...
        </button>
        <button 
          v-else
          class="ms-pagination__btn"
          :class="{ 'ms-pagination__btn--active': page === currentPage }"
          @click="handleGoToPage(page)"
        >
          {{ page }}
        </button>
      </template>
      
      <button 
        class="ms-pagination__btn"
        :disabled="currentPage === totalPages"
        @click="handleGoToPage(currentPage + 1)"
      >
        &gt;
      </button>
    </div>

    <!-- Right: Totals - Table structure to align with columns -->
    <div v-if="totals" class="ms-pagination__totals-wrapper">
      <table class="ms-pagination__totals-table">
        <tr>
          <td class="ms-pagination__totals-spacer ms-pagination__totals-spacer--checkbox"></td>
          <td class="ms-pagination__totals-spacer ms-pagination__totals-spacer--stt"></td>
          <td class="ms-pagination__totals-spacer"></td>
          <td class="ms-pagination__totals-spacer"></td>
          <td class="ms-pagination__totals-spacer"></td>
          <td class="ms-pagination__totals-spacer"></td>
          <td class="ms-pagination__totals-cell ms-pagination__totals-cell--number">
            <span class="ms-pagination__total-item">{{ totals.quantity }}</span>
          </td>
          <td class="ms-pagination__totals-cell ms-pagination__totals-cell--number">
            <span class="ms-pagination__total-item">{{ formatNumber(totals.cost) }}</span>
          </td>
          <td class="ms-pagination__totals-cell ms-pagination__totals-cell--number">
            <span class="ms-pagination__total-item">{{ formatNumber(totals.depreciation) }}</span>
          </td>
          <td class="ms-pagination__totals-cell ms-pagination__totals-cell--number">
            <span class="ms-pagination__total-item">{{ formatNumber(totals.remainingValue) }}</span>
          </td>
          <td class="ms-pagination__totals-spacer ms-pagination__totals-spacer--actions"></td>
        </tr>
      </table>
    </div>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsPagination component
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy:
*/

import { computed, ref, watch } from 'vue'
import MsDropdown from '@/components/base/ms-dropdown/MsDropdown.vue'

// Format number helper
const formatNumber = (num) => {
  return num.toLocaleString('vi-VN')
}

const props = defineProps({
  // Tổng số bản ghi
  total: {
    type: Number,
    required: true,
    default: 0
  },
  // Trang hiện tại
  currentPage: {
    type: Number,
    required: true,
    default: 1
  },
  // Số bản ghi mỗi trang
  pageSize: {
    type: Number,
    required: true,
    default: 20
  },
  // Tổng số trang
  totalPages: {
    type: Number,
    required: true,
    default: 1
  },
  // Danh sách các trang hiển thị
  visiblePages: {
    type: Array,
    required: true,
    default: () => []
  },
  // Các tùy chọn pageSize
  pageSizeOptions: {
    type: Array,
    default: () => [20, 50, 100]
  },
  // Totals values
  totals: {
    type: Object,
    default: () => ({
      quantity: 0,
      cost: 0,
      depreciation: 0,
      remainingValue: 0
    })
  }
})

const emit = defineEmits(['update:currentPage', 'update:pageSize', 'change'])

// Computed: Chuyển đổi pageSizeOptions thành format cho dropdown
const pageSizeDropdownOptions = computed(() => {
  return props.pageSizeOptions.map(size => ({
    value: size,
    label: String(size)
  }))
})

// Model cho dropdown
const pageSizeModel = ref(props.pageSize)

// Watch pageSize từ props để sync với model
watch(() => props.pageSize, (newVal) => {
  pageSizeModel.value = newVal
})

// Watch pageSizeModel để emit khi thay đổi
watch(pageSizeModel, (newVal) => {
  if (newVal !== props.pageSize) {
    handlePageSizeChange(newVal)
  }
})

// Computed: Tính toán các trang cần hiển thị với ellipsis
const displayPages = computed(() => {
  const pages = []
  const { currentPage, totalPages } = props
  
  if (totalPages <= 1) {
    return [1]
  }
  
  // Nếu chỉ có ít trang (<= 5), hiển thị tất cả
  if (totalPages <= 5) {
    for (let i = 1; i <= totalPages; i++) {
      pages.push(i)
    }
    return pages
  }
  
  // Luôn hiển thị trang đầu
  pages.push(1)
  
  // Nếu currentPage gần đầu (trang 1, 2, 3)
  if (currentPage <= 3) {
    // Hiển thị: 1, 2, 3, ..., 10
    if (currentPage === 1) {
      pages.push(2)
    } else if (currentPage === 2) {
      pages.push(2, 3)
    } else {
      pages.push(2, 3, 4)
    }
    if (totalPages > 4) {
      pages.push('ellipsis-end')
    }
    pages.push(totalPages)
  }
  // Nếu currentPage gần cuối
  else if (currentPage >= totalPages - 2) {
    // Hiển thị: 1, ..., 8, 9, 10
    pages.push('ellipsis-start')
    if (currentPage === totalPages) {
      pages.push(totalPages - 1, totalPages)
    } else if (currentPage === totalPages - 1) {
      pages.push(totalPages - 2, totalPages - 1, totalPages)
    } else {
      pages.push(totalPages - 3, totalPages - 2, totalPages - 1, totalPages)
    }
  }
  // Nếu currentPage ở giữa
  else {
    // Hiển thị: 1, ..., current-1, current, current+1, ..., 10
    pages.push('ellipsis-start')
    pages.push(currentPage - 1, currentPage, currentPage + 1)
    pages.push('ellipsis-end')
    pages.push(totalPages)
  }
  
  return pages
})

/*
  Mô tả: Xử lý chuyển trang
  
  CreatedBy: DDToan - (11/1/2026)
*/
const handleGoToPage = (page) => {
  if (page >= 1 && page <= props.totalPages && page !== props.currentPage) {
    emit('update:currentPage', page)
    emit('change', { page, pageSize: props.pageSize })
  }
}

/*
  Mô tả: Xử lý thay đổi pageSize
  
  CreatedBy: DDToan - (11/1/2026)
*/
const handlePageSizeChange = (size) => {
  const sizeNum = Number(size)
  if (props.pageSizeOptions.includes(sizeNum) && sizeNum > 0) {
    emit('update:pageSize', sizeNum)
    emit('change', { page: 1, pageSize: sizeNum })
  }
}
</script>

<style scoped>
/*
  Mô tả: Styles cho MsPagination component
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy:
*/

.ms-pagination {
  display: flex;
  align-items: center;
  padding: 16px;
  background-color: #ffffff;
  border-top: 1px solid #e8e8e8;
  gap: 0;
}

.ms-pagination__left {
  display: flex;
  align-items: center;
  gap: 0;
  flex-shrink: 0;
}

.ms-pagination__info {
  font-size: 14px;
  color: #595959;
  white-space: nowrap;
  margin-right: 8px;
}

.ms-pagination__info strong {
  color: #182026;
  font-weight: 600;
}

.ms-pagination__nav {
  display: flex;
  gap: 4px;
  align-items: center;
  flex-shrink: 0;
  margin-left: 16px;
}

.ms-pagination__btn {
  min-width: 28px;
  height: 28px;
  padding: 0 6px;
  border: none;
  border-radius: 2px;
  background-color: transparent;
  color: #182026;
  font-size: 14px;
  cursor: pointer;
  transition: all var(--transition-fast);
  display: flex;
  align-items: center;
  justify-content: center;
}

.ms-pagination__btn:hover:not(:disabled) {
  color: #0095da;
}

.ms-pagination__btn--active {
  background-color: #e6e6e6;
  border: none;
  color: #182026;
  font-weight: 400;
}

.ms-pagination__btn:disabled {
  opacity: 0.3;
  cursor: not-allowed;
  color: #bfbfbf;
}

.ms-pagination__btn--ellipsis {
  border: none;
  background: transparent;
  cursor: default;
  opacity: 1;
  color: #182026;
  min-width: auto;
  padding: 0 4px;
}

.ms-pagination__btn--ellipsis:hover {
  border: none;
  color: #182026;
}

.ms-pagination__size {
  display: flex;
  align-items: center;
}

.ms-pagination__dropdown {
  width: auto;
  min-width: 60px;
}

.ms-pagination__dropdown :deep(.ms-dropdown__trigger) {
  height: 28px;
  padding: 0 24px 0 8px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  background-color: #ffffff;
  font-size: 14px;
  color: #182026;
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 59px;
  margin-left: 15px;
}

.ms-pagination__dropdown :deep(.ms-dropdown__arrow) {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  width: 8px;
  height: 5px;
  font-size: 0;
  -webkit-mask-image: url('@/assets/icons/qlts-icon.svg');
  mask-image: url('@/assets/icons/qlts-icon.svg');
  -webkit-mask-size: 504px 754px;
  mask-size: 504px 754px;
  -webkit-mask-repeat: no-repeat;
  mask-repeat: no-repeat;
  -webkit-mask-position: -72px -338px; /* icon-chevron-down-toolbar */
  mask-position: -72px -338px;
  background-color: #595959;
}

.ms-pagination__dropdown :deep(.ms-dropdown--open .ms-dropdown__arrow) {
  -webkit-mask-position: -28px -338px !important; /* icon-chevron-up-toolbar */
  mask-position: -28px -338px !important;
}

/* Dropdown menu mở lên trên */
.ms-pagination__dropdown :deep(.ms-dropdown__menu) {
  top: auto !important;
  bottom: calc(100% + 4px) !important;
}

.ms-pagination__totals-wrapper {
  flex: 1;
  margin-left: -16px;
  margin-right: -16px;
  overflow: visible;
  display: block;
}

.ms-pagination__totals-table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  table-layout: auto;
}

.ms-pagination__totals-spacer {
  padding: 0 12px;
  height: 0;
  border: none;
  width: auto;
  min-width: 0;
}

.ms-pagination__totals-spacer--checkbox {
  width: 48px;
  min-width: 48px;
  max-width: 48px;
  padding: 0 8px;
}

.ms-pagination__totals-spacer--stt {
  width: 64px;
  min-width: 64px;
  max-width: 64px;
  padding: 0 12px;
}

.ms-pagination__totals-spacer--actions {
  width: 96px;
  min-width: 96px;
  max-width: 96px;
  padding: 0 8px;
}

.ms-pagination__totals-cell {
  width: auto;
  padding: 0;
  height: 100%;
  border: none;
  vertical-align: middle;
}

.ms-pagination__totals-cell--number {
  text-align: right;
  padding-right: 16px;
  padding-left: 12px;
  white-space: nowrap;
}

.ms-pagination__total-item {
  font-family: 'Roboto', sans-serif;
  font-variant-numeric: tabular-nums;
  font-size: 14px;
  font-weight: 700;
  color: #182026;
  white-space: nowrap;
}
</style>
