<!--
  Mô tả: View hiển thị danh sách tài sản
  Features:
  - Search và filter
  - Table danh sách tài sản
  - Pagination
  - CRUD actions
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
-->

<template>
  <div class="asset-list">
    <div class="asset-list__container">
      <!-- Toolbar: Search, Filters, Actions -->
      <div class="asset-list__toolbar">
      <!-- Left: Search & Filters -->
      <div class="toolbar__left">
        <!-- Search -->
        <div class="toolbar__search">
          <i class="icon icon-search-toolbar"></i>
          <input 
            type="text" 
            class="toolbar__search-input" 
            placeholder="Tìm kiếm tài sản"
            v-model="searchText"
          />
        </div>

        <!-- Filter: Loại tài sản -->
        <div class="toolbar__filter">
          <i class="icon icon-filter"></i>
          <MsDropdown
            ref="assetTypeDropdownRef"
            v-model="selectedAssetType"
            :options="assetTypeOptions"
            placeholder="Loại tài sản"
            class="toolbar__dropdown toolbar__dropdown--asset-type"
          />
        </div>

        <!-- Filter: Bộ phận sử dụng -->
        <div class="toolbar__filter">
          <i class="icon icon-filter"></i>
          <MsDropdown
            ref="departmentDropdownRef"
            v-model="selectedDepartment"
            :options="departmentOptions"
            placeholder="Bộ phận sử dụng"
            class="toolbar__dropdown toolbar__dropdown--department"
          />
        </div>
        
        <!-- Hidden element để đo width của item dài nhất -->
        <div ref="measureRef" class="toolbar__measure" style="visibility: hidden; position: absolute; white-space: nowrap; font-size: 14px; padding: 0 12px;"></div>
      </div>

      <!-- Right: Action Buttons -->
      <div class="toolbar__right">
        <!-- Button: Thêm tài sản -->
        <button class="btn btn--primary" @click="handleAddAsset">
          <i class="icon icon-add"></i>
          Thêm tài sản
        </button>

        <!-- Button: Excel -->
        <button class="btn btn--outline" title="Xuất Excel">
          <i class="icon icon-excel"></i>
        </button>

        <!-- Button: Delete -->
        <button class="btn btn--outline btn--danger" title="Xóa" :disabled="selectedAssets.length === 0">
          <i class="icon icon-delete"></i>
        </button>
      </div>
    </div>

    <!-- Table -->
    <div class="asset-list__table-wrapper">
      <table class="asset-table">
        <thead class="asset-table__head">
          <tr>
            <th class="asset-table__th asset-table__th--checkbox">
              <label class="asset-table__checkbox-wrapper">
                <input type="checkbox" v-model="selectAll" @change="toggleSelectAll" />
                <span class="asset-table__checkbox-check"></span>
              </label>
            </th>
            <th class="asset-table__th asset-table__th--stt">STT</th>
            <th class="asset-table__th">Mã tài sản</th>
            <th class="asset-table__th">Tên tài sản</th>
            <th class="asset-table__th">Loại tài sản</th>
            <th class="asset-table__th">Bộ phận sử dụng</th>
            <th class="asset-table__th asset-table__th--number">Số lượng</th>
            <th class="asset-table__th asset-table__th--number">Nguyên giá</th>
            <th class="asset-table__th asset-table__th--number">HM/KH lũy kế</th>
            <th class="asset-table__th asset-table__th--number">Giá trị còn lại</th>
            <th class="asset-table__th asset-table__th--actions">Chức năng</th>
          </tr>
        </thead>
        <tbody class="asset-table__body">
          <tr 
            v-for="(asset, index) in paginatedAssets" 
            :key="asset.id"
            class="asset-table__row"
            :class="{ 'asset-table__row--selected': selectedAssets.includes(asset.id) }"
          >
            <td class="asset-table__td asset-table__td--checkbox">
              <label class="asset-table__checkbox-wrapper">
                <input type="checkbox" :value="asset.id" v-model="selectedAssets" />
                <span class="asset-table__checkbox-check"></span>
              </label>
            </td>
            <td class="asset-table__td asset-table__td--stt">{{ (currentPage - 1) * pageSize + index + 1 }}</td>
            <td class="asset-table__td">{{ asset.code }}</td>
            <td class="asset-table__td">{{ asset.name }}</td>
            <td class="asset-table__td">{{ asset.type }}</td>
            <td class="asset-table__td">{{ asset.department }}</td>
            <td class="asset-table__td asset-table__td--number">{{ asset.quantity }}</td>
            <td class="asset-table__td asset-table__td--number">{{ formatNumber(asset.cost) }}</td>
            <td class="asset-table__td asset-table__td--number">{{ formatNumber(asset.depreciation) }}</td>
            <td class="asset-table__td asset-table__td--number">{{ formatNumber(asset.remainingValue) }}</td>
            <td class="asset-table__td asset-table__td--actions">
              <button class="action-btn" title="Sửa" @click="handleEdit(asset)">
                <i class="icon icon-edit"></i>
              </button>
              <button class="action-btn" title="Nhân bản" @click="handleDuplicate(asset)">
                <i class="icon icon-duplicate"></i>
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination -->
    <MsPagination
      :total="totalRecords"
      :current-page="currentPage"
      :page-size="pageSize"
      :total-pages="totalPages"
      :visible-pages="visiblePages"
      :page-size-options="pageSizeOptions"
      @update:current-page="goToPage"
      @update:page-size="setPageSize"
    >
      <template #totals>
        <span class="pagination__total-item">{{ selectedTotals.quantity }}</span>
        <span class="pagination__total-item">{{ formatNumber(selectedTotals.cost) }}</span>
        <span class="pagination__total-item">{{ formatNumber(selectedTotals.depreciation) }}</span>
        <span class="pagination__total-item">{{ formatNumber(selectedTotals.remainingValue) }}</span>
      </template>
    </MsPagination>

    <!-- Asset Form Modal -->
    <AssetForm
      v-model="isFormOpen"
      :title="formTitle"
      :asset-data="selectedAssetData"
      :existing-assets="assets"
      @save="handleSaveAsset"
      @cancel="handleCancelForm"
    />
    </div>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho AssetList view
  Features:
  - Search và filter
  - Table danh sách tài sản
  - Pagination (sử dụng usePagination composable)
  - CRUD actions
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: DDToan - (11/1/2026) - Refactor pagination logic sang composable
*/

import { ref, computed, onMounted, nextTick, watch } from 'vue'
import { MOCK_ASSETS } from '@/constants/assetData'
import AssetForm from './AssetForm.vue'
import MsDropdown from '@/components/base/ms-dropdown/MsDropdown.vue'
import MsPagination from '@/components/base/ms-pagination/MsPagination.vue'
import { usePagination } from '@/composables/usePagination'

// State
const searchText = ref('')
const selectedAssetType = ref('')
const selectedDepartment = ref('')
const selectedAssets = ref([])
const selectAll = ref(false)
const isFormOpen = ref(false)
const selectedAssetData = ref(null)
const formTitle = ref('Thêm tài sản')

// Mock data
const assets = ref(MOCK_ASSETS)

// Dropdown options
const departmentOptions = ref([
  { value: 'ke-toan', label: 'Phòng Hành chính Kế toán' },
  { value: 'thu-ky', label: 'Phòng Thư ký' }
])

const assetTypeOptions = ref([
  { value: 'may-tinh', label: 'Máy tính' },
  { value: 'thiet-bi', label: 'Thiết bị' }
])

// Refs
const assetTypeDropdownRef = ref(null)
const departmentDropdownRef = ref(null)
const measureRef = ref(null)

// Tính toán width của item dài nhất
const calculateMaxWidth = (options) => {
  if (!measureRef.value || !options || options.length === 0) return null
  
  let maxWidth = 0
  options.forEach(option => {
    measureRef.value.textContent = option.label
    const width = measureRef.value.offsetWidth
    if (width > maxWidth) {
      maxWidth = width
    }
  })
  
  // Cộng thêm padding và icon space: 
  // - 38px (left padding cho icon filter) 
  // - 32px (right padding cho icon chevron)
  // - 12px x 2 (item padding) = 24px
  // Tổng: 38 + 32 + 24 = 94px
  return maxWidth + 94
}

// Set min-width cho dropdown trigger
const setDropdownWidths = async () => {
  await nextTick()
  
  if (assetTypeOptions.value.length > 0) {
    const maxWidth = calculateMaxWidth(assetTypeOptions.value)
    if (maxWidth) {
      document.documentElement.style.setProperty('--asset-type-dropdown-width', `${maxWidth}px`)
    }
  }
  
  if (departmentOptions.value.length > 0) {
    const maxWidth = calculateMaxWidth(departmentOptions.value)
    if (maxWidth) {
      document.documentElement.style.setProperty('--department-dropdown-width', `${maxWidth}px`)
    }
  }
}

// Watch options để tính lại width khi thay đổi
watch([departmentOptions, assetTypeOptions], () => {
  setDropdownWidths()
}, { deep: true })

onMounted(() => {
  setDropdownWidths()
})

// Computed: Tổng số bản ghi
const totalRecords = computed(() => assets.value.length)

// Sử dụng composable usePagination
const {
  currentPage,
  pageSize,
  totalPages,
  startIndex,
  endIndex,
  visiblePages,
  goToPage,
  setPageSize,
  pageSizeOptions
} = usePagination({
  total: totalRecords,
  initialPageSize: 20,
  pageSizeOptions: [20, 50, 100],
  maxVisiblePages: 5
})

// Computed: Danh sách tài sản đã phân trang
const paginatedAssets = computed(() => {
  return assets.value.slice(startIndex.value, endIndex.value)
})

const selectedTotals = computed(() => {
  const selected = assets.value.filter(asset => selectedAssets.value.includes(asset.id))
  return {
    quantity: selected.reduce((sum, asset) => sum + asset.quantity, 0),
    cost: selected.reduce((sum, asset) => sum + asset.cost, 0),
    depreciation: selected.reduce((sum, asset) => sum + asset.depreciation, 0),
    remainingValue: selected.reduce((sum, asset) => sum + asset.remainingValue, 0)
  }
})

// Methods
const toggleSelectAll = () => {
  if (selectAll.value) {
    selectedAssets.value = paginatedAssets.value.map(a => a.id)
  } else {
    selectedAssets.value = []
  }
}

const formatNumber = (num) => {
  return num.toLocaleString('vi-VN')
}

const handleAddAsset = () => {
  selectedAssetData.value = null
  formTitle.value = 'Thêm tài sản'
  isFormOpen.value = true
}

const handleEdit = (asset) => {
  selectedAssetData.value = { ...asset }
  formTitle.value = 'Sửa tài sản'
  isFormOpen.value = true
}

const handleDuplicate = (asset) => {
  selectedAssetData.value = { ...asset, id: null, code: '' }
  formTitle.value = 'Thêm tài sản'
  isFormOpen.value = true
}

const handleSaveAsset = (assetData) => {
  console.log('Save asset:', assetData)
  // TODO: Implement save logic (API call)
  // For now, just add to mock data
  if (!assetData.id) {
    // Add new asset
    const newAsset = {
      id: Date.now(),
      ...assetData
    }
    assets.value.push(newAsset)
  } else {
    // Update existing asset
    const index = assets.value.findIndex(a => a.id === assetData.id)
    if (index > -1) {
      assets.value[index] = { ...assetData }
    }
  }
}

const handleCancelForm = () => {
  selectedAssetData.value = null
}
</script>

<style scoped>
/*
  Mô tả: Styles cho AssetList view
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

.asset-list {
  display: flex;
  flex-direction: column;
  height: 100%;
  margin: 0 auto;
  padding: 16px;
  background-color: #f0f2f5;
  box-sizing: border-box;
  font-family: sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  line-height: 18px;
}

.asset-list__container {
  display: flex;
  flex-direction: column;
  height: 100%;
  background-color: #ffffff;
  border-radius: 4px;
  border: 1px solid #e8e8e8;
  box-shadow: 0 1px 4px rgba(0, 0, 0, 0.08);
  overflow: hidden;
}

/* ============================================ */
/* TOOLBAR                                      */
/* ============================================ */

.asset-list__toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  gap: 12px;
  padding: 16px;
  /* background-color: transparent; */
  border-bottom: 1px solid #e8e8e8;
}

.toolbar__left {
  display: flex;
  gap: 8px;
  flex: 1;
}

.toolbar__right {
  display: flex;
  gap: 8px;
}

/* Search */
.toolbar__search {
  position: relative;
  width: 240px;
}

.toolbar__search .icon {
  position: absolute;
  left: 12px;
  top: 50%;
  transform: translateY(-50%);
  color: #999;
  pointer-events: none;
}

.toolbar__search-input {
  width: 100%;
  height: 36px;
  padding: 0 12px 0 38px;
  border: 1px solid #afafaf;
  border-radius: 2.5px;
  font-size: 14px;
  transition: all var(--transition-fast);
  background-color: #ffffff;
  overflow: hidden;
}

.toolbar__search-input::placeholder {
  color: #bfbfbf;
}

.toolbar__search-input:focus {
  border-color: var(--color-primary);
  outline: none;
}

/* Filter */
.toolbar__filter {
  position: relative;
  min-width: 160px;
}

.toolbar__filter .icon {
  position: absolute;
  left: 12px;
  top: 50%;
  transform: translateY(-50%);
  color: #999;
  pointer-events: none;
  z-index: 1;
}

.toolbar__select {
  width: 100%;
  height: 36px;
  padding: 0 32px 0 38px;
  border: 1px solid #afafaf;
  border-radius: 2.5px;
  font-size: 14px;
  cursor: pointer;
  appearance: none;
  background: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="10" height="6"><path d="M1 1l4 4 4-4" stroke="%23999" fill="none" stroke-width="1.2"/></svg>') no-repeat right 12px center;
  background-color: #ffffff;
  color: #182026;
  overflow: hidden;
}

.toolbar__select:focus {
  border-color: var(--color-primary);
  outline: none;
}

/* Dropdown component in toolbar - giống popup */
.toolbar__dropdown {
  width: 100%;
  min-width: 160px;
}

.toolbar__dropdown :deep(.ms-dropdown__trigger) {
  height: 36px;
  padding: 0 32px 0 38px;
  border: 1px solid #afafaf;
  border-radius: 2.5px;
  background-color: #ffffff;
  font-size: 14px;
  color: #182026;
  position: relative;
  box-sizing: border-box;
}

.toolbar__dropdown--asset-type :deep(.ms-dropdown__trigger) {
  width: var(--asset-type-dropdown-width, 160px);
  min-width: var(--asset-type-dropdown-width, 160px);
  max-width: 100%;
}

.toolbar__dropdown--department :deep(.ms-dropdown__trigger) {
  width: var(--department-dropdown-width, 260px);
  min-width: var(--department-dropdown-width, 260px);
  max-width: 100%;
}

/* Đảm bảo dropdown container responsive */
.toolbar__dropdown--asset-type,
.toolbar__dropdown--department {
  width: 100%;
  min-width: 160px;
}

.toolbar__dropdown :deep(.ms-dropdown__trigger:hover) {
  border-color: #40a9ff;
}

.toolbar__dropdown :deep(.ms-dropdown--open .ms-dropdown__trigger) {
  border-color: #1aa4c8;
  box-shadow: 0 0 0 2px rgba(26, 164, 200, 0.1);
}

/* Icon chevron giống popup */
.toolbar__dropdown :deep(.ms-dropdown__arrow) {
  color: #595959;
}

/* Dropdown menu - kích thước luôn bằng item dài nhất, không thay đổi khi chọn */
.toolbar__dropdown :deep(.ms-dropdown__menu) {
  width: max-content !important;
  min-width: 100%;
  left: 0;
  right: auto;
}

/* List phải có width 100% của menu để item có thể phủ toàn bộ */
.toolbar__dropdown :deep(.ms-dropdown__list) {
  width: 100%;
  display: block;
}

/* Item hover giống popup - màu xanh nhạt #e6f7ff, phủ toàn bộ width của box */
.toolbar__dropdown :deep(.ms-dropdown__item) {
  width: 100% !important;
  box-sizing: border-box;
  white-space: nowrap;
  min-width: 100%;
}

.toolbar__dropdown :deep(.ms-dropdown__item:hover) {
  background-color: #e6f7ff;
}

.toolbar__dropdown :deep(.ms-dropdown__item--selected) {
  background-color: #e6f7ff;
}

/* Responsive - đảm bảo width không vượt quá container */
@media (max-width: 768px) {
  .toolbar__dropdown--asset-type :deep(.ms-dropdown__trigger),
  .toolbar__dropdown--department :deep(.ms-dropdown__trigger) {
    width: 100%;
    min-width: 100%;
    max-width: 100%;
  }
}

/* Buttons */
.btn {
  height: 36px;
  padding: 0 12px;
  border-radius: 2.5px;
  font-size: 14px;
  font-weight: 400;
  display: inline-flex;
  align-items: center;
  gap: 6px;
  cursor: pointer;
  transition: all var(--transition-fast);
  border: 1px solid transparent;
  white-space: nowrap;
  overflow: hidden;
}

.btn--primary {
  background-color:#1aa4c8;
  color: #ffffff;
  border-color: #1aa4c8;
  border-radius: 3px;
box-shadow: inset 0 2px 6px rgba(0, 0, 0, .16);
}

/* .btn--primary:hover {
  background-color: #007bb5;
  border-color: #007bb5;
} */

.btn--outline {
  background-color: #ffffff;
  color: #595959;
  border-color: #afafaf;
  padding: 0 8px;
  min-width: 32px;
  justify-content: center;
}

.btn--outline:hover {
  border-color: #0095da;
  color: #0095da;
}

.btn--danger:hover {
  border-color: var(--color-danger);
  color: var(--color-danger);
}

.btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.btn .icon {
  flex-shrink: 0;
}

/* ============================================ */
/* TABLE                                        */
/* ============================================ */

.asset-list__table-wrapper {
  flex: 1;
  background-color: #ffffff;
  overflow: auto;
}

.asset-table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
}

.asset-table__head {
  background-color: #fafafa;
  position: sticky;
  top: 0;
  z-index: 10;
}

.asset-table__th {
  padding: 12px;
  text-align: left;
  font-size: 13px;
  font-weight: 700;
  color: #182026;
  border-bottom: 1px solid #e8e8e8;
  white-space: nowrap;
  background-color: #fafafa;
  line-height: 18px;
  vertical-align: middle;
  height: 38px;
}

.asset-table__th--checkbox {
  width: 48px;
  text-align: center;
  padding: 12px 8px;
}

.asset-table__th--stt {
  width: 64px;
  text-align: center;
}

.asset-table__th--number {
  text-align: right;
  padding-right: 16px;
}

.asset-table__th--actions {
  width: 96px;
  text-align: center;
}

.asset-table__body {
  background-color: #ffffff;
}

.asset-table__row {
  transition: background-color 0.2s ease;
}

.asset-table__row:hover {
  background-color: rgba(26, 164, 200, 0.08);
}

.asset-table__row--selected {
  background-color: #e6f7ff;
}

.asset-table__td {
  padding: 12px;
  font-size: 14px;
  color: #182026;
  border-bottom: 1px solid #e8e8e8;
  line-height: 18px;
  vertical-align: middle;
  height: 40px;
}

.asset-table__td--checkbox {
  text-align: center;
  padding: 12px 8px;
}

/* Checkbox wrapper */
.asset-table__checkbox-wrapper {
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 14px;
  height: 14px;
  cursor: pointer;
}

/* Checkbox styling với icon từ SVG */
.asset-table input[type="checkbox"] {
  position: absolute;
  width: 14px;
  height: 14px;
  appearance: none;
  -webkit-appearance: none;
  -moz-appearance: none;
  margin: 0;
  padding: 0;
  border: none;
  background-color: #090f39;
  cursor: pointer;
  -webkit-mask-image: url('@/assets/icons/qlts-icon.svg');
  mask-image: url('@/assets/icons/qlts-icon.svg');
  -webkit-mask-size: 504px 754px;
  mask-size: 504px 754px;
  -webkit-mask-repeat: no-repeat;
  mask-repeat: no-repeat;
  /* Icon ô vuông (giữ nguyên khi checked) */
  -webkit-mask-position: -113px -377px;
  mask-position: -113px -377px;
  z-index: 1;
}

/* Icon tích bên trong ô vuông khi checked */
.asset-table__checkbox-check {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 10px;
  height: 10px;
  margin: 0;
  padding: 0;
  background-color: #090f39;
  -webkit-mask-image: url('@/assets/icons/qlts-icon.svg');
  mask-image: url('@/assets/icons/qlts-icon.svg');
  -webkit-mask-size: 504px 754px;
  mask-size: 504px 754px;
  -webkit-mask-repeat: no-repeat;
  mask-repeat: no-repeat;
  /* Icon tích - điều chỉnh mask-position để căn giữa */
  /* TODO: Nếu icon tích vẫn lệch, điều chỉnh mask-position với offset */
  /* Ví dụ: nếu lệch sang phải 1px, dùng: -161px -424px */
  /* Ví dụ: nếu lệch xuống dưới 1px, dùng: -160px -425px */
  -webkit-mask-position: -160px -424px;
  mask-position: -160px -424px;
  z-index: 2;
  pointer-events: none;
  opacity: 0;
  transition: opacity 0.2s ease;
}

/* Hiển thị icon tích khi checkbox được checked */
.asset-table__checkbox-wrapper input[type="checkbox"]:checked ~ .asset-table__checkbox-check,
.asset-table__checkbox-wrapper input[type="checkbox"]:checked + .asset-table__checkbox-check {
  opacity: 1;
}

.asset-table__td--stt {
  text-align: center;
  color: #595959;
  font-weight: 400;
}

.asset-table__td--number {
  text-align: right;
  font-family: 'Roboto', sans-serif;
  font-variant-numeric: tabular-nums;
  padding-right: 16px;
}

.asset-table__td--actions {
  text-align: center;
  padding: 4px 8px;
}

.action-btn {
  width: 32px;
  height: 32px;
  border-radius: 2px;
  border: none;
  background: none;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  color: #595959;
  transition: all 0.2s ease;
  margin: 0 1px;
  padding: 0;
  opacity: 0;
  visibility: hidden;
}

.asset-table__row:hover .action-btn {
  opacity: 1;
  visibility: visible;
}

.action-btn:hover {
  background-color: #e6f7ff;
  color: #0095da;
}

.action-btn .icon {
  flex-shrink: 0;
}

/* ============================================ */
/* PAGINATION TOTALS (slot content)             */
/* ============================================ */

.pagination__total-item {
  font-family: 'Roboto', sans-serif;
  font-variant-numeric: tabular-nums;
  white-space: nowrap;
}

/* ============================================ */
/* RESPONSIVE                                   */
/* ============================================ */

@media (max-width: 1024px) {
  .asset-list__toolbar {
    flex-direction: column;
    align-items: stretch;
  }
  
  .toolbar__left {
    flex-wrap: wrap;
  }
  
  .toolbar__search {
    width: 100%;
  }
}
</style>
