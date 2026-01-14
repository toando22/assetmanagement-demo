/*
  Mô tả: API service cho Fixed Assets
  CreatedBy: DDToan - (13/1/2026)
*/

import axiosClient from './axiosClient'

/**
 * Lấy danh sách tài sản
 * @param {object} params - Query parameters (page, pageSize, search, assetTypeId, departmentId)
 * @returns {Promise} - Danh sách tài sản
 */
export const getFixedAssets = async (params = {}) => {
  try {
    const response = await axiosClient.get('/api/v1/fixed-assets', {
      params: params
    })
    return response
  } catch (error) {
    console.error('Error in getFixedAssets:', error)
    throw error
  }
}

/**
 * Lấy tài sản theo ID
 * @param {string} id - ID của tài sản
 * @returns {Promise} - Thông tin tài sản
 */
export const getFixedAssetById = async (id) => {
  try {
    const response = await axiosClient.get(`/api/v1/fixed-assets/${id}`)
    return response
  } catch (error) {
    console.error('Error in getFixedAssetById:', error)
    throw error
  }
}

/**
 * Lấy mã tài sản mới tự động
 * @returns {Promise} - Mã tài sản mới (ví dụ: "TS00001")
 */
export const getNewAssetCode = async () => {
  try {
    const response = await axiosClient.get('/api/v1/fixed-assets/new-code')
    return response
  } catch (error) {
    console.error('Error in getNewAssetCode:', error)
    throw error
  }
}

/**
 * Tạo mới tài sản
 * @param {object} data - Dữ liệu tài sản
 * @returns {Promise} - Kết quả tạo mới
 */
export const createFixedAsset = async (data) => {
  try {
    const response = await axiosClient.post('/api/v1/fixed-assets', data)
    return response
  } catch (error) {
    console.error('Error in createFixedAsset:', error)
    throw error
  }
}

/**
 * Cập nhật tài sản
 * @param {string} id - ID của tài sản
 * @param {object} data - Dữ liệu tài sản cần cập nhật
 * @returns {Promise} - Kết quả cập nhật
 */
export const updateFixedAsset = async (id, data) => {
  try {
    const response = await axiosClient.put(`/api/v1/fixed-assets/${id}`, data)
    return response
  } catch (error) {
    console.error('Error in updateFixedAsset:', error)
    throw error
  }
}

/**
 * Xóa tài sản
 * @param {string} id - ID của tài sản
 * @returns {Promise} - Kết quả xóa
 */
export const deleteFixedAsset = async (id) => {
  try {
    const response = await axiosClient.delete(`/api/v1/fixed-assets/${id}`)
    return response
  } catch (error) {
    console.error('Error in deleteFixedAsset:', error)
    throw error
  }
}
