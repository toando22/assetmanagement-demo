<!--
  Mô tả: Component Header với breadcrumb, filters và user actions
  Features:
  - Breadcrumb/Title
  - Filters (Năm, Sổ tài chính)
  - Action icons (notification, grid, help, user)
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
-->

<template>
  <header class="header">
    <!-- Left: Page Title/Breadcrumb -->
    <div class="header__left">
      <h1 class="header__title">{{ pageTitle }}</h1>
    </div>

    <!-- Right: Filters and Actions -->
    <div class="header__right">
      <!-- Label: Sổ tài chính -->
      <span class="header__label">Sở tài chính</span>

      <!-- Filter: Năm với label bên trong -->
      <div class="header__year-filter">
        <MsDropdown
          v-model="selectedYear"
          :options="yearOptions"
          placeholder="Năm"
          class="header__year-dropdown"
        />
        <!-- 2 icon cố định (mũi tên lên và xuống) -->
        <i class="icon icon-chevron-up-toolbar header__year-icon-up"></i>
        <i class="icon icon-chevron-down-toolbar header__year-icon-down"></i>
      </div>

      <!-- Divider -->
      <div class="header__divider"></div>

      <!-- Action Icons -->
      <div class="header__actions">
        <!-- Notification -->
        <button 
          class="header__action-btn" 
          title="Thông báo"
          @click="handleNotificationClick"
        >
          <i class="icon icon-notification"></i>
        </button>

        <!-- Grid -->
        <button 
          class="header__action-btn" 
          title="Lưới"
          @click="handleGridClick"
        >
          <i class="icon icon-grid"></i>
        </button>

        <!-- Help -->
        <button 
          class="header__action-btn" 
          title="Trợ giúp"
          @click="handleHelpClick"
        >
          <i class="icon icon-help"></i>
        </button>

        <!-- User Profile -->
        <div class="header__user">
          <button 
            class="header__user-btn"
            @click="toggleUserMenu"
          >
            <i class="icon icon-user"></i>
            <i class="icon icon--sm icon-chevron-down"></i>
          </button>
          
          <!-- User Dropdown Menu -->
          <transition name="fade-slide">
            <div v-if="isUserMenuOpen" class="header__user-menu">
              <div class="header__user-info">
                <div class="header__user-name">DDToan</div>
                <div class="header__user-email">ddtoan@misa.vn</div>
              </div>
              <div class="header__user-menu-divider"></div>
              <ul class="header__user-menu-list">
                <li><a href="#" @click.prevent="handleProfile">Thông tin cá nhân</a></li>
                <li><a href="#" @click.prevent="handleSettings">Cài đặt</a></li>
                <li><a href="#" @click.prevent="handleLogout">Đăng xuất</a></li>
              </ul>
            </div>
          </transition>
        </div>
      </div>
    </div>
  </header>
</template>

<script setup>
/*
  Mô tả: Script setup cho TheHeader component
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

import { ref, computed } from 'vue'
import MsDropdown from '@/components/base/ms-dropdown/MsDropdown.vue'

// Props
const props = defineProps({
  pageTitle: {
    type: String,
    default: 'Danh sách tài sản'
  }
})

// State
const selectedYear = ref(2021)
const isUserMenuOpen = ref(false)

// Computed: Danh sách năm (từ năm hiện tại trở về trước 10 năm)
const years = computed(() => {
  const currentYear = new Date().getFullYear()
  const yearsList = []
  for (let i = 0; i <= 10; i++) {
    yearsList.push(currentYear - i)
  }
  return yearsList
})

// Options cho dropdown năm - chỉ hiển thị năm trong dropdown
const yearOptions = computed(() => {
  return years.value.map(year => ({
    value: year,
    label: year.toString()
  }))
})

/*
  Mô tả: Toggle user menu dropdown
  
  CreatedBy: DDToan - (09/1/2026)
*/
const toggleUserMenu = () => {
  isUserMenuOpen.value = !isUserMenuOpen.value
}

/*
  Mô tả: Xử lý click notification
  
  CreatedBy: DDToan - (09/1/2026)
*/
const handleNotificationClick = () => {
  console.log('Notification clicked')
  // TODO: Implement notification logic
}

/*
  Mô tả: Xử lý click grid
  
  CreatedBy: DDToan - (09/1/2026)
*/
const handleGridClick = () => {
  console.log('Grid clicked')
  // TODO: Implement grid logic
}

/*
  Mô tả: Xử lý click help
  
  CreatedBy: DDToan - (09/1/2026)
*/
const handleHelpClick = () => {
  console.log('Help clicked')
  // TODO: Implement help logic
}

/*
  Mô tả: Xử lý click profile
  
  CreatedBy: DDToan - (09/1/2026)
*/
const handleProfile = () => {
  console.log('Profile clicked')
  isUserMenuOpen.value = false
  // TODO: Navigate to profile page
}

/*
  Mô tả: Xử lý click settings
  
  CreatedBy: DDToan - (09/1/2026)
*/
const handleSettings = () => {
  console.log('Settings clicked')
  isUserMenuOpen.value = false
  // TODO: Navigate to settings page
}

/*
  Mô tả: Xử lý logout
  
  CreatedBy: DDToan - (09/1/2026)
*/
const handleLogout = () => {
  console.log('Logout clicked')
  isUserMenuOpen.value = false
  // TODO: Implement logout logic
}
</script>

<style scoped>
/*
  Mô tả: Styles cho Header component
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

.header {
  height: var(--header-height);
  background-color: var(--color-white);
  border-bottom: 1px solid var(--color-border);
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 var(--spacing-xl);
  position: sticky;
  top: 0;
  z-index: var(--z-sticky);
}

/* Left */
.header__left {
  display: flex;
  align-items: center;
  gap: var(--spacing-lg);
}

.header__title {
  font-size: var(--font-size-lg);
  font-weight: var(--font-weight-semibold);
  color: var(--color-text-dark);
  margin: 0;
}

/* Right */
.header__right {
  display: flex;
  align-items: center;
  gap: var(--spacing-lg);
}

/* Label: Sổ tài chính */
.header__label {
  font-size: 13px;
  color: #001031;
  white-space: nowrap;
  font-weight: 500;
}

/* Year Filter Container */
.header__year-filter {
  position: relative;
  height: 30px;
  min-width: 112px;
}

/* Dropdown năm - style giống 100% hình ảnh 2 */
.header__year-dropdown {
  width: 100%;
  height: 100%;
}

.header__year-dropdown :deep(.ms-dropdown__trigger) {
  height: 30px;
  padding: 0 32px 0 12px;
  border: 1px solid #1aa4c8;
  border-radius: 3px;
  background-color: #e6f7ff;
  font-size: 13px;
  font-weight: 700;
  color: #001031;
  display: flex;
  align-items: center;
  cursor: pointer;
  position: relative;
  box-sizing: border-box;
}

/* Text "Năm" và năm - style riêng, thêm "Năm " vào trước */
.header__year-dropdown :deep(.ms-dropdown__text) {
  display: inline;
  font-weight: 700;
  color: #001031;
}

.header__year-dropdown :deep(.ms-dropdown__text)::before {
  content: 'Năm ';
  font-weight: 700;
  color: #001031;
}

/* Ẩn icon chevron mặc định - không chuyển động */
.header__year-dropdown :deep(.ms-dropdown__arrow) {
  display: none;
}

/* 2 icon cố định (mũi tên lên và xuống) - sử dụng class icon convention */
.header__year-icon-up,
.header__year-icon-down {
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  pointer-events: none;
  z-index: 1;
}

.header__year-icon-up {
  top: calc(50% - 4px);
}

.header__year-icon-down {
  top: calc(50% + 4px);
}

/* Dropdown menu */
.header__year-dropdown :deep(.ms-dropdown__menu) {
  width: max-content;
  min-width: 100%;
  left: 0;
  right: auto;
  top: calc(100% + 4px);
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
}

/* List items */
.header__year-dropdown :deep(.ms-dropdown__list) {
  width: 100%;
  display: block;
}

/* Item - phủ toàn bộ width */
.header__year-dropdown :deep(.ms-dropdown__item) {
  width: 100% !important;
  box-sizing: border-box;
  height: 32px;
  padding: 0 12px;
  display: flex;
  align-items: center;
  font-size: 13px;
  font-weight: 700;
  color: #001031;
  white-space: nowrap;
  cursor: pointer;
  transition: background-color 0.2s ease;
}

/* Item hover - màu xanh nhạt giống dropdown trong popup */
.header__year-dropdown :deep(.ms-dropdown__item:hover) {
  background-color: #e6f7ff;
}

/* Item selected - màu xanh nhạt với checkmark giống mẫu hình 2 */
.header__year-dropdown :deep(.ms-dropdown__item--selected) {
  background-color: #e6f7ff;
  color: #001031;
}

.header__year-dropdown :deep(.ms-dropdown__item--selected:hover) {
  background-color: #e6f7ff;
  color: #001031;
}

/* Hiển thị icon check cho item được chọn */
.header__year-dropdown :deep(.ms-dropdown__check) {
  display: block;
  width: 16px;
  height: 16px;
  color: #1aa4c8;
  flex-shrink: 0;
}

/* Divider */
.header__divider {
  width: 1px;
  height: 24px;
  background-color: var(--color-border);
}

/* Actions */
.header__actions {
  display: flex;
  align-items: center;
  gap: var(--spacing-md);
}

.header__action-btn {
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  background: none;
  border: none;
  border-radius: var(--radius-base);
  color: var(--color-text-secondary);
  cursor: pointer;
  transition: all var(--transition-fast);
}

.header__action-btn:hover {
  background-color: var(--color-bg-hover);
  color: var(--color-primary);
}

/* User */
.header__user {
  position: relative;
}

.header__user-btn {
  display: flex;
  align-items: center;
  gap: var(--spacing-xs);
  height: 32px;
  padding: 0 var(--spacing-sm);
  background: none;
  border: none;
  border-radius: var(--radius-base);
  color: var(--color-text-secondary);
  cursor: pointer;
  transition: all var(--transition-fast);
}

.header__user-btn:hover {
  background-color: var(--color-bg-hover);
  color: var(--color-primary);
}

.header__user-menu {
  position: absolute;
  top: calc(100% + 8px);
  right: 0;
  min-width: 200px;
  background-color: var(--color-white);
  border: 1px solid var(--color-border);
  border-radius: var(--radius-md);
  box-shadow: var(--shadow-lg);
  padding: var(--spacing-sm) 0;
  z-index: var(--z-dropdown);
}

.header__user-info {
  padding: var(--spacing-md) var(--spacing-lg);
}

.header__user-name {
  font-size: var(--font-size-base);
  font-weight: var(--font-weight-semibold);
  color: var(--color-text-dark);
}

.header__user-email {
  font-size: var(--font-size-sm);
  color: var(--color-text-gray);
  margin-top: var(--spacing-xs);
}

.header__user-menu-divider {
  height: 1px;
  background-color: var(--color-border);
  margin: var(--spacing-sm) 0;
}

.header__user-menu-list {
  list-style: none;
  margin: 0;
  padding: 0;
}

.header__user-menu-list li {
  margin: 0;
}

.header__user-menu-list a {
  display: block;
  padding: var(--spacing-sm) var(--spacing-lg);
  font-size: var(--font-size-base);
  color: var(--color-text-primary);
  text-decoration: none;
  transition: all var(--transition-fast);
}

.header__user-menu-list a:hover {
  background-color: var(--color-bg-hover);
  color: var(--color-primary);
}

/* Icon - Đã được định nghĩa trong icons.css */

/* Transitions */
.fade-slide-enter-active,
.fade-slide-leave-active {
  transition: all var(--transition-fast);
}

.fade-slide-enter-from,
.fade-slide-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>
