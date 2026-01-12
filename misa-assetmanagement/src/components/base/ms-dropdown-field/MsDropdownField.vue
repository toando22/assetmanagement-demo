<!--
  Mô tả: Wrapper component cho MsDropdown với label và required
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div class="ms-dropdown-field" :class="{ 'ms-dropdown-field--error': error, 'ms-dropdown-field--disabled': disabled }">
    <label v-if="label" class="ms-dropdown-field__label" :for="fieldId">
      {{ label }}
      <span v-if="required" class="ms-dropdown-field__required">*</span>
    </label>
    <MsDropdown
      :id="fieldId"
      :model-value="modelValue"
      :options="options"
      :placeholder="placeholder"
      :with-checkbox="withCheckbox"
      :with-table="withTable"
      @update:model-value="$emit('update:modelValue', $event)"
      @change="$emit('change', $event)"
    />
    <span v-if="error && errorMessage" class="ms-dropdown-field__error-message">
      {{ errorMessage }}
    </span>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsDropdownField component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { computed } from 'vue'
import MsDropdown from '../ms-dropdown/MsDropdown.vue'

const props = defineProps({
  modelValue: {
    type: [String, Number, Array],
    default: ''
  },
  label: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: 'Chọn...'
  },
  options: {
    type: Array,
    default: () => []
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
  },
  withCheckbox: {
    type: Boolean,
    default: false
  },
  withTable: {
    type: Boolean,
    default: false
  }
})

defineEmits(['update:modelValue', 'change'])

const fieldId = computed(() => `dropdown-${Math.random().toString(36).substr(2, 9)}`)
</script>

<style scoped>
.ms-dropdown-field {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.ms-dropdown-field__label {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #182026;
  line-height: 18px;
}

.ms-dropdown-field__required {
  color: #e74c3c;
}

.ms-dropdown-field__error-message {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #e74c3c;
  line-height: 18px;
}
</style>
