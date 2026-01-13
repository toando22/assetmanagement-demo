<!--
  Mô tả: Dropdown/Select component
  Features: Regular dropdown, Dropdown with checkbox, Table dropdown
  States: Default, Hover, Active, Focus, Open
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div class="ms-dropdown" :class="{ 'ms-dropdown--open': isOpen }">
    <div class="ms-dropdown__trigger" @click="toggleDropdown">
      <span class="ms-dropdown__text">{{ selectedText }}</span>
      <i class="ms-dropdown__arrow icon icon-chevron-down-toolbar"></i>
    </div>

    <transition name="dropdown-fade">
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
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsDropdown component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { ref, computed, onMounted, onBeforeUnmount } from 'vue'

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
})

const emit = defineEmits(['update:modelValue', 'change'])

const isOpen = ref(false)

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
  if (!dropdown) {
    isOpen.value = false
  }
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onBeforeUnmount(() => {
  document.removeEventListener('click', handleClickOutside)
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
  z-index: 1000;
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
