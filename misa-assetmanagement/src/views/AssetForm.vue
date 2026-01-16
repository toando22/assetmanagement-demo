<!--
  Mô tả: Form thêm mới/sửa tài sản
  Features:
  - Form 2-3 cột với các trường thông tin tài sản
  - Validation đầy đủ với MsDialog
  - Auto-fill từ dropdown selections
  - Auto-calculation (Hao mòn năm)
  - Format số với dấu chấm
  - Unsaved changes tracking
  - TabOrder đúng
  - Resize form
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: DDToan - (11/1/2026) - Nâng cấp với validation, auto-calculation, dialogs
-->

<template>
  <div>
    <MsPopup
      :model-value="modelValue"
      :title="title"
      width="870px"
      :close-on-overlay="false"
      :show-default-footer="false"
      @update:model-value="handlePopupClose"
      @close="handlePopupClose"
    >
      <template #footer>
        <MsButton variant="secondary" @click="handleCancel">Hủy</MsButton>
        <MsButton variant="primary" @click="handleSave">Lưu</MsButton>
      </template>

      <div class="asset-form" style="padding-top: 0;">
        <div class="asset-form__content">
          <!-- Row 1: Mã tài sản, Tên tài sản -->
          <div class="asset-form__row">
            <div class="asset-form__field">
              <MsInput
                v-model="formData.assetCode"
                label="Mã tài sản"
                placeholder="Nhập mã tài sản"
                :required="true"
                tabindex="1"
                :error="fieldErrors.assetCode"
                :error-message="fieldErrors.assetCode"
                @blur="handleAssetCodeBlur"
              />
            </div>
            <div class="asset-form__field">
              <MsInput
                v-model="formData.assetName"
                label="Tên tài sản"
                placeholder="Nhập tên tài sản"
                :required="true"
                tabindex="2"
                :error="fieldErrors.assetName"
                :error-message="fieldErrors.assetName"
              />
            </div>
          </div>

          <!-- Row 2: Mã bộ phận sử dụng, Tên phòng ban -->
          <div class="asset-form__row">
            <div class="asset-form__field">
              <MsDropdownField
                v-model="formData.departmentCode"
                label="Mã bộ phận sử dụng"
                placeholder="Chọn mã bộ phận sử dụng"
                :options="departmentOptions"
                :required="true"
                tabindex="3"
                @change="handleDepartmentChange"
              />
            </div>
            <div class="asset-form__field">
              <MsInput
                v-model="formData.departmentName"
                label="Tên bộ phận sử dụng"
                placeholder=""
                :disabled="true"
                tabindex="-1"
              />
            </div>
          </div>

          <!-- Row 3: Mã loại tài sản, Tên loại tài sản -->
          <div class="asset-form__row">
            <div class="asset-form__field">
              <MsDropdownField
                v-model="formData.assetTypeCode"
                label="Mã loại tài sản"
                placeholder="Chọn mã loại tài sản"
                :options="assetTypeOptions"
                :required="true"
                tabindex="4"
                @change="handleAssetTypeChange"
              />
            </div>
            <div class="asset-form__field">
              <MsInput
                v-model="formData.assetTypeName"
                label="Tên loại tài sản"
                placeholder=""
                :disabled="true"
                tabindex="-1"
              />
            </div>
          </div>

          <!-- Row 4: Số lượng, Nguyên giá, Tỷ lệ hao mòn -->
          <div class="asset-form__row asset-form__row--three">
            <div class="asset-form__field">
              <MsInputNumber
                v-model="formData.quantity"
                label="Số lượng"
                placeholder="01"
                :required="true"
                :min="1"
                :step="1"
                tabindex="5"
                :error="fieldErrors.quantity"
                :error-message="fieldErrors.quantity"
              />
            </div>
            <div class="asset-form__field">
              <MsInputCurrency
                v-model="formData.originalCost"
                label="Nguyên giá"
                placeholder="0"
                :required="true"
                tabindex="6"
                :error="fieldErrors.originalCost"
                :error-message="fieldErrors.originalCost"
              />
            </div>
            <div class="asset-form__field">
              <MsInput
                v-model="formData.depreciationRate"
                label="Tỷ lệ hao mòn (%)"
                placeholder="Nhập tỷ lệ hao mòn"
                :required="true"
                tabindex="7"
                :error="fieldErrors.depreciationRate"
                :error-message="fieldErrors.depreciationRate"
              />
            </div>
          </div>

          <!-- Row 5: Ngày mua, Ngày bắt đầu sử dụng, Năm theo dõi -->
          <div class="asset-form__row asset-form__row--three">
            <div class="asset-form__field">
              <MsInputDate
                v-model="formData.purchaseDate"
                label="Ngày mua"
                :required="true"
                tabindex="8"
              />
            </div>
            <div class="asset-form__field">
              <MsInputDate
                v-model="formData.startUseDate"
                label="Ngày bắt đầu sử dụng"
                :required="true"
                tabindex="9"
              />
            </div>
            <div class="asset-form__field">
              <MsInput
                v-model="formData.trackingYear"
                label="Năm theo dõi"
                placeholder=""
                :disabled="true"
                tabindex="-1"
              />
            </div>
          </div>

          <!-- Row 6: Số năm sử dụng, Giá trị hao mòn năm -->
          <div class="asset-form__row">
            <div class="asset-form__field">
              <MsInputNumber
                v-model="formData.yearsOfUse"
                label="Số năm sử dụng"
                placeholder="0"
                :required="true"
                :min="1"
                :step="1"
                tabindex="10"
                :error="fieldErrors.yearsOfUse"
                :error-message="fieldErrors.yearsOfUse"
              />
            </div>
            <div class="asset-form__field">
              <MsInputCurrency
                v-model="formData.annualDepreciationValue"
                label="Giá trị hao mòn năm"
                placeholder="0"
                :required="true"
                tabindex="11"
                :error="fieldErrors.annualDepreciationValue"
                :error-message="fieldErrors.annualDepreciationValue"
              />
            </div>
          </div>
        </div>
      </div>
    </MsPopup>

    <!-- Error Dialog -->
    <MsDialog
      v-model="showErrorDialog"
      type="error"
      :message="errorMessage"
      :buttons="[{ label: 'Đóng', variant: 'primary', action: 'close' }]"
      @close="showErrorDialog = false"
    />

    <!-- Success Dialog -->
    <MsDialog
      v-model="showSuccessDialog"
      type="success"
      message="Lưu dữ liệu thành công"
      :buttons="[]"
    />

    <!-- Cancel Confirm Dialog -->
    <MsDialog
      v-model="showCancelConfirmDialog"
      type="confirm"
      message="Bạn có muốn hủy bỏ khai báo tài sản này?"
      :buttons="[
        { label: 'Không', variant: 'secondary', action: 'cancel' },
        { label: 'Hủy bỏ', variant: 'primary', action: 'confirm' }
      ]"
      @button-click="handleCancelDialogClick"
    />

    <!-- Unsaved Changes Dialog -->
    <MsDialog
      v-model="showUnsavedDialog"
      type="warning"
      message="Thông tin thay đổi sẽ không được cập nhật nếu bạn không lưu. Bạn có muốn lưu các thay đổi này?"
      :buttons="[
        { label: 'Hủy bỏ', variant: 'secondary', action: 'cancel' },
        { label: 'Không lưu', variant: 'secondary', action: 'discard' },
        { label: 'Lưu', variant: 'primary', action: 'save' }
      ]"
      @button-click="handleUnsavedDialogClick"
    />
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho AssetForm component
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: DDToan - (11/1/2026) - Nâng cấp với validation, auto-calculation, dialogs
*/

import { ref, watch, computed } from 'vue'
import MsPopup from '@/components/base/ms-popup/MsPopup.vue'
import MsButton from '@/components/base/ms-button/MsButton.vue'
import MsInput from '@/components/base/ms-input/MsInput.vue'
import MsInputNumber from '@/components/base/ms-input-number/MsInputNumber.vue'
import MsInputDate from '@/components/base/ms-input-date/MsInputDate.vue'
import MsInputCurrency from '@/components/base/ms-input-currency/MsInputCurrency.vue'
import MsDropdownField from '@/components/base/ms-dropdown-field/MsDropdownField.vue'
import MsDialog from '@/components/base/ms-dialog/MsDialog.vue'
import { DEPARTMENTS, ASSET_TYPES, getDepartmentByCode, getAssetTypeByCode, MOCK_ASSETS } from '@/constants/assetData'
import { formatNumber, parseNumber, formatDate, parseDate, getYearFromDate } from '@/utils/format'
import { validateAssetForm } from '@/utils/validate'
import { getDepartments } from '@/api/departmentApi'
import { getFixedAssetCategories } from '@/api/fixedAssetCategoryApi'
import { getNewAssetCode, createFixedAsset, updateFixedAsset } from '@/api/fixedAssetApi'

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  title: {
    type: String,
    default: 'Thêm tài sản'
  },
  assetData: {
    type: Object,
    default: () => null
  },
  existingAssets: {
    type: Array,
    default: () => []
  }
})

const emit = defineEmits(['update:modelValue', 'save', 'cancel', 'saved'])

// Form data
const formData = ref({
  assetCode: '',
  assetName: '',
  departmentCode: '',
  departmentName: '',
  assetTypeCode: '',
  assetTypeName: '',
  quantity: 1,
  originalCost: 0,
  depreciationRate: '',
  purchaseDate: '',
  startUseDate: '',
  trackingYear: '',
  yearsOfUse: 0,
  annualDepreciationValue: 0
})

// Initial form data để track unsaved changes
const initialFormData = ref(null)

// Field errors
const fieldErrors = ref({})

// Dialog states
const showErrorDialog = ref(false)
const showSuccessDialog = ref(false)
const showCancelConfirmDialog = ref(false)
const showUnsavedDialog = ref(false)
const errorMessage = ref('')

// State: Danh sách departments từ API
const departments = ref([])

// Map để lookup department theo code (dùng cho auto-fill)
const departmentMap = ref(new Map())

/*
  Mô tả: Load danh sách departments từ API
  CreatedBy: DDToan - (14/1/2026)
*/
const loadDepartments = async () => {
  try {
    const response = await getDepartments()
    
    // Xử lý response - có thể là array hoặc object có data property
    let apiDepartments = []
    if (Array.isArray(response)) {
      apiDepartments = response
    } else if (response && Array.isArray(response.data)) {
      apiDepartments = response.data
    } else if (response && response.data) {
      apiDepartments = [response.data]
    }
    
    // Lưu vào state
    departments.value = apiDepartments
    
    // Tạo map để lookup nhanh theo code
    departmentMap.value.clear()
    apiDepartments.forEach(dept => {
      const code = dept.departmentCode || dept.department_code || dept.code
      const name = dept.departmentName || dept.department_name || dept.name
      if (code) {
        departmentMap.value.set(code, { code, name })
      }
    })
  } catch (error) {
    console.error('Error loading departments:', error)
    // Fallback về constants nếu API lỗi
    departments.value = DEPARTMENTS.map(dept => ({
      departmentCode: dept.code,
      departmentName: dept.name
    }))
    // Tạo map từ constants
    departmentMap.value.clear()
    DEPARTMENTS.forEach(dept => {
      departmentMap.value.set(dept.code, { code: dept.code, name: dept.name })
    })
  }
}

// Dropdown options từ API (thay thế constants)
const departmentOptions = computed(() => {
  if (departments.value.length > 0) {
    // Dùng data từ API
    return departments.value.map(dept => ({
      value: dept.departmentCode || dept.department_code || dept.code,
      label: dept.departmentCode || dept.department_code || dept.code
    }))
  } else {
    // Fallback về constants nếu chưa load được
    return DEPARTMENTS.map(dept => ({
      value: dept.code,
      label: dept.code
    }))
  }
})

// State: Danh sách fixed asset categories từ API
const assetCategories = ref([])

// Map để lookup category theo code (dùng cho auto-fill)
const categoryMap = ref(new Map())

/*
  Mô tả: Load danh sách fixed asset categories từ API
  CreatedBy: DDToan - (14/1/2026)
*/
const loadAssetCategories = async () => {
  try {
    const response = await getFixedAssetCategories()
    
    // Xử lý response - có thể là array hoặc object có data property
    let apiCategories = []
    if (Array.isArray(response)) {
      apiCategories = response
    } else if (response && Array.isArray(response.data)) {
      apiCategories = response.data
    } else if (response && response.data) {
      apiCategories = [response.data]
    }
    
    // Lưu vào state
    assetCategories.value = apiCategories
    
    // Tạo map để lookup nhanh theo code
    categoryMap.value.clear()
    apiCategories.forEach(cat => {
      // Debug: Log toàn bộ object để xem field names thực tế (chỉ khi cần debug)
      // console.log('Raw category object:', cat)
      // console.log('All category keys:', Object.keys(cat))
      
      const code = cat.fixedAssetCategoryCode || cat.fixed_asset_category_code || cat.code
      const name = cat.fixedAssetCategoryName || cat.fixed_asset_category_name || cat.name
      
      // Backend entity dùng FixedAssetCategoryLifeTime (không phải LifeYears)
      // Thử nhiều format: PascalCase, snake_case, và camelCase
      // Parse về number để đảm bảo type đúng
      let lifeYearsRaw = cat.fixedAssetCategoryLifeTime || 
                        cat.fixed_asset_category_life_time || 
                        cat.fixedAssetCategoryLifeYears || 
                        cat.fixed_asset_category_life_years || 
                        cat.lifeYears || 
                        cat.lifeTime ||
                        0
      
      // Nếu không tìm thấy, thử tìm trong tất cả các keys có chứa "life", "year", "time"
      if (!lifeYearsRaw || lifeYearsRaw === 0) {
        const lifeYearKeys = Object.keys(cat).filter(key => 
          key.toLowerCase().includes('life') || 
          key.toLowerCase().includes('year') ||
          (key.toLowerCase().includes('time') && !key.toLowerCase().includes('created') && !key.toLowerCase().includes('modified'))
        )
        if (lifeYearKeys.length > 0) {
          // Thử lấy giá trị từ field đầu tiên
          const firstKey = lifeYearKeys[0]
          const value = cat[firstKey]
          if (value !== undefined && value !== null) {
            // console.log(`  → Tìm thấy field "${firstKey}" với giá trị:`, value)
            lifeYearsRaw = value
          }
        }
      }
      
      let lifeYears = typeof lifeYearsRaw === 'string' ? parseInt(lifeYearsRaw, 10) || 0 : (lifeYearsRaw || 0)
      
      // Parse về number để đảm bảo type đúng
      const depreciationRateRaw = cat.fixedAssetCategoryDepreciationRate || 
                                  cat.fixed_asset_category_depreciation_rate || 
                                  cat.depreciationRate || 
                                  0
      let depreciationRate = typeof depreciationRateRaw === 'string' ? parseFloat(depreciationRateRaw) || 0 : (depreciationRateRaw || 0)
      
      // Nếu lifeYears = 0 (dữ liệu trong DB chưa có), fallback về constants
      if (lifeYears === 0 && code) {
        const assetTypeFromConstants = getAssetTypeByCode(code)
        if (assetTypeFromConstants && assetTypeFromConstants.lifeYears) {
          console.log(`  → Fallback về constants cho category [${code}]: lifeYears = ${assetTypeFromConstants.lifeYears}`)
          lifeYears = assetTypeFromConstants.lifeYears
        }
      }
      
      // Nếu depreciationRate = 0 (dữ liệu trong DB chưa có), fallback về constants
      if (depreciationRate === 0 && code) {
        const assetTypeFromConstants = getAssetTypeByCode(code)
        if (assetTypeFromConstants && assetTypeFromConstants.depreciationRate) {
          console.log(`  → Fallback về constants cho category [${code}]: depreciationRate = ${assetTypeFromConstants.depreciationRate}`)
          depreciationRate = assetTypeFromConstants.depreciationRate
        }
      }
      
      // Debug: Log chi tiết để kiểm tra (chỉ log khi có vấn đề)
      if (code && (!lifeYears || lifeYears === 0)) {
        console.warn(`Category [${code}] có lifeYears = 0, đã fallback về constants`)
      }
      
      if (code) {
        categoryMap.value.set(code, { 
          code, 
          name, 
          lifeYears, 
          depreciationRate 
        })
      }
    })
    
    // Debug: Log để kiểm tra (chỉ log khi cần debug)
    // console.log('Loaded categories from API:', apiCategories)
    // console.log('Category map:', Array.from(categoryMap.value.entries()))
  } catch (error) {
    console.error('Error loading asset categories:', error)
    // Fallback về constants nếu API lỗi
    assetCategories.value = ASSET_TYPES.map(type => ({
      fixedAssetCategoryCode: type.code,
      fixedAssetCategoryName: type.name,
      fixedAssetCategoryLifeYears: type.lifeYears,
      fixedAssetCategoryDepreciationRate: type.depreciationRate
    }))
    // Tạo map từ constants
    categoryMap.value.clear()
    ASSET_TYPES.forEach(type => {
      categoryMap.value.set(type.code, { 
        code: type.code, 
        name: type.name, 
        lifeYears: type.lifeYears, 
        depreciationRate: type.depreciationRate 
      })
    })
  }
}

// Dropdown options từ API (thay thế constants)
const assetTypeOptions = computed(() => {
  if (assetCategories.value.length > 0) {
    // Dùng data từ API
    return assetCategories.value.map(cat => ({
      value: cat.fixedAssetCategoryCode || cat.fixed_asset_category_code || cat.code,
      label: cat.fixedAssetCategoryCode || cat.fixed_asset_category_code || cat.code
    }))
  } else {
    // Fallback về constants nếu chưa load được
    return ASSET_TYPES.map(type => ({
      value: type.code,
      label: type.code
    }))
  }
})

// Check unsaved changes
const hasUnsavedChanges = computed(() => {
  if (!initialFormData.value) return false
  return JSON.stringify(formData.value) !== JSON.stringify(initialFormData.value)
})


/*
  Mô tả: Reset form về giá trị mặc định
  CreatedBy: DDToan - (11/1/2026)
  EditBy: DDToan - (14/1/2026) - Sử dụng API để lấy mã tài sản mới
  EditBy: DDToan - (16/1/2026) - Đơn giản hóa: chỉ nhận mã từ AssetList, không gọi API
  @param {string} preGeneratedCode - Mã tài sản đã được lấy từ AssetList (bắt buộc)
*/
const resetForm = async (preGeneratedCode) => {
  const today = new Date()
  const currentYear = today.getFullYear()
  
  // Lấy mã từ AssetList (đã được gọi API trước đó)
  // Tin tưởng tuyệt đối vào API Backend, không có retry/fallback
  const assetCode = preGeneratedCode && typeof preGeneratedCode === 'string' 
    ? preGeneratedCode.trim() 
    : ''
  
  formData.value = {
    assetCode: assetCode,
    assetName: '',
    departmentCode: '',
    departmentName: '',
    assetTypeCode: '',
    assetTypeName: '',
    quantity: 1,
    originalCost: 0,
    depreciationRate: '',
    purchaseDate: formatDate(today),
    startUseDate: formatDate(today),
    trackingYear: currentYear.toString(),
    yearsOfUse: 0,
    annualDepreciationValue: 0
  }
  
  initialFormData.value = null
  fieldErrors.value = {}
}

// Tính Hao mòn năm = Tỷ lệ hao mòn × Nguyên giá
const calculateAnnualDepreciation = () => {
  const originalCost = parseNumber(formData.value.originalCost)
  const depreciationRate = parseFloat(formData.value.depreciationRate) || 0
  
  if (originalCost > 0 && depreciationRate > 0) {
    const annualDep = (originalCost * depreciationRate) / 100
    formData.value.annualDepreciationValue = Math.round(annualDep)
  } else {
    formData.value.annualDepreciationValue = 0
  }
}

/*
  Mô tả: Auto-fill khi chọn Mã bộ phận
  CreatedBy: DDToan - (11/1/2026)
  EditBy: DDToan - (14/1/2026) - Sử dụng data từ API thay vì constants
*/
const handleDepartmentChange = (option) => {
  if (option && option.value) {
    // Tìm department từ map (ưu tiên) hoặc constants (fallback)
    const deptFromMap = departmentMap.value.get(option.value)
    if (deptFromMap) {
      formData.value.departmentName = deptFromMap.name
    } else {
      // Fallback về constants nếu không tìm thấy trong map
      const dept = getDepartmentByCode(option.value)
      if (dept) {
        formData.value.departmentName = dept.name
      }
    }
  } else {
    formData.value.departmentName = ''
  }
}

/*
  Mô tả: Auto-fill khi chọn Mã loại tài sản
  CreatedBy: DDToan - (11/1/2026)
  EditBy: DDToan - (14/1/2026) - Sử dụng data từ API thay vì constants
*/
const handleAssetTypeChange = (option) => {
  if (option && option.value) {
    // Tìm category từ map (ưu tiên) hoặc constants (fallback)
    const categoryFromMap = categoryMap.value.get(option.value)
    
    if (categoryFromMap) {
      formData.value.assetTypeName = categoryFromMap.name
      // Đảm bảo lifeYears là number và > 0
      const lifeYearsValue = categoryFromMap.lifeYears || 0
      formData.value.yearsOfUse = lifeYearsValue
      formData.value.depreciationRate = categoryFromMap.depreciationRate ? categoryFromMap.depreciationRate.toString() : ''
      // Trigger tính toán lại Hao mòn năm
      calculateAnnualDepreciation()
    } else {
      console.warn('Category not found in map, trying constants fallback')
      // Fallback về constants nếu không tìm thấy trong map
      const assetType = getAssetTypeByCode(option.value)
      if (assetType) {
        formData.value.assetTypeName = assetType.name
        formData.value.yearsOfUse = assetType.lifeYears
        formData.value.depreciationRate = assetType.depreciationRate.toString()
        // Trigger tính toán lại Hao mòn năm
        calculateAnnualDepreciation()
      } else {
        console.error('Category not found in both map and constants:', option.value)
      }
    }
  } else {
    formData.value.assetTypeName = ''
    formData.value.yearsOfUse = 0
    formData.value.depreciationRate = ''
    formData.value.annualDepreciationValue = 0
  }
}

/*
  Mô tả: Xử lý khi blur trường mã tài sản - Chỉ tự sinh khi người dùng xóa trắng
  CreatedBy: DDToan - (16/1/2026)
  EditBy: DDToan - (16/1/2026) - Chỉ tự sinh NẾU VÀ CHỈ NẾU input đang rỗng
*/
const handleAssetCodeBlur = async () => {
  // Chỉ tự sinh mã khi đang thêm mới (không có assetData.id)
  if (!props.assetData || !props.assetData.id) {
    const currentCode = formData.value.assetCode || ''
    const trimmedCode = currentCode.trim()
    
    // Chỉ tự sinh mã NẾU VÀ CHỈ NẾU giá trị trong ô input đang là Rỗng/Empty
    // Nếu trong ô đã có chữ (bất kể là "TS00001" hay "MãCủaTôi"), giữ nguyên
    if (!trimmedCode || trimmedCode === '') {
      try {
        const newCode = await getNewAssetCode()
        if (newCode && newCode.trim() !== '') {
          formData.value.assetCode = newCode.trim()
        }
      } catch (error) {
        console.error('Error generating asset code on blur:', error)
        // Không set mã nếu generate fail (để user tự nhập)
      }
    }
  }
}

// Watch Nguyên giá và Tỷ lệ hao mòn để tính lại Hao mòn năm
watch(
  [() => formData.value.originalCost, () => formData.value.depreciationRate],
  () => {
    calculateAnnualDepreciation()
  }
)

// Watch Ngày mua để auto-fill Năm theo dõi
watch(
  () => formData.value.purchaseDate,
  (newDate) => {
    if (newDate) {
      const year = getYearFromDate(newDate)
      if (year) {
        formData.value.trackingYear = year.toString()
      }
    }
  }
)

// Watch Số năm sử dụng để validate Tỷ lệ hao mòn
watch(
  () => formData.value.yearsOfUse,
  (newValue) => {
    if (newValue > 0) {
      const expectedRate = (1 / newValue) * 100
      // Chỉ update nếu chưa có giá trị hoặc giá trị hiện tại không khớp
      if (!formData.value.depreciationRate || Math.abs(parseFloat(formData.value.depreciationRate) - expectedRate) > 0.01) {
        // Không tự động update, chỉ validate khi save
      }
    }
  }
)

// Watch modal open/close
watch(
  () => props.modelValue,
  async (isOpen) => {
    if (isOpen) {
      // Load departments và categories từ API khi mở form (await để đảm bảo data đã load xong)
      await loadDepartments()
      await loadAssetCategories()
      
      // Mở form
      if (props.assetData && props.assetData.isNew) {
        // Add mode hoặc Nhân bản mode
        // EditBy: DDToan - (17/1/2026) - Kiểm tra nếu có dữ liệu đầy đủ (nhân bản) thì load vào form, nếu không thì reset form
        const hasFullData = props.assetData.name || props.assetData.assetName || 
                           props.assetData.departmentCode || props.assetData.assetTypeCode
        
        if (hasFullData) {
          // Nhân bản mode: Load dữ liệu đầy đủ vào form
          formData.value = {
            assetCode: props.assetData.code || props.assetData.assetCode || '',
            assetName: props.assetData.name || props.assetData.assetName || '',
            departmentCode: props.assetData.departmentCode || '',
            departmentName: props.assetData.departmentName || '',
            assetTypeCode: props.assetData.assetTypeCode || '',
            assetTypeName: props.assetData.assetTypeName || '',
            quantity: props.assetData.quantity || 1,
            originalCost: props.assetData.cost || props.assetData.originalCost || 0,
            depreciationRate: props.assetData.depreciationRate || '',
            purchaseDate: props.assetData.purchaseDate || formatDate(new Date()),
            startUseDate: props.assetData.startUseDate || formatDate(new Date()),
            trackingYear: props.assetData.trackingYear || new Date().getFullYear().toString(),
            yearsOfUse: props.assetData.yearsOfUse || 0,
            annualDepreciationValue: props.assetData.annualDepreciationValue || 0
          }
        } else {
          // Add mode: Chỉ có mã, reset form về mặc định
          const preGeneratedCode = props.assetData.code || props.assetData.assetCode || ''
          await resetForm(preGeneratedCode)
        }
      } else if (props.assetData && props.assetData.id) {
        // Edit mode: load data từ assetData
        formData.value = {
          assetCode: props.assetData.code || props.assetData.assetCode || '',
          assetName: props.assetData.name || props.assetData.assetName || '',
          departmentCode: props.assetData.departmentCode || '',
          departmentName: props.assetData.departmentName || '',
          assetTypeCode: props.assetData.assetTypeCode || '',
          assetTypeName: props.assetData.assetTypeName || '',
          quantity: props.assetData.quantity || 1,
          originalCost: props.assetData.cost || props.assetData.originalCost || 0,
          depreciationRate: props.assetData.depreciationRate || '',
          purchaseDate: props.assetData.purchaseDate || formatDate(new Date()),
          startUseDate: props.assetData.startUseDate || formatDate(new Date()),
          trackingYear: props.assetData.trackingYear || new Date().getFullYear().toString(),
          yearsOfUse: props.assetData.yearsOfUse || 0,
          annualDepreciationValue: props.assetData.annualDepreciationValue || 0
        }
      } else {
        // Fallback: Nếu không có assetData, vẫn cần reset form với mã rỗng
        // AssetList phải luôn gọi API trước khi mở form, nên trường hợp này không nên xảy ra
        await resetForm('')
      }
      
      // Lưu snapshot ban đầu
      initialFormData.value = JSON.parse(JSON.stringify(formData.value))
      fieldErrors.value = {}
    } else {
      // Đóng form: không cần reset (sẽ reset khi mở lại)
      // await resetForm() // Comment lại để không reset khi đóng
    }
  }
)

// Xử lý đóng popup (click X hoặc overlay)
const handlePopupClose = () => {
  if (hasUnsavedChanges.value) {
    // Có thay đổi chưa lưu → hiển thị dialog
    showUnsavedDialog.value = true
  } else {
    // Không có thay đổi → đóng form
    emit('update:modelValue', false)
  }
}

// Xử lý click button trong Unsaved Dialog
const handleUnsavedDialogClick = (button) => {
  if (button.action === 'save') {
    // Lưu
    showUnsavedDialog.value = false
    handleSave()
  } else if (button.action === 'discard') {
    // Không lưu → đóng form
    showUnsavedDialog.value = false
    emit('update:modelValue', false)
  } else if (button.action === 'cancel') {
    // Hủy → giữ form mở
    showUnsavedDialog.value = false
  }
}

// Xử lý click button trong Cancel Dialog
const handleCancelDialogClick = (button) => {
  if (button.action === 'confirm') {
    // Hủy bỏ → đóng form
    showCancelConfirmDialog.value = false
    emit('update:modelValue', false)
    emit('cancel')
  } else if (button.action === 'cancel') {
    // Không → giữ form mở
    showCancelConfirmDialog.value = false
  }
}

// Validate form
const validateForm = () => {
  // Get existing assets từ props hoặc MOCK_ASSETS
  const existingAssets = props.existingAssets.length > 0 
    ? props.existingAssets 
    : MOCK_ASSETS.map(asset => ({ code: asset.code }))
  
  const currentAssetCode = props.assetData ? (props.assetData.code || props.assetData.assetCode) : ''
  
  const result = validateAssetForm(formData.value, existingAssets, currentAssetCode)
  
  // Set field errors
  fieldErrors.value = {}
  result.errors.forEach(error => {
    fieldErrors.value[error.field] = error.message
  })
  
  return result
}

/*
  Mô tả: Map dữ liệu từ form format sang API format (backend)
  CreatedBy: DDToan - (14/1/2026)
*/
const mapFormDataToApiFormat = (formData) => {
  // Format date từ dd/mm/yyyy sang yyyy-MM-dd
  const formatDateForApi = (dateString) => {
    if (!dateString) return null
    try {
      // Parse từ dd/mm/yyyy
      const parts = dateString.split('/')
      if (parts.length === 3) {
        const day = parts[0]
        const month = parts[1]
        const year = parts[2]
        return `${year}-${month}-${day}`
      }
      return dateString
    } catch {
      return dateString
    }
  }
  
  // Tìm departmentId từ departmentCode
  const department = departments.value.find(
    dept => (dept.departmentCode || dept.department_code || dept.code) === formData.departmentCode
  )
  const departmentId = department 
    ? (department.departmentId || department.department_id || department.id) 
    : null
  
  // Tìm fixedAssetCategoryId từ assetTypeCode
  const category = assetCategories.value.find(
    cat => (cat.fixedAssetCategoryCode || cat.fixed_asset_category_code || cat.code) === formData.assetTypeCode
  )
  const fixedAssetCategoryId = category 
    ? (category.fixedAssetCategoryId || category.fixed_asset_category_id || category.id) 
    : null
  
  return {
    fixedAssetCode: formData.assetCode,
    fixedAssetName: formData.assetName,
    departmentId: departmentId,
    fixedAssetCategoryId: fixedAssetCategoryId,
    fixedAssetQuantity: formData.quantity || 1,
    fixedAssetOriginalCost: parseNumber(formData.originalCost) || 0,
    fixedAssetDepreciationRate: parseFloat(formData.depreciationRate) || 0,
    fixedAssetPurchaseDate: formatDateForApi(formData.purchaseDate),
    fixedAssetStartUseDate: formatDateForApi(formData.startUseDate),
    fixedAssetTrackingYear: parseInt(formData.trackingYear) || new Date().getFullYear(),
    fixedAssetYearsOfUse: formData.yearsOfUse || 0, // EditBy: DDToan - (16/1/2026) - Đổi từ fixedAssetLifeTime để khớp với Backend Entity
    fixedAssetAnnualDepreciationValue: parseNumber(formData.annualDepreciationValue) || 0
  }
}

/*
  Mô tả: Xử lý Save - Gọi API Create hoặc Update
  CreatedBy: DDToan - (11/1/2026)
  EditBy: DDToan - (14/1/2026) - Tích hợp API Create/Update
  EditBy: DDToan - (16/1/2026) - Tự sinh mã tài sản nếu trống hoặc là giá trị mặc định khi thêm mới
*/
const handleSave = async () => {
  // Clear errors
  fieldErrors.value = {}
  
  // Tự sinh mã tài sản nếu trống (chỉ khi thêm mới)
  // Chỉ tự sinh khi mã thực sự rỗng, không check "TS00001" vì đó có thể là mã hợp lệ từ Backend
  if (!props.assetData || !props.assetData.id) {
    const currentCode = formData.value.assetCode || ''
    const trimmedCode = currentCode.trim()
    
    // Chỉ tự sinh mã nếu mã trống hoặc chỉ có khoảng trắng
    if (!trimmedCode || trimmedCode === '') {
      try {
        const newCode = await getNewAssetCode()
        if (newCode && newCode.trim() !== '') {
          formData.value.assetCode = newCode.trim()
        }
      } catch (error) {
        console.error('Error generating asset code before save:', error)
        // Nếu generate fail, vẫn tiếp tục validate (sẽ báo lỗi nếu mã trống)
      }
    }
  }
  
  // Validate
  const validation = validateForm()
  
  if (!validation.isValid) {
    // Có lỗi → hiển thị dialog lỗi đầu tiên
    const firstError = validation.errors[0]
    errorMessage.value = firstError.message
    showErrorDialog.value = true
    return
  }
  
  try {
    // Map dữ liệu từ form format sang API format
    const apiData = mapFormDataToApiFormat(formData.value)
    
    // Debug: Log data trước khi gửi lên server
    console.log('API Data to send:', apiData)
    console.log('API Data (JSON):', JSON.stringify(apiData, null, 2))
    
    // Gọi API Create hoặc Update
    if (props.assetData && props.assetData.id) {
      // Update mode
      const assetId = props.assetData.id
      await updateFixedAsset(assetId, apiData)
    } else {
      // Create mode
      await createFixedAsset(apiData)
    }
    
    // Hiển thị success dialog
    showSuccessDialog.value = true
    
    // Tự đóng success dialog và đóng form sau 2s để user thấy thông báo
    // EditBy: DDToan - (17/1/2026) - Đảm bảo hiển thị thông báo thành công và đóng form
    setTimeout(() => {
      showSuccessDialog.value = false
      // Đóng form sau khi hiển thị thông báo
      emit('update:modelValue', false)
      // Emit event để parent reload danh sách
      emit('saved')
    }, 2000)
  } catch (error) {
    console.error('Error saving asset:', error)
    console.error('Error details:', {
      message: error.message,
      response: error.response,
      data: error.data,
      fullError: error
    })
    // Hiển thị error message chi tiết hơn
    let errorMsg = 'Có lỗi xảy ra khi lưu dữ liệu. Vui lòng thử lại sau.'
    if (error.message) {
      errorMsg = error.message
    } else if (error.data && error.data.message) {
      errorMsg = error.data.message
    } else if (error.response && error.response.message) {
      errorMsg = error.response.message
    }
    errorMessage.value = errorMsg
    showErrorDialog.value = true
  }
}

// Xử lý Cancel
/*
  Mô tả: Xử lý khi click nút [Hủy]
  - Trường hợp 1 (Cơ bản): Luôn hiển thị dialog xác nhận hủy bỏ "Bạn có muốn hủy bỏ khai báo tài sản này?"
  - Trường hợp 2 (Đang Sửa và có thay đổi): Hiển thị dialog cảnh báo "Thông tin thay đổi sẽ không được cập nhật..."
  
  CreatedBy: DDToan - (11/1/2026)
*/
const handleCancel = () => {
  // Trường hợp 2: Đang Sửa (có assetData) và có thay đổi
  if (props.assetData && hasUnsavedChanges.value) {
    // Hiển thị dialog cảnh báo dữ liệu thay đổi
    showUnsavedDialog.value = true
  } else {
    // Trường hợp 1: Cơ bản - Luôn hiển thị dialog xác nhận hủy bỏ
    showCancelConfirmDialog.value = true
  }
}
</script>

<style scoped>
/*
  Mô tả: Styles cho AssetForm component
  Spacing theo thiết kế:
  - Header: 20px từ top, 20px từ left, 16px từ right
  - Content: 20px từ title (padding-top), 16px từ left/right
  - Gap giữa columns: 16px
  - Gap giữa rows: 28px (36px từ label đến label - 8px gap label-input = 28px từ input đến label tiếp theo)
  - Footer: 52px từ content, 36px từ bottom, buttons 10px gap, 16px từ right
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

.asset-form {
  width: 100%;
}

.asset-form__content {
  display: flex;
  flex-direction: column;
  gap: 0;
  padding: 0;
}

.asset-form__row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
  margin-bottom: 28px;
  width: 100%;
}

.asset-form__row:last-of-type {
  margin-bottom: 0;
}

.asset-form__row--three {
  grid-template-columns: 1fr 1fr 1fr;
}

.asset-form__field {
  display: flex;
  flex-direction: column;
  width: 100%;
}

/* Icon chevron cho dropdown trong form - ẩn icon mặc định */
.asset-form__field :deep(.ms-dropdown__arrow) {
  display: none;
}

/* Icon chevron-down khi đóng - căn giữa */
.asset-form__field :deep(.ms-dropdown__trigger)::after {
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
.asset-form__field :deep(.ms-dropdown--open .ms-dropdown__trigger)::after {
  -webkit-mask-position: -28px -338px !important; /* icon-chevron-up-toolbar */
  mask-position: -28px -338px !important;
}
</style>
