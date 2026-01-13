<!--
  Mô tả: Number input component với spinner controls
  States: Default, Hover, Active, Focus, Disable, Error
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div
    class="ms-input-number"
    :class="{ 'ms-input-number--error': error, 'ms-input-number--disabled': disabled }"
  >
    <label v-if="label" class="ms-input-number__label" :for="inputId">
      {{ label }}
      <span v-if="required" class="ms-input-number__required">*</span>
    </label>
    <div class="ms-input-number__wrapper">
      <input
        :id="inputId"
        class="ms-input-number__field"
        type="number"
        :value="modelValue"
        :placeholder="placeholder"
        :disabled="disabled"
        :min="min"
        :max="max"
        :step="step"
        @input="handleInput"
        @focus="$emit('focus', $event)"
        @blur="$emit('blur', $event)"
      />
      <div class="ms-input-number__controls">
        <button
          type="button"
          class="ms-input-number__btn ms-input-number__btn--up"
          :disabled="disabled || (max !== undefined && Number(modelValue) >= max)"
          @click="increment"
        >
          <i class="icon icon-chevron-up-toolbar"></i>
        </button>
        <div class="ms-input-number__divider"></div>
        <button
          type="button"
          class="ms-input-number__btn ms-input-number__btn--down"
          :disabled="disabled || (min !== undefined && Number(modelValue) <= min)"
          @click="decrement"
        >
          <i class="icon icon-chevron-down-toolbar"></i>
        </button>
      </div>
    </div>
    <span v-if="error && errorMessage" class="ms-input-number__error-message">
      {{ errorMessage }}
    </span>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsInputNumber component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { computed } from 'vue'

const props = defineProps({
  modelValue: {
    type: [String, Number],
    default: 0,
  },
  label: {
    type: String,
    default: '',
  },
  placeholder: {
    type: String,
    default: '',
  },
  disabled: {
    type: Boolean,
    default: false,
  },
  error: {
    type: Boolean,
    default: false,
  },
  errorMessage: {
    type: String,
    default: 'Error message',
  },
  required: {
    type: Boolean,
    default: false,
  },
  min: {
    type: Number,
    default: undefined,
  },
  max: {
    type: Number,
    default: undefined,
  },
  step: {
    type: Number,
    default: 1,
  },
})

const emit = defineEmits(['update:modelValue', 'focus', 'blur'])

const inputId = computed(() => `input-number-${Math.random().toString(36).substr(2, 9)}`)

const handleInput = (event) => {
  let value = event.target.value
  if (value === '') {
    emit('update:modelValue', '')
    return
  }

  const numValue = Number(value)
  if (props.min !== undefined && numValue < props.min) {
    value = props.min
  }
  if (props.max !== undefined && numValue > props.max) {
    value = props.max
  }

  emit('update:modelValue', value)
}

const increment = () => {
  const currentValue = Number(props.modelValue) || 0
  const newValue = currentValue + props.step
  if (props.max === undefined || newValue <= props.max) {
    emit('update:modelValue', newValue)
  }
}

const decrement = () => {
  const currentValue = Number(props.modelValue) || 0
  const newValue = Math.max(0, currentValue - props.step)
  if (props.min === undefined || newValue >= props.min) {
    emit('update:modelValue', newValue)
  }
}
</script>

<style scoped>
.ms-input-number {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.ms-input-number__label {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #182026;
  line-height: 18px;
}

.ms-input-number__required {
  color: #e74c3c;
}

.ms-input-number__wrapper {
  position: relative;
}

.ms-input-number__field {
  width: 100%;
  height: 36px;
  padding: 0 24px 0 12px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  background-color: #ffffff;
  transition: all 0.2s ease;
  box-sizing: border-box;
  -moz-appearance: textfield;
}

.ms-input-number__field::-webkit-outer-spin-button,
.ms-input-number__field::-webkit-inner-spin-button {
  -webkit-appearance: none;
  margin: 0;
}

.ms-input-number__field::placeholder {
  color: #bfbfbf;
  font-style: italic;
}

/* Hover state */
.ms-input-number__field:hover:not(:disabled) {
  border-color: #40a9ff;
}

/* Focus state */
.ms-input-number__field:focus {
  border-color: #1aa4c8;
  outline: none;
  background-color: #ffffff;
}

/* Disabled state */
.ms-input-number--disabled .ms-input-number__field {
  background-color: #f5f5f5;
  color: #bfbfbf;
  cursor: not-allowed;
  border-color: #d9d9d9;
}

/* Error state */
.ms-input-number--error .ms-input-number__field {
  border-color: #e74c3c;
}

.ms-input-number--error .ms-input-number__field:focus {
  border-color: #e74c3c;
}

/* Controls */
.ms-input-number__controls {
  position: absolute;
  right: 4px;
  top: 50%;
  transform: translateY(-50%);
  width: 16px;
  height: 18px;
  display: flex;
  flex-direction: column;
  background-color: transparent;
  pointer-events: none;
}

.ms-input-number__btn {
  flex: 1;
  border: none;
  background-color: transparent;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: opacity 0.2s ease;
  padding: 0;
  min-height: 0;
  pointer-events: auto;
}

.ms-input-number__btn .icon {
  width: 8px;
  height: 5px;
  color: #090f39;
  transition: color 0.2s ease;
}

.ms-input-number__btn:hover:not(:disabled) .icon {
  color: #1aa4c8;
}

.ms-input-number__btn:hover:not(:disabled) {
  opacity: 0.8;
}

.ms-input-number__btn:active:not(:disabled) {
  opacity: 0.6;
}

.ms-input-number__btn:disabled {
  opacity: 0.4;
  cursor: not-allowed;
}

.ms-input-number__divider {
  height: 1px;
  background-color: #d9d9d9;
}

.ms-input-number__error-message {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #e74c3c;
  line-height: 18px;
}
</style>
