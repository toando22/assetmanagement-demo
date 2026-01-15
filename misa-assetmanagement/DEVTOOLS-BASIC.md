# 🎯 Hướng Dẫn DevTools Cơ Bản - Phím Tắt & Debug Đơn Giản

## 📌 PHÍM TẮT QUAN TRỌNG NHẤT

### Mở/Đóng DevTools
- **F12** - Mở/đóng DevTools
- **Ctrl + Shift + I** (Windows) / **Cmd + Option + I** (Mac) - Mở DevTools
- **Ctrl + Shift + J** - Mở DevTools và focus vào Console tab
- **Ctrl + Shift + C** - Mở DevTools ở chế độ Inspect Element

### Điều hướng giữa các Tab
- **Ctrl + [`** - Tab trước đó
- **Ctrl + ]** - Tab tiếp theo
- **Ctrl + 1-9** - Chuyển đến tab số 1-9

### Console (Quan trọng nhất!)
- **Ctrl + L** - Xóa console
- **Ctrl + K** - Xóa console (một số trình duyệt)
- **Esc** - Mở/đóng console drawer (ở dưới cùng)

### Elements Tab
- **Ctrl + F** - Tìm kiếm trong Elements
- **H** - Ẩn/hiện element được chọn
- **Delete** - Xóa element được chọn (tạm thời)
- **Ctrl + Z** - Undo thay đổi

### Network Tab
- **Ctrl + E** - Bắt đầu/ngừng recording
- **Ctrl + R** - Reload và record

### Sources Tab
- **Ctrl + P** - Tìm file nhanh
- **Ctrl + Shift + P** - Command palette
- **F8** - Pause/Resume
- **F10** - Step over
- **F11** - Step into
- **Shift + F11** - Step out

---

## 🎯 CÁC THAO TÁC DEBUG ĐƠN GIẢN

### 1. Kiểm tra Element (Đơn giản nhất)

**Bước 1:** Mở DevTools (F12)

**Bước 2:** Click vào icon **Inspect** (hoặc Ctrl+Shift+C)

**Bước 3:** Hover vào dropdown pagination "20" trên web

**Bước 4:** Click vào nó → Element sẽ được highlight trong Elements tab

**Bước 5:** Trong Elements tab, bạn sẽ thấy:
```html
<div class="ms-dropdown asset-table__pagination-dropdown">
  <div class="ms-dropdown__trigger">...</div>
</div>
```

### 2. Xem Styles của Element

**Sau khi chọn element trong Elements tab:**

- Bên phải sẽ có tab **Styles**
- Xem các CSS properties như `position`, `z-index`, `overflow`
- Có thể bật/tắt từng property bằng cách click vào checkbox

**Ví dụ:** Tìm `.asset-list__table-wrapper` và xem `overflow: auto`

### 3. Thay đổi CSS tạm thời (Để test)

**Trong Elements tab:**
1. Chọn element
2. Ở tab Styles, tìm property muốn thay đổi
3. Click vào giá trị và sửa
4. Ví dụ: Đổi `overflow: auto` thành `overflow: visible` để test

**Lưu ý:** Thay đổi này chỉ tạm thời, reload trang sẽ mất!

### 4. Console - Lệnh đơn giản nhất

**Mở Console (Ctrl+Shift+J) và thử:**

```javascript
// 1. Tìm element
document.querySelector('.asset-table__pagination-dropdown')

// 2. Xem có tồn tại không?
document.querySelector('.asset-table__pagination-dropdown') ? 'Có' : 'Không'

// 3. Click vào dropdown
document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger').click()

// 4. Xem menu có hiển thị không?
document.querySelector('.ms-dropdown__menu--teleported') ? 'Có menu' : 'Không có menu'
```

### 5. Xem vị trí của Element

**Trong Console:**
```javascript
// Lấy dropdown trigger
const trigger = document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger')

// Xem vị trí
trigger.getBoundingClientRect()
```

**Kết quả sẽ hiển thị:**
```
{
  x: 123.45,
  y: 678.90,
  width: 60,
  height: 28,
  top: 678.90,
  left: 123.45,
  bottom: 706.90,
  right: 183.45
}
```

### 6. Kiểm tra Menu có bị che không

**Mở dropdown trước, sau đó chạy trong Console:**
```javascript
const trigger = document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger')
const menu = document.querySelector('.ms-dropdown__menu--teleported')

if (trigger && menu) {
  const triggerRect = trigger.getBoundingClientRect()
  const menuRect = menu.getBoundingClientRect()
  const distance = triggerRect.top - menuRect.bottom
  
  console.log('Khoảng cách:', distance, 'px')
  console.log('Menu bị che?', distance < 4 ? 'CÓ' : 'KHÔNG')
}
```

### 7. Test Scroll

**Bước 1:** Mở dropdown

**Bước 2:** Trong Console, chạy:
```javascript
const tableWrapper = document.querySelector('.asset-list__table-wrapper')
tableWrapper.addEventListener('scroll', () => {
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  if (menu) {
    console.log('Menu position:', menu.getBoundingClientRect().top)
  }
})
```

**Bước 3:** Scroll table và xem console có log không

### 8. Xem Computed Styles (Styles thực tế)

**Trong Console:**
```javascript
const menu = document.querySelector('.ms-dropdown__menu--teleported')
if (menu) {
  const styles = window.getComputedStyle(menu)
  console.log('Position:', styles.position)
  console.log('Z-index:', styles.zIndex)
  console.log('Visibility:', styles.visibility)
}
```

### 9. Breakpoint đơn giản (Dừng code)

**Trong Sources tab:**
1. Tìm file `MsDropdown.vue`
2. Tìm function `updateTeleportedMenuPosition`
3. Click vào số dòng bên trái để đặt breakpoint (chấm đỏ)
4. Mở dropdown → Code sẽ dừng ở đó
5. Xem giá trị các biến ở bên phải

**Hoặc dùng `debugger` trong code:**
```javascript
// Thêm vào code
debugger; // Code sẽ dừng ở đây
```

### 10. Xem Network Requests

**Khi load trang:**
1. Mở tab **Network**
2. Reload trang (F5)
3. Xem các requests API
4. Click vào request để xem:
   - Headers
   - Response
   - Timing

---

## 🎨 CÁC THAO TÁC THƯỜNG DÙNG

### Highlight Element trên trang
- **Ctrl + Shift + C** → Click vào element trên trang
- Element sẽ được highlight

### Copy Element
- Chọn element trong Elements tab
- Right-click → **Copy** → **Copy element**
- Hoặc **Copy selector** để lấy CSS selector

### Edit HTML tạm thời
- Chọn element trong Elements tab
- Right-click → **Edit as HTML**
- Sửa và nhấn Enter

### Xem Event Listeners
- Chọn element trong Elements tab
- Tab **Event Listeners** bên phải
- Xem các events đã được attach

### Clear Console
- **Ctrl + L** hoặc click icon 🚫 trong Console

### Filter trong Console
- Gõ `$` để filter elements
- Gõ `$$` để querySelectorAll
- Ví dụ: `$$('.ms-dropdown')`

---

## 🚀 WORKFLOW DEBUG ĐƠN GIẢN

### Khi dropdown không hoạt động:

**1. Kiểm tra element có tồn tại:**
```javascript
document.querySelector('.asset-table__pagination-dropdown')
```

**2. Kiểm tra có mở được không:**
```javascript
document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger').click()
```

**3. Kiểm tra menu có hiển thị:**
```javascript
const menu = document.querySelector('.ms-dropdown__menu--teleported')
console.log('Menu:', menu)
console.log('Visible:', menu ? window.getComputedStyle(menu).visibility : 'N/A')
```

**4. Kiểm tra vị trí:**
```javascript
const menu = document.querySelector('.ms-dropdown__menu--teleported')
if (menu) {
  const rect = menu.getBoundingClientRect()
  console.log('Top:', rect.top, 'Left:', rect.left)
}
```

**5. Kiểm tra z-index:**
```javascript
const menu = document.querySelector('.ms-dropdown__menu--teleported')
if (menu) {
  console.log('Z-index:', window.getComputedStyle(menu).zIndex)
}
```

---

## 💡 MẸO NHỎ

1. **Console là bạn tốt nhất** - Hầu hết debug đều bắt đầu từ Console

2. **Elements tab để xem cấu trúc** - Hiểu HTML structure

3. **Styles tab để xem CSS** - Tìm vấn đề về styling

4. **Network tab để xem API** - Kiểm tra data từ backend

5. **Sources tab để debug code** - Đặt breakpoint, xem variables

---

## 📝 CHECKLIST DEBUG NHANH

Khi gặp vấn đề với dropdown:

- [ ] Mở DevTools (F12)
- [ ] Kiểm tra element có tồn tại trong Console
- [ ] Xem element trong Elements tab
- [ ] Kiểm tra CSS trong Styles tab
- [ ] Test click trong Console
- [ ] Xem vị trí menu
- [ ] Kiểm tra z-index
- [ ] Test scroll nếu cần

---

## 🎯 BÀI TẬP THỰC HÀNH

**Bài 1: Tìm dropdown pagination**
```javascript
// Chạy trong Console
document.querySelector('.asset-table__pagination-dropdown')
```

**Bài 2: Mở dropdown**
```javascript
// Chạy trong Console
document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger').click()
```

**Bài 3: Xem menu**
```javascript
// Chạy sau khi mở dropdown
const menu = document.querySelector('.ms-dropdown__menu--teleported')
console.log('Menu:', menu)
console.log('Position:', menu?.getBoundingClientRect())
```

**Bài 4: Kiểm tra styles**
```javascript
const menu = document.querySelector('.ms-dropdown__menu--teleported')
if (menu) {
  const styles = window.getComputedStyle(menu)
  console.log({
    position: styles.position,
    zIndex: styles.zIndex,
    visibility: styles.visibility
  })
}
```

---

**Chúc bạn debug vui vẻ! 🎉**
