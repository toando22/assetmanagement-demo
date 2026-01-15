// ============================================
// SCRIPT DEBUG DROPDOWN PAGINATION
// Copy toàn bộ code này vào Console (F12) và chạy
// ============================================

// Function debug all-in-one
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
        top: triggerRect.top.toFixed(2),
        left: triggerRect.left.toFixed(2),
        bottom: triggerRect.bottom.toFixed(2),
        width: triggerRect.width.toFixed(2)
      },
      menu: {
        top: menuRect.top.toFixed(2),
        left: menuRect.left.toFixed(2),
        bottom: menuRect.bottom.toFixed(2),
        width: menuRect.width.toFixed(2),
        height: menuRect.height.toFixed(2)
      },
      distance: distance.toFixed(2) + 'px',
      isOverlapping: distance < 4 ? '⚠️ CÓ - Menu bị che!' : '✅ KHÔNG'
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
      hasScroll: tableWrapper.scrollHeight > tableWrapper.clientHeight ? '✅ Có' : '❌ Không'
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
  return { dropdown, trigger, menu, tableWrapper }
}

// Function để test scroll
function testScroll() {
  const tableWrapper = document.querySelector('.asset-list__table-wrapper')
  if (!tableWrapper) {
    console.error('❌ Không tìm thấy table wrapper!')
    return
  }
  
  let scrollCount = 0
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  
  const scrollHandler = () => {
    scrollCount++
    if (menu) {
      const menuRect = menu.getBoundingClientRect()
      const trigger = document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger')
      const triggerRect = trigger?.getBoundingClientRect()
      
      if (scrollCount <= 5 || scrollCount % 10 === 0) {
        console.log(`📜 Scroll #${scrollCount}:`, {
          menuTop: menuRect.top.toFixed(2),
          triggerTop: triggerRect?.top.toFixed(2),
          distance: triggerRect ? (triggerRect.top - menuRect.bottom).toFixed(2) : 'N/A'
        })
      }
    }
  }
  
  tableWrapper.addEventListener('scroll', scrollHandler, { passive: true })
  console.log('✅ Đã bật scroll monitor. Hãy scroll table và xem console.')
  console.log('💡 Để dừng: chạy stopScrollTest()')
  
  window.stopScrollTest = () => {
    tableWrapper.removeEventListener('scroll', scrollHandler)
    console.log('🛑 Đã dừng scroll monitor')
  }
}

// Function để test resize
function testResize() {
  let resizeCount = 0
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  
  const resizeHandler = () => {
    resizeCount++
    if (menu) {
      const menuRect = menu.getBoundingClientRect()
      console.log(`📐 Resize #${resizeCount}:`, {
        menuTop: menuRect.top.toFixed(2),
        menuLeft: menuRect.left.toFixed(2),
        windowWidth: window.innerWidth,
        windowHeight: window.innerHeight,
        menuInViewport: menuRect.top >= 0 && menuRect.left >= 0 && 
                       menuRect.bottom <= window.innerHeight && 
                       menuRect.right <= window.innerWidth
      })
    }
  }
  
  window.addEventListener('resize', resizeHandler)
  console.log('✅ Đã bật resize monitor. Hãy resize window và xem console.')
  console.log('💡 Để dừng: chạy stopResizeTest()')
  
  window.stopResizeTest = () => {
    window.removeEventListener('resize', resizeHandler)
    console.log('🛑 Đã dừng resize monitor')
  }
}

// Function để mở dropdown programmatically
function openDropdown() {
  const trigger = document.querySelector('.asset-table__pagination-dropdown .ms-dropdown__trigger')
  if (trigger) {
    trigger.click()
    console.log('✅ Đã click vào dropdown trigger')
    setTimeout(() => debugDropdownPagination(), 100)
  } else {
    console.error('❌ Không tìm thấy dropdown trigger!')
  }
}

// Function để highlight elements
function highlightElements() {
  const dropdown = document.querySelector('.asset-table__pagination-dropdown')
  const trigger = dropdown?.querySelector('.ms-dropdown__trigger')
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  const tableWrapper = document.querySelector('.asset-list__table-wrapper')
  
  // Highlight trigger
  if (trigger) {
    trigger.style.outline = '3px solid #00ff00'
    trigger.style.outlineOffset = '2px'
    console.log('🟢 Trigger được highlight màu xanh lá')
  }
  
  // Highlight menu
  if (menu) {
    menu.style.outline = '3px solid #ff0000'
    menu.style.outlineOffset = '2px'
    console.log('🔴 Menu được highlight màu đỏ')
  }
  
  // Highlight table wrapper
  if (tableWrapper) {
    tableWrapper.style.outline = '3px solid #0000ff'
    tableWrapper.style.outlineOffset = '2px'
    console.log('🔵 Table wrapper được highlight màu xanh dương')
  }
  
  console.log('💡 Để bỏ highlight: chạy removeHighlights()')
  
  window.removeHighlights = () => {
    if (trigger) trigger.style.outline = ''
    if (menu) menu.style.outline = ''
    if (tableWrapper) tableWrapper.style.outline = ''
    console.log('✅ Đã bỏ highlight')
  }
}

// Function để check z-index conflicts
function checkZIndex() {
  const menu = document.querySelector('.ms-dropdown__menu--teleported')
  if (!menu) {
    console.error('❌ Menu không mở! Hãy mở dropdown trước.')
    return
  }
  
  const menuRect = menu.getBoundingClientRect()
  const centerX = menuRect.left + menuRect.width / 2
  const centerY = menuRect.top + menuRect.height / 2
  
  const elements = document.elementsFromPoint(centerX, centerY)
  const menuZIndex = parseInt(window.getComputedStyle(menu).zIndex) || 0
  
  console.group('🔍 Z-index Analysis')
  console.log('Menu z-index:', menuZIndex)
  console.log('Elements tại vị trí menu (từ trên xuống):')
  
  elements.forEach((el, index) => {
    if (el === menu) {
      console.log(`  ${index + 1}. ✅ ${el.tagName}.${el.className} (Menu - z-index: ${menuZIndex})`)
    } else {
      const zIndex = parseInt(window.getComputedStyle(el).zIndex) || 'auto'
      const position = window.getComputedStyle(el).position
      const isOverMenu = index < elements.indexOf(menu)
      
      if (isOverMenu && position !== 'static') {
        console.log(`  ${index + 1}. ⚠️ ${el.tagName}.${el.className} (z-index: ${zIndex}, position: ${position}) - CÓ THỂ CHE MENU!`)
      } else {
        console.log(`  ${index + 1}. ${el.tagName}.${el.className} (z-index: ${zIndex})`)
      }
    }
  })
  
  console.groupEnd()
}

// ============================================
// HƯỚNG DẪN SỬ DỤNG
// ============================================

console.log(`
╔══════════════════════════════════════════════════════════╗
║     🐛 DEBUG DROPDOWN PAGINATION - SCRIPT ĐÃ SẴN SÀNG    ║
╚══════════════════════════════════════════════════════════╝

📋 CÁC LỆNH CÓ SẴN:

1. debugDropdownPagination()  - Debug tổng quan (chạy ngay!)
2. openDropdown()              - Mở dropdown programmatically
3. highlightElements()         - Highlight các elements quan trọng
4. testScroll()                - Test scroll event
5. testResize()                - Test resize event
6. checkZIndex()               - Kiểm tra z-index conflicts

💡 VÍ DỤ SỬ DỤNG:
   debugDropdownPagination()  // Chạy ngay để xem tình trạng
   openDropdown()             // Mở dropdown
   highlightElements()        // Xem các elements
   testScroll()               // Test scroll
   
🎯 BẮT ĐẦU: Chạy debugDropdownPagination() ngay!
`)

// Tự động chạy lần đầu
setTimeout(() => {
  console.log('🚀 Tự động chạy debug lần đầu...')
  debugDropdownPagination()
}, 500)
