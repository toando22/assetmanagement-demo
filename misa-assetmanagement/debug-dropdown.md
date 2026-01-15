# Hướng dẫn Debug Dropdown Pagination

## Bước 1: Mở DevTools
- Nhấn **F12** hoặc **Ctrl+Shift+I** (Windows) / **Cmd+Option+I** (Mac)
- Hoặc Right-click → **Inspect**

## Bước 2: Debug Dropdown Pagination

### 2.1. Tìm Dropdown Element

**Trong Console, chạy:**
```javascript
// Tìm dropdown pagination
const dropdown = document.querySelector('.asset-table__pagination-dropdown')
console.log('Dropdown element:', dropdown)

// Tìm trigger button
const trigger = dropdown?.querySelector('.ms-dropdown__trigger')
console.log('Trigger element:', trigger)

// Tìm menu (có thể ở body nếu teleport)
const menu = document.querySelector('.ms-dropdown__menu--teleported')
console.log('Menu element:', menu)
```

### 2.2. Kiểm tra Vị trí Menu

**Chạy khi dropdown đang mở:**
```javascript
// Lấy vị trí trigger
const trigger = document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger')
const triggerRect = trigger?.getBoundingClientRect()
console.log('Trigger position:', {
  top: triggerRect?.top,
  left: triggerRect?.left,
  bottom: triggerRect?.bottom,
  right: triggerRect?.right,
  width: triggerRect?.width,
  height: triggerRect?.height
})

// Lấy vị trí menu
const menu = document.querySelector('.ms-dropdown__menu--teleported')
const menuRect = menu?.getBoundingClientRect()
console.log('Menu position:', {
  top: menuRect?.top,
  left: menuRect?.left,
  bottom: menuRect?.bottom,
  right: menuRect?.right,
  width: menuRect?.width,
  height: menuRect?.height
})

// Kiểm tra khoảng cách
if (triggerRect && menuRect) {
  const distance = triggerRect.top - menuRect.bottom
  console.log('Khoảng cách giữa trigger và menu:', distance, 'px')
  console.log('Menu có bị che không?', distance < 4 ? 'CÓ' : 'KHÔNG')
}
```

### 2.3. Kiểm tra Scroll Container

**Chạy để kiểm tra scroll container:**
```javascript
// Tìm table wrapper
const tableWrapper = document.querySelector('.asset-list__table-wrapper')
console.log('Table wrapper:', tableWrapper)
console.log('Has overflow:', window.getComputedStyle(tableWrapper).overflow)
console.log('Scroll height:', tableWrapper?.scrollHeight)
console.log('Client height:', tableWrapper?.clientHeight)
console.log('Có scroll không?', tableWrapper?.scrollHeight > tableWrapper?.clientHeight)
```

### 2.4. Test Scroll Event

**Chạy để test scroll:**
```javascript
// Lắng nghe scroll
const tableWrapper = document.querySelector('.asset-list__table-wrapper')
let scrollCount = 0

const scrollHandler = () => {
  scrollCount++
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  if (menu) {
    const menuRect = menu.getBoundingClientRect()
    console.log(`Scroll #${scrollCount} - Menu position:`, {
      top: menuRect.top,
      left: menuRect.left
    })
  }
}

tableWrapper?.addEventListener('scroll', scrollHandler)
console.log('Đã thêm scroll listener. Hãy scroll table và xem console.')

// Để dừng:
// tableWrapper.removeEventListener('scroll', scrollHandler)
```

### 2.5. Test Resize Event

**Chạy để test resize:**
```javascript
let resizeCount = 0

const resizeHandler = () => {
  resizeCount++
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  if (menu) {
    const menuRect = menu.getBoundingClientRect()
    console.log(`Resize #${resizeCount} - Menu position:`, {
      top: menuRect.top,
      left: menuRect.left,
      windowWidth: window.innerWidth,
      windowHeight: window.innerHeight
    })
  }
}

window.addEventListener('resize', resizeHandler)
console.log('Đã thêm resize listener. Hãy resize window và xem console.')

// Để dừng:
// window.removeEventListener('resize', resizeHandler)
```

### 2.6. Kiểm tra Z-index và Stacking Context

**Chạy để kiểm tra z-index:**
```javascript
const menu = document.querySelector('.ms-dropdown__menu--teleported')
if (menu) {
  const styles = window.getComputedStyle(menu)
  console.log('Menu z-index:', styles.zIndex)
  console.log('Menu position:', styles.position)
  console.log('Menu visibility:', styles.visibility)
  
  // Kiểm tra các element có thể che menu
  const elementsAtPosition = document.elementsFromPoint(
    menu.getBoundingClientRect().left + 10,
    menu.getBoundingClientRect().top + 10
  )
  console.log('Elements tại vị trí menu:', elementsAtPosition)
}
```

### 2.7. Monitor Dropdown State

**Chạy để monitor state:**
```javascript
// Tìm Vue component instance (nếu có Vue DevTools)
const dropdown = document.querySelector('.asset-table__pagination-dropdown')

// Kiểm tra classes
console.log('Dropdown classes:', dropdown?.className)
console.log('Is open?', dropdown?.classList.contains('ms-dropdown--open'))

// Kiểm tra style của menu
const menu = document.querySelector('.ms-dropdown__menu--teleported')
if (menu) {
  console.log('Menu inline styles:', menu.style.cssText)
  console.log('Menu computed styles:', {
    position: window.getComputedStyle(menu).position,
    top: window.getComputedStyle(menu).top,
    left: window.getComputedStyle(menu).left,
    zIndex: window.getComputedStyle(menu).zIndex,
    visibility: window.getComputedStyle(menu).visibility
  })
}
```

### 2.8. Test Click Outside

**Chạy để test click outside:**
```javascript
let clickCount = 0

const clickHandler = (e) => {
  clickCount++
  const dropdown = document.querySelector('.asset-table__pagination-dropdown')
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  const clickedInDropdown = dropdown?.contains(e.target)
  const clickedInMenu = menu?.contains(e.target)
  
  console.log(`Click #${clickCount}:`, {
    target: e.target,
    clickedInDropdown,
    clickedInMenu,
    shouldClose: !clickedInDropdown && !clickedInMenu
  })
}

document.addEventListener('click', clickHandler, true)
console.log('Đã thêm click listener. Click vào các vị trí khác nhau và xem console.')

// Để dừng:
// document.removeEventListener('click', clickHandler, true)
```

### 2.9. Kiểm tra Performance

**Chạy để kiểm tra performance:**
```javascript
// Monitor số lần update position
let updateCount = 0
const originalRAF = window.requestAnimationFrame
window.requestAnimationFrame = function(callback) {
  return originalRAF(function(...args) {
    updateCount++
    if (updateCount % 10 === 0) {
      console.log(`Position updated ${updateCount} times`)
    }
    return callback(...args)
  })
}

console.log('Đã bật performance monitor. Mở dropdown và scroll để xem.')
```

### 2.10. Debug Helper Function (All-in-one)

**Chạy function này để debug tất cả:**
```javascript
function debugDropdownPagination() {
  console.group('🔍 Debug Dropdown Pagination')
  
  // 1. Elements
  const dropdown = document.querySelector('.asset-table__pagination-dropdown')
  const trigger = dropdown?.querySelector('.ms-dropdown__trigger')
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  const tableWrapper = document.querySelector('.asset-list__table-wrapper')
  
  console.log('📦 Elements:', {
    dropdown: !!dropdown,
    trigger: !!trigger,
    menu: !!menu,
    tableWrapper: !!tableWrapper
  })
  
  // 2. Dropdown state
  const isOpen = dropdown?.classList.contains('ms-dropdown--open')
  console.log('📊 State:', {
    isOpen,
    menuVisible: menu ? window.getComputedStyle(menu).visibility !== 'hidden' : false
  })
  
  // 3. Positions
  if (trigger && menu) {
    const triggerRect = trigger.getBoundingClientRect()
    const menuRect = menu.getBoundingClientRect()
    const distance = triggerRect.top - menuRect.bottom
    
    console.log('📍 Positions:', {
      trigger: {
        top: triggerRect.top,
        left: triggerRect.left,
        bottom: triggerRect.bottom,
        width: triggerRect.width
      },
      menu: {
        top: menuRect.top,
        left: menuRect.left,
        bottom: menuRect.bottom,
        width: menuRect.width,
        height: menuRect.height
      },
      distance: distance.toFixed(2) + 'px',
      isOverlapping: distance < 4
    })
  }
  
  // 4. Styles
  if (menu) {
    const styles = window.getComputedStyle(menu)
    console.log('🎨 Menu Styles:', {
      position: styles.position,
      top: styles.top,
      left: styles.left,
      zIndex: styles.zIndex,
      visibility: styles.visibility,
      display: styles.display
    })
  }
  
  // 5. Scroll container
  if (tableWrapper) {
    const styles = window.getComputedStyle(tableWrapper)
    console.log('📜 Scroll Container:', {
      overflow: styles.overflow,
      scrollHeight: tableWrapper.scrollHeight,
      clientHeight: tableWrapper.clientHeight,
      scrollTop: tableWrapper.scrollTop,
      hasScroll: tableWrapper.scrollHeight > tableWrapper.clientHeight
    })
  }
  
  // 6. Viewport
  console.log('🖥️ Viewport:', {
    width: window.innerWidth,
    height: window.innerHeight,
    scrollX: window.scrollX,
    scrollY: window.scrollY
  })
  
  console.groupEnd()
}

// Chạy function
debugDropdownPagination()

// Hoặc gọi lại khi cần:
// debugDropdownPagination()
```

## Bước 3: Sử dụng Vue DevTools (Nếu có)

1. Cài đặt Vue DevTools extension cho Chrome
2. Mở Vue tab trong DevTools
3. Tìm component `MsDropdown` hoặc `AssetList`
4. Kiểm tra:
   - Props: `teleport`, `placement`, `scrollContainer`
   - State: `isOpen`, `teleportedMenuStyle`
   - Refs: `triggerRef`, `menuRef`, `scrollContainerRef`

## Bước 4: Breakpoints trong Code

Thêm `debugger` vào code để dừng execution:

```javascript
// Trong MsDropdown.vue - function updateTeleportedMenuPosition
const updateTeleportedMenuPosition = () => {
  debugger // ← Dừng ở đây khi function được gọi
  if (!triggerRef.value || !menuRef.value) return
  // ...
}
```

## Bước 5: Common Issues và Cách Fix

### Issue 1: Menu không hiển thị
- Kiểm tra: `menu.style.visibility` có phải `'visible'` không?
- Kiểm tra: `menu.style.display` có phải không phải `'none'` không?
- Fix: Đảm bảo `updateTeleportedMenuPosition()` được gọi

### Issue 2: Menu bị lệch khi scroll
- Kiểm tra: Scroll listener có được thêm vào `scrollContainer` không?
- Fix: Đảm bảo `scrollContainer` prop được truyền đúng

### Issue 3: Menu bị che bởi table rows
- Kiểm tra: `z-index` của menu có đủ cao không? (nên là 10000)
- Kiểm tra: Menu có `position: fixed` không?
- Fix: Kiểm tra CSS của `.ms-dropdown__menu--teleported`

### Issue 4: Flicker khi mở
- Kiểm tra: Menu có `visibility: hidden` ban đầu không?
- Fix: Đảm bảo `teleportedMenuStyle` khởi tạo với `visibility: 'hidden'`

## Tips

1. **Sử dụng Console để test nhanh:**
   ```javascript
   // Mở dropdown programmatically
   const trigger = document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger')
   trigger?.click()
   ```

2. **Monitor events:**
   ```javascript
   // Monitor tất cả events
   const monitor = (eventType) => {
     document.addEventListener(eventType, (e) => {
       console.log(`${eventType}:`, e.target)
     }, true)
   }
   
   monitor('click')
   monitor('scroll')
   monitor('resize')
   ```

3. **Check Vue component state:**
   ```javascript
   // Nếu có Vue DevTools
   const app = document.querySelector('#app').__vue_app__
   console.log('Vue app:', app)
   ```
