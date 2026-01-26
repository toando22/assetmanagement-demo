<!--
  Mô tả: Button component
  Types: Main button, Sub button, Outline button
  States: Default, Hover, Active, Focus, Disable
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <button
    class="ms-button"
    :class="[
      `ms-button--${variant}`,
      { 'ms-button--disabled': disabled }
    ]"
    :disabled="disabled"
    @click="$emit('click', $event)"
  >
    <i v-if="icon" class="ms-button__icon icon" :class="icon"></i>
    <span v-if="$slots.default" class="ms-button__text">
      <slot></slot>
    </span>
  </button>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsButton component
  
  CreatedBy: DDToan - (09/1/2026)
*/

defineProps({
  variant: {
    type: String,
    default: 'primary', // primary, secondary, outline
    validator: (value) => ['primary', 'secondary', 'outline'].includes(value)
  },
  icon: {
    type: String,
    default: ''
  },
  disabled: {
    type: Boolean,
    default: false
  }
})

defineEmits(['click'])
</script>

<style scoped>
.ms-button {
  height: 36px;
  padding: 0 16px;
  border-radius: 2px;
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  font-weight: 400;
  line-height: 18px;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  cursor: pointer;
  transition: all 0.2s ease;
  border: 1px solid transparent;
  white-space: nowrap;
  box-sizing: border-box;
}

.ms-button__icon {
  width: 16px;
  height: 16px;
}

/* ============================================ */
/* PRIMARY BUTTON (Main button)                 */
/* ============================================ */

.ms-button--primary {
  background-color: #1aa4c8;
  color: #ffffff;
  border-radius: 3px;
  box-shadow: inset 0 2px 6px rgba(0, 0, 0, .16);
  width: 86px;
}

.ms-button--primary:hover:not(:disabled) {
  background-color: #1590b0;
  border-color: #1590b0;
}

.ms-button--primary:active:not(:disabled) {
  background-color: #107c98;
  border-color: #107c98;
}

.ms-button--primary:focus:not(:disabled) {
  background-color: #1aa4c8;
  border-color: #1aa4c8;
  box-shadow: 0 0 0 2px rgba(26, 164, 200, 0.2);
}

/* ============================================ */
/* SECONDARY BUTTON (Sub button)                */
/* ============================================ */

.ms-button--secondary {
  background-color: #ffffff;
  color: #1aa4c8;
  border-color: #1aa4c8;
  width: 86px;
}

.ms-button--secondary:hover:not(:disabled) {
  background-color: #f0fafc;
  border-color: #1590b0;
  color: #1590b0;
}

.ms-button--secondary:active:not(:disabled) {
  background-color: #e0f5f9;
  border-color: #107c98;
  color: #107c98;
}

.ms-button--secondary:focus:not(:disabled) {
  background-color: #ffffff;
  border-color: #1aa4c8;
  box-shadow: 0 0 0 2px rgba(26, 164, 200, 0.2);
}

/* ============================================ */
/* OUTLINE BUTTON                               */
/* ============================================ */

.ms-button--outline {
  background-color: #ffffff;
  color: #595959;
  border-color: #d9d9d9;
  padding: 0 12px;
}

.ms-button--outline:hover:not(:disabled) {
  border-color: #1aa4c8;
  color: #1aa4c8;
  background-color: #f5fbff;
}

.ms-button--outline:active:not(:disabled) {
  border-color: #1590b0;
  color: #1590b0;
  background-color: #e6f7ff;
}

.ms-button--outline:focus:not(:disabled) {
  border-color: #1aa4c8;
  color: #1aa4c8;
  box-shadow: 0 0 0 2px rgba(26, 164, 200, 0.2);
}

/* ============================================ */
/* DISABLED STATE                               */
/* ============================================ */

.ms-button--disabled {
  opacity: 0.4;
  cursor: not-allowed;
}
</style>
