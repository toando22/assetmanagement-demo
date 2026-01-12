/*
  Mô tả: Cấu hình Vue Router cho ứng dụng
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '../layouts/MainLayout.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: MainLayout,
      children: [
        {
          path: '',
          name: 'asset-list',
          component: () => import('../views/AssetList.vue'),
          meta: {
            title: 'Danh sách tài sản'
          }
        },
        {
          path: 'components',
          name: 'component-demo',
          component: () => import('../views/ComponentDemo.vue'),
          meta: {
            title: 'Component Library'
          }
        },
        // TODO: Thêm các routes khác ở đây
      ]
    }
  ],
})

export default router
