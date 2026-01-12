<!--
  Mô tả: Dialog component cho confirm, error, warning, success, info
  Types: confirm, error, warning, success, info
  Features: Icon, message, custom buttons
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
-->

<template>
  <teleport to="body">
    <transition name="dialog-fade">
      <div v-if="modelValue" class="ms-dialog" @click.self="handleOverlayClick">
        <div class="ms-dialog__container">
          <!-- Icon và Message (horizontal layout) -->
          <div class="ms-dialog__content">
            <!-- Icon -->
            <div v-if="showIcon" class="ms-dialog__icon" :class="`ms-dialog__icon--${type}`">
              <i v-if="type === 'success'" class="icon icon-check"></i>
              <i v-else-if="type === 'error'" class="icon icon-close"></i>
              <i v-else-if="type === 'warning' || type === 'confirm'" class="icon icon-warning icon--warning"></i>
              <span v-else-if="type === 'info'" class="ms-dialog__icon-text">i</span>
              <span v-else class="ms-dialog__icon-text">?</span>
            </div>

            <!-- Message -->
            <div class="ms-dialog__message">
              {{ message }}
            </div>
          </div>

          <!-- Buttons -->
          <div v-if="buttons && buttons.length > 0" class="ms-dialog__footer">
            <MsButton
              v-for="(button, index) in buttons"
              :key="index"
              :variant="button.variant || 'primary'"
              @click="handleButtonClick(button)"
            >
              {{ button.label }}
            </MsButton>
          </div>
        </div>
      </div>
    </transition>
  </teleport>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsDialog component
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
*/

import { watch } from 'vue'
import MsButton from '../ms-button/MsButton.vue'

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  type: {
    type: String,
    default: 'confirm',
    validator: (value) => ['confirm', 'error', 'warning', 'success', 'info'].includes(value)
  },
  message: {
    type: String,
    default: ''
  },
  title: {
    type: String,
    default: ''
  },
  buttons: {
    type: Array,
    default: () => [
      { label: 'Đóng', variant: 'primary' }
    ]
  },
  showIcon: {
    type: Boolean,
    default: true
  },
  closeOnOverlay: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['update:modelValue', 'confirm', 'cancel', 'close', 'button-click'])

const handleButtonClick = (button) => {
  emit('button-click', button)
  
  // Auto close nếu button có action là 'close' hoặc không có action
  if (!button.action || button.action === 'close') {
    emit('update:modelValue', false)
    emit('close')
  }
  
  // Emit confirm nếu button có action là 'confirm'
  if (button.action === 'confirm') {
    emit('confirm', button)
  }
  
  // Emit cancel nếu button có action là 'cancel'
  if (button.action === 'cancel') {
    emit('cancel', button)
  }
}

const handleOverlayClick = () => {
  if (props.closeOnOverlay) {
    emit('update:modelValue', false)
    emit('close')
  }
}

// Lock body scroll when dialog is open
watch(() => props.modelValue, (newValue) => {
  if (newValue) {
    document.body.style.overflow = 'hidden'
  } else {
    document.body.style.overflow = ''
  }
})
</script>

<style scoped>
/*
  Mô tả: Styles cho MsDialog component
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
*/

.ms-dialog {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 10000; /* Cao hơn MsPopup (9999) để hiển thị trên popup */
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: rgba(0, 0, 0, 0.5);
}

.ms-dialog__container {
  background-color: #ffffff;
  border-radius: 4px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
  min-width: 400px;
  max-width: 600px;
  padding: 24px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.ms-dialog__content {
  display: flex;
  align-items: center;
  gap: 16px;
  width: 100%;
}

.ms-dialog__icon {
  width: 48px;
  height: 48px;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}

.ms-dialog__icon--success {
  background-color: var(--color-success-light, #e6f7f2);
  color: var(--color-success, #00a76a);
  border-radius: 50%;
}

.ms-dialog__icon--error {
  background-color: var(--color-danger-light, #fde8e6);
  color: var(--color-danger, #e01e01);
  border-radius: 50%;
}

.ms-dialog__icon--warning {
  background-color: transparent;
  color: var(--color-warning, #ff9300);
  width: 56px;
  height: 56px;
}


.ms-dialog__icon--info {
  background-color: var(--color-primary-light, #e6f4fb);
  color: var(--color-primary, #0095da);
  border-radius: 50%;
}

.ms-dialog__icon--confirm {
  background-color: transparent;
  color: var(--color-warning, #ff9300);
  width: 56px;
  height: 56px;
  transform: scale(2);
}



.ms-dialog__icon-text {
  font-size: 32px;
  font-weight: 700;
  line-height: 1;
}

/* Override icon-text size for success/error/info (circle icons) */
.ms-dialog__icon--success .ms-dialog__icon-text,
.ms-dialog__icon--error .ms-dialog__icon-text,
.ms-dialog__icon--info .ms-dialog__icon-text {
  font-size: 32px;
  margin-top: 0;
}

.ms-dialog__message {
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  line-height: 20px;
  text-align: left;
  word-wrap: break-word;
  flex: 1;
}

.ms-dialog__footer {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 10px;
  width: 100%;
  margin-top: 8px;
}

/* Transition */
.dialog-fade-enter-active,
.dialog-fade-leave-active {
  transition: opacity 0.3s ease;
}

.dialog-fade-enter-active .ms-dialog__container,
.dialog-fade-leave-active .ms-dialog__container {
  transition: transform 0.3s ease, opacity 0.3s ease;
}

.dialog-fade-enter-from,
.dialog-fade-leave-to {
  opacity: 0;
}

.dialog-fade-enter-from .ms-dialog__container,
.dialog-fade-leave-to .ms-dialog__container {
  transform: scale(0.9);
  opacity: 0;
}
</style>
