<!--
  Mô tả: Text field/Input component
  States: Default, Hover, Active, Focus, Disable, Error
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div class="ms-input" :class="{ 'ms-input--error': error, 'ms-input--disabled': disabled }">
    <label v-if="label" class="ms-input__label" :for="inputId">
      {{ label }}
      <span v-if="required" class="ms-input__required">*</span>
    </label>
    <div class="ms-input__wrapper">
      <input
        :id="inputId"
        class="ms-input__field"
        :type="type"
        :value="modelValue || ''"
        :placeholder="!modelValue && !placeholder ? 'Không có dữ liệu' : placeholder"
        :disabled="disabled"
        @input="$emit('update:modelValue', $event.target.value)"
        @focus="$emit('focus', $event)"
        @blur="$emit('blur', $event)"
      />
    </div>
    <span v-if="error && errorMessage" class="ms-input__error-message">
      {{ errorMessage }}
    </span>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsInput component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { computed } from 'vue'

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
    default: 'Roboto - 14pt - Regular'
  },
  type: {
    type: String,
    default: 'text'
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

defineEmits(['update:modelValue', 'focus', 'blur'])

const inputId = computed(() => `input-${Math.random().toString(36).substr(2, 9)}`)
</script>

<style scoped>
.ms-input {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.ms-input__label {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #182026;
  line-height: 18px;
}

.ms-input__required {
  color: #e74c3c;
}

.ms-input__wrapper {
  position: relative;
}

.ms-input__field {
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
}

.ms-input__field:not(:disabled):empty::placeholder {
  content: 'Không có dữ liệu';
}

.ms-input__field::placeholder {
  color: #bfbfbf;
  font-style: italic;
}

/* Hover state */
.ms-input__field:hover:not(:disabled) {
  border-color: #40a9ff;
}

/* Focus state */
.ms-input__field:focus {
  border-color: #1aa4c8;
  outline: none;
  background-color: #ffffff;
}

.ms-input__field:focus::placeholder {
  color: #1aa4c8;
}

/* Disabled state */
.ms-input--disabled .ms-input__field {
  background-color: #f5f5f5;
  color: #bfbfbf;
  cursor: not-allowed;
  border-color: #d9d9d9;
}

/* Error state */
.ms-input--error .ms-input__field {
  border-color: #e74c3c;
}

.ms-input--error .ms-input__field:focus {
  border-color: #e74c3c;
}

.ms-input__error-message {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #e74c3c;
  line-height: 18px;
}
</style>
