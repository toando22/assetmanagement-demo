/*
  Mô tả: API service cho Departments
  CreatedBy: DDToan - (14/1/2026)
*/

import axiosClient from './axiosClient'

/**
 * Lấy danh sách tất cả bộ phận
 * CreatedBy: DDToan - (14/1/2026)
 * @returns {Promise} - Danh sách bộ phận
 */
export const getDepartments = async () => {
  try {
    const response = await axiosClient.get('/api/v1/departments')
    return response
  } catch (error) {
    console.error('Error in getDepartments:', error)
    throw error
  }
}

/**
 * Lấy bộ phận theo ID
 * CreatedBy: DDToan - (14/1/2026)
 * @param {string} id - ID của bộ phận
 * @returns {Promise} - Thông tin bộ phận
 */
export const getDepartmentById = async (id) => {
  try {
    const response = await axiosClient.get(`/api/v1/departments/${id}`)
    return response
  } catch (error) {
    console.error('Error in getDepartmentById:', error)
    throw error
  }
}
