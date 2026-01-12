/*
  Mô tả: Utility functions cho validation dữ liệu form tài sản
  
  CreatedBy: DDToan - (11/1/2026)
  
  EditBy: 
*/

import { parseNumber, parseDecimal } from './format.js'

/**
 * Validate form tài sản (tổng hợp tất cả validation)
 * @param {object} formData - Dữ liệu form cần validate
 * @param {array} existingAssets - Danh sách tài sản hiện có (để check trùng mã)
 * @param {string} currentAssetCode - Mã tài sản hiện tại (nếu đang sửa)
 * @returns {object} - { isValid: boolean, errors: array }
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const validateAssetForm = (formData, existingAssets = [], currentAssetCode = '') => {
  const errors = []
  
  // 1. Validate required fields
  const requiredErrors = validateRequiredFields(formData)
  if (requiredErrors.length > 0) {
    errors.push(...requiredErrors)
  }
  
  // 2. Validate business rules (chỉ validate nếu không có lỗi required)
  if (requiredErrors.length === 0) {
    const businessErrors = validateBusinessRules(formData)
    if (businessErrors.length > 0) {
      errors.push(...businessErrors)
    }
  }
  
  // 3. Validate trùng mã tài sản (chỉ khi thêm mới hoặc đổi mã)
  if (formData.assetCode && formData.assetCode !== currentAssetCode) {
    const duplicateError = validateDuplicateCode(formData.assetCode, existingAssets)
    if (duplicateError) {
      errors.push(duplicateError)
    }
  }
  
  return {
    isValid: errors.length === 0,
    errors
  }
}

/**
 * Validate các trường bắt buộc
 * @param {object} formData - Dữ liệu form
 * @returns {array} - Mảng các lỗi [{ field, message }]
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const validateRequiredFields = (formData) => {
  const errors = []
  const requiredFields = [
    { key: 'assetCode', label: 'Mã tài sản' },
    { key: 'assetName', label: 'Tên tài sản' },
    { key: 'departmentCode', label: 'Mã bộ phận sử dụng' },
    { key: 'assetTypeCode', label: 'Mã loại tài sản' },
    { key: 'purchaseDate', label: 'Ngày mua' },
    { key: 'startUseDate', label: 'Ngày bắt đầu sử dụng' },
    { key: 'originalCost', label: 'Nguyên giá' },
    { key: 'depreciationRate', label: 'Tỷ lệ hao mòn' },
    { key: 'trackingYear', label: 'Năm theo dõi' }
  ]
  
  requiredFields.forEach(field => {
    const value = formData[field.key]
    if (!value || value === '' || value === 0 || value === '0') {
      errors.push({
        field: field.key,
        message: `Cần phải nhập thông tin ${field.label}.`
      })
    }
  })
  
  return errors
}

/**
 * Validate business rules
 * @param {object} formData - Dữ liệu form
 * @returns {array} - Mảng các lỗi [{ field, message }]
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const validateBusinessRules = (formData) => {
  const errors = []
  
  // 1. Validate Số lượng phải là số nguyên dương
  const quantity = Number(formData.quantity)
  if (isNaN(quantity) || quantity <= 0 || !Number.isInteger(quantity)) {
    errors.push({
      field: 'quantity',
      message: 'Số lượng phải là số nguyên dương.'
    })
  }
  
  // 2. Validate Tỷ lệ hao mòn = 1 / Số năm sử dụng
  const yearsOfUse = Number(formData.yearsOfUse)
  const depreciationRate = parseDecimal(formData.depreciationRate)
  
  if (yearsOfUse > 0 && depreciationRate > 0) {
    const expectedRate = (1 / yearsOfUse) * 100
    const rateDifference = Math.abs(depreciationRate - expectedRate)
    
    // Cho phép sai lệch 0.01% (do làm tròn)
    if (rateDifference > 0.01) {
      errors.push({
        field: 'depreciationRate',
        message: 'Tỷ lệ hao mòn phải bằng 1/Số năm sử dụng.'
      })
    }
  }
  
  // 3. Validate Hao mòn năm <= Nguyên giá
  const originalCost = parseNumber(formData.originalCost)
  const annualDepreciation = parseNumber(formData.annualDepreciationValue)
  
  if (originalCost > 0 && annualDepreciation > originalCost) {
    errors.push({
      field: 'annualDepreciationValue',
      message: 'Hao mòn năm phải nhỏ hơn hoặc bằng nguyên giá.'
    })
  }
  
  // 4. Validate Nguyên giá phải là số dương
  if (originalCost <= 0) {
    errors.push({
      field: 'originalCost',
      message: 'Nguyên giá phải lớn hơn 0.'
    })
  }
  
  return errors
}

/**
 * Validate trùng mã tài sản
 * @param {string} assetCode - Mã tài sản cần check
 * @param {array} existingAssets - Danh sách tài sản hiện có
 * @returns {object|null} - Lỗi nếu trùng, null nếu không trùng
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const validateDuplicateCode = (assetCode, existingAssets = []) => {
  if (!assetCode || assetCode === '') {
    return null
  }
  
  const isDuplicate = existingAssets.some(asset => 
    asset.code && asset.code.toString().toLowerCase() === assetCode.toString().toLowerCase()
  )
  
  if (isDuplicate) {
    return {
      field: 'assetCode',
      message: 'Mã tài sản đã tồn tại trong hệ thống.'
    }
  }
  
  return null
}

/**
 * Validate số (không cho text/ký tự đặc biệt)
 * @param {string|number} value - Giá trị cần validate
 * @param {boolean} allowDecimal - Cho phép số thập phân (mặc định: false)
 * @returns {boolean} - true nếu hợp lệ, false nếu không hợp lệ
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const validateNumber = (value, allowDecimal = false) => {
  if (value === null || value === undefined || value === '') {
    return true // Empty is valid (will be handled by required validation)
  }
  
  const stringValue = value.toString()
  
  // Remove dấu chấm (ngăn cách hàng nghìn) và dấu phẩy (thập phân)
  const cleanedValue = stringValue.replace(/\./g, '').replace(',', '.')
  
  if (allowDecimal) {
    // Cho phép số thập phân
    return /^\d+\.?\d*$/.test(cleanedValue)
  } else {
    // Chỉ cho phép số nguyên
    return /^\d+$/.test(cleanedValue)
  }
}

/**
 * Validate định dạng ngày dd/mm/YYYY
 * @param {string} dateString - Chuỗi ngày cần validate
 * @returns {boolean} - true nếu hợp lệ, false nếu không hợp lệ
 * 
 * CreatedBy: DDToan - (11/1/2026)
 */
export const validateDate = (dateString) => {
  if (!dateString || dateString === '') {
    return true // Empty is valid (will be handled by required validation)
  }
  
  // Check format dd/mm/YYYY
  const dateRegex = /^(\d{2})\/(\d{2})\/(\d{4})$/
  const match = dateString.match(dateRegex)
  
  if (!match) {
    return false
  }
  
  const day = parseInt(match[1], 10)
  const month = parseInt(match[2], 10)
  const year = parseInt(match[3], 10)
  
  // Validate range
  if (month < 1 || month > 12) {
    return false
  }
  
  if (day < 1 || day > 31) {
    return false
  }
  
  // Validate actual date
  const date = new Date(year, month - 1, day)
  if (
    date.getDate() !== day ||
    date.getMonth() !== month - 1 ||
    date.getFullYear() !== year
  ) {
    return false
  }
  
  return true
}
