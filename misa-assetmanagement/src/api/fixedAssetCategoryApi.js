/*
  Mô tả: API service cho Fixed Asset Categories
  CreatedBy: DDToan - (13/1/2026)
*/

import axiosClient from './axiosClient'

/**
 * Lấy danh sách tất cả loại tài sản
 * CreatedBy: DDToan - (14/1/2026)
 * @returns {Promise} - Danh sách loại tài sản
 */
export const getFixedAssetCategories = async () => {
  try {
    const response = await axiosClient.get('/api/v1/fixed-asset-categories')
    return response
  } catch (error) {
    console.error('Error in getFixedAssetCategories:', error)
    throw error
  }
}

/**
 * Lấy loại tài sản theo ID
 * CreatedBy: DDToan - (14/1/2026)
 * @param {string} id - ID của loại tài sản
 * @returns {Promise} - Thông tin loại tài sản
 */
export const getFixedAssetCategoryById = async (id) => {
  try {
    const response = await axiosClient.get(`/api/v1/fixed-asset-categories/${id}`)
    return response
  } catch (error) {
    console.error('Error in getFixedAssetCategoryById:', error)
    throw error
  }
}
