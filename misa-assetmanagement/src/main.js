/*
  Mô tả: Entry point của ứng dụng Vue
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(router)

app.mount('#app')
