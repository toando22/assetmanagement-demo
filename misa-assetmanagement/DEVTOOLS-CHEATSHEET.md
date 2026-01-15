# 🚀 DevTools Cheat Sheet - Tham Khảo Nhanh

## ⌨️ PHÍM TẮT QUAN TRỌNG

| Phím tắt | Chức năng |
|----------|-----------|
| **F12** | Mở/đóng DevTools |
| **Ctrl + Shift + I** | Mở DevTools |
| **Ctrl + Shift + J** | Mở Console |
| **Ctrl + Shift + C** | Inspect Element |
| **Ctrl + L** | Xóa Console |
| **Esc** | Mở/đóng Console drawer |
| **Ctrl + [`** | Tab trước |
| **Ctrl + ]** | Tab sau |

---

## 🎯 LỆNH CONSOLE ĐƠN GIẢN

### Tìm Element
```javascript
// Tìm 1 element
document.querySelector('.asset-table__pagination-dropdown')

// Tìm nhiều elements
document.querySelectorAll('.ms-dropdown')
```

### Click Element
```javascript
document.querySelector('.ms-dropdown__trigger').click()
```

### Xem Vị trí
```javascript
const el = document.querySelector('.ms-dropdown__trigger')
el.getBoundingClientRect()
```

### Xem Styles
```javascript
const el = document.querySelector('.ms-dropdown__menu--teleported')
window.getComputedStyle(el).zIndex
window.getComputedStyle(el).position
```

### Kiểm tra có tồn tại
```javascript
document.querySelector('.ms-dropdown') ? 'Có' : 'Không'
```

---

## 🔍 DEBUG DROPDOWN - 4 BƯỚC

### Bước 1: Tìm dropdown
```javascript
document.querySelector('.asset-table__pagination-dropdown')
```

### Bước 2: Mở dropdown
```javascript
document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger').click()
```

### Bước 3: Kiểm tra menu
```javascript
const menu = document.querySelector('.ms-dropdown__menu--teleported')
console.log('Menu:', menu)
```

### Bước 4: Xem vị trí
```javascript
const menu = document.querySelector('.ms-dropdown__menu--teleported')
if (menu) {
  console.log('Vị trí:', menu.getBoundingClientRect())
  console.log('Z-index:', window.getComputedStyle(menu).zIndex)
}
```

---

## 📋 CHECKLIST NHANH

- [ ] F12 mở DevTools
- [ ] Ctrl+Shift+J mở Console
- [ ] Tìm element: `document.querySelector('.class-name')`
- [ ] Click: `.click()`
- [ ] Xem vị trí: `.getBoundingClientRect()`
- [ ] Xem style: `window.getComputedStyle(el).property`

---

## 💡 MẸO

- **Console.log mọi thứ** để xem giá trị
- **Elements tab** để xem HTML structure
- **Styles tab** để xem CSS
- **Ctrl+L** để clear console khi rối

---

**In ra và dán lên màn hình để tham khảo nhanh! 📌**
