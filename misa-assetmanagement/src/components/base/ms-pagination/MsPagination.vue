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
    <!-- Left: Total records -->
    <div class="ms-pagination__info">
      Tổng số: <strong>{{ total }} bản ghi</strong>
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
      
      <button 
        v-for="page in visiblePages" 
        :key="page"
        class="ms-pagination__btn"
        :class="{ 'ms-pagination__btn--active': page === currentPage }"
        @click="handleGoToPage(page)"
      >
        {{ page }}
      </button>
      
      <button 
        class="ms-pagination__btn"
        :disabled="currentPage === totalPages"
        @click="handleGoToPage(currentPage + 1)"
      >
        &gt;
      </button>
    </div>

    <!-- Right: Page size selector -->
    <div class="ms-pagination__size">
      <select 
        class="ms-pagination__select" 
        :value="pageSize" 
        @change="handlePageSizeChange($event.target.value)"
      >
        <option v-for="size in pageSizeOptions" :key="size" :value="size">{{ size }}</option>
      </select>
    </div>

    <!-- Totals: Optional slot for totals -->
    <div v-if="$slots.totals" class="ms-pagination__totals">
      <slot name="totals"></slot>
    </div>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsPagination component
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy:
*/

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
  }
})

const emit = defineEmits(['update:currentPage', 'update:pageSize', 'change'])

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
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  background-color: #ffffff;
  border-top: 1px solid #e8e8e8;
}

.ms-pagination__info {
  font-size: 14px;
  color: #595959;
  min-width: 150px;
  white-space: nowrap;
}

.ms-pagination__info strong {
  color: #182026;
  font-weight: 600;
}

.ms-pagination__nav {
  display: flex;
  gap: 4px;
  align-items: center;
}

.ms-pagination__btn {
  min-width: 28px;
  height: 28px;
  padding: 0 6px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  background-color: #ffffff;
  color: #182026;
  font-size: 14px;
  cursor: pointer;
  transition: all var(--transition-fast);
  display: flex;
  align-items: center;
  justify-content: center;
}

.ms-pagination__btn:hover:not(:disabled) {
  border-color: #0095da;
  color: #0095da;
}

.ms-pagination__btn--active {
  background-color: #0095da;
  border-color: #0095da;
  color: #ffffff;
  font-weight: 400;
}

.ms-pagination__btn:disabled {
  opacity: 0.3;
  cursor: not-allowed;
  color: #bfbfbf;
}

.ms-pagination__size {
  display: flex;
  align-items: center;
  gap: var(--spacing-sm);
}

.ms-pagination__select {
  height: 28px;
  padding: 0 24px 0 8px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  font-size: 14px;
  cursor: pointer;
  background: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="10" height="6"><path d="M1 1l4 4 4-4" stroke="%23999" fill="none" stroke-width="1.2"/></svg>') no-repeat right 8px center;
  background-color: #ffffff;
  appearance: none;
  color: #182026;
}

.ms-pagination__totals {
  display: flex;
  gap: 16px;
  align-items: center;
  font-size: 14px;
  font-weight: 700;
  color: #182026;
  margin-left: auto;
}
</style>
