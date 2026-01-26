<!--
  Mô tả: Layout chính của ứng dụng, tích hợp Sidebar + Header + Content
  Features:
  - Responsive layout
  - Sidebar có thể thu gọn
  - Header cố định
  - Content area với router-view
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
-->

<template>
  <div class="main-layout">
    <!-- Sidebar -->
    <TheSidebar />

    <!-- Main Content Area -->
    <div class="main-layout__content">
      <!-- Header -->
      <TheHeader :page-title="pageTitle" />

      <!-- Page Content -->
      <main class="main-layout__page">
        <router-view v-slot="{ Component }">
          <transition name="page-fade" mode="out-in">
            <component :is="Component" />
          </transition>
        </router-view>
      </main>
    </div>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MainLayout component
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: DDToan - (24/1/2026) - Thêm yearFilter state và provide cho children
*/

import { ref, watch, provide } from 'vue'
import { useRoute } from 'vue-router'
import TheSidebar from './TheSideBar.vue'
import TheHeader from './TheHeader.vue'
import { useYearFilter } from '@/composables/useYearFilter'

// State
const pageTitle = ref('Danh sách tài sản')

// Router
const route = useRoute()

// Year Filter: Quản lý state năm filter và provide cho children components
// CreatedBy: DDToan - (24/1/2026)
const yearFilter = useYearFilter()
provide('yearFilter', yearFilter)

/*
  Mô tả: Cập nhật page title khi route thay đổi
  
  CreatedBy: DDToan - (09/1/2026)
*/
watch(
  () => route.meta.title,
  (newTitle) => {
    if (newTitle) {
      pageTitle.value = newTitle
    }
  },
  { immediate: true }
)
</script>

<style scoped>
/*
  Mô tả: Styles cho MainLayout component
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

.main-layout {
  display: flex;
  width: 100%;
  height: 100vh;
  overflow: hidden;
  background-color: var(--color-bg-secondary);
}

.main-layout__content {
  flex: 1;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.main-layout__page {
  flex: 1;
  overflow-y: auto;
  overflow-x: hidden;
  padding: 0;
  background-color: #f0f2f5;
}

/* Page Transition */
.page-fade-enter-active,
.page-fade-leave-active {
  transition: opacity var(--transition-base);
}

.page-fade-enter-from,
.page-fade-leave-to {
  opacity: 0;
}

/* Scrollbar cho page content */
.main-layout__page::-webkit-scrollbar {
  width: 8px;
}

.main-layout__page::-webkit-scrollbar-track {
  background: var(--color-bg-secondary);
}

.main-layout__page::-webkit-scrollbar-thumb {
  background: var(--color-border-dark);
  border-radius: var(--radius-base);
}

.main-layout__page::-webkit-scrollbar-thumb:hover {
  background: var(--color-text-gray);
}

/* Responsive */
@media (max-width: 768px) {
  .main-layout__page {
    padding: var(--spacing-lg);
  }
}
</style>
