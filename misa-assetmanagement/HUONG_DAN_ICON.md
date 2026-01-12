# 📖 HƯỚNG DẪN TÌM VÀ ĐIỀN MASK-POSITION CHO ICONS

## 🎯 Mục đích

File `qlts-icon.svg` là một **sprite sheet** chứa nhiều icons. Chúng ta sử dụng CSS `mask-position` để "cắt" và hiển thị từng icon riêng lẻ.

---

## 🔍 Cách 1: Sử dụng Browser DevTools (Nhanh nhất)

### Bước 1: Mở file sprite trong browser

```
http://localhost:5173/src/assets/icons/qlts-icon.svg
```

### Bước 2: Inspect Element

1. Click chuột phải vào icon bạn muốn → **Inspect**
2. Trong DevTools, tìm element `<path>` hoặc `<g>` của icon
3. Xem thuộc tính `transform="translate(X, Y)"`

Ví dụ:
```xml
<g id="icon-notification" transform="translate(108, 642)">
```

→ Vị trí: **X = 108px, Y = 642px**

### Bước 3: Điền vào CSS

```css
.icon-notification {
  -webkit-mask-position: -108px -642px; /* Chú ý dấu trừ (-) */
  mask-position: -108px -642px;
}
```

**⚠️ Quan trọng:** Mask-position cần dấu **trừ (-)** vì ta đang dịch chuyển sprite ngược lại.

---

## 🔍 Cách 2: Sử dụng Design Tool (Figma, Illustrator...)

### Bước 1: Mở file trong design tool

Import `qlts-icon.svg` vào:
- Figma
- Adobe Illustrator
- Inkscape (free)

### Bước 2: Chọn icon

Click vào icon bạn muốn sử dụng

### Bước 3: Xem tọa độ

Nhìn vào panel **Properties/Transform**:
- **X**: Tọa độ ngang
- **Y**: Tọa độ dọc

### Bước 4: Điền vào CSS

```css
.icon-your-name {
  -webkit-mask-position: -Xpx -Ypx;
  mask-position: -Xpx -Ypx;
}
```

---

## 📋 DANH SÁCH ICONS CẦN ĐIỀN

### Header Icons (Ưu tiên cao)

```css
/* src/assets/css/icons.css */

/* Icon Notification - Thông báo */
.icon-notification {
  -webkit-mask-position: -XXXpx -YYYpx; /* TODO: Bạn cần tìm */
  mask-position: -XXXpx -YYYpx;
}

/* Icon Grid - Lưới */
.icon-grid {
  -webkit-mask-position: -XXXpx -YYYpx; /* TODO: Bạn cần tìm */
  mask-position: -XXXpx -YYYpx;
}

/* Icon Help - Trợ giúp */
.icon-help {
  -webkit-mask-position: -XXXpx -YYYpx; /* TODO: Bạn cần tìm */
  mask-position: -XXXpx -YYYpx;
}

/* Icon User - Người dùng */
.icon-user {
  -webkit-mask-position: -XXXpx -YYYpx; /* TODO: Bạn cần tìm */
  mask-position: -XXXpx -YYYpx;
}

/* Icon Chevron Down - Mũi tên xuống */
.icon-chevron-down {
  -webkit-mask-position: -XXXpx -YYYpx; /* TODO: Bạn cần tìm */
  mask-position: -XXXpx -YYYpx;
}
```

### Sidebar Icons

```css
/* Icon Dashboard - Tổng quan */
.icon-dashboard { ... }

/* Icon Asset - Tài sản */
.icon-asset { ... }

/* Icon Tools - Công cụ dụng cụ */
.icon-tools { ... }

/* Icon Category - Danh mục */
.icon-category { ... }

/* Icon Search - Tra cứu */
.icon-search { ... }

/* Icon Report - Báo cáo */
.icon-report { ... }
```

---

## 🎨 Cách sử dụng Icon sau khi điền

### Trong Vue Template:

```vue
<template>
  <!-- Icon đơn giản -->
  <i class="icon icon-notification"></i>
  
  <!-- Icon nhỏ hơn -->
  <i class="icon icon--sm icon-user"></i>
  
  <!-- Icon với màu custom -->
  <i class="icon icon-help" style="color: #0095da;"></i>
  
  <!-- Icon trong button -->
  <button class="icon-btn">
    <i class="icon icon-add"></i>
  </button>
</template>
```

---

## 🧪 Test Icon

Sau khi điền mask-position, mở browser và kiểm tra:

1. Mở DevTools (F12)
2. Inspect icon element
3. Trong **Computed** tab, xem `mask-position`
4. Nếu icon không hiển thị đúng:
   - Thử điều chỉnh giá trị X, Y
   - Kiểm tra xem có dấu `-` chưa
   - Xem `mask-size` có đúng `504px 754px` không

---

## 💡 Tips

### Tính nhanh mask-position:

1. **Sprite size:** 504px × 754px (xem trong CSS)
2. **Icon size:** 24px × 24px (default)
3. Nếu icons xếp theo grid:
   - Icon thứ 1: `0px 0px`
   - Icon thứ 2: `-24px 0px` (cạnh phải)
   - Icon hàng 2: `0px -24px` (hàng dưới)

### Debug:

Thêm class này tạm thời để xem toàn bộ sprite:

```css
.icon-debug {
  -webkit-mask-position: 0px 0px;
  mask-position: 0px 0px;
  width: 504px;
  height: 754px;
}
```

```vue
<i class="icon icon-debug"></i>
```

---

## 📞 Cần giúp đỡ?

Nếu không tìm được vị trí icon:
1. Gửi screenshot icon trong file SVG
2. Ghi chú tên icon cần tìm
3. Tôi sẽ giúp bạn tìm mask-position chính xác

---

**CreatedBy:** DDToan - (09/1/2026)
