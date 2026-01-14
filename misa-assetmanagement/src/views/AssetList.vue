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
        <button class="btn btn--outline" title="Xuất Excel">
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

    <!-- Loading Indicator -->
    <div v-if="loading" class="asset-list__loading">
      <div class="loading-spinner">Đang tải dữ liệu...</div>
    </div>

    <!-- Error Message -->
    <div v-if="errorMessage && !loading" class="asset-list__error">
      <div class="error-message">{{ errorMessage }}</div>
    </div>

    <!-- Table Container -->
    <div class="asset-list__container" v-if="!loading">
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
              <th class="asset-table__th asset-table__th--code">Mã tài sản</th>
              <th class="asset-table__th asset-table__th--name">Tên tài sản</th>
              <th class="asset-table__th asset-table__th--type">Loại tài sản</th>
              <th class="asset-table__th asset-table__th--department">Bộ phận sử dụng</th>
              <th class="asset-table__th asset-table__th--number asset-table__th--quantity">Số lượng</th>
              <th class="asset-table__th asset-table__th--number asset-table__th--cost">Nguyên giá</th>
              <th class="asset-table__th asset-table__th--number asset-table__th--depreciation">HM/KH lũy kế</th>
              <th class="asset-table__th asset-table__th--number asset-table__th--remaining">Giá trị còn lại</th>
              <th class="asset-table__th asset-table__th--actions">Chức năng</th>
            </tr>
          </thead>
          <tbody class="asset-table__body">
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
                  <input type="checkbox" :value="asset.id" v-model="selectedAssets" />
                  <span class="asset-table__checkbox-check"></span>
                </label>
              </td>
              <td class="asset-table__td asset-table__td--stt">
                {{ (currentPage - 1) * pageSize + index + 1 }}
              </td>
              <td class="asset-table__td asset-table__td--code" :title="asset.code">{{ asset.code }}</td>
              <td class="asset-table__td asset-table__td--name" :title="asset.name">{{ asset.name }}</td>
              <td class="asset-table__td asset-table__td--type" :title="asset.type">{{ asset.type }}</td>
              <td class="asset-table__td asset-table__td--department" :title="asset.department">{{ asset.department }}</td>
              <td class="asset-table__td asset-table__td--number asset-table__td--quantity">{{ asset.quantity }}</td>
              <td class="asset-table__td asset-table__td--number asset-table__td--cost">
                {{ formatNumber(asset.cost) }}
              </td>
              <td class="asset-table__td asset-table__td--number asset-table__td--depreciation">
                {{ formatNumber(asset.depreciation) }}
              </td>
              <td class="asset-table__td asset-table__td--number asset-table__td--remaining">
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
                <span class="asset-table__total-item">{{ selectedTotals.quantity }}</span>
              </td>
              <td class="asset-table__td asset-table__td--number">
                <span class="asset-table__total-item">{{ formatNumber(selectedTotals.cost) }}</span>
              </td>
              <td class="asset-table__td asset-table__td--number">
                <span class="asset-table__total-item">{{
                  formatNumber(selectedTotals.depreciation)
                }}</span>
              </td>
              <td class="asset-table__td asset-table__td--number">
                <span class="asset-table__total-item">{{
                  formatNumber(selectedTotals.remainingValue)
                }}</span>
              </td>
              <td class="asset-table__td asset-table__td--actions"></td>
            </tr>
          </tfoot>
        </table>
      </div>

      <!-- Asset Form Modal -->
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
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: DDToan - (11/1/2026) - chuyển pagination logic sang composable
*/

import { ref, computed, onMounted, nextTick, watch } from 'vue'
import { MOCK_ASSETS } from '@/constants/assetData'
import AssetForm from './AssetForm.vue'
import MsDropdown from '@/components/base/ms-dropdown/MsDropdown.vue'
import MsDialog from '@/components/base/ms-dialog/MsDialog.vue'
import MsContextMenu from '@/components/base/ms-context-menu/MsContextMenu.vue'
import { usePagination } from '@/composables/usePagination'
import { useKeyboardNavigation } from '@/composables/useKeyboardNavigation'
import { getFixedAssets } from '@/api/fixedAssetApi'
import { getDepartments } from '@/api/departmentApi'
import { getFixedAssetCategories } from '@/api/fixedAssetCategoryApi'

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
const showContextMenu = ref(false)
const contextMenuX = ref(0)
const contextMenuY = ref(0)
const contextMenuAsset = ref(null)

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
    yearsOfUse: apiAsset.fixedAssetLifeTime || apiAsset.fixed_asset_life_time || apiAsset.yearsOfUse || 0,
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
 * Load danh sách tài sản từ API với pagination
 * @param {number} page - Số trang (mặc định: currentPage.value)
 * @param {number} size - Số bản ghi mỗi trang (mặc định: pageSize.value)
 */
const loadAssets = async (page = null, size = null) => {
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
    
    // Gọi API lấy danh sách tài sản với pagination params
    const response = await getFixedAssets({
      page: currentPageNum,
      pageSize: currentPageSize
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
    
    // Nếu không có dữ liệu, fallback về mock data (để test UI)
    if (assets.value.length === 0 && total === 0) {
      console.warn('No data from API, using mock data for testing')
      assets.value = MOCK_ASSETS.map(mapAssetFromApi)
      totalRecordsFromBackend.value = MOCK_ASSETS.length
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
    loadAssets(newPage, pageSize.value)
  }
})

// Watch: Khi pageSize thay đổi, gọi lại API với pageSize mới
// Lưu ý: setPageSize trong composable đã reset về trang 1, nên sẽ gọi với page = 1
watch(pageSize, (newSize) => {
  if (newSize > 0) {
    loadAssets(currentPage.value, newSize)
  }
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

const selectedTotals = computed(() => {
  const selected = assets.value.filter((asset) => selectedAssets.value.includes(asset.id))
  return {
    quantity: selected.reduce((sum, asset) => sum + (Number(asset.quantity) || 0), 0),
    cost: selected.reduce((sum, asset) => sum + (Number(asset.cost) || 0), 0),
    depreciation: selected.reduce((sum, asset) => sum + (Number(asset.depreciation) || 0), 0),
    remainingValue: selected.reduce((sum, asset) => sum + (Number(asset.remainingValue) || 0), 0),
  }
})

// Methods
const toggleSelectAll = () => {
  if (selectAll.value) {
    selectedAssets.value = paginatedAssets.value.map((a) => a.id)
  } else {
    selectedAssets.value = []
  }
}

const formatNumber = (num) => {
  // Xử lý trường hợp undefined, null hoặc không phải số
  if (num === null || num === undefined || isNaN(num)) {
    return '0'
  }
  return Number(num).toLocaleString('vi-VN')
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
const handleAssetSaved = async () => {
  // Reload danh sách tài sản từ API sau khi save thành công (giữ nguyên trang hiện tại)
  await loadAssets(currentPage.value, pageSize.value)
}

const handleCancelForm = () => {
  selectedAssetData.value = null
}

// Computed: Message cho dialog xóa
const deleteDialogMessage = computed(() => {
  if (!assetToDelete.value) return ''
  return `Bạn có muốn xóa tài sản ${assetToDelete.value.code} - ${assetToDelete.value.name} ?`
})

// Computed: Buttons cho dialog xóa
const deleteDialogButtons = computed(() => [
  { label: 'Không', variant: 'outline', action: 'cancel' },
  { label: 'Xóa', variant: 'primary', action: 'confirm' },
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

// Handler: Xác nhận xóa
const handleConfirmDelete = () => {
  if (selectedAssets.value.length === 0) return

  // Xóa tất cả các tài sản đã chọn
  selectedAssets.value.forEach((assetId) => {
    const index = assets.value.findIndex((a) => a.id === assetId)
    if (index > -1) {
      assets.value.splice(index, 1)
    }
  })

  // Xóa tất cả khỏi danh sách selected
  selectedAssets.value = []

  // Reset selectAll
  selectAll.value = false

  // Điều chỉnh trang nếu cần (nếu trang hiện tại không còn bản ghi)
  if (paginatedAssets.value.length === 0 && currentPage.value > 1) {
    goToPage(currentPage.value - 1)
  }

  // Đóng dialog và reset
  showDeleteDialog.value = false
  assetToDelete.value = null
}

// Handler: Hủy xóa
const handleCancelDelete = () => {
  showDeleteDialog.value = false
  assetToDelete.value = null
}

// Computed: Context Menu Items
const contextMenuItems = computed(() => {
  if (!contextMenuAsset.value) return []
  
  return [
    {
      label: 'Thêm',
      icon: 'add',
      action: () => handleAddAsset()
    },
    {
      label: 'Sửa',
      icon: 'edit',
      action: () => handleEdit(contextMenuAsset.value)
    },
    {
      label: 'Nhân bản',
      icon: 'duplicate',
      action: () => handleDuplicate(contextMenuAsset.value)
    },
    {
      label: 'Xóa',
      icon: 'delete',
      action: () => {
        if (contextMenuAsset.value) {
          selectedAssets.value = [contextMenuAsset.value.id]
          handleDeleteClick()
        }
      }
    }
  ]
})

// Handler: Context Menu
const handleContextMenu = (event, asset, index) => {
  contextMenuX.value = event.clientX
  contextMenuY.value = event.clientY
  contextMenuAsset.value = asset
  showContextMenu.value = true
}

// Handler: Context Menu Select
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
  min-width: 60px;
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
