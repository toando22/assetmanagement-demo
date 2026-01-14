<!--
  Mô tả: Context Menu component - Menu hiển thị khi click chuột phải
  Features:
  - Position tại vị trí click
  - Keyboard navigation (↑↓, Enter, Esc)
  - Auto-close khi click outside
  - Tái sử dụng cho nhiều màn hình
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
-->

<template>
  <teleport to="body">
    <transition name="context-menu-fade">
      <div
        v-if="modelValue"
        class="ms-context-menu-overlay"
        @click.self="handleClose"
      >
        <div
          ref="menuRef"
          class="ms-context-menu"
          :style="menuStyle"
          @click.stop
        >
          <button
            v-for="(item, index) in items"
            :key="index"
            class="ms-context-menu__item"
            :class="{
              'ms-context-menu__item--disabled': item.disabled,
              'ms-context-menu__item--focused': focusedIndex === index
            }"
            :disabled="item.disabled"
            @click="handleItemClick(item)"
            @mouseenter="focusedIndex = index"
          >
            <i v-if="item.icon" :class="['icon', `icon-${item.icon}`, 'ms-context-menu__item-icon']"></i>
            <span class="ms-context-menu__item-label">{{ item.label }}</span>
            <span v-if="item.shortcut" class="ms-context-menu__item-shortcut">{{ item.shortcut }}</span>
          </button>
        </div>
      </div>
    </transition>
  </teleport>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsContextMenu component
  
  CreatedBy: DDToan - (11/1/2026)
*/

import { ref, computed, watch, onMounted, onUnmounted, nextTick } from 'vue'

const props = defineProps({
  modelValue: {
    type: Boolean,
    default: false
  },
  items: {
    type: Array,
    default: () => []
  },
  x: {
    type: Number,
    default: 0
  },
  y: {
    type: Number,
    default: 0
  }
})

const emit = defineEmits(['update:modelValue', 'select', 'close'])

const menuRef = ref(null)
const focusedIndex = ref(-1)

// Computed: Tính toán vị trí menu để không vượt viewport
const menuStyle = computed(() => {
  if (!menuRef.value) {
    return {
      left: `${props.x}px`,
      top: `${props.y}px`
    }
  }

  const menuWidth = menuRef.value.offsetWidth || 200
  const menuHeight = menuRef.value.offsetHeight || 300
  const viewportWidth = window.innerWidth
  const viewportHeight = window.innerHeight

  let left = props.x
  let top = props.y

  // Điều chỉnh nếu vượt bên phải
  if (left + menuWidth > viewportWidth) {
    left = viewportWidth - menuWidth - 10
  }

  // Điều chỉnh nếu vượt bên dưới
  if (top + menuHeight > viewportHeight) {
    top = viewportHeight - menuHeight - 10
  }

  // Đảm bảo không vượt bên trái và trên
  left = Math.max(10, left)
  top = Math.max(10, top)

  return {
    left: `${left}px`,
    top: `${top}px`
  }
})

// Methods
const handleItemClick = (item) => {
  if (item.disabled) return
  
  emit('select', item)
  emit('update:modelValue', false)
  emit('close')
  
  // Gọi action nếu có
  if (item.action && typeof item.action === 'function') {
    item.action()
  }
}

const handleClose = () => {
  emit('update:modelValue', false)
  emit('close')
}

// Keyboard navigation
const handleKeyDown = (event) => {
  if (!props.modelValue || props.items.length === 0) return

  switch (event.key) {
    case 'ArrowDown':
      event.preventDefault()
      focusedIndex.value = (focusedIndex.value + 1) % props.items.length
      break
    case 'ArrowUp':
      event.preventDefault()
      focusedIndex.value = focusedIndex.value <= 0 
        ? props.items.length - 1 
        : focusedIndex.value - 1
      break
    case 'Enter':
      event.preventDefault()
      if (focusedIndex.value >= 0 && focusedIndex.value < props.items.length) {
        const item = props.items[focusedIndex.value]
        if (!item.disabled) {
          handleItemClick(item)
        }
      }
      break
    case 'Escape':
      event.preventDefault()
      handleClose()
      break
  }
}

// Click outside để đóng menu
const handleClickOutside = (event) => {
  if (!props.modelValue) return
  
  if (menuRef.value && !menuRef.value.contains(event.target)) {
    handleClose()
  }
}

// Watch: Reset focused index khi mở menu
watch(() => props.modelValue, (newValue) => {
  if (newValue) {
    focusedIndex.value = -1
    nextTick(() => {
      // Focus item đầu tiên không disabled
      const firstEnabledIndex = props.items.findIndex(item => !item.disabled)
      if (firstEnabledIndex >= 0) {
        focusedIndex.value = firstEnabledIndex
      }
    })
  }
})

onMounted(() => {
  document.addEventListener('keydown', handleKeyDown)
  document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
  document.removeEventListener('keydown', handleKeyDown)
  document.removeEventListener('click', handleClickOutside)
})
</script>

<style scoped>
/*
  Mô tả: Styles cho MsContextMenu component
  
  CreatedBy: DDToan - (11/1/2026)
*/

.ms-context-menu-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 10000;
  background: transparent;
}

.ms-context-menu {
  position: fixed;
  background-color: #ffffff;
  border-radius: 4px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
  min-width: 180px;
  padding: 4px 0;
  font-family: 'Roboto', sans-serif;
  z-index: 10001;
}

.ms-context-menu__item {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 12px;
  border: none;
  background: transparent;
  cursor: pointer;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  text-align: left;
  transition: background-color 0.2s ease;
  box-sizing: border-box;
}

.ms-context-menu__item:hover:not(:disabled),
.ms-context-menu__item--focused:not(:disabled) {
  background-color: #e6f7ff;
  color: #1aa4c8;
}

.ms-context-menu__item:disabled,
.ms-context-menu__item--disabled {
  opacity: 0.4;
  cursor: not-allowed;
  color: #bfbfbf;
}

.ms-context-menu__item-icon {
  width: 16px;
  height: 16px;
  flex-shrink: 0;
  color: inherit;
}

.ms-context-menu__item-label {
  flex: 1;
  white-space: nowrap;
}

.ms-context-menu__item-shortcut {
  font-size: 12px;
  color: #999;
  margin-left: auto;
}

/* Transition */
.context-menu-fade-enter-active,
.context-menu-fade-leave-active {
  transition: opacity 0.15s ease;
}

.context-menu-fade-enter-active .ms-context-menu,
.context-menu-fade-leave-active .ms-context-menu {
  transition: transform 0.15s ease, opacity 0.15s ease;
}

.context-menu-fade-enter-from,
.context-menu-fade-leave-to {
  opacity: 0;
}

.context-menu-fade-enter-from .ms-context-menu,
.context-menu-fade-leave-to .ms-context-menu {
  transform: scale(0.95);
  opacity: 0;
}
</style>
