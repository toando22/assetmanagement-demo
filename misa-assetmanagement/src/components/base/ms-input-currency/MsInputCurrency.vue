<!--
  Mô tả: Input component cho số tiền với format dấu chấm (.) ngăn cách hàng nghìn
  Features: Format số, validate chỉ số, căn lề phải
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
-->

<template>
  <div class="ms-input-currency" :class="{ 'ms-input-currency--error': error, 'ms-input-currency--disabled': disabled }">
    <label v-if="label" class="ms-input-currency__label" :for="inputId">
      {{ label }}
      <span v-if="required" class="ms-input-currency__required">*</span>
    </label>
    <div class="ms-input-currency__wrapper">
      <input
        :id="inputId"
        class="ms-input-currency__field"
        type="text"
        :value="displayValue"
        :placeholder="!modelValue && !placeholder ? 'Không có dữ liệu' : placeholder"
        :disabled="disabled"
        @input="handleInput"
        @focus="handleFocus"
        @blur="handleBlur"
        @keydown="handleKeydown"
      />
    </div>
    <span v-if="error && errorMessage" class="ms-input-currency__error-message">
      {{ errorMessage }}
    </span>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsInputCurrency component
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
*/

import { computed } from 'vue'
import { formatNumber, parseNumber } from '@/utils/format.js'

const props = defineProps({
  modelValue: {
    type: [String, Number],
    default: ''
  },
  label: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: '0'
  },
  disabled: {
    type: Boolean,
    default: false
  },
  error: {
    type: Boolean,
    default: false
  },
  errorMessage: {
    type: String,
    default: 'Error message'
  },
  required: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['update:modelValue', 'focus', 'blur'])

const inputId = computed(() => `input-currency-${Math.random().toString(36).substr(2, 9)}`)

// Format giá trị để hiển thị
const displayValue = computed(() => {
  if (!props.modelValue || props.modelValue === '' || props.modelValue === 0) {
    return ''
  }
  return formatNumber(props.modelValue)
})

// Xử lý input
const handleInput = (event) => {
  let value = event.target.value
  
  // Remove tất cả ký tự không phải số
  value = value.replace(/[^\d]/g, '')
  
  // Nếu rỗng, emit empty string
  if (value === '') {
    emit('update:modelValue', '')
    return
  }
  
  // Parse và emit số
  const numValue = parseNumber(value)
  emit('update:modelValue', numValue)
}

// Xử lý focus - giữ nguyên format
const handleFocus = (event) => {
  emit('focus', event)
}

// Xử lý blur - format lại
const handleBlur = (event) => {
  emit('blur', event)
}

// Xử lý keydown - chỉ cho phép số và một số phím đặc biệt
const handleKeydown = (event) => {
  // Cho phép: Backspace, Delete, Tab, Escape, Enter, Arrow keys
  const allowedKeys = [
    'Backspace', 'Delete', 'Tab', 'Escape', 'Enter',
    'ArrowLeft', 'ArrowRight', 'ArrowUp', 'ArrowDown',
    'Home', 'End'
  ]
  
  // Cho phép Ctrl/Cmd + A, C, V, X
  if (event.ctrlKey || event.metaKey) {
    if (['a', 'c', 'v', 'x'].includes(event.key.toLowerCase())) {
      return
    }
  }
  
  // Nếu là phím được phép, cho qua
  if (allowedKeys.includes(event.key)) {
    return
  }
  
  // Chỉ cho phép số (0-9)
  if (!/^\d$/.test(event.key)) {
    event.preventDefault()
  }
}
</script>

<style scoped>
/*
  Mô tả: Styles cho MsInputCurrency component
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
*/

.ms-input-currency {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.ms-input-currency__label {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #182026;
  line-height: 18px;
}

.ms-input-currency__required {
  color: #e74c3c;
}

.ms-input-currency__wrapper {
  position: relative;
}

.ms-input-currency__field {
  width: 100%;
  height: 36px;
  padding: 0 12px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  background-color: #ffffff;
  transition: all 0.2s ease;
  box-sizing: border-box;
  text-align: right; /* Căn lề phải cho số */
}

.ms-input-currency__field::placeholder {
  color: #bfbfbf;
  font-style: italic;
  text-align: left; /* Placeholder căn trái */
}

/* Hover state */
.ms-input-currency__field:hover:not(:disabled) {
  border-color: #40a9ff;
}

/* Focus state */
.ms-input-currency__field:focus {
  border-color: #1aa4c8;
  outline: none;
  background-color: #ffffff;
  text-align: right; /* Giữ căn phải khi focus */
}

.ms-input-currency__field:focus::placeholder {
  color: #1aa4c8;
}

/* Disabled state */
.ms-input-currency--disabled .ms-input-currency__field {
  background-color: #f5f5f5;
  color: #bfbfbf;
  cursor: not-allowed;
  border-color: #d9d9d9;
}

/* Error state */
.ms-input-currency--error .ms-input-currency__field {
  border-color: #e74c3c;
}

.ms-input-currency--error .ms-input-currency__field:focus {
  border-color: #e74c3c;
}

.ms-input-currency__error-message {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #e74c3c;
  line-height: 18px;
}
</style>
