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
        <span class="header__year-label">Năm</span>
        <select class="header__year-select" v-model="selectedYear">
          <option v-for="year in years" :key="year" :value="year">{{ year }}</option>
        </select>
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
  display: flex;
  align-items: center;
  height: 30px;
  min-width: 112px;
  background-color: rgba(26, 164, 200, 0.2);
  border: 1px solid rgba(26, 164, 200, 0.4);
  border-radius: 3px;
  padding: 0 8px 0 12px;
  gap: 8px;
}

/* Label "Năm" bên trong */
.header__year-label {
  font-size: 13px;
  color: #001031;
  font-weight: 500;
  white-space: nowrap;
  pointer-events: none;
}

/* Select năm */
.header__year-select {
  flex: 1;
  height: 100%;
  border: none;
  background: transparent;
  color: #001031;
  font-size: 13px;
  font-weight: 700;
  cursor: pointer;
  outline: none;
  padding: 0;
  appearance: none;
  -webkit-appearance: none;
  -moz-appearance: none;
  text-align: right;
  padding-right: 16px;
}

/* Custom dropdown arrow */
.header__year-filter::after {
  content: '';
  position: absolute;
  right: 8px;
  top: 50%;
  transform: translateY(-50%);
  width: 0;
  height: 0;
  border-left: 4px solid transparent;
  border-right: 4px solid transparent;
  border-top: 5px solid #001031;
  pointer-events: none;
}

.header__year-select option {
  background-color: white;
  color: #001031;
  font-weight: 700;
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
