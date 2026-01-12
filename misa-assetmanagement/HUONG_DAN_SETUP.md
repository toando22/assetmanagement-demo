# 📖 HƯỚNG DẪN SETUP DỰ ÁN

## ✅ Đã hoàn thành

Layout cơ bản đã được xây dựng xong bao gồm:

- ✅ Cấu trúc thư mục assets (fonts, icons, css)
- ✅ CSS Variables với Design System
- ✅ MainLayout với Sidebar + Header
- ✅ Sidebar có chức năng thu gọn/mở rộng
- ✅ Header với filters và user menu
- ✅ Routing cơ bản
- ✅ Template SVG sprite cho icons

## 🚀 Các bước tiếp theo

### Bước 1: Copy Font Files

1. Mở folder **`dist`** (folder chứa file font của bạn)
2. Copy **TẤT CẢ** các file font (`.ttf`, `.woff`, `.woff2`, `.eot`)
3. Paste vào folder: `src/assets/fonts/`

Ví dụ:

```
src/assets/fonts/
├── 05a65153efe56028d343c10b53faa583.ttf
├── 29c95c2d68c76de3ee30f7b2c0220da9.ttf
├── 5bf5f8dab6f35fab3ae560d8cc2923b8.woff
├── 54fbc883d110b217f19a4cfe3898979d.woff
└── ... (các file khác)
```

### Bước 2: Copy Icon Files

1. Mở folder **`icon`** (folder chứa icon của bạn)
2. Copy **TẤT CẢ** các file icon (`.svg`, `.png`, `.ico`)
3. Paste vào folder: `src/assets/icons/`

Ví dụ:

```
src/assets/icons/
├── sprite.svg (đã có sẵn)
├── qlts-icon.svg (bạn copy vào)
├── qlts-icon.png (bạn copy vào)
└── QLTS-icon-16x16.ico (bạn copy vào)
```

### Bước 3: Thêm Icon vào SVG Sprite

File: `src/assets/icons/sprite.svg`

#### Cách thêm icon:

1. Mở file SVG icon của bạn (ví dụ: `dashboard.svg`)
2. Tìm phần `<path>` hoặc `<g>` bên trong
3. Copy code đó
4. Mở file `sprite.svg`
5. Thêm vào theo format:

```xml
<symbol id="icon-dashboard" viewBox="0 0 24 24">
  <!-- Paste SVG path vào đây -->
  <path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z"/>
</symbol>
```

#### Ví dụ đầy đủ:

Giả sử bạn có file `dashboard.svg` như sau:

```xml
<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24">
  <path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z"/>
</svg>
```

Bạn copy phần `<path>` và thêm vào `sprite.svg`:

```xml
<symbol id="icon-dashboard" viewBox="0 0 24 24">
  <path d="M3 13h8V3H3v10zm0 8h8v-6H3v6zm10 0h8V11h-8v10zm0-18v6h8V3h-8z"/>
</symbol>
```

#### Danh sách icon cần thêm:

Dựa vào ảnh giao diện, bạn cần thêm các icon sau:

**Sidebar:**

- `icon-dashboard` - Tổng quan
- `icon-asset` - Tài sản
- `icon-asset-htdb` - Tài sản HT-ĐB
- `icon-tools` - Công cụ dụng cụ
- `icon-category` - Danh mục
- `icon-search` - Tra cứu
- `icon-report` - Báo cáo

**Header:**

- `icon-notification` - Thông báo
- `icon-grid` - Lưới
- `icon-help` - Trợ giúp
- `icon-user` - Người dùng
- `icon-chevron-down` - Mũi tên xuống

**Actions:**

- `icon-add` - Thêm
- `icon-edit` - Sửa
- `icon-delete` - Xóa
- `icon-copy` - Nhân bản
- `icon-filter` - Lọc
- `icon-close` - Đóng
- `icon-calendar` - Lịch

**Navigation:**

- `icon-chevron-left` - Mũi tên trái
- `icon-chevron-right` - Mũi tên phải
- `icon-menu-toggle` - Thu gọn menu

### Bước 4: Cập nhật Font Declaration (Optional)

Nếu bạn muốn sử dụng font custom:

1. Mở file: `src/assets/css/fonts.css`
2. Uncomment các dòng `@font-face`
3. Thay `[TÊN_FILE]` bằng tên file font thực tế

Ví dụ:

```css
@font-face {
  font-family: 'Roboto';
  font-style: normal;
  font-weight: 400;
  src:
    url('@/assets/fonts/5bf5f8dab6f35fab3ae560d8cc2923b8.woff2') format('woff2'),
    url('@/assets/fonts/5bf5f8dab6f35fab3ae560d8cc2923b8.woff') format('woff');
  font-display: swap;
}
```

### Bước 5: Chạy ứng dụng

```bash
npm run dev
```

Mở trình duyệt và truy cập: `http://localhost:5173`

## 🎨 Kiểm tra Layout

Sau khi chạy ứng dụng, bạn có thể:

1. ✅ **Sidebar**: Click nút thu gọn/mở rộng ở dưới cùng sidebar
2. ✅ **Header**: Click vào icon user ở góc phải trên để xem dropdown menu
3. ✅ **Responsive**: Resize trình duyệt để xem sidebar tự động thu gọn khi < 1366px
4. ✅ **Colors**: Xem demo các màu sắc từ Design System trên trang

## 📁 Cấu trúc dự án

```
src/
├── assets/
│   ├── css/
│   │   ├── variables.css      ✅ Đã tạo
│   │   ├── fonts.css          ✅ Đã tạo
│   │   ├── base.css           ✅ Đã tạo
│   │   └── main.css           ✅ Đã tạo
│   ├── fonts/                 ⏳ Bạn cần copy font vào
│   └── icons/
│       └── sprite.svg         ✅ Đã tạo (cần thêm icon)
├── layouts/
│   ├── MainLayout.vue         ✅ Đã tạo
│   ├── TheHeader.vue          ✅ Đã tạo
│   └── TheSidebar.vue         ✅ Đã tạo
├── views/
│   └── AssetList.vue          ✅ Đã tạo (placeholder)
├── router/
│   └── index.js               ✅ Đã cấu hình
├── App.vue                    ✅ Đã update
└── main.js                    ✅ Đã update
```

## 🎯 Tiếp theo

Sau khi hoàn thành các bước trên, chúng ta sẽ:

1. Xây dựng Base Components (Button, Input, Table, Dialog...)
2. Xây dựng Features Components (AssetTable, AssetForm, AssetFilter...)
3. Tích hợp API và xử lý dữ liệu
4. Implement các chức năng CRUD
5. Thêm validation và error handling

## 📞 Hỗ trợ

Nếu có vấn đề, hãy:

- Kiểm tra console để xem có lỗi không
- Kiểm tra đường dẫn file font và icon có đúng không
- Đảm bảo tất cả file đã được copy đúng vị trí

---

**CreatedBy:** DDToan - (09/1/2026)
