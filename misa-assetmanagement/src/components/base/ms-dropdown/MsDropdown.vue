<!--
  Mô tả: Dropdown/Select component
  Features: Regular dropdown, Dropdown with checkbox, Table dropdown
  States: Default, Hover, Active, Focus, Open
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div class="ms-dropdown" :class="{ 'ms-dropdown--open': isOpen }" ref="dropdownRef">
    <div class="ms-dropdown__trigger" @click="toggleDropdown" ref="triggerRef">
      <span class="ms-dropdown__text">{{ selectedText }}</span>
      <i class="ms-dropdown__arrow icon icon-chevron-down-toolbar"></i>
    </div>

    <!-- Menu render bình thường (khi không teleport) -->
    <transition v-if="!teleport" name="dropdown-fade">
      <div v-if="isOpen" class="ms-dropdown__menu">
        <!-- Regular dropdown -->
        <div v-if="!withCheckbox && !withTable" class="ms-dropdown__list">
          <div
            v-for="option in options"
            :key="option.value"
            class="ms-dropdown__item"
            :class="{ 'ms-dropdown__item--selected': option.value === modelValue }"
            @click="selectOption(option)"
          >
            <i v-if="option.value === modelValue" class="ms-dropdown__check icon icon-check"></i>
            {{ option.label }}
          </div>
        </div>

        <!-- Dropdown with checkbox -->
        <div v-if="withCheckbox" class="ms-dropdown__list">
          <div
            v-for="option in options"
            :key="option.value"
            class="ms-dropdown__item ms-dropdown__item--checkbox"
            @click.stop="toggleCheckbox(option)"
          >
            <input
              type="checkbox"
              :checked="isChecked(option.value)"
              @click.stop
              @change="toggleCheckbox(option)"
            />
            <span>{{ option.label }}</span>
          </div>
        </div>

        <!-- Table dropdown -->
        <div v-if="withTable" class="ms-dropdown__table">
          <table class="ms-dropdown__table-content">
            <thead>
              <tr>
                <th>Mã</th>
                <th>Tên</th>
              </tr>
            </thead>
            <tbody>
              <tr
                v-for="option in options"
                :key="option.value"
                class="ms-dropdown__table-row"
                :class="{ 'ms-dropdown__table-row--selected': option.value === modelValue }"
                @click="selectOption(option)"
              >
                <td>{{ option.value }}</td>
                <td>{{ option.label }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </transition>

    <!-- Menu render bằng Teleport (khi teleport = true) -->
    <Teleport v-if="teleport" to="body">
      <transition name="dropdown-fade">
        <div 
          v-if="isOpen" 
          class="ms-dropdown__menu ms-dropdown__menu--teleported"
          :style="teleportedMenuStyle"
          ref="menuRef"
        >
          <!-- Regular dropdown -->
          <div v-if="!withCheckbox && !withTable" class="ms-dropdown__list">
            <div
              v-for="option in options"
              :key="option.value"
              class="ms-dropdown__item"
              :class="{ 'ms-dropdown__item--selected': option.value === modelValue }"
              @click="selectOption(option)"
            >
              <i v-if="option.value === modelValue" class="ms-dropdown__check icon icon-check"></i>
              {{ option.label }}
            </div>
          </div>

          <!-- Dropdown with checkbox -->
          <div v-if="withCheckbox" class="ms-dropdown__list">
            <div
              v-for="option in options"
              :key="option.value"
              class="ms-dropdown__item ms-dropdown__item--checkbox"
              @click.stop="toggleCheckbox(option)"
            >
              <input
                type="checkbox"
                :checked="isChecked(option.value)"
                @click.stop
                @change="toggleCheckbox(option)"
              />
              <span>{{ option.label }}</span>
            </div>
          </div>

          <!-- Table dropdown -->
          <div v-if="withTable" class="ms-dropdown__table">
            <table class="ms-dropdown__table-content">
              <thead>
                <tr>
                  <th>Mã</th>
                  <th>Tên</th>
                </tr>
              </thead>
              <tbody>
                <tr
                  v-for="option in options"
                  :key="option.value"
                  class="ms-dropdown__table-row"
                  :class="{ 'ms-dropdown__table-row--selected': option.value === modelValue }"
                  @click="selectOption(option)"
                >
                  <td>{{ option.value }}</td>
                  <td>{{ option.label }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </transition>
    </Teleport>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsDropdown component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { ref, computed, onMounted, onBeforeUnmount, nextTick, watch } from 'vue'

const props = defineProps({
  modelValue: {
    type: [String, Number, Array],
    default: '',
  },
  options: {
    type: Array,
    default: () => [],
  },
  placeholder: {
    type: String,
    default: 'Roboto - 14pt - Regular',
  },
  withCheckbox: {
    type: Boolean,
    default: false,
  },
  withTable: {
    type: Boolean,
    default: false,
  },
  teleport: {
    type: Boolean,
    default: false,
  },
  placement: {
    type: String,
    default: 'bottom', // 'top' | 'bottom'
    validator: (value) => ['top', 'bottom'].includes(value),
  },
  scrollContainer: {
    type: [HTMLElement, String, Object],
    default: null, // Element, selector string, hoặc Vue ref
  },
})

const emit = defineEmits(['update:modelValue', 'change'])

const isOpen = ref(false)
const dropdownRef = ref(null)
const triggerRef = ref(null)
const menuRef = ref(null)
const teleportedMenuStyle = ref({ visibility: 'hidden' }) // Ẩn ban đầu để tránh flicker
const scrollContainerRef = ref(null)

const selectedText = computed(() => {
  if (props.withCheckbox) {
    if (!props.modelValue || props.modelValue.length === 0) {
      return props.placeholder || 'Không có dữ liệu'
    }
    return `${props.modelValue.length} selected`
  }

  const selected = props.options.find((opt) => opt.value === props.modelValue)
  if (selected) {
    return selected.label
  }
  return props.placeholder || 'Không có dữ liệu'
})

const toggleDropdown = () => {
  isOpen.value = !isOpen.value
  if (isOpen.value && props.teleport) {
    // Ẩn menu ban đầu để tránh flicker
    teleportedMenuStyle.value = { visibility: 'hidden' }
    nextTick(() => {
      updateTeleportedMenuPosition()
    })
  }
}

// Tính toán vị trí cho menu teleported
const updateTeleportedMenuPosition = () => {
  if (!triggerRef.value || !menuRef.value) return

  const triggerRect = triggerRef.value.getBoundingClientRect()
  const menu = menuRef.value
  
  // Đo menu height (có thể cần force reflow)
  const tempVisibility = menu.style.visibility
  menu.style.visibility = 'hidden'
  menu.style.display = 'block'
  const menuHeight = menu.offsetHeight || 240
  const menuWidth = triggerRect.width
  menu.style.visibility = tempVisibility
  menu.style.display = ''

  let top, left

  if (props.placement === 'top') {
    // Mở lên trên
    top = triggerRect.top - menuHeight - 4
    // Nếu không đủ chỗ phía trên, mở xuống dưới
    if (top < 0) {
      top = triggerRect.bottom + 4
    }
  } else {
    // Mở xuống dưới
    top = triggerRect.bottom + 4
    // Nếu không đủ chỗ phía dưới, mở lên trên
    if (top + menuHeight > window.innerHeight) {
      top = triggerRect.top - menuHeight - 4
    }
  }

  left = triggerRect.left
  // Kiểm tra boundaries ngang
  if (left + menuWidth > window.innerWidth) {
    left = window.innerWidth - menuWidth - 8
  }
  if (left < 0) {
    left = 8
  }

  teleportedMenuStyle.value = {
    position: 'fixed',
    top: `${top}px`,
    left: `${left}px`,
    width: `${menuWidth}px`,
    zIndex: '10000',
    visibility: 'visible', // Hiện sau khi tính toán xong
  }
}

// Tự động cập nhật vị trí khi scroll hoặc resize
let updatePositionTimer = null
const scheduleUpdatePosition = () => {
  if (updatePositionTimer) {
    cancelAnimationFrame(updatePositionTimer)
  }
  updatePositionTimer = requestAnimationFrame(() => {
    if (isOpen.value && props.teleport) {
      updateTeleportedMenuPosition()
    }
  })
}

const selectOption = (option) => {
  emit('update:modelValue', option.value)
  emit('change', option)
  if (!props.withCheckbox) {
    isOpen.value = false
  }
}

const isChecked = (value) => {
  if (!Array.isArray(props.modelValue)) return false
  return props.modelValue.includes(value)
}

const toggleCheckbox = (option) => {
  if (!Array.isArray(props.modelValue)) {
    emit('update:modelValue', [option.value])
    return
  }

  const newValue = [...props.modelValue]
  const index = newValue.indexOf(option.value)

  if (index > -1) {
    newValue.splice(index, 1)
  } else {
    newValue.push(option.value)
  }

  emit('update:modelValue', newValue)
  emit('change', newValue)
}

const handleClickOutside = (event) => {
  const dropdown = event.target.closest('.ms-dropdown')
  const teleportedMenu = menuRef.value && menuRef.value.contains(event.target)
  if (!dropdown && !teleportedMenu) {
    isOpen.value = false
  }
}

// Tìm scroll container
const findScrollContainer = () => {
  if (!props.scrollContainer) return null
  
  // Nếu là Vue ref (có .value)
  if (props.scrollContainer && typeof props.scrollContainer === 'object' && 'value' in props.scrollContainer) {
    return props.scrollContainer.value
  }
  
  if (typeof props.scrollContainer === 'string') {
    // Nếu là selector string
    return document.querySelector(props.scrollContainer)
  }
  
  // Nếu là HTMLElement
  return props.scrollContainer
}

// Watch isOpen để thêm/xóa event listeners
watch(
  () => isOpen.value && props.teleport,
  (shouldListen) => {
    if (shouldListen) {
      // Tìm scroll container
      scrollContainerRef.value = findScrollContainer()
      
      // Thêm listeners khi mở
      nextTick(() => {
        window.addEventListener('scroll', scheduleUpdatePosition, true)
        window.addEventListener('resize', scheduleUpdatePosition)
        
        // Lắng nghe scroll của container nếu có
        if (scrollContainerRef.value) {
          scrollContainerRef.value.addEventListener('scroll', scheduleUpdatePosition, { passive: true })
        }
      })
    } else {
      // Xóa listeners khi đóng
      window.removeEventListener('scroll', scheduleUpdatePosition, true)
      window.removeEventListener('resize', scheduleUpdatePosition)
      
      if (scrollContainerRef.value) {
        scrollContainerRef.value.removeEventListener('scroll', scheduleUpdatePosition)
      }
    }
  },
  { immediate: false }
)

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
  window.removeEventListener('scroll', scheduleUpdatePosition, true)
  window.removeEventListener('resize', scheduleUpdatePosition)
  
  if (scrollContainerRef.value) {
    scrollContainerRef.value.removeEventListener('scroll', scheduleUpdatePosition)
  }
  
  if (updatePositionTimer) {
    cancelAnimationFrame(updatePositionTimer)
  }
})
</script>

<style scoped>
.ms-dropdown {
  position: relative;
  width: 100%;
}

.ms-dropdown__trigger {
  height: 36px;
  padding: 0 32px 0 12px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  background-color: #ffffff;
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  display: flex;
  align-items: center;
  cursor: pointer;
  transition: all 0.2s ease;
  position: relative;
}

.ms-dropdown__text {
  flex: 1;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.ms-dropdown__arrow {
  position: absolute;
  right: 12px;
  width: 12px;
  height: 12px;
  color: #595959;
  transition: transform 0.2s ease;
}

.ms-dropdown--open .ms-dropdown__arrow {
  transform: rotate(180deg);
}

/* Hover state */
.ms-dropdown__trigger:hover {
  border-color: #40a9ff;
}

/* Open/Focus state */
.ms-dropdown--open .ms-dropdown__trigger {
  border-color: #1aa4c8;
  box-shadow: 0 0 0 2px rgba(26, 164, 200, 0.1);
}

/* Dropdown menu */
.ms-dropdown__menu {
  position: absolute;
  top: calc(100% + 4px);
  left: 0;
  right: 0;
  max-height: 240px;
  background-color: #ffffff;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
  overflow-y: auto;
  z-index: 9999;
}

/* Menu teleported */
.ms-dropdown__menu--teleported {
  position: fixed !important;
  max-height: 240px;
}

.ms-dropdown__list {
  padding: 4px 0;
}

.ms-dropdown__item {
  height: 32px;
  padding: 0 12px;
  display: flex;
  align-items: center;
  gap: 8px;
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.ms-dropdown__item:hover {
  background-color: #e6f7ff;
}

.ms-dropdown__item--selected {
  background-color: #e6f7ff;
  color: #182026;
}

.ms-dropdown__item--selected:hover {
  background-color: #e6f7ff;
}

.ms-dropdown__check {
  width: 16px;
  height: 16px;
  color: #1aa4c8;
}

/* Checkbox variant */
.ms-dropdown__item--checkbox {
  gap: 8px;
}

.ms-dropdown__item--checkbox input[type='checkbox'] {
  margin: 0;
  cursor: pointer;
}

/* Table variant */
.ms-dropdown__table {
  padding: 0;
}

.ms-dropdown__table-content {
  width: 100%;
  border-collapse: collapse;
}

.ms-dropdown__table-content thead {
  background-color: #e6f7ff;
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 700;
  color: #182026;
}

.ms-dropdown__table-content th,
.ms-dropdown__table-content td {
  padding: 8px 12px;
  text-align: left;
  border-bottom: 1px solid #e8e8e8;
}

.ms-dropdown__table-row {
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  color: #182026;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

.ms-dropdown__table-row:hover {
  background-color: #e6f7ff;
}

.ms-dropdown__table-row--selected {
  background-color: #e6f7ff;
}

.ms-dropdown__table-row--selected:hover {
  background-color: #e6f7ff;
}

/* Transition */
.dropdown-fade-enter-active,
.dropdown-fade-leave-active {
  transition:
    opacity 0.2s ease,
    transform 0.2s ease;
}

.dropdown-fade-enter-from,
.dropdown-fade-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>
