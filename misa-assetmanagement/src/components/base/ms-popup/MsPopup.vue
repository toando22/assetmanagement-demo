<!--
  Mô tả: Popup/Modal component
  Features: Dialog, Alert, Confirm
  States: Open, Close
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <teleport to="body">
    <transition name="popup-fade">
      <div v-if="modelValue" class="ms-popup">
        <div class="ms-popup__overlay" @click="handleOverlayClick"></div>
        <div class="ms-popup__container" :style="{ width: width }">
          <!-- Header -->
          <div class="ms-popup__header">
            <h3 class="ms-popup__title">{{ title }}</h3>
            <button class="ms-popup__close" @click="handleClose">
              <i class="icon icon-close"></i>
            </button>
          </div>

          <!-- Content -->
          <div class="ms-popup__content">
            <slot></slot>
          </div>

          <!-- Footer -->
          <div v-if="$slots.footer || showDefaultFooter" class="ms-popup__footer">
            <slot name="footer">
              <MsButton variant="secondary" @click="handleCancel">
                {{ cancelText }}
              </MsButton>
              <MsButton variant="primary" @click="handleConfirm">
                {{ confirmText }}
              </MsButton>
            </slot>
          </div>
        </div>
      </div>
    </transition>
  </teleport>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsPopup component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { watch } from 'vue'
import MsButton from '../ms-button/MsButton.vue'

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  title: {
    type: String,
    default: 'Popup Title'
  },
  width: {
    type: String,
    default: '600px'
  },
  closeOnOverlay: {
    type: Boolean,
    default: true
  },
  showDefaultFooter: {
    type: Boolean,
    default: true
  },
  confirmText: {
    type: String,
    default: 'Xác nhận'
  },
  cancelText: {
    type: String,
    default: 'Hủy'
  }
})

const emit = defineEmits(['update:modelValue', 'confirm', 'cancel', 'close'])

const handleClose = () => {
  emit('update:modelValue', false)
  emit('close')
}

const handleOverlayClick = () => {
  if (props.closeOnOverlay) {
    handleClose()
  }
}

const handleConfirm = () => {
  emit('confirm')
}

const handleCancel = () => {
  emit('cancel')
  handleClose()
}

// Lock body scroll when popup is open
watch(() => props.modelValue, (newValue) => {
  if (newValue) {
    document.body.style.overflow = 'hidden'
  } else {
    document.body.style.overflow = ''
  }
})
</script>

<style scoped>
.ms-popup {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 9999;
  display: flex;
  align-items: center;
  justify-content: center;
}

.ms-popup__overlay {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background-color: rgba(0, 0, 0, 0.5);
}

.ms-popup__container {
  position: relative;
  background-color: #ffffff;
  border-radius: 4px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
  width: 870px;
  max-width: 90vw;
  max-height: 90vh;
  display: flex;
  flex-direction: column;
  z-index: 1;
  resize: both;
  overflow: hidden;
  max-height: calc(100vh - 32px);
  resize: none;
}

.ms-popup__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 20px 20px 0 20px;
  border-bottom: none;
}

.ms-popup__title {
  font-family: 'Roboto', sans-serif;
  font-size: 20px;
  font-weight: 700;
  color: #182026;
  margin: 0;
  line-height: 26px;
}

.ms-popup__close {
  width: 24px;
  height: 24px;
  border: none;
  background: none;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 2px;
  color: #666666;
  transition: all 0.2s ease;
  padding: 0;
}

.ms-popup__close:hover {
  background-color: #f5f5f5;
  color: #333333;
}

.ms-popup__close .icon {
  width: 20px;
  height: 20px;
}

.ms-popup__content {
  padding: 20px 16px 0 16px;
  overflow: visible;
  flex: 1;
  min-height: 0;
}

.ms-popup__footer {
  display: flex;
  align-items: center;
  justify-content: flex-end;
  gap: 10px;
  padding: 52px 16px 36px 16px;
  border-top: none;
}

/* Transition */
.popup-fade-enter-active,
.popup-fade-leave-active {
  transition: opacity 0.3s ease;
}

.popup-fade-enter-active .ms-popup__container,
.popup-fade-leave-active .ms-popup__container {
  transition: transform 0.3s ease, opacity 0.3s ease;
}

.popup-fade-enter-from,
.popup-fade-leave-to {
  opacity: 0;
}

.popup-fade-enter-from .ms-popup__container,
.popup-fade-leave-to .ms-popup__container {
  transform: scale(0.9);
  opacity: 0;
}
</style>
