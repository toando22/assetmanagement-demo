<!--
  Mô tả: Date input component với calendar icon
  States: Default, Hover, Active, Focus, Disable, Error
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div class="ms-input-date" :class="{ 'ms-input-date--error': error, 'ms-input-date--disabled': disabled }">
    <label v-if="label" class="ms-input-date__label" :for="inputId">
      {{ label }}
      <span v-if="required" class="ms-input-date__required">*</span>
    </label>
    <div class="ms-input-date__wrapper">
      <input
        :id="inputId"
        class="ms-input-date__field"
        type="text"
        :value="modelValue"
        :placeholder="placeholder || 'dd/mm/yyyy'"
        :disabled="disabled"
        @input="handleInput"
        @focus="$emit('focus', $event)"
        @blur="handleBlur"
      />
      <i class="ms-input-date__icon icon icon-calendar"></i>
    </div>
    <span v-if="error && errorMessage" class="ms-input-date__error-message">
      {{ errorMessage }}
    </span>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsInputDate component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { computed } from 'vue'

const props = defineProps({
  modelValue: {
    type: String,
    default: ''
  },
  label: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: ''
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

const inputId = computed(() => `input-date-${Math.random().toString(36).substr(2, 9)}`)

const handleInput = (event) => {
  emit('update:modelValue', event.target.value)
}

const handleBlur = (event) => {
  emit('blur', event)
}
</script>

<style scoped>
.ms-input-date {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.ms-input-date__label {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #182026;
  line-height: 18px;
}

.ms-input-date__required {
  color: #e74c3c;
}

.ms-input-date__wrapper {
  position: relative;
}

.ms-input-date__field {
  width: 100%;
  height: 36px;
  padding: 0 36px 0 12px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  background-color: #ffffff;
  transition: all 0.2s ease;
  box-sizing: border-box;
}

.ms-input-date__field::placeholder {
  color: #bfbfbf;
  font-style: italic;
}

/* Hover state */
.ms-input-date__field:hover:not(:disabled) {
  border-color: #40a9ff;
}

/* Focus state */
.ms-input-date__field:focus {
  border-color: #1aa4c8;
  outline: none;
  background-color: #ffffff;
}

/* Disabled state */
.ms-input-date--disabled .ms-input-date__field {
  background-color: #f5f5f5;
  color: #bfbfbf;
  cursor: not-allowed;
  border-color: #d9d9d9;
}

/* Error state */
.ms-input-date--error .ms-input-date__field {
  border-color: #e74c3c;
}

.ms-input-date--error .ms-input-date__field:focus {
  border-color: #e74c3c;
}

/* Calendar icon */
.ms-input-date__icon {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
width: 18px;
	height: 18px;
  color: #090f39;
  pointer-events: none;
  /* TODO: Thêm mask-position cho icon calendar từ SVG */
   -webkit-mask-position: -287px -67px; 
   mask-position: -287px -67px; 
}

.ms-input-date__error-message {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #e74c3c;
  line-height: 18px;
}
</style>
