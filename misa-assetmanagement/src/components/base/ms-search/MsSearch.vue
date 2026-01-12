<!--
  Mô tả: Search input component
  States: Default, Hover, Active, Focused, Disable
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div class="ms-search" :class="{ 'ms-search--disabled': disabled }">
    <i class="ms-search__icon icon icon-search"></i>
    <input
      class="ms-search__input"
      type="text"
      :value="modelValue"
      :placeholder="placeholder"
      :disabled="disabled"
      @input="$emit('update:modelValue', $event.target.value)"
      @focus="handleFocus"
      @blur="handleBlur"
    />
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsSearch component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { ref } from 'vue'

defineProps({
  modelValue: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: 'Tìm kiếm tài sản'
  },
  disabled: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['update:modelValue', 'focus', 'blur'])

const isFocused = ref(false)

const handleFocus = (event) => {
  isFocused.value = true
  emit('focus', event)
}

const handleBlur = (event) => {
  isFocused.value = false
  emit('blur', event)
}
</script>

<style scoped>
.ms-search {
  position: relative;
  width: 100%;
}

.ms-search__icon {
  position: absolute;
  left: 12px;
  top: 50%;
  transform: translateY(-50%);
  width: 16px;
  height: 16px;
  color: #999999;
  transition: color 0.2s ease;
  pointer-events: none;
}

.ms-search__input {
  width: 100%;
  height: 38px;
  padding: 0 12px 0 40px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #182026;
  background-color: #ffffff;
  transition: all 0.2s ease;
  box-sizing: border-box;
}

.ms-search__input::placeholder {
  color: #bfbfbf;
  font-style: italic;
}

/* Hover state */
.ms-search:hover:not(.ms-search--disabled) .ms-search__input {
  border-color: #40a9ff;
}

/* Focus/Active state */
.ms-search__input:focus {
  border-color: #1aa4c8;
  outline: none;
  background-color: #f5fbff;
}

.ms-search__input:focus::placeholder {
  color: #1aa4c8;
}

.ms-search__input:focus ~ .ms-search__icon,
.ms-search:has(.ms-search__input:focus) .ms-search__icon {
  color: #1aa4c8;
}

/* Disabled state */
.ms-search--disabled .ms-search__input {
  background-color: #f5f5f5;
  color: #bfbfbf;
  cursor: not-allowed;
  border-color: #d9d9d9;
}

.ms-search--disabled .ms-search__icon {
  color: #bfbfbf;
}
</style>
