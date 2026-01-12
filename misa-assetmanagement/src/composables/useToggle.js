/*
  Mô tả: Composable để toggle boolean state
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

import { ref } from 'vue'

/**
 * Composable để toggle boolean state
 * @param {boolean} initialValue - Giá trị khởi tạo (mặc định false)
 * @returns {object} - { value, toggle, setTrue, setFalse }
 */
export function useToggle(initialValue = false) {
  const value = ref(initialValue)

  /*
    Mô tả: Toggle giá trị boolean
    
    CreatedBy: DDToan - (09/1/2026)
  */
  const toggle = () => {
    value.value = !value.value
  }

  /*
    Mô tả: Set giá trị thành true
    
    CreatedBy: DDToan - (09/1/2026)
  */
  const setTrue = () => {
    value.value = true
  }

  /*
    Mô tả: Set giá trị thành false
    
    CreatedBy: DDToan - (09/1/2026)
  */
  const setFalse = () => {
    value.value = false
  }

  return {
    value,
    toggle,
    setTrue,
    setFalse
  }
}
