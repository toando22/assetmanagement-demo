<!--
  Mô tả: View hiển thị danh sách tài sản
  Features:
  - Search và filter
  - Table danh sách tài sản
  - Pagination
  - CRUD actions
  - Tùy biến độ rộng cột (resize columns)
  - Thông báo xóa thông minh (hiển thị số lượng khi xóa nhiều)
  - Chọn nhiều với Ctrl và Shift (Ctrl+Click, Shift+Click)
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: DDTOAN - (18/1/2026) - Thêm tính năng tùy biến độ rộng cột
  EditBy: DDTOAN - (18/1/2026) - Cải thiện thông báo xóa nhiều tài sản
  EditBy: DDTOAN - (24/1/2026) - Thêm tính năng chọn nhiều với Ctrl và Shift
-->

<template>
  <div class="asset-list">
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
        <div
          ref="measureRef"
          class="toolbar__measure"
          style="
            visibility: hidden;
            position: absolute;
            white-space: nowrap;
            font-size: 14px;
            padding: 0 12px;
          "
        ></div>
      </div>

      <!-- Right: Action Buttons -->
      <div class="toolbar__right">
        <!-- Button: Thêm tài sản -->
        <button class="btn btn--primary" @click="handleAddAsset">
          <i class="icon icon-add"></i>
          Thêm tài sản
        </button>

        <!-- Button: Excel -->
        <button class="btn btn--outline" title="Xuất Excel" @click="handleExportExcelClick">
          <i class="icon icon-excel"></i>
        </button>

        <!-- Button: Delete -->
        <button
          class="btn btn--outline btn--danger"
          title="Xóa"
          :disabled="selectedAssets.length === 0"
          @click="handleDeleteClick"
        >
          <i class="icon icon-delete"></i>
        </button>
      </div>
    </div>

    <!-- Loading dữ liệu -->
    <div v-if="loading" class="asset-list__loading">
      <div class="loading-spinner">Đang tải dữ liệu...</div>
    </div>

    <!-- Thông báo lỗi -->
    <div v-if="errorMessage && !loading" class="asset-list__error">
      <div class="error-message">{{ errorMessage }}</div>
    </div>

    <!-- Table Container -->
    <div class="asset-list__container" v-if="!loading">
      <!-- Table -->
      <div class="asset-list__table-wrapper" ref="tableWrapperRef">
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
              <th
                class="asset-table__th asset-table__th--code"
                :class="{ 'asset-table__th--resizing': isResizing && resizeState.columnKey === 'code' }"
                :style="{ width: columnWidths.code + 'px', minWidth: columnWidths.code + 'px', maxWidth: columnWidths.code + 'px' }"
              >
                Mã tài sản
                <div
                  class="asset-table__resize-handle"
                  @mousedown.prevent="startResize('code', $event)"
                ></div>
              </th>
              <th
                class="asset-table__th asset-table__th--name"
                :class="{ 'asset-table__th--resizing': isResizing && resizeState.columnKey === 'name' }"
                :style="{ width: columnWidths.name + 'px', minWidth: columnWidths.name + 'px', maxWidth: columnWidths.name + 'px' }"
              >
                Tên tài sản
                <div
                  class="asset-table__resize-handle"
                  @mousedown.prevent="startResize('name', $event)"
                ></div>
              </th>
              <th
                class="asset-table__th asset-table__th--type"
                :class="{ 'asset-table__th--resizing': isResizing && resizeState.columnKey === 'type' }"
                :style="{ width: columnWidths.type + 'px', minWidth: columnWidths.type + 'px', maxWidth: columnWidths.type + 'px' }"
              >
                Loại tài sản
                <div
                  class="asset-table__resize-handle"
                  @mousedown.prevent="startResize('type', $event)"
                ></div>
              </th>
              <th
                class="asset-table__th asset-table__th--department"
                :class="{ 'asset-table__th--resizing': isResizing && resizeState.columnKey === 'department' }"
                :style="{ width: columnWidths.department + 'px', minWidth: columnWidths.department + 'px', maxWidth: columnWidths.department + 'px' }"
              >
                Bộ phận sử dụng
                <div
                  class="asset-table__resize-handle"
                  @mousedown.prevent="startResize('department', $event)"
                ></div>
              </th>
              <th
                class="asset-table__th asset-table__th--number asset-table__th--quantity"
                :class="{ 'asset-table__th--resizing': isResizing && resizeState.columnKey === 'quantity' }"
                :style="{ width: columnWidths.quantity + 'px', minWidth: columnWidths.quantity + 'px', maxWidth: columnWidths.quantity + 'px' }"
              >
                Số lượng
                <div
                  class="asset-table__resize-handle"
                  @mousedown.prevent="startResize('quantity', $event)"
                ></div>
              </th>
              <th
                class="asset-table__th asset-table__th--number asset-table__th--cost"
                :class="{ 'asset-table__th--resizing': isResizing && resizeState.columnKey === 'cost' }"
                :style="{ width: columnWidths.cost + 'px', minWidth: columnWidths.cost + 'px', maxWidth: columnWidths.cost + 'px' }"
              >
                Nguyên giá
                <div
                  class="asset-table__resize-handle"
                  @mousedown.prevent="startResize('cost', $event)"
                ></div>
              </th>
              <th
                class="asset-table__th asset-table__th--number asset-table__th--depreciation"
                :class="{ 'asset-table__th--resizing': isResizing && resizeState.columnKey === 'depreciation' }"
                :style="{ width: columnWidths.depreciation + 'px', minWidth: columnWidths.depreciation + 'px', maxWidth: columnWidths.depreciation + 'px' }"
              >
                HM/KH lũy kế
                <div
                  class="asset-table__resize-handle"
                  @mousedown.prevent="startResize('depreciation', $event)"
                ></div>
              </th>
              <th
                class="asset-table__th asset-table__th--number asset-table__th--remaining"
                :class="{ 'asset-table__th--resizing': isResizing && resizeState.columnKey === 'remaining' }"
                :style="{ width: columnWidths.remaining + 'px', minWidth: columnWidths.remaining + 'px', maxWidth: columnWidths.remaining + 'px' }"
              >
                Giá trị còn lại
                <div
                  class="asset-table__resize-handle"
                  @mousedown.prevent="startResize('remaining', $event)"
                ></div>
              </th>
              <th class="asset-table__th asset-table__th--actions">Chức năng</th>
            </tr>
          </thead>
          <tbody class="asset-table__body">
            <!-- Empty state: Không có dữ liệu -->
            <tr v-if="paginatedAssets.length === 0 && !loading" class="asset-table__row asset-table__row--empty">
              <td colspan="11" class="asset-table__td asset-table__td--empty">
                <div class="asset-table__empty-message">
                  <i class="icon icon-search-toolbar" style="font-size: 24px; color: #bfbfbf; margin-bottom: 8px;"></i>
                  <p style="color: #595959; font-size: 14px; margin: 0;">
                    Không tìm thấy tài sản nào thỏa mãn điều kiện tìm kiếm
                  </p>
                </div>
              </td>
            </tr>
            
            <!-- Danh sách tài sản -->
            <tr
              v-for="(asset, index) in paginatedAssets"
              :key="asset.id"
              class="asset-table__row"
              :class="{
                'asset-table__row--selected': selectedAssets.includes(asset.id),
                'asset-table__row--focused': focusedRowIndex === index
              }"
              :data-row-index="index"
              @contextmenu.prevent="handleContextMenu($event, asset, index)"
            >
              <td class="asset-table__td asset-table__td--checkbox">
                <label class="asset-table__checkbox-wrapper">
                  <input 
                    type="checkbox" 
                    :value="asset.id" 
                    :checked="selectedAssets.includes(asset.id)"
                    @click="handleCheckboxClick($event, asset, index)"
                  />
                  <span class="asset-table__checkbox-check"></span>
                </label>
              </td>
              <td class="asset-table__td asset-table__td--stt">
                {{ (currentPage - 1) * pageSize + index + 1 }}
              </td>
              <td
                class="asset-table__td asset-table__td--code"
                :style="{ width: columnWidths.code + 'px', minWidth: columnWidths.code + 'px', maxWidth: columnWidths.code + 'px' }"
                :title="asset.code"
              >
                {{ asset.code }}
              </td>
              <td
                class="asset-table__td asset-table__td--name"
                :style="{ width: columnWidths.name + 'px', minWidth: columnWidths.name + 'px', maxWidth: columnWidths.name + 'px' }"
                :title="asset.name"
              >
                {{ asset.name }}
              </td>
              <td
                class="asset-table__td asset-table__td--type"
                :style="{ width: columnWidths.type + 'px', minWidth: columnWidths.type + 'px', maxWidth: columnWidths.type + 'px' }"
                :title="asset.type"
              >
                {{ asset.type }}
              </td>
              <td
                class="asset-table__td asset-table__td--department"
                :style="{ width: columnWidths.department + 'px', minWidth: columnWidths.department + 'px', maxWidth: columnWidths.department + 'px' }"
                :title="asset.department"
              >
                {{ asset.department }}
              </td>
              <td
                class="asset-table__td asset-table__td--number asset-table__td--quantity"
                :style="{ width: columnWidths.quantity + 'px', minWidth: columnWidths.quantity + 'px', maxWidth: columnWidths.quantity + 'px' }"
              >
                {{ asset.quantity }}
              </td>
              <td
                class="asset-table__td asset-table__td--number asset-table__td--cost"
                :style="{ width: columnWidths.cost + 'px', minWidth: columnWidths.cost + 'px', maxWidth: columnWidths.cost + 'px' }"
              >
                {{ formatNumber(asset.cost) }}
              </td>
              <td
                class="asset-table__td asset-table__td--number asset-table__td--depreciation"
                :style="{ width: columnWidths.depreciation + 'px', minWidth: columnWidths.depreciation + 'px', maxWidth: columnWidths.depreciation + 'px' }"
              >
                {{ formatNumber(asset.depreciation) }}
              </td>
              <td
                class="asset-table__td asset-table__td--number asset-table__td--remaining"
                :style="{ width: columnWidths.remaining + 'px', minWidth: columnWidths.remaining + 'px', maxWidth: columnWidths.remaining + 'px' }"
              >
                {{ formatNumber(asset.remainingValue) }}
              </td>
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
          <tfoot class="asset-table__foot">
            <tr class="asset-table__row asset-table__row--pagination">
              <!-- Pagination Left + Nav (colspan 6) -->
              <td class="asset-table__td asset-table__td--pagination-controls" colspan="6">
                <div class="asset-table__pagination-container">
                  <div class="asset-table__pagination-left">
                    <div class="asset-table__pagination-info">
                      Tổng số: <strong>{{ totalRecords }} bản ghi</strong>
                    </div>
                    <div class="asset-table__pagination-size">
                      <MsDropdown
                        :model-value="pageSize"
                        :options="pageSizeDropdownOptions"
                        class="asset-table__pagination-dropdown"
                        teleport
                        placement="top"
                        :scroll-container="tableWrapperRef"
                        @update:model-value="setPageSize"
                      />
                    </div>
                  </div>
                  <div class="asset-table__pagination-nav">
                    <button
                      class="asset-table__pagination-btn"
                      :disabled="currentPage === 1"
                      @click="goToPage(currentPage - 1)"
                    >
                      <i class="icon icon-chevron-left"></i>
                    </button>

                    <template v-for="page in displayPages" :key="page">
                      <button
                        v-if="page === 'ellipsis-start' || page === 'ellipsis-end'"
                        class="asset-table__pagination-btn asset-table__pagination-btn--ellipsis"
                        disabled
                      >
                        <i class="icon icon-ellipsis"></i>
                      </button>
                      <button
                        v-else
                        class="asset-table__pagination-btn"
                        :class="{ 'asset-table__pagination-btn--active': page === currentPage }"
                        @click="goToPage(page)"
                      >
                        {{ page }}
                      </button>
                    </template>

                    <button
                      class="asset-table__pagination-btn"
                      :disabled="currentPage === totalPages"
                      @click="goToPage(currentPage + 1)"
                    >
                      <i class="icon icon-chevron-right"></i>
                    </button>
                  </div>
                </div>
              </td>

              <!-- Totals columns - căn thẳng với các cột tương ứng -->
              <td class="asset-table__td asset-table__td--number">
                <span class="asset-table__total-item">{{ pageTotals.quantity }}</span>
              </td>
              <td class="asset-table__td asset-table__td--number">
                <span class="asset-table__total-item">{{ formatNumber(pageTotals.cost) }}</span>
              </td>
              <td class="asset-table__td asset-table__td--number">
                <span class="asset-table__total-item">{{
                  formatNumber(pageTotals.depreciation)
                }}</span>
              </td>
              <td class="asset-table__td asset-table__td--number">
                <span class="asset-table__total-item">{{
                  formatNumber(pageTotals.remainingValue)
                }}</span>
              </td>
              <td class="asset-table__td asset-table__td--actions"></td>
            </tr>
          </tfoot>
        </table>
      </div>

      <!-- Modal Thêm tài sản -->
      <AssetForm
        v-model="isFormOpen"
        :title="formTitle"
        :asset-data="selectedAssetData"
        :existing-assets="assets"
        @save="handleSaveAsset"
        @saved="handleAssetSaved"
        @cancel="handleCancelForm"
      />
    </div>

    <!-- Delete Confirmation Dialog -->
    <MsDialog
      v-model="showDeleteDialog"
      type="confirm"
      :message="deleteDialogMessage"
      :buttons="deleteDialogButtons"
      @confirm="handleConfirmDelete"
      @cancel="handleCancelDelete"
    />

    <!-- Export Excel Confirmation Dialog -->
    <MsDialog
      v-model="showExportDialog"
      type="confirm"
      message="Bạn có muốn xuất danh sách tài sản ra file Excel không?"
      :buttons="exportDialogButtons"
      @button-click="handleExportDialogClick"
    />

    <!-- Context Menu -->
    <MsContextMenu
      v-model="showContextMenu"
      :items="contextMenuItems"
      :x="contextMenuX"
      :y="contextMenuY"
      @select="handleContextMenuSelect"
      @close="showContextMenu = false"
    />
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
  - Tùy biến độ rộng cột (sử dụng useColumnResize composable)
  - Filter theo năm (từ Header)
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: DDToan - (11/1/2026) - chuyển pagination logic sang composable
  EditBy: DDTOAN - (18/1/2026) - thêm tính năng tùy biến độ rộng cột
  EditBy: DDToan - (24/1/2026) - Thêm filter theo năm từ Header
  EditBy: DDTOAN - (24/1/2026) - Thêm tính năng chọn nhiều với Ctrl và Shift
*/

import { ref, computed, onMounted, nextTick, watch, inject } from 'vue'
import { MOCK_ASSETS } from '@/constants/assetData'
import AssetForm from './AssetForm.vue'
import MsDropdown from '@/components/base/ms-dropdown/MsDropdown.vue'
import MsDialog from '@/components/base/ms-dialog/MsDialog.vue'
import MsContextMenu from '@/components/base/ms-context-menu/MsContextMenu.vue'
import { usePagination } from '@/composables/usePagination'
import { useKeyboardNavigation } from '@/composables/useKeyboardNavigation'
import { useColumnResize } from '@/composables/useColumnResize'
import { getFixedAssets, getNewAssetCode, deleteFixedAsset, deleteMultipleFixedAssets, cloneFixedAsset, exportFixedAssetsToExcel } from '@/api/fixedAssetApi'
import { getDepartments } from '@/api/departmentApi'
import { getFixedAssetCategories } from '@/api/fixedAssetCategoryApi'

// Inject yearFilter từ MainLayout
// CreatedBy: DDToan - (24/1/2026)
const yearFilter = inject('yearFilter', null)
const selectedYear = yearFilter ? yearFilter.selectedYear : ref(new Date().getFullYear())

// State
const searchText = ref('')
const selectedAssetType = ref('')
const selectedDepartment = ref('')
const selectedAssets = ref([])
const selectAll = ref(false)
const isFormOpen = ref(false)
const selectedAssetData = ref(null)
const formTitle = ref('Thêm tài sản')
const showDeleteDialog = ref(false)
const assetToDelete = ref(null)
const showExportDialog = ref(false)
const showContextMenu = ref(false)
const contextMenuX = ref(0)
const contextMenuY = ref(0)
const contextMenuAsset = ref(null)

// State: Track index của item được chọn cuối cùng (để dùng cho Shift+Click)
// CreatedBy: DDTOAN - (24/1/2026)
const lastSelectedIndex = ref(null)

// State
const loading = ref(false)
const errorMessage = ref('')

// Data
const assets = ref([])

// State: Danh sách departments và categories từ API
const departments = ref([])
const assetCategories = ref([])

// Dropdown options
const departmentOptions = ref([])
const assetTypeOptions = ref([])

// Maps để lookup nhanh
const departmentMap = ref(new Map()) // Map<departmentId, {code, name}>
const categoryMap = ref(new Map()) // Map<categoryId, {code, name}>

// Refs
const assetTypeDropdownRef = ref(null)
const departmentDropdownRef = ref(null)
const measureRef = ref(null)
const tableWrapperRef = ref(null)

// Column Resize: Định nghĩa độ rộng mặc định cho các cột có thể resize
// CreatedBy: DDTOAN - (18/1/2026)
const defaultColumnWidths = {
  code: 120,
  name: 150,
  type: 140,
  department: 160,
  quantity: 80,
  cost: 120,
  depreciation: 120,
  remaining: 120
}

// Sử dụng composable useColumnResize để quản lý độ rộng cột
// CreatedBy: DDTOAN - (18/1/2026)
const {
  columnWidths,
  isResizing,
  resizeState,
  startResize,
  handleResize,
  endResize,
  resetColumnWidths
} = useColumnResize(defaultColumnWidths, 'asset-table-column-widths')

// Tính toán width của item dài nhất
const calculateMaxWidth = (options) => {
  if (!measureRef.value || !options || options.length === 0) return null

  let maxWidth = 0
  options.forEach((option) => {
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
watch(
  [departmentOptions, assetTypeOptions],
  () => {
    setDropdownWidths()
  },
  { deep: true },
)

/**
 * Format date từ backend (yyyy-MM-dd hoặc ISO string) sang dd/mm/yyyy
 */
const formatDate = (dateString) => {
  if (!dateString) return ''
  try {
    const date = new Date(dateString)
    if (isNaN(date.getTime())) return dateString
    const day = String(date.getDate()).padStart(2, '0')
    const month = String(date.getMonth() + 1).padStart(2, '0')
    const year = date.getFullYear()
    return `${day}/${month}/${year}`
  } catch {
    return dateString
  }
}

/**
 * Map dữ liệu từ API format sang frontend format
 * @param {object} apiAsset - Dữ liệu từ API
 * @returns {object} - Dữ liệu format cho frontend
 */
const mapAssetFromApi = (apiAsset) => {
  // Lấy department info từ map
  const departmentId = apiAsset.departmentId || apiAsset.department_id
  const departmentInfo = departmentId ? departmentMap.value.get(departmentId) : null
  
  // Lấy category info từ map
  const categoryId = apiAsset.fixedAssetCategoryId || apiAsset.fixed_asset_category_id
  const categoryInfo = categoryId ? categoryMap.value.get(categoryId) : null

  return {
    id: apiAsset.fixedAssetId || apiAsset.fixed_asset_id || apiAsset.id,
    code: apiAsset.fixedAssetCode || apiAsset.fixed_asset_code || apiAsset.code,
    name: apiAsset.fixedAssetName || apiAsset.fixed_asset_name || apiAsset.name,
    type: categoryInfo?.name || apiAsset.fixedAssetCategoryName || apiAsset.fixed_asset_category_name || apiAsset.type || '',
    department: departmentInfo?.name || apiAsset.departmentName || apiAsset.department_name || apiAsset.department || '',
    quantity: apiAsset.fixedAssetQuantity || apiAsset.fixed_asset_quantity || apiAsset.quantity || 1,
    cost: apiAsset.fixedAssetOriginalCost || apiAsset.fixed_asset_original_cost || apiAsset.cost || 0,
    depreciation: apiAsset.fixedAssetAnnualDepreciationValue || apiAsset.fixed_asset_annual_depreciation_value || apiAsset.depreciation || 0,
    remainingValue: (apiAsset.fixedAssetOriginalCost || apiAsset.fixed_asset_original_cost || apiAsset.cost || 0) - 
                   (apiAsset.fixedAssetAnnualDepreciationValue || apiAsset.fixed_asset_annual_depreciation_value || apiAsset.depreciation || 0),
    // Thêm các field cần thiết cho form
    departmentCode: departmentInfo?.code || apiAsset.departmentCode || apiAsset.department_code || '',
    departmentName: departmentInfo?.name || apiAsset.departmentName || apiAsset.department_name || '',
    departmentId: departmentId,
    assetTypeCode: categoryInfo?.code || apiAsset.fixedAssetCategoryCode || apiAsset.fixed_asset_category_code || apiAsset.assetTypeCode || '',
    assetTypeName: categoryInfo?.name || apiAsset.fixedAssetCategoryName || apiAsset.fixed_asset_category_name || apiAsset.assetTypeName || '',
    fixedAssetCategoryId: categoryId,
    originalCost: apiAsset.fixedAssetOriginalCost || apiAsset.fixed_asset_original_cost || apiAsset.cost || 0,
    depreciationRate: apiAsset.fixedAssetDepreciationRate || apiAsset.fixed_asset_depreciation_rate || apiAsset.depreciationRate || 0,
    purchaseDate: formatDate(apiAsset.fixedAssetPurchaseDate || apiAsset.fixed_asset_purchase_date || apiAsset.purchaseDate),
    startUseDate: formatDate(apiAsset.fixedAssetStartUseDate || apiAsset.fixed_asset_start_use_date || apiAsset.startUseDate),
    trackingYear: apiAsset.fixedAssetTrackingYear || apiAsset.fixed_asset_tracking_year || apiAsset.trackingYear || '',
    yearsOfUse: apiAsset.fixedAssetYearsOfUse || apiAsset.fixed_asset_years_of_use || apiAsset.fixedAssetLifeTime || apiAsset.fixed_asset_life_time || apiAsset.yearsOfUse || 0, // EditBy: DDToan - (16/1/2026) - Ưu tiên fixedAssetYearsOfUse, giữ fallback cho tương thích ngược
    annualDepreciationValue: apiAsset.fixedAssetAnnualDepreciationValue || apiAsset.fixed_asset_annual_depreciation_value || apiAsset.annualDepreciationValue || 0
  }
}

/**
 * Load danh sách Departments và Categories để map vào assets
 */
const loadMasterData = async () => {
  try {
    // Load Departments
    const departmentsResponse = await getDepartments()
    const departmentsData = Array.isArray(departmentsResponse) ? departmentsResponse : (departmentsResponse.data || [])
    
    // Lưu vào state
    departments.value = departmentsData
    
    // Tạo map và dropdown options cho Departments
    departmentMap.value.clear()
    departmentOptions.value = []
    departmentsData.forEach(dept => {
      const id = dept.departmentId || dept.department_id || dept.id
      const code = dept.departmentCode || dept.department_code || dept.code
      const name = dept.departmentName || dept.department_name || dept.name
      
      if (id) {
        departmentMap.value.set(id, { code, name })
        departmentOptions.value.push({ value: id, label: name })
      }
    })
    
    // Load Categories
    const categoriesResponse = await getFixedAssetCategories()
    const categoriesData = Array.isArray(categoriesResponse) ? categoriesResponse : (categoriesResponse.data || [])
    
    // Lưu vào state
    assetCategories.value = categoriesData
    
    // Tạo map và dropdown options cho Categories
    categoryMap.value.clear()
    assetTypeOptions.value = []
    categoriesData.forEach(cat => {
      const id = cat.fixedAssetCategoryId || cat.fixed_asset_category_id || cat.id
      const code = cat.fixedAssetCategoryCode || cat.fixed_asset_category_code || cat.code
      const name = cat.fixedAssetCategoryName || cat.fixed_asset_category_name || cat.name
      
      if (id) {
        categoryMap.value.set(id, { code, name })
        assetTypeOptions.value.push({ value: id, label: name })
      }
    })
  } catch (error) {
    console.error('Error loading master data:', error)
    // Không throw error, chỉ log để không block việc load assets
  }
}

// State: Tổng số bản ghi từ backend (dùng cho pagination)
const totalRecordsFromBackend = ref(0)

// Computed: Tổng số bản ghi (ưu tiên từ backend, fallback về length của assets)
const totalRecords = computed(() => {
  return totalRecordsFromBackend.value || assets.value.length
})

// Sử dụng composable usePagination (phải khởi tạo trước loadAssets)
const {
  currentPage,
  pageSize,
  totalPages,
  startIndex,
  endIndex,
  visiblePages,
  goToPage,
  setPageSize,
  pageSizeOptions,
} = usePagination({
  total: totalRecords,
  initialPageSize: 20,
  pageSizeOptions: [20, 50, 100],
  maxVisiblePages: 5,
})

/**
 * Load danh sách tài sản từ API với pagination, search và filters
 * @param {number} page - Số trang (mặc định: currentPage.value)
 * @param {number} size - Số bản ghi mỗi trang (mặc định: pageSize.value)
 * @param {string} keyword - Từ khóa tìm kiếm (mặc định: searchText.value)
 * @param {string} departmentId - ID bộ phận sử dụng (mặc định: selectedDepartment.value)
 * @param {string} categoryId - ID loại tài sản (mặc định: selectedAssetType.value)
 * @param {number} trackingYear - Năm theo dõi (mặc định: selectedYear.value, 0 = lấy tất cả)
 * CreatedBy: DDToan - (09/1/2026)
 * EditBy: DDToan - (17/1/2026) - Thêm support cho search và filters
 * EditBy: DDToan - (24/1/2026) - Thêm support cho trackingYear filter
 */
const loadAssets = async (page = null, size = null, keyword = null, departmentId = null, categoryId = null, trackingYear = null) => {
  try {
    loading.value = true
    errorMessage.value = ''
    
    // Load master data trước (Departments và Categories) - chỉ load 1 lần
    if (departments.value.length === 0 || assetCategories.value.length === 0) {
      await loadMasterData()
    }
    
    // Lấy page và pageSize từ params hoặc từ state hiện tại
    const currentPageNum = page !== null ? page : currentPage.value
    const currentPageSize = size !== null ? size : pageSize.value
    
    // Lấy keyword và filters từ params hoặc từ state hiện tại
    const searchKeyword = keyword !== null ? keyword : (searchText.value || null)
    const filterDepartmentId = departmentId !== null ? departmentId : (selectedDepartment.value || null)
    const filterCategoryId = categoryId !== null ? categoryId : (selectedAssetType.value || null)
    // Lấy trackingYear từ params hoặc từ selectedYear (nếu không có thì dùng 0 = lấy tất cả)
    const filterTrackingYear = trackingYear !== null ? trackingYear : (selectedYear.value || 0)
    
    // Gọi API lấy danh sách tài sản với pagination, search và filters
    // EditBy: DDToan - (17/1/2026) - Thêm keyword, departmentId, categoryId để hỗ trợ search và filter
    // EditBy: DDToan - (24/1/2026) - Thêm trackingYear để filter theo năm
    const response = await getFixedAssets({
      pageIndex: currentPageNum,
      pageSize: currentPageSize,
      keyword: searchKeyword,
      departmentId: filterDepartmentId,
      categoryId: filterCategoryId,
      trackingYear: filterTrackingYear
    })
    
    // Xử lý response - có thể là array hoặc object có data property
    let apiAssets = []
    let total = 0
    
    if (Array.isArray(response)) {
      // Response là array trực tiếp (fallback - không có pagination info)
      apiAssets = response
      total = response.length
    } else if (response && Array.isArray(response.data)) {
      // Response có format: { data: [...], total: 100, page: 1, pageSize: 20 }
      apiAssets = response.data
      total = response.total || response.totalCount || response.totalRecords || 0
    } else if (response && response.data) {
      // Response có data nhưng không phải array
      apiAssets = [response.data]
      total = 1
    }
    
    // Map từ API format sang frontend format (sẽ dùng departmentMap và categoryMap)
    assets.value = apiAssets.map(mapAssetFromApi)
    
    // Cập nhật totalRecordsFromBackend từ backend
    totalRecordsFromBackend.value = total
    
    // Không fallback về mock data khi đang search/filter
    // Chỉ fallback khi không có search/filter và không có dữ liệu (trường hợp lỗi API)
    // CreatedBy: DDToan - (24/1/2026)
    // EditBy: DDToan - (24/1/2026) - Bỏ fallback mock data khi filter theo năm
    if (assets.value.length === 0 && total === 0) {
      // Kiểm tra xem có đang filter theo năm không (trackingYear khác 0 và khác null)
      const isFilteringByYear = filterTrackingYear !== 0 && filterTrackingYear !== null
      
      // Chỉ fallback về mock data nếu KHÔNG có search/filter nào (bao gồm cả filter năm)
      // Nếu đang search/filter mà không có kết quả thì giữ nguyên (rỗng) để hiển thị thông báo
      const hasSearchOrFilter = searchKeyword || filterDepartmentId || filterCategoryId || isFilteringByYear
      if (!hasSearchOrFilter) {
        console.warn('No data from API, using mock data for testing')
        assets.value = MOCK_ASSETS.map(mapAssetFromApi)
        totalRecordsFromBackend.value = MOCK_ASSETS.length
      }
      // Nếu có search/filter mà không có kết quả, giữ assets.value = [] để hiển thị empty state
    }
  } catch (error) {
    console.error('Error loading assets:', error)
    errorMessage.value = error.message || 'Có lỗi xảy ra khi tải dữ liệu. Vui lòng thử lại sau.'
    
    // Fallback về mock data nếu có lỗi (để test UI)
    console.warn('Using mock data due to error')
    assets.value = MOCK_ASSETS.map(mapAssetFromApi)
    totalRecordsFromBackend.value = MOCK_ASSETS.length
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  loadAssets() // Load data từ API
  setDropdownWidths()
})

// Watch: Khi currentPage thay đổi, gọi lại API với trang mới
watch(currentPage, (newPage) => {
  if (newPage > 0) {
    loadAssets(newPage, pageSize.value, null, null, null, null) // Giữ nguyên search, filters và năm
    // Reset lastSelectedIndex khi chuyển trang
    // CreatedBy: DDTOAN - (24/1/2026)
    lastSelectedIndex.value = null
  }
})

// Watch: Khi pageSize thay đổi, gọi lại API với pageSize mới
// Lưu ý: setPageSize trong composable đã reset về trang 1, nên sẽ gọi với page = 1
watch(pageSize, (newSize) => {
  if (newSize > 0) {
    loadAssets(currentPage.value, newSize, null, null, null, null) // Giữ nguyên search, filters và năm
  }
})

/*
  Mô tả: Watch selectedYear - Filter theo năm, reset về trang 1 và reload danh sách
  CreatedBy: DDToan - (24/1/2026)
*/
watch(selectedYear, (newYear) => {
  // Reset về trang 1 khi thay đổi năm
  goToPage(1)
  // Gọi API với năm mới, giữ nguyên search và filters khác
  loadAssets(1, pageSize.value, null, null, null, newYear)
})

/*
  Mô tả: Watch searchText với debounce 300ms - Tự động search khi user ngừng gõ
  CreatedBy: DDToan - (17/1/2026)
*/
let searchTimeout = null
watch(searchText, (newValue) => {
  // Clear timeout cũ nếu có
  if (searchTimeout) {
    clearTimeout(searchTimeout)
  }
  
  // Debounce: đợi 300ms sau khi user ngừng gõ mới gọi API
  searchTimeout = setTimeout(() => {
    // Reset về trang 1 khi search
    goToPage(1)
    // Gọi API với keyword mới, giữ nguyên filters và năm
    loadAssets(1, pageSize.value, newValue, null, null, null)
  }, 300)
})

/*
  Mô tả: Watch selectedAssetType - Filter theo loại tài sản, hiển thị ngay tức thì
  CreatedBy: DDToan - (17/1/2026)
*/
watch(selectedAssetType, (newValue) => {
  // Reset về trang 1 khi filter
  goToPage(1)
  // Gọi API với filter mới, giữ nguyên search, filter department và năm
  loadAssets(1, pageSize.value, null, null, newValue, null)
})

/*
  Mô tả: Watch selectedDepartment - Filter theo bộ phận sử dụng, hiển thị ngay tức thì
  CreatedBy: DDToan - (17/1/2026)
*/
watch(selectedDepartment, (newValue) => {
  // Reset về trang 1 khi filter
  goToPage(1)
  // Gọi API với filter mới, giữ nguyên search, filter category và năm
  loadAssets(1, pageSize.value, null, newValue, null, null)
})


// Computed: Danh sách tài sản đã phân trang
// Lưu ý: Backend đã phân trang, không cần slice nữa
const paginatedAssets = computed(() => {
  // Backend đã trả về đúng số bản ghi của trang hiện tại
  // Không cần slice nữa, dùng trực tiếp assets.value
  return assets.value
})

// Keyboard navigation composable (phải đặt sau paginatedAssets)
const {
  focusedRowIndex,
  focusRow,
  resetFocus,
} = useKeyboardNavigation({
  items: paginatedAssets,
  onSelect: (item, index) => {
    // Khi nhấn Enter/Space, chọn row đó
    const itemId = item.id
    if (selectedAssets.value.includes(itemId)) {
      const idx = selectedAssets.value.indexOf(itemId)
      selectedAssets.value.splice(idx, 1)
    } else {
      selectedAssets.value.push(itemId)
    }
    // Update lastSelectedIndex khi chọn bằng keyboard
    // CreatedBy: DDTOAN - (24/1/2026)
    lastSelectedIndex.value = index
  },
  getItemId: (item) => item.id,
})

// Sử dụng visiblePages từ composable (đã được tối ưu và tái sử dụng)
const displayPages = computed(() => visiblePages.value)

// Computed: Chuyển đổi pageSizeOptions thành format cho dropdown
const pageSizeDropdownOptions = computed(() => {
  if (!pageSizeOptions || !Array.isArray(pageSizeOptions)) {
    return []
  }
  return pageSizeOptions.map((size) => ({
    value: size,
    label: String(size),
  }))
})

/*
  Mô tả: Tính tổng của tất cả bản ghi trong trang hiện tại
  CreatedBy: DDToan - (09/1/2026)
  EditBy: DDToan - (17/1/2026) - Đổi từ tổng các bản ghi được chọn sang tổng tất cả bản ghi trong trang
*/
const pageTotals = computed(() => {
  // Tính tổng từ tất cả bản ghi trong trang hiện tại (paginatedAssets)
  return {
    quantity: paginatedAssets.value.reduce((sum, asset) => sum + (Number(asset.quantity) || 0), 0),
    cost: paginatedAssets.value.reduce((sum, asset) => sum + (Number(asset.cost) || 0), 0),
    depreciation: paginatedAssets.value.reduce((sum, asset) => sum + (Number(asset.depreciation) || 0), 0),
    remainingValue: paginatedAssets.value.reduce((sum, asset) => sum + (Number(asset.remainingValue) || 0), 0),
  }
})

// Methods
const toggleSelectAll = () => {
  if (selectAll.value) {
    selectedAssets.value = paginatedAssets.value.map((a) => a.id)
    // Set lastSelectedIndex về index cuối cùng
    lastSelectedIndex.value = paginatedAssets.value.length - 1
  } else {
    selectedAssets.value = []
    lastSelectedIndex.value = null
  }
}

/*
  Mô tả: Xử lý chọn nhiều với Ctrl và Shift
  - Ctrl + Click: Toggle item mà không ảnh hưởng các item khác
  - Shift + Click: Chọn range từ item cuối cùng được chọn đến item hiện tại
  - Click thông thường: Toggle item (giữ nguyên hành vi hiện tại)
  
  CreatedBy: DDTOAN - (24/1/2026)
*/
const handleCheckboxClick = (event, asset, index) => {
  const isCtrlPressed = event.ctrlKey || event.metaKey // Hỗ trợ cả Ctrl (Windows) và Cmd (Mac)
  const isShiftPressed = event.shiftKey
  
  if (isShiftPressed) {
    // Shift + Click: Chọn range
    if (lastSelectedIndex.value !== null) {
      // Có lastSelectedIndex → Chọn range từ đó đến index hiện tại
      handleShiftSelection(index)
    } else {
      // Chưa có lastSelectedIndex → Chọn từ đầu đến index hiện tại
      handleShiftSelectionFromStart(index)
    }
  } else if (isCtrlPressed) {
    // Ctrl + Click: Toggle item mà không clear selection khác
    handleCtrlSelection(asset.id)
  } else {
    // Click thông thường: Toggle item
    handleNormalSelection(asset.id)
  }
  
  // Update last selected index
  lastSelectedIndex.value = index
}

/*
  Mô tả: Xử lý Shift + Click - Chọn range từ lastSelectedIndex đến currentIndex
  CreatedBy: DDTOAN - (24/1/2026)
*/
const handleShiftSelection = (currentIndex) => {
  const startIndex = Math.min(lastSelectedIndex.value, currentIndex)
  const endIndex = Math.max(lastSelectedIndex.value, currentIndex)
  
  // Lấy tất cả IDs trong range
  const rangeIds = paginatedAssets.value
    .slice(startIndex, endIndex + 1)
    .map(asset => asset.id)
  
  // Thay thế selection hiện tại bằng range
  selectedAssets.value = rangeIds
}

/*
  Mô tả: Xử lý Shift + Click khi chưa có lastSelectedIndex - Chọn từ đầu đến currentIndex
  CreatedBy: DDTOAN - (24/1/2026)
*/
const handleShiftSelectionFromStart = (currentIndex) => {
  // Chọn từ đầu danh sách đến index hiện tại
  const rangeIds = paginatedAssets.value
    .slice(0, currentIndex + 1)
    .map(asset => asset.id)
  
  // Thay thế selection hiện tại bằng range
  selectedAssets.value = rangeIds
}

/*
  Mô tả: Xử lý Ctrl + Click - Toggle item mà không ảnh hưởng các item khác
  CreatedBy: DDTOAN - (24/1/2026)
*/
const handleCtrlSelection = (assetId) => {
  const index = selectedAssets.value.indexOf(assetId)
  if (index > -1) {
    // Đã được chọn → Bỏ chọn
    selectedAssets.value.splice(index, 1)
  } else {
    // Chưa được chọn → Thêm vào
    selectedAssets.value.push(assetId)
  }
}

/*
  Mô tả: Xử lý click thông thường - Toggle item
  CreatedBy: DDTOAN - (24/1/2026)
*/
const handleNormalSelection = (assetId) => {
  const index = selectedAssets.value.indexOf(assetId)
  if (index > -1) {
    // Đã được chọn → Bỏ chọn
    selectedAssets.value.splice(index, 1)
  } else {
    // Chưa được chọn → Thêm vào
    selectedAssets.value.push(assetId)
  }
}

const formatNumber = (num) => {
  // Xử lý trường hợp undefined, null hoặc không phải số
  if (num === null || num === undefined || isNaN(num)) {
    return '0'
  }
  return Number(num).toLocaleString('vi-VN')
}

/*
  Mô tả: Xử lý khi click nút "Thêm tài sản"
  - Gọi API lấy mã tài sản mới TRƯỚC KHI mở form
  - Truyền mã mới vào AssetForm thông qua selectedAssetData
  CreatedBy: DDToan - (09/1/2026)
  EditBy: DDToan - (16/1/2026) - Gọi API lấy mã mới trước khi mở form để đảm bảo mã luôn mới nhất
*/
/*
  Mô tả: Xử lý khi click nút "Thêm tài sản"
  EditBy: DDToan - (16/1/2026) - Cải thiện validation mã trước khi mở form
*/
const handleAddAsset = async () => {
  try {
    // Gọi API lấy mã mới TRƯỚC KHI mở form
    // getNewAssetCode() đã được cải thiện để luôn trả về string hợp lệ hoặc throw error
    const assetCode = await getNewAssetCode()
    
    // Validate mã: phải là string không rỗng
    if (!assetCode || typeof assetCode !== 'string' || assetCode.trim() === '') {
      console.error('handleAddAsset: Invalid asset code received:', assetCode)
      throw new Error('Không thể lấy mã tài sản mới. Vui lòng thử lại.')
    }
    
    // Tạo object với mã mới và flag isNew để AssetForm nhận biết đây là thêm mới
    selectedAssetData.value = {
      code: assetCode.trim(), // Mã mới từ API (đã được validate)
      isNew: true // Flag để phân biệt với edit mode
    }
    
    formTitle.value = 'Thêm tài sản'
    isFormOpen.value = true
  } catch (error) {
    console.error('Error getting new asset code:', error)
    // Fallback: Vẫn mở form nhưng để AssetForm tự sinh mã (retry logic)
    // AssetForm sẽ tự gọi API lại khi mở form
    selectedAssetData.value = {
      code: '', // Để AssetForm tự sinh mã
      isNew: true
    }
    formTitle.value = 'Thêm tài sản'
    isFormOpen.value = true
  }
}

/*
  Mô tả: Xử lý sửa tài sản - Mở form với dữ liệu tài sản
  CreatedBy: DDToan - (09/1/2026)
  EditBy: DDTOAN - (24/1/2026) - Đảm bảo asset có đầy đủ dữ liệu, tìm lại từ assets.value nếu cần
*/
const handleEdit = (asset) => {
  // Đảm bảo asset có đầy đủ dữ liệu
  // Nếu asset từ contextMenu không đầy đủ, tìm lại từ assets.value
  const fullAsset = assets.value.find(a => a.id === asset.id) || asset
  
  selectedAssetData.value = { ...fullAsset }
  formTitle.value = 'Sửa tài sản'
  isFormOpen.value = true
}

/*
  Mô tả: Xử lý nhân bản tài sản - Gọi API để lấy dữ liệu với mã mới
  CreatedBy: DDToan - (09/1/2026)
  EditBy: DDToan - (17/1/2026) - Tích hợp API nhân bản, xử lý lỗi và map dữ liệu
  EditBy: DDTOAN - (24/1/2026) - Đảm bảo asset được truyền vào đúng (từ contextMenu hoặc table action)
*/
const handleDuplicate = async (asset) => {
  try {
    // Đảm bảo master data đã được load (để map dữ liệu đúng)
    if (departments.value.length === 0 || assetCategories.value.length === 0) {
      await loadMasterData()
    }
    
    // Gọi API nhân bản để lấy dữ liệu với mã mới từ backend
    const clonedAsset = await cloneFixedAsset(asset.id)
    
    // Map từ API format sang frontend format
    const mappedAsset = mapAssetFromApi(clonedAsset)
    
    // Set dữ liệu để mở form (đã có mã mới và ID reset từ backend)
    // Đảm bảo ID được reset về null để AssetForm nhận biết đây là thêm mới (nhân bản)
    selectedAssetData.value = {
      ...mappedAsset,
      id: null, // Reset ID để đảm bảo AssetForm nhận biết đây là thêm mới, không phải edit
      isNew: true // Flag để phân biệt với edit mode
    }
    
    formTitle.value = 'Thêm tài sản'
    isFormOpen.value = true
  } catch (error) {
    console.error('Error cloning asset:', error)
    
    // Xử lý lỗi: hiển thị error message
    let errorMsg = 'Không thể nhân bản tài sản. Vui lòng thử lại sau.'
    if (error.message) {
      errorMsg = error.message
    } else if (error.userMsg) {
      errorMsg = error.userMsg
    } else if (error.response && error.response.userMsg) {
      errorMsg = error.response.userMsg
    }
    
    errorMessage.value = errorMsg
  }
}

/*
  Mô tả: Xử lý khi form save (legacy - giữ lại để tương thích)
  CreatedBy: DDToan - (09/1/2026)
  EditBy: DDToan - (14/1/2026) - Giữ lại để tương thích, logic thực tế đã chuyển sang handleAssetSaved
*/
const handleSaveAsset = (assetData) => {
  // Logic này đã được chuyển sang API call trong AssetForm
  // Giữ lại để tương thích nếu có component khác gọi
}

/*
  Mô tả: Xử lý sau khi save thành công - Reload danh sách từ API
  CreatedBy: DDToan - (14/1/2026)
  EditBy: DDToan - (14/1/2026) - Reload với trang hiện tại
*/
/*
  Mô tả: Xử lý sau khi save thành công - Reload danh sách từ API
  CreatedBy: DDToan - (14/1/2026)
  EditBy: DDToan - (14/1/2026) - Reload với trang hiện tại
  EditBy: DDToan - (16/1/2026) - Reset selectedAssetData để lần sau mở form sẽ lấy mã mới
  EditBy: DDToan - (17/1/2026) - Form đã được đóng trong AssetForm, chỉ cần reload danh sách
*/
const handleAssetSaved = async () => {
  // Reload danh sách tài sản từ API sau khi save thành công (giữ nguyên trang, search, filters và năm)
  await loadAssets(currentPage.value, pageSize.value, null, null, null, null)
  
  // Reset selectedAssetData để lần sau mở form sẽ lấy mã mới từ API
  selectedAssetData.value = null
}

const handleCancelForm = () => {
  selectedAssetData.value = null
}

// Computed: Message cho dialog xóa
// EditBy: DDTOAN - (18/1/2026) - Hiển thị số lượng khi xóa nhiều tài sản
const deleteDialogMessage = computed(() => {
  const count = selectedAssets.value.length
  
  if (count === 0) return ''
  
  if (count === 1) {
    // Xóa 1 tài sản: Hiển thị "Mã - Tên"
    if (!assetToDelete.value) return ''
    return `Bạn có muốn xóa tài sản ${assetToDelete.value.code} - ${assetToDelete.value.name} ?`
  } else {
    // Xóa nhiều tài sản: Hiển thị số lượng
    return `${count} tài sản đã được chọn. Bạn có muốn xóa các tài sản này khỏi danh sách?`
  }
})

// Computed: Buttons cho dialog xóa
const deleteDialogButtons = computed(() => [
  { label: 'Không', variant: 'outline', action: 'cancel' },
  { label: 'Xóa', variant: 'primary', action: 'confirm' },
])

// Computed: Buttons cho dialog xuất Excel
const exportDialogButtons = computed(() => [
  { label: 'Không', variant: 'outline', action: 'cancel' },
  { label: 'Xuất Excel', variant: 'primary', action: 'confirm' },
])

// Handler: Click button xóa
const handleDeleteClick = () => {
  if (selectedAssets.value.length === 0) return

  // Lấy tài sản đầu tiên được chọn để hiển thị trong dialog
  const firstSelectedId = selectedAssets.value[0]
  const asset = assets.value.find((a) => a.id === firstSelectedId)

  if (asset) {
    assetToDelete.value = asset
    showDeleteDialog.value = true
  }
}

/*
  Mô tả: Xử lý xác nhận xóa tài sản - Gọi API xóa và reload danh sách
  CreatedBy: DDToan - (09/1/2026)
  EditBy: DDToan - (17/1/2026) - Tích hợp API xóa, xử lý lỗi và reload danh sách sau khi xóa thành công
*/
const handleConfirmDelete = async () => {
  if (selectedAssets.value.length === 0) return

  try {
    loading.value = true
    errorMessage.value = ''

    // Gọi API xóa: xóa nhiều nếu > 1, xóa 1 nếu = 1
    if (selectedAssets.value.length > 1) {
      // Xóa nhiều tài sản
      await deleteMultipleFixedAssets(selectedAssets.value)
    } else {
      // Xóa 1 tài sản
      await deleteFixedAsset(selectedAssets.value[0])
    }

    // Xóa thành công → Reload danh sách từ API
    await loadAssets(currentPage.value, pageSize.value, null, null, null, null) // Giữ nguyên search, filters và năm

    // Reset state
    selectedAssets.value = []
    selectAll.value = false
    lastSelectedIndex.value = null // Reset lastSelectedIndex sau khi xóa
    showDeleteDialog.value = false
    assetToDelete.value = null

    // Điều chỉnh trang nếu cần (nếu trang hiện tại không còn bản ghi)
    if (paginatedAssets.value.length === 0 && currentPage.value > 1) {
      goToPage(currentPage.value - 1)
    }
  } catch (error) {
    console.error('Error deleting assets:', error)
    
    // Xử lý lỗi: lấy message từ error response
    let errorMsg = 'Có lỗi xảy ra khi xóa tài sản. Vui lòng thử lại sau.'
    if (error.message) {
      errorMsg = error.message
    } else if (error.userMsg) {
      errorMsg = error.userMsg
    } else if (error.response && error.response.userMsg) {
      errorMsg = error.response.userMsg
    }
    
    errorMessage.value = errorMsg
    
    // Không đóng dialog để user có thể thử lại hoặc hủy
    // showDeleteDialog.value = false // Comment lại để giữ dialog mở
  } finally {
    loading.value = false
  }
}

// Handler: Hủy xóa
const handleCancelDelete = () => {
  showDeleteDialog.value = false
  assetToDelete.value = null
}

/*
  Mô tả: Xử lý khi click button Excel - Hiển thị dialog xác nhận
  CreatedBy: DDToan - (17/1/2026)
*/
const handleExportExcelClick = () => {
  showExportDialog.value = true
}

/*
  Mô tả: Xử lý click button trong Export Dialog
  CreatedBy: DDToan - (17/1/2026)
*/
const handleExportDialogClick = (button) => {
  if (button.action === 'confirm') {
    // Xác nhận xuất Excel
    showExportDialog.value = false
    handleConfirmExport()
  } else if (button.action === 'cancel') {
    // Hủy
    showExportDialog.value = false
  }
}

/*
  Mô tả: Xử lý xuất Excel - Gọi API và download file
  CreatedBy: DDToan - (17/1/2026)
*/
const handleConfirmExport = async () => {
  try {
    loading.value = true
    errorMessage.value = ''
    
    // Lấy filters hiện tại để truyền lên API (bao gồm năm từ Header)
    const exportParams = {
      keyword: searchText.value || null,
      departmentId: selectedDepartment.value || null,
      categoryId: selectedAssetType.value || null,
      trackingYear: selectedYear.value || 0 // Sử dụng năm từ Header, mặc định 0 = lấy tất cả
    }
    
    // Gọi API xuất Excel
    const blob = await exportFixedAssetsToExcel(exportParams)
    
    // Tạo URL từ Blob và trigger download
    const url = window.URL.createObjectURL(blob)
    const link = document.createElement('a')
    link.href = url
    
    // Tạo tên file với timestamp
    const now = new Date()
    const day = String(now.getDate()).padStart(2, '0')
    const month = String(now.getMonth() + 1).padStart(2, '0')
    const year = now.getFullYear()
    const hours = String(now.getHours()).padStart(2, '0')
    const minutes = String(now.getMinutes()).padStart(2, '0')
    link.download = `Danh_sach_tai_san_${day}${month}${year}_${hours}${minutes}.xlsx`
    
    // kích hoạt tải xuống
    document.body.appendChild(link)
    link.click()
    
    // Xóa link sau khi tải xuống
    document.body.removeChild(link)
    window.URL.revokeObjectURL(url) //giải phóng URL tạm để tránh rò rỉ bộ nhớ
    
  } catch (error) {
    console.error('Error exporting Excel:', error)
    
    // Xử lý lỗi
    let errorMsg = 'Không thể xuất file Excel. Vui lòng thử lại sau.'
    if (error.message) {
      errorMsg = error.message
    } else if (error.userMsg) {
      errorMsg = error.userMsg
    } else if (error.response && error.response.userMsg) {
      errorMsg = error.response.userMsg
    }
    
    errorMessage.value = errorMsg
  } finally {
    loading.value = false
  }
}

// Computed: Danh sách items cho context menu
/*
  Mô tả: Danh sách items cho context menu
  CreatedBy: DDToan - (09/1/2026)
  EditBy: DDTOAN - (24/1/2026) - Sửa lỗi timing issue: Lưu asset vào biến local để tránh bị null khi action được gọi
*/
const contextMenuItems = computed(() => {
  if (!contextMenuAsset.value) return []
  
  // Lưu asset vào biến local để tránh bị null khi action được gọi
  // (vì handleContextMenuSelect reset contextMenuAsset.value = null trước khi action chạy)
  const asset = contextMenuAsset.value
  
  return [
    {
      label: 'Thêm',
      icon: 'add',
      action: () => handleAddAsset()
    },
    {
      label: 'Sửa',
      icon: 'edit',
      action: () => {
        // Tìm asset đầy đủ từ assets.value để đảm bảo có đầy đủ dữ liệu cho form
        const fullAsset = assets.value.find(a => a.id === asset.id)
        if (fullAsset) {
          handleEdit(fullAsset)
        } else {
          // Fallback: Dùng asset từ contextMenu nếu không tìm thấy
          handleEdit(asset)
        }
      }
    },
    {
      label: 'Nhân bản',
      icon: 'duplicate',
      action: () => handleDuplicate(asset)
    },
    {
      label: 'Xóa',
      icon: 'delete',
      action: () => {
        // Set selectedAssets với ID từ asset đã capture
        selectedAssets.value = [asset.id]
        
        // Tìm asset đầy đủ từ assets.value để hiển thị trong dialog
        const fullAsset = assets.value.find(a => a.id === asset.id)
        if (fullAsset) {
          assetToDelete.value = fullAsset
        } else {
          // Fallback: Dùng asset từ contextMenu
          assetToDelete.value = asset
        }
        
        handleDeleteClick()
      }
    }
  ]
})

// Hàm xử lý hiển thị context menu
const handleContextMenu = (event, asset, index) => {
  contextMenuX.value = event.clientX
  contextMenuY.value = event.clientY
  contextMenuAsset.value = asset
  showContextMenu.value = true
}

// Hàm xử lý khi chọn item trong context menu
const handleContextMenuSelect = (item) => {
  // Action đã được gọi trong item.action
  showContextMenu.value = false
  contextMenuAsset.value = null
}
</script>

<style scoped>
/*
  Mô tả: Styles cho AssetList view
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: DDTOAN - (18/1/2026) - thêm styles cho resize handles và responsive
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
  gap: 0;
}

.asset-list__container {
  display: flex;
  flex-direction: column;
  flex: 1;
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
  padding: 16px 0px;
  background-color: transparent;
  margin-bottom: 0;
  margin-left: 0;
  margin-right: 0;
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
  background: url('data:image/svg+xml;utf8,<svg xmlns="http://www.w3.org/2000/svg" width="10" height="6"><path d="M1 1l4 4 4-4" stroke="%23999" fill="none" stroke-width="1.2"/></svg>')
    no-repeat right 12px center;
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

/* Icon chevron cho dropdown trong toolbar - ẩn icon mặc định */
.toolbar__dropdown :deep(.ms-dropdown__arrow) {
  display: none;
}

/* Icon chevron-down khi đóng - căn giữa */
.toolbar__dropdown :deep(.ms-dropdown__trigger)::after {
  content: '';
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  width: 8px;
  height: 5px;
  pointer-events: none;
  -webkit-mask-image: url('@/assets/icons/qlts-icon.svg');
  mask-image: url('@/assets/icons/qlts-icon.svg');
  -webkit-mask-size: 504px 754px;
  mask-size: 504px 754px;
  -webkit-mask-repeat: no-repeat;
  mask-repeat: no-repeat;
  -webkit-mask-position: -72px -338px; /* icon-chevron-down-toolbar - mặc định */
  mask-position: -72px -338px;
  background-color: #595959;
}

/* Icon chevron-up khi mở - tại đúng vị trí của chevron-down (căn giữa) */
.toolbar__dropdown :deep(.ms-dropdown--open .ms-dropdown__trigger)::after {
  -webkit-mask-position: -28px -338px !important; /* icon-chevron-up-toolbar */
  mask-position: -28px -338px !important;
}

/* Đảm bảo override cho cả 2 dropdown cụ thể với specificity cao hơn */
.toolbar__dropdown--asset-type :deep(.ms-dropdown--open .ms-dropdown__trigger)::after,
.toolbar__dropdown--department :deep(.ms-dropdown--open .ms-dropdown__trigger)::after {
  -webkit-mask-position: -28px -338px !important; /* icon-chevron-up-toolbar */
  mask-position: -28px -338px !important;
}

/* Thêm selector với class cụ thể để tăng specificity */
.toolbar__dropdown.toolbar__dropdown--asset-type
  :deep(.ms-dropdown.ms-dropdown--open .ms-dropdown__trigger)::after,
.toolbar__dropdown.toolbar__dropdown--department
  :deep(.ms-dropdown.ms-dropdown--open .ms-dropdown__trigger)::after {
  -webkit-mask-position: -28px -338px !important;
  mask-position: -28px -338px !important;
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
  background-color: #1aa4c8;
  color: #ffffff;
  border-color: #1aa4c8;
  border-radius: 3px;
  box-shadow: inset 0 2px 6px rgba(0, 0, 0, 0.16);
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
  position: relative;
  display: flex;
  flex-direction: column;
}

.asset-table {
  width: 100%;
  border-collapse: separate;
  border-spacing: 0;
  table-layout: fixed; /* Fix width cố định cho các cột */
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

.asset-table__th--code {
  width: 120px;
  min-width: 120px;
  max-width: 120px;
}

.asset-table__th--name {
  width: 150px;
  min-width: 150px;
  max-width: 150px;
}

.asset-table__th--type {
  width: 140px;
  min-width: 140px;
  max-width: 140px;
}

.asset-table__th--department {
  width: 160px;
  min-width: 160px;
  max-width: 160px;
}

.asset-table__th--number {
  text-align: right;
  padding-right: 16px;
}

.asset-table__th--quantity {
  width: 80px;
  min-width: 80px;
  max-width: 80px;
}

.asset-table__th--cost {
  width: 120px;
  min-width: 120px;
  max-width: 120px;
}

.asset-table__th--depreciation {
  width: 120px;
  min-width: 120px;
  max-width: 120px;
}

.asset-table__th--remaining {
  width: 120px;
  min-width: 120px;
  max-width: 120px;
}

.asset-table__th--actions {
  width: 96px;
  min-width: 96px;
  max-width: 96px;
  text-align: center;
}


/* COLUMN RESIZE HANDLES                        */
/* CreatedBy: DDTOAN - (18/1/2026)       */


.asset-table__th {
  position: relative;
}

.asset-table__resize-handle {
  position: absolute;
  top: 0;
  right: 0;
  width: 4px;
  height: 100%;
  cursor: col-resize;
  background-color: transparent;
  z-index: 1;
  user-select: none;
  transition: background-color 0.2s ease;
}

.asset-table__resize-handle:hover {
  background-color: #1aa4c8;
}

.asset-table__th--resizing {
  user-select: none;
}

.asset-table__th--resizing .asset-table__resize-handle {
  background-color: #1aa4c8;
}

/* Ẩn resize handle trên mobile để đảm bảo responsive */
@media (max-width: 768px) {
  .asset-table__resize-handle {
    display: none;
  }
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

.asset-table__row--focused {
  outline: 2px solid #1aa4c8;
  outline-offset: -2px;
}

.asset-table__row--focused:not(.asset-table__row--selected) {
  background-color: #f0fafc;
}

.asset-table__td {
  padding: 12px;
  font-size: 13px; /* Đổi từ 14px → 13px */
  color: #182026;
  border-bottom: 1px solid #e8e8e8;
  line-height: 18px;
  vertical-align: middle;
  height: 40px;
  box-sizing: border-box;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.asset-table__td--checkbox {
  text-align: center;
  padding: 12px 8px;
  overflow: visible; /* Checkbox không cần ellipsis */
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
.asset-table input[type='checkbox'] {
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
.asset-table__checkbox-wrapper input[type='checkbox']:checked ~ .asset-table__checkbox-check,
.asset-table__checkbox-wrapper input[type='checkbox']:checked + .asset-table__checkbox-check {
  opacity: 1;
}

.asset-table__td--stt {
  text-align: center;
  color: #595959;
  font-weight: 400;
  overflow: visible; /* STT không cần ellipsis */
}

.asset-table__td--code {
  width: 120px;
  min-width: 120px;
  max-width: 120px;
}

.asset-table__td--name {
  width: 150px;
  min-width: 150px;
  max-width: 150px;
}

.asset-table__td--type {
  width: 140px;
  min-width: 140px;
  max-width: 140px;
}

.asset-table__td--department {
  width: 160px;
  min-width: 160px;
  max-width: 160px;
}

.asset-table__td--number {
  text-align: right;
  font-family: 'Roboto', sans-serif;
  font-variant-numeric: tabular-nums;
  padding-right: 16px;
}

.asset-table__td--quantity {
  width: 80px;
  min-width: 80px;
  max-width: 80px;
  text-align: right;
  font-family: 'Roboto', sans-serif;
  font-variant-numeric: tabular-nums;
  padding-right: 16px;
}

.asset-table__td--cost {
  width: 120px;
  min-width: 120px;
  max-width: 120px;
}

.asset-table__td--depreciation {
  width: 120px;
  min-width: 120px;
  max-width: 120px;
}

.asset-table__td--remaining {
  width: 120px;
  min-width: 120px;
  max-width: 120px;
}

.asset-table__td--actions {
  text-align: center;
  padding: 4px 8px;
  overflow: visible; /* Actions không cần ellipsis */
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
/* TABLE FOOTER (Pagination Row)                 */
/* ============================================ */

.asset-table__foot {
  background-color: #ffffff;
  margin: 0;
  padding: 0;
}

.asset-table__row--pagination {
  background-color: #ffffff;
  margin: 0;
}

.asset-table__row--pagination:hover {
  background-color: rgba(26, 164, 200, 0.08);
}

.asset-table__row--pagination .asset-table__td {
  position: sticky;
  bottom: 0;
  z-index: 5;
  border-top: none;
  border-bottom: 1px solid #e8e8e8;
  background-color: #ffffff;
  height: 40px;
  min-height: 40px;
  max-height: 40px;
  line-height: 18px;
  vertical-align: middle;
  padding: 12px;
  box-sizing: border-box;
}

/* Pagination Controls Section */
.asset-table__td--pagination-controls {
  padding: 12px 16px !important;
  height: 40px;
  line-height: 18px;
  vertical-align: middle;
}

.asset-table__pagination-container {
  display: flex;
  align-items: center;
  justify-content: flex-start;
  gap: 16px;
}

.asset-table__pagination-left {
  display: flex;
  align-items: center;
  gap: 8px;
  flex-shrink: 0;
}

.asset-table__pagination-info {
  font-size: 14px;
  color: #595959;
  white-space: nowrap;
}

.asset-table__pagination-info strong {
  color: #182026;
  font-weight: 600;
}

.asset-table__pagination-size {
  display: flex;
  align-items: center;
}

.asset-table__pagination-dropdown {
  width: auto;
  min-width: 80px;
  margin-left: 15px;
}

.asset-table__pagination-dropdown :deep(.ms-dropdown__trigger) {
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
}

.asset-table__pagination-dropdown :deep(.ms-dropdown__arrow) {
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
  -webkit-mask-position: -72px -338px;
  mask-position: -72px -338px;
  background-color: #595959;
}

.asset-table__pagination-dropdown :deep(.ms-dropdown--open .ms-dropdown__arrow) {
  -webkit-mask-position: -28px -338px !important;
  mask-position: -28px -338px !important;
}

.asset-table__pagination-dropdown :deep(.ms-dropdown__menu) {
  top: auto !important;
  bottom: calc(100% + 4px) !important;
  z-index: 9999 !important;
  min-width: 90px !important;
}

.asset-table__pagination-dropdown :deep(.ms-dropdown__menu--teleported) {
  min-width: 90px !important;
}

/* Pagination Navigation */
.asset-table__pagination-nav {
  display: flex;
  gap: 4px;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.asset-table__pagination-btn {
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

.asset-table__pagination-btn .icon {
  display: inline-block;
  flex-shrink: 0;
}

.asset-table__pagination-btn:hover:not(:disabled) {
  color: #0095da;
}

.asset-table__pagination-btn--active {
  background-color: #e6e6e6;
  border: none;
  color: #182026;
  font-weight: 400;
}

.asset-table__pagination-btn:disabled {
  opacity: 0.3;
  cursor: not-allowed;
  color: #bfbfbf;
}

.asset-table__pagination-btn--ellipsis {
  border: none;
  background: transparent;
  cursor: default;
  opacity: 1;
  color: #182026;
  min-width: auto;
  padding: 0 4px;
}

.asset-table__pagination-btn--ellipsis .icon {
  display: inline-block;
  flex-shrink: 0;
}

.asset-table__pagination-btn--ellipsis:hover {
  border: none;
  color: #182026;
}

/* Totals in pagination row */
.asset-table__total-item {
  font-family: 'Roboto', sans-serif;
  font-variant-numeric: tabular-nums;
  font-size: 14px;
  font-weight: 700;
  color: #182026;
  white-space: nowrap;
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

/* ============================================ */
/* LOADING & ERROR                              */
/* ============================================ */

.asset-list__loading {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 40px;
  background-color: #ffffff;
  border-radius: 4px;
  border: 1px solid #e8e8e8;
  margin-bottom: 16px;
}

.loading-spinner {
  font-size: 14px;
  color: #595959;
}

.asset-list__error {
  padding: 16px;
  background-color: #fff2f0;
  border: 1px solid #ffccc7;
  border-radius: 4px;
  margin-bottom: 16px;
}

.error-message {
  color: #ff4d4f;
  font-size: 14px;
}

/* Empty state styling */
.asset-table__row--empty {
  height: 200px;
}

.asset-table__td--empty {
  text-align: center;
  vertical-align: middle;
  padding: 40px 20px;
}

.asset-table__empty-message {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 8px;
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
