# 📚 HƯỚNG DẪN CHI TIẾT CẤU TRÚC DỰ ÁN VUE 3

**Dành cho:** Người mới học Vue 3  
**Mục đích:** Hiểu rõ từng folder, file và cách Vue 3 hoạt động

---

## 🌳 CÂY THỤ MỤC DỰ ÁN (HIỆN ĐẠI)

```
misa-assetmanagement/                    ← Thư mục gốc dự án
│
├── 📄 package.json                      ← File cấu hình Node.js (dependencies, scripts)
├── 📄 vite.config.js                    ← File cấu hình Vite (build tool)
├── 📄 index.html                        ← File HTML chính (entry point)
├── 📄 jsconfig.json                     ← Cấu hình JavaScript (path aliases)
│
├── 📁 public/                           ← Thư mục tĩnh (không qua build process)
│   ├── favicon.ico                      ← Icon hiển thị trên tab trình duyệt
│   ├── icons-sprite.svg                 ← File chứa tất cả icons SVG
│   └── qlts-icon.svg                    ← Icon của ứng dụng
│
├── 📁 e2e/                              ← End-to-end testing (Playwright)
│   └── vue.spec.js                      ← Test tự động
│
├── 📁 src/                              ← ⭐ THƯ MỤC CHÍNH - Tất cả code ở đây
│   │
│   ├── 📄 main.js                       ← 🔥 ENTRY POINT - File đầu tiên được chạy
│   ├── 📄 App.vue                       ← 🎯 ROOT COMPONENT - Component gốc
│   │
│   ├── 📁 api/                          ← 📡 API LAYER - Giao tiếp với backend
│   │   └── axiosClient.js               ← Cấu hình axios (HTTP client)
│   │
│   ├── 📁 assets/                       ← 🎨 TÀI NGUYÊN TĨNH
│   │   ├── 📁 css/                      ← Stylesheets
│   │   │   ├── variables.css            ← Biến CSS (màu sắc, spacing, ...)
│   │   │   ├── fonts.css                ← Khai báo font chữ
│   │   │   ├── base.css                 ← Reset CSS, styles cơ bản
│   │   │   ├── icons.css                ← Styles cho icons
│   │   │   ├── commons.css              ← Utility classes (margin, padding, ...)
│   │   │   ├── main.css                 ← Import tất cả CSS (entry point)
│   │   │   └── style.css                ← File CSS thay thế (có đầy đủ hơn)
│   │   ├── 📁 fonts/                    ← File font chữ (.ttf, .woff, .eot)
│   │   ├── 📁 icons/                    ← Icon files (.svg, .png)
│   │   └── 📁 images/                   ← Hình ảnh (.png, .jpg, .ico)
│   │
│   ├── 📁 components/                   ← 🧩 COMPONENTS - Các thành phần tái sử dụng
│   │   ├── 📁 base/                     ← Base Components (UI components cơ bản)
│   │   │   ├── ms-button/               ← Component Button
│   │   │   │   └── MsButton.vue
│   │   │   ├── ms-input/                ← Component Input
│   │   │   │   └── MsInput.vue
│   │   │   ├── ms-dropdown/             ← Component Dropdown
│   │   │   │   └── MsDropdown.vue
│   │   │   ├── ms-dialog/               ← Component Dialog
│   │   │   │   └── MsDialog.vue
│   │   │   └── ... (10 base components)
│   │   └── index.js                     ← Export tất cả components (barrel export)
│   │
│   ├── 📁 composables/                  ← 🔄 COMPOSABLES - Logic tái sử dụng (Vue 3)
│   │   ├── useToggle.js                 ← Hook để toggle boolean state
│   │   └── usePagination.js             ← Hook để xử lý phân trang
│   │
│   ├── 📁 constants/                    ← 📌 CONSTANTS - Dữ liệu cố định
│   │   └── assetData.js                 ← Danh sách phòng ban, loại tài sản
│   │
│   ├── 📁 enums/                        ← 🔢 ENUMS - Các giá trị enum
│   │   └── index.js                     ← FormMode, RequestStatus, ...
│   │
│   ├── 📁 layouts/                      ← 🏗️ LAYOUTS - Bố cục trang
│   │   ├── MainLayout.vue               ← Layout chính (Sidebar + Header + Content)
│   │   ├── TheHeader.vue                ← Header component
│   │   └── TheSideBar.vue               ← Sidebar component
│   │
│   ├── 📁 router/                       ← 🛣️ ROUTING - Định tuyến (Vue Router)
│   │   └── index.js                     ← Cấu hình routes
│   │
│   ├── 📁 utils/                        ← 🛠️ UTILITIES - Hàm tiện ích
│   │   ├── format.js                    ← Format số, ngày, tiền tệ
│   │   ├── validate.js                  ← Validation functions
│   │   └── common.js                    ← Các hàm tiện ích chung (hiện đang trống)
│   │
│   └── 📁 views/                        ← 📄 VIEWS - Các trang (page-level components)
│       ├── AssetList.vue                ← Trang danh sách tài sản
│       └── AssetForm.vue                ← Trang form tài sản
│
└── 📁 node_modules/                     ← Thư mục chứa packages (tự động tạo)

```

---

## 🔍 PHÂN TÍCH CHI TIẾT TỪNG PHẦN

### 📦 1. THƯ MỤC GỐC (Root Level)

#### 📄 `package.json`

**Là gì?**  
File cấu hình của Node.js project, giống như "hồ sơ" của dự án.

**Chứa gì?**

- `dependencies`: Các thư viện cần thiết để chạy ứng dụng (vue, vue-router)
- `devDependencies`: Các công cụ phát triển (vite, prettier, playwright)
- `scripts`: Các lệnh để chạy dự án (`npm run dev`, `npm run build`)

**Ví dụ:**

```json
{
  "dependencies": {
    "vue": "^3.5.26", // Framework Vue 3
    "vue-router": "^4.6.4" // Thư viện routing
  },
  "scripts": {
    "dev": "vite", // Chạy development server
    "build": "vite build" // Build production
  }
}
```

**Vue 3 sử dụng như thế nào?**  
Khi bạn chạy `npm install`, Node.js đọc file này và tải về tất cả dependencies vào `node_modules/`.

---

#### 📄 `vite.config.js`

**Là gì?**  
File cấu hình Vite - công cụ build và dev server cho Vue 3.

**Chứa gì?**

- Plugin Vue
- Path aliases (`@` → `src/`)
- Cấu hình build

**Ví dụ:**

```javascript
export default defineConfig({
  plugins: [vue()], // Plugin để Vite hiểu file .vue
  resolve: {
    alias: {
      '@': './src', // Cho phép import như: import App from '@/App.vue'
    },
  },
})
```

**Vai trò trong Vue 3:**  
Vite biên dịch file `.vue` thành JavaScript, xử lý CSS, và tạo dev server nhanh.

---

#### 📄 `index.html`

**Là gì?**  
File HTML gốc - entry point của ứng dụng.

**Chứa gì?**

```html
<!DOCTYPE html>
<html>
  <head>
    <title>Vite App</title>
  </head>
  <body>
    <div id="app"></div>
    ← Vue sẽ mount vào đây
    <script type="module" src="/src/main.js"></script>
    ← Load file main.js
  </body>
</html>
```

**Vai trò:**  
Khi trình duyệt mở ứng dụng, nó load `index.html` → load `main.js` → Vue khởi tạo và render vào `<div id="app">`.

---

### 📁 2. THƯ MỤC `public/`

**Là gì?**  
Thư mục chứa file tĩnh - files không qua quá trình build, copy trực tiếp vào thư mục `dist/`.

**Files trong này:**

- `favicon.ico`: Icon hiển thị trên tab trình duyệt
- `icons-sprite.svg`: File SVG chứa tất cả icons (sử dụng `<use>` để reference)
- `qlts-icon.svg`: Icon của ứng dụng

**Truy cập như thế nào?**  
Files trong `public/` có thể truy cập trực tiếp: `/icons-sprite.svg` (không cần `@/` hay đường dẫn tương đối).

**Ví dụ sử dụng:**

```vue
<svg class="icon">
  <use xlink:href="/icons-sprite.svg#icon-dashboard"></use>
</svg>
```

---

### 🔥 3. THƯ MỤC `src/` - THƯ MỤC QUAN TRỌNG NHẤT

Đây là nơi chứa **TẤT CẢ** code của ứng dụng.

---

#### 📄 `src/main.js` - ENTRY POINT

**Là gì?**  
File JavaScript đầu tiên được chạy khi ứng dụng khởi động.

**Code:**

```javascript
import { createApp } from 'vue' // Import function từ Vue 3
import App from './App.vue' // Import root component
import router from './router' // Import router

const app = createApp(App) // Tạo Vue app instance
app.use(router) // Đăng ký router
app.mount('#app') // Mount vào <div id="app">
```

**Giải thích từng dòng:**

1. **`createApp(App)`**:
   - Vue 3 tạo một "ứng dụng Vue" mới
   - `App` là component gốc (root component)

2. **`app.use(router)`**:
   - Đăng ký Vue Router để ứng dụng có thể điều hướng giữa các trang

3. **`app.mount('#app')`**:
   - Gắn ứng dụng vào element có id="app" trong `index.html`
   - Từ đây, Vue bắt đầu render và quản lý DOM

**Luồng hoạt động:**

```
index.html → main.js → App.vue → Router → Views
```

---

#### 📄 `src/App.vue` - ROOT COMPONENT

**Là gì?**  
Component gốc của ứng dụng - component đầu tiên được render.

**Cấu trúc Vue Component (Single File Component - SFC):**

```vue
<template>
  <!-- HTML Template -->
  <RouterView /> ← Vue Router sẽ render views ở đây
</template>

<script setup>
// JavaScript Logic
import { RouterView } from 'vue-router'
</script>

<style>
/* CSS Styles */
@import '@/assets/css/main.css';
</style>
```

**3 phần của Vue Component:**

1. **`<template>`**: HTML - Cấu trúc UI
2. **`<script setup>`**: JavaScript - Logic, state, functions (Vue 3 Composition API)
3. **`<style>`**: CSS - Styling

**`RouterView` là gì?**  
Là component của Vue Router - nơi hiển thị component tương ứng với route hiện tại.

**Ví dụ:**

- Khi URL là `/` → Render `AssetList.vue`
- Khi URL là `/assets/new` → Render `AssetForm.vue`

---

### 📁 4. `src/api/` - API LAYER

**Là gì?**  
Thư mục chứa code giao tiếp với backend (server).

**File `axiosClient.js`:**  
Cấu hình axios (thư viện HTTP client) để gửi request đến API.

**Ví dụ cấu trúc:**

```javascript
import axios from 'axios'

const apiClient = axios.create({
  baseURL: 'https://api.example.com',
  timeout: 10000,
})

export default apiClient
```

**Sử dụng:**

```javascript
import apiClient from '@/api/axiosClient'

// GET request
const response = await apiClient.get('/assets')

// POST request
await apiClient.post('/assets', assetData)
```

**Tại sao tách riêng?**

- Tái sử dụng: Chỉ cấu hình một lần, dùng nhiều nơi
- Dễ maintain: Thay đổi baseURL ở một chỗ
- Tập trung xử lý lỗi, interceptors

---

### 📁 5. `src/assets/` - TÀI NGUYÊN TĨNH

**Là gì?**  
Thư mục chứa file tĩnh: CSS, fonts, icons, images.

**Khác với `public/` như thế nào?**

- **`assets/`**: Files qua quá trình build (optimize, minify)
- **`public/`**: Files copy trực tiếp, không qua build

**Khi nào dùng `assets/`?**  
Khi file cần được xử lý bởi build tool (CSS được minify, images được optimize).

**Khi nào dùng `public/`?**  
Khi file cần giữ nguyên đường dẫn (favicon, robots.txt).

---

#### 📁 `src/assets/css/` - Stylesheets

**Cấu trúc CSS:**

```
css/
├── variables.css    ← Biến CSS (--primary-color: #019160)
├── fonts.css        ← @font-face declarations
├── base.css         ← Reset CSS, base styles
├── icons.css        ← Icon styles
├── commons.css      ← Utility classes (.mt-16, .text-center)
├── main.css         ← Import tất cả (entry point)
└── style.css        ← File thay thế (đầy đủ hơn)
```

**Thứ tự import quan trọng:**

```css
/* main.css hoặc style.css */
@import './variables.css'; /* 1. Biến trước */
@import './fonts.css'; /* 2. Fonts */
@import './base.css'; /* 3. Base styles */
@import './icons.css'; /* 4. Icons */
@import './commons.css'; /* 5. Utilities cuối cùng */
```

**Tại sao cần thứ tự này?**  
CSS variables phải được định nghĩa trước khi sử dụng.

**Ví dụ:**

```css
/* variables.css */
:root {
  --primary-color: #019160;
}

/* base.css */
.button {
  background-color: var(--primary-color); /* Sử dụng biến */
}
```

---

### 📁 6. `src/components/` - COMPONENTS

**Là gì?**  
Thư mục chứa các component Vue có thể tái sử dụng.

**2 loại components:**

1. **Base Components** (`components/base/`):
   - UI components cơ bản, tái sử dụng nhiều nơi
   - Ví dụ: Button, Input, Dropdown, Dialog

2. **Feature Components** (`components/feature/` - chưa có):
   - Components cụ thể cho một feature
   - Ví dụ: AssetTable, AssetFilter

---

#### Cấu trúc Base Component:

```
components/base/
└── ms-button/              ← Folder của component
    └── MsButton.vue        ← File component
```

**Tại sao mỗi component một folder?**

- Dễ mở rộng: Có thể thêm `MsButton.test.js`, `MsButton.stories.js`
- Tổ chức tốt: Mỗi component có folder riêng

**File `components/index.js` (Barrel Export):**

```javascript
// Export tất cả components từ một file
export { default as MsButton } from './base/ms-button/MsButton.vue'
export { default as MsInput } from './base/ms-input/MsInput.vue'
```

**Lợi ích:**

```javascript
// Thay vì:
import MsButton from '@/components/base/ms-button/MsButton.vue'
import MsInput from '@/components/base/ms-input/MsInput.vue'

// Có thể viết:
import { MsButton, MsInput } from '@/components'
```

---

#### Ví dụ Component: `MsButton.vue`

**Cấu trúc:**

```vue
<template>
  <button class="ms-button" :class="variantClass">
    <slot></slot>
    <!-- Nội dung button -->
  </button>
</template>

<script setup>
// Props - Dữ liệu nhận từ component cha
defineProps({
  variant: {
    type: String,
    default: 'primary', // primary, secondary, outline
  },
})

// Emits - Sự kiện gửi lên component cha
defineEmits(['click'])
</script>

<style scoped>
.ms-button {
  /* Styles chỉ áp dụng cho component này */
}
</style>
```

**Sử dụng:**

```vue
<template>
  <MsButton variant="primary" @click="handleClick"> Click me </MsButton>
</template>

<script setup>
import { MsButton } from '@/components'
</script>
```

**Giải thích:**

- **`<slot>`**: Vùng chứa nội dung từ component cha (ở đây là "Click me")
- **`defineProps`**: Định nghĩa props (dữ liệu từ component cha)
- **`defineEmits`**: Định nghĩa events (sự kiện gửi lên component cha)
- **`scoped`**: CSS chỉ áp dụng cho component này

---

### 📁 7. `src/composables/` - COMPOSABLES (Vue 3 Feature)

**Là gì?**  
Thư mục chứa các composable functions - logic tái sử dụng (giống React hooks).

**Composable là gì?**  
Function JavaScript trả về reactive state và methods - có thể dùng lại nhiều component.

**Quy ước naming:**

- Bắt đầu bằng `use` (ví dụ: `useToggle`, `usePagination`)

---

#### Ví dụ: `useToggle.js`

**Code:**

```javascript
import { ref } from 'vue'

export function useToggle(initialValue = false) {
  const value = ref(initialValue) // Reactive state

  const toggle = () => {
    value.value = !value.value // Đổi giá trị
  }

  return {
    value, // State
    toggle, // Method
  }
}
```

**Sử dụng trong component:**

```vue
<script setup>
import { useToggle } from '@/composables/useToggle'

// Sử dụng composable
const { value: isOpen, toggle } = useToggle(false)
// value được đổi tên thành isOpen để dễ hiểu

// isOpen.value = false (ban đầu)
// Khi gọi toggle() → isOpen.value = true
</script>

<template>
  <button @click="toggle">
    {{ isOpen ? 'Đóng' : 'Mở' }}
  </button>
</template>
```

**Lợi ích:**

- **Tái sử dụng**: Logic toggle có thể dùng ở nhiều component
- **Tách biệt logic**: Logic tách khỏi template
- **Dễ test**: Test logic độc lập

**Vue 3 Composition API:**

- `ref()`: Tạo reactive reference (cho primitive values)
- `reactive()`: Tạo reactive object
- `computed()`: Tạo computed property
- `watch()`: Theo dõi thay đổi

---

### 📁 8. `src/constants/` - CONSTANTS

**Là gì?**  
Thư mục chứa dữ liệu cố định, không thay đổi.

**Ví dụ: `assetData.js`**

```javascript
export const DEPARTMENTS = [
  { code: '01', name: 'Ban Giám hiệu' },
  { code: '02', name: 'Phòng Hành chính' },
  // ...
]

export const ASSET_TYPES = [
  { code: '1', name: 'Nhà, công trình xây dựng' },
  // ...
]
```

**Sử dụng:**

```vue
<script setup>
import { DEPARTMENTS, ASSET_TYPES } from '@/constants/assetData'
</script>

<template>
  <select>
    <option v-for="dept in DEPARTMENTS" :key="dept.code">
      {{ dept.name }}
    </option>
  </select>
</template>
```

**Tại sao tách riêng?**

- Dễ maintain: Thay đổi ở một chỗ
- Tái sử dụng: Dùng nhiều nơi
- Tách biệt dữ liệu và logic

---

### 📁 9. `src/enums/` - ENUMS

**Là gì?**  
Thư mục chứa các enum - tập hợp giá trị cố định.

**Ví dụ: `enums/index.js`**

```javascript
export const FormMode = {
  ADD: 'add',
  EDIT: 'edit',
  VIEW: 'view',
  DUPLICATE: 'duplicate',
}

export const RequestStatus = {
  IDLE: 'idle',
  LOADING: 'loading',
  SUCCESS: 'success',
  ERROR: 'error',
}
```

**Sử dụng:**

```javascript
import { FormMode, RequestStatus } from '@/enums'

// Thay vì:
if (mode === 'add') { ... }

// Dùng:
if (mode === FormMode.ADD) { ... }
```

**Lợi ích:**

- **Type safety**: Tránh typo ('add' vs 'Add')
- **Autocomplete**: IDE gợi ý các giá trị
- **Refactoring**: Dễ đổi tên

---

### 📁 10. `src/layouts/` - LAYOUTS

**Là gì?**  
Thư mục chứa layout components - bố cục trang.

**Cấu trúc:**

```
layouts/
├── MainLayout.vue    ← Layout chính (Sidebar + Header + Content)
├── TheHeader.vue     ← Header component
└── TheSideBar.vue    ← Sidebar component
```

**Ví dụ: `MainLayout.vue`**

```vue
<template>
  <div class="main-layout">
    <TheSidebar />
    <!-- Sidebar bên trái -->

    <div class="content">
      <TheHeader />
      <!-- Header trên cùng -->
      <main>
        <RouterView />
        <!-- Nội dung trang (views) -->
      </main>
    </div>
  </div>
</template>
```

**Luồng:**

```
App.vue
  └── RouterView
      └── MainLayout (từ router)
          ├── TheSidebar
          ├── TheHeader
          └── RouterView (nội dung views)
              └── AssetList.vue / AssetForm.vue
```

**Tại sao prefix `The`?**  
Quy ước naming cho component chỉ có một instance trong app (TheHeader, TheSidebar).

---

### 📁 11. `src/router/` - ROUTING

**Là gì?**  
Thư mục cấu hình routing - định tuyến giữa các trang.

**File: `router/index.js`**

```javascript
import { createRouter, createWebHistory } from 'vue-router'
import MainLayout from '../layouts/MainLayout.vue'

const router = createRouter({
  history: createWebHistory(), // Sử dụng HTML5 History API
  routes: [
    {
      path: '/', // URL
      component: MainLayout, // Component layout
      children: [
        {
          path: '', // URL con
          name: 'asset-list', // Tên route
          component: () => import('../views/AssetList.vue'), // Lazy load
          meta: {
            title: 'Danh sách tài sản',
          },
        },
      ],
    },
  ],
})

export default router
```

**Giải thích:**

- **`path`**: URL (ví dụ: `/`, `/assets`)
- **`name`**: Tên route (dùng để navigate: `router.push({ name: 'asset-list' })`)
- **`component`**: Component sẽ render
- **`meta`**: Metadata (title, permissions, ...)

**Lazy Loading:**

```javascript
// Thay vì:
component: AssetList // Load ngay

// Dùng:
component: () => import('../views/AssetList.vue') // Chỉ load khi cần
```

**Lợi ích lazy loading:**

- Giảm bundle size ban đầu
- Tải nhanh hơn
- Chỉ load code khi cần

---

### 📁 12. `src/utils/` - UTILITIES

**Là gì?**  
Thư mục chứa các hàm tiện ích - functions có thể dùng lại.

**Files:**

- `format.js`: Format số, ngày, tiền tệ
- `validate.js`: Validation functions
- `common.js`: Các hàm chung (hiện trống)

**Ví dụ: `format.js`**

```javascript
export const formatNumber = (value) => {
  // Format 1000000 → "1.000.000"
  return value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, '.')
}

export const formatCurrency = (value) => {
  // Format tiền tệ
  return formatNumber(value) + ' đ'
}
```

**Sử dụng:**

```vue
<script setup>
import { formatNumber, formatCurrency } from '@/utils/format'

const price = 1000000
const formatted = formatCurrency(price) // "1.000.000 đ"
</script>
```

**Tại sao tách riêng?**

- Tái sử dụng: Dùng nhiều nơi
- Dễ test: Test functions độc lập
- Tổ chức tốt: Logic tách khỏi components

---

### 📁 13. `src/views/` - VIEWS

**Là gì?**  
Thư mục chứa page-level components - các trang của ứng dụng.

**Views vs Components:**

- **Views**: Trang đầy đủ (AssetList, AssetForm)
- **Components**: Phần nhỏ, tái sử dụng (Button, Input)

**Ví dụ: `AssetList.vue`**

```vue
<template>
  <div class="asset-list">
    <h1>Danh sách tài sản</h1>
    <MsButton>Thêm mới</MsButton>
    <!-- ... -->
  </div>
</template>

<script setup>
import { MsButton } from '@/components'
// ...
</script>
```

**Luồng:**

```
URL: / → Router → AssetList.vue → Sử dụng components, composables, utils
```

---

## 🔄 LUỒNG HOẠT ĐỘNG TỔNG QUAN

```
1. index.html
   ↓
2. main.js (entry point)
   ├── Import App.vue
   ├── Import router
   └── mount('#app')
   ↓
3. App.vue (root component)
   └── <RouterView /> (router sẽ render views ở đây)
   ↓
4. Router (router/index.js)
   ├── Route: / → AssetList.vue
   └── Route: /assets/new → AssetForm.vue
   ↓
5. Views (AssetList.vue, AssetForm.vue)
   ├── Sử dụng Layouts (MainLayout)
   ├── Sử dụng Components (MsButton, MsInput)
   ├── Sử dụng Composables (useToggle, usePagination)
   ├── Sử dụng Utils (format, validate)
   ├── Gọi API (api/axiosClient)
   └── Sử dụng Constants/Enums
```

---

## 🎯 TÓM TẮT - KHI NÀO DÙNG FOLDER NÀO?

| Folder/File            | Khi nào dùng?                   | Ví dụ                               |
| ---------------------- | ------------------------------- | ----------------------------------- |
| `src/main.js`          | File khởi tạo app               | Không cần sửa (trừ khi thêm plugin) |
| `src/App.vue`          | Component gốc                   | Thường không cần sửa                |
| `src/views/`           | Tạo trang mới                   | AssetList.vue, AssetForm.vue        |
| `src/components/base/` | Tạo UI component tái sử dụng    | MsButton, MsInput                   |
| `src/composables/`     | Tạo logic tái sử dụng           | useToggle, usePagination            |
| `src/utils/`           | Tạo hàm tiện ích                | formatNumber, validateEmail         |
| `src/api/`             | Cấu hình API, tạo API functions | axiosClient.js                      |
| `src/constants/`       | Dữ liệu cố định                 | DEPARTMENTS, ASSET_TYPES            |
| `src/enums/`           | Enum values                     | FormMode, RequestStatus             |
| `src/router/`          | Thêm route mới                  | routes trong router/index.js        |
| `src/layouts/`         | Tạo layout mới                  | MainLayout, AuthLayout              |
| `src/assets/css/`      | Thêm styles                     | variables.css, base.css             |

---

## 💡 BEST PRACTICES

1. **Naming Conventions:**
   - Components: PascalCase (MsButton.vue)
   - Composables: camelCase với prefix `use` (useToggle.js)
   - Utils: camelCase (formatNumber)
   - Constants: UPPER_SNAKE_CASE (DEPARTMENTS)

2. **Folder Organization:**
   - Mỗi component một folder (nếu có nhiều files liên quan)
   - Tách base components và feature components
   - Barrel exports (index.js) để import dễ dàng

3. **File Structure:**
   - Single File Component (SFC): `.vue` file có 3 phần (template, script, style)
   - Separated concerns: Logic tách khỏi template

4. **Import Paths:**
   - Dùng alias `@` thay vì relative paths
   - `import { MsButton } from '@/components'` thay vì `import MsButton from '../../components/...'`

---

## ❓ CÂU HỎI THƯỜNG GẶP

**Q: Tại sao có cả `main.css` và `style.css`?**  
A: Hiện đang có vấn đề - `main.css` đang được dùng nhưng thiếu `commons.css`. Nên dùng `style.css` hoặc cập nhật `main.css`.

**Q: `components/base/` và `components/feature/` khác nhau gì?**  
A: Base = UI components tái sử dụng (Button, Input). Feature = Components cụ thể cho feature (AssetTable, AssetFilter).

**Q: Composables và Utils khác nhau gì?**  
A: Composables dùng Vue reactivity (ref, reactive), Utils là pure functions không phụ thuộc Vue.

**Q: Views và Components khác nhau gì?**  
A: Views = trang đầy đủ (route-level), Components = phần nhỏ tái sử dụng.

---

## 📚 TÀI LIỆU THAM KHẢO

- [Vue 3 Documentation](https://vuejs.org/)
- [Vue Router](https://router.vuejs.org/)
- [Composition API](https://vuejs.org/guide/extras/composition-api-faq.html)
- [Vite](https://vitejs.dev/)

---

**Tài liệu này giúp bạn hiểu rõ từng phần trong dự án Vue 3. Hãy tham khảo khi cần!** 🚀
