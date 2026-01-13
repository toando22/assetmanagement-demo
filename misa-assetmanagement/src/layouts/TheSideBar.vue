<!--
  Mô tả: Component Sidebar với chức năng thu gọn/mở rộng
  Features:
  - Thu gọn/mở rộng bằng nút toggle
  - Highlight menu item active
  - Responsive
  - Icon SVG sprite
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
-->

<template>
  <aside class="sidebar" :class="{ 'sidebar--collapsed': isCollapsed }">
    <!-- Logo và Title -->
    <div class="sidebar__header">
      <div class="sidebar__logo">
        <i class="icon icon-logo-misa"></i>
      </div>
      <transition name="fade">
        <span v-if="!isCollapsed" class="sidebar__title">MISA QLTS</span>
      </transition>
    </div>

    <!-- Menu Items -->
    <nav class="sidebar__nav">
      <ul class="sidebar__menu">
        <li 
          v-for="item in menuItems" 
          :key="item.id"
          class="sidebar__menu-item"
          :class="{ 'sidebar__menu-item--active': item.id === activeMenu }"
          @click="handleMenuClick(item)"
        >
          <a 
            href="#" 
            class="sidebar__menu-link" 
            @click.prevent
            :data-tooltip="item.label"
          >
            <i :class="['icon', `icon-${item.icon}`]" class="sidebar__menu-icon"></i>
            <transition name="fade">
              <span v-if="!isCollapsed" class="sidebar__menu-text">{{ item.label }}</span>
            </transition>
            <!-- Chevron cho submenu -->
            <transition name="fade">
              <i 
                v-if="!isCollapsed && item.hasSubmenu" 
                class="icon icon--sm icon-chevron-down sidebar__menu-chevron"
                :class="{ 'sidebar__menu-chevron--open': item.isOpen }"
              ></i>
            </transition>
            <!-- Tooltip for collapsed state -->
            <span v-if="isCollapsed" class="sidebar__menu-tooltip">{{ item.label }}</span>
          </a>
        </li>
      </ul>
    </nav>

    <!-- Footer với Toggle Button -->
    <div class="sidebar__footer">
      <button 
        class="sidebar__toggle" 
        @click="toggleSidebar"
        :title="isCollapsed ? 'Mở rộng menu' : 'Thu gọn menu'"
      >
        <i :class="['icon', 'icon--sm', isCollapsed ? 'icon-chevron-right' : 'icon-chevron-left']"></i>
      </button>
    </div>
  </aside>
</template>

<script setup>
/*
  Mô tả: Script setup cho TheSidebar component
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

import { ref } from 'vue'

// State
const isCollapsed = ref(false)
const activeMenu = ref('tai-san') // Menu active mặc định

// Danh sách menu items
const menuItems = ref([
  { id: 'tong-quan', label: 'Tổng quan', icon: 'dashboard', hasSubmenu: false, isOpen: false },
  { id: 'tai-san', label: 'Tài sản', icon: 'asset', hasSubmenu: true, isOpen: false },
  { id: 'tai-san-htdb', label: 'Tài sản HT-ĐB', icon: 'asset-htdb', hasSubmenu: true, isOpen: false },
  { id: 'cong-cu', label: 'Công cụ dụng cụ', icon: 'tools', hasSubmenu: true, isOpen: false },
  { id: 'danh-muc', label: 'Danh mục', icon: 'category', hasSubmenu: false, isOpen: false },
  { id: 'tra-cuu', label: 'Tra cứu', icon: 'search', hasSubmenu: true, isOpen: false },
  { id: 'bao-cao', label: 'Báo cáo', icon: 'report', hasSubmenu: true, isOpen: false },
])

/*
  Mô tả: Toggle trạng thái thu gọn/mở rộng sidebar
  
  CreatedBy: DDToan - (09/1/2026)
*/
const toggleSidebar = () => {
  isCollapsed.value = !isCollapsed.value
}

/*
  Mô tả: Xử lý click vào menu item
  
  CreatedBy: DDToan - (09/1/2026)
*/
const handleMenuClick = (item) => {
  // Set active menu
  activeMenu.value = item.id
  
  // Toggle submenu nếu có
  if (item.hasSubmenu) {
    item.isOpen = !item.isOpen
  }
  
  // TODO: Thêm router navigation sau
}
</script>

<style scoped>
/*
  Mô tả: Styles cho Sidebar component
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/
.sidebar {
  width: var(--sidebar-width);
  height: 100vh;
  background-color: var(--color-sidebar-bg);
  display: flex;
  flex-direction: column;
  transition: width var(--transition-base);
  position: relative;
  z-index: var(--z-sticky);
}

.sidebar--collapsed {
  width: var(--sidebar-collapsed-width);
}

/* Header */
.sidebar__header {
  height: 50px;
  display: flex;
  align-items: center;
  padding: 0 var(--spacing-md);
  gap: var(--spacing-md);
  
}

.sidebar__logo {
  width: 40px;
  height: 40px;
  flex-shrink: 0;
  display: flex;
  align-items: center;
  justify-content: center;
}

.sidebar__logo .icon.icon-logo-misa {
  width: 40px;
  height: 40px;
  background-color: transparent;
}

.sidebar__title {
  font-size: var(--font-size-lg);
  font-weight: var(--font-weight-bold);
  color: var(--color-white);
  white-space: nowrap;
}

/* Navigation */
.sidebar__nav {
  flex: 1;
  overflow-y: auto;
  overflow-x: hidden;
  padding: var(--spacing-md) 0;
}

.sidebar__menu {
  list-style: none;
  margin: 0;
  padding: 0;
}

.sidebar__menu-item {
  margin: 0;
  padding: 0;
}

/* Spacing đều hai bên cho menu item khi collapsed */
.sidebar--collapsed .sidebar__menu-item {
  padding: 0 var(--spacing-sm);
}

.sidebar__menu-link {
  display: flex;
  align-items: center;
  gap: var(--spacing-md);
  padding: var(--spacing-md);
  color: #a6b5c5;
  text-decoration: none;
  transition: all var(--transition-fast);
  cursor: pointer;
  position: relative;
}

.sidebar__menu-link:hover {
  background-color: #1aa4c8;
  border-radius: 6px;
 /* color: #ffffff; */
}


.sidebar__menu-item--active .sidebar__menu-link {
  background-color: #1aa4c8;
  color: #ffffff;
  border-radius: 6px;
}

/* Active state khi sidebar collapsed */
.sidebar--collapsed .sidebar__menu-item--active .sidebar__menu-link {
  background-color: #1aa4c8;
  color: #ffffff;
  border-radius: 6px;
  overflow: hidden;
}

.sidebar__menu-icon {
  flex-shrink: 0;
  color: inherit;
  /* Size được định nghĩa trong icons.css */
}

/* Căn giữa icon khi collapsed */
.sidebar--collapsed .sidebar__menu-icon {
  margin: 0;
  display: flex;
  align-items: center;
  justify-content: center;
}

.sidebar__menu-text {
  flex: 1;
  font-size: var(--font-size-base);
  font-weight: var(--font-weight-normal);
  white-space: nowrap;
}

.sidebar__menu-chevron {
  margin-left: auto;
  transition: transform var(--transition-fast);
  color: inherit;
}

.sidebar__menu-chevron--open {
  transform: rotate(180deg);
}

/* Collapsed state styles */
.sidebar--collapsed .sidebar__menu-link {
  justify-content: center;
  align-items: center;
  width: 44px;
  height: 40px;
  padding: 0;
  margin: 0 auto;
  border-radius: 4px;
  transition: all var(--transition-fast);
  display: flex;
}

.sidebar--collapsed .sidebar__header {
  justify-content: center;
}

/* Tooltip for collapsed sidebar */
.sidebar__menu-tooltip {
  position: absolute;
  left: 100%;
  top: 50%;
  transform: translateY(-50%);
  margin-left: 8px;
  padding: 6px 12px;
  background-color: #2c3e50;
  color: #ffffff;
  font-size: 13px;
  white-space: nowrap;
  border-radius: 4px;
  opacity: 0;
  visibility: hidden;
  transition: all 0.2s ease;
  pointer-events: none;
  z-index: 1000;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}

.sidebar__menu-tooltip::before {
  content: '';
  position: absolute;
  right: 100%;
  top: 50%;
  transform: translateY(-50%);
  border: 6px solid transparent;
  border-right-color: #2c3e50;
}

.sidebar--collapsed .sidebar__menu-link:hover .sidebar__menu-tooltip {
  opacity: 1;
  visibility: visible;
}

/* Footer */
.sidebar__footer {
  padding: var(--spacing-md);
  border-top: none;
}

/* Khi sidebar mở rộng: thêm border-top cho footer */
.sidebar:not(.sidebar--collapsed) .sidebar__footer {
  border-top: 1px solid #E0E0E0;
  display: flex;
  justify-content: flex-end;
  align-items: center;
}

/* Toggle Button */
.sidebar__toggle {
  width: 32px;
  height: 32px;
  border-radius: 4px;
  background-color: transparent;
  border: 1px solid rgba(255, 255, 255, 0.15);
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  transition: all var(--transition-fast);
  color: rgba(255, 255, 255, 0.6);
}

/* Khi sidebar mở rộng: đặt nút về bên phải */
.sidebar:not(.sidebar--collapsed) .sidebar__toggle {
  /* Nút đã được căn về bên phải bởi flex-end của footer */
}

/* Khi sidebar thu gọn: căn giữa nút */
.sidebar--collapsed .sidebar__footer {
  display: flex;
  justify-content: center;
  align-items: center;
}

.sidebar__toggle:hover {
  background-color: rgba(255, 255, 255, 0.08);
  border-color: rgba(255, 255, 255, 0.3);
  color: rgba(255, 255, 255, 0.9);
}

.sidebar__toggle:active {
  transform: scale(0.95);
  background-color: rgba(255, 255, 255, 0.12);
}

/* Ẩn toggle button trên responsive nhỏ */
@media (max-width: 1279px) {
  .sidebar__toggle {
    display: none;
  }
}

/* Transitions */
.fade-enter-active,
.fade-leave-active {
  transition: opacity var(--transition-fast);
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

/* Scrollbar cho sidebar nav */
.sidebar__nav::-webkit-scrollbar {
  width: 4px;
}

.sidebar__nav::-webkit-scrollbar-track {
  background: transparent;
}

.sidebar__nav::-webkit-scrollbar-thumb {
  background: rgba(255, 255, 255, 0.2);
  border-radius: var(--radius-base);
}

.sidebar__nav::-webkit-scrollbar-thumb:hover {
  background: rgba(255, 255, 255, 0.3);
}

/* Responsive: Tự động thu gọn khi màn hình nhỏ */
@media (max-width: 1279px) {
  .sidebar {
    width: var(--sidebar-collapsed-width);
  }
  
  /* Ẩn text và chevron khi responsive */
  .sidebar__title,
  .sidebar__menu-text,
  .sidebar__menu-chevron {
    display: none;
  }
  
  /* Spacing đều hai bên cho menu item khi responsive collapsed */
  .sidebar__menu-item {
    padding: 0 var(--spacing-sm);
  }
  
  /* Điều chỉnh padding khi collapsed */
  .sidebar__menu-link {
    justify-content: center;
    align-items: center;
    width: 44px;
    height: 40px;
    padding: 0;
    margin: 0 auto;
    border-radius: 4px;
    display: flex;
  }
  
  .sidebar__header {
    justify-content: center;
  }
  
  /* Hover effect cho responsive collapsed */
  .sidebar__menu-link:hover {
    background-color: #1aa4c8;
    border-radius: 6px;
    overflow: hidden;
  }
  
  /* Active state cho responsive collapsed */
  .sidebar__menu-item--active .sidebar__menu-link {
    background-color: #1aa4c8;
    color: #ffffff;
    border-radius: 6px;
    overflow: hidden;
  }
}

/* Responsive: Ẩn sidebar trên mobile */
@media (max-width: 768px) {
  .sidebar {
    position: fixed;
    left: -100%;
    z-index: 1000;
    transition: left var(--transition-base);
  }
  
  .sidebar.sidebar--show-mobile {
    left: 0;
  }
}
</style>
