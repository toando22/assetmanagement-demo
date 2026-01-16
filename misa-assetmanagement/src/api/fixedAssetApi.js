/*
  Mô tả: API service cho Fixed Assets
  CreatedBy: DDToan - (13/1/2026)
*/

import axiosClient from './axiosClient'

/**
 * Lấy danh sách tài sản
 * @param {object} params - Query parameters (pageIndex, pageSize, keyword, departmentId, categoryId)
 * @returns {Promise} - Danh sách tài sản
 * EditBy: DDToan - (17/1/2026) - Cập nhật comment để khớp với backend API params
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
 * @returns {Promise<string>} - Mã tài sản mới (ví dụ: "TS00001")
 * EditBy: DDToan - (16/1/2026) - Cải thiện parse response và validation
 */
export const getNewAssetCode = async () => {
  try {
    const response = await axiosClient.get('/api/v1/fixed-assets/new-code')
    
    // Backend trả về Ok(code) → response.data = string
    // Nhưng cần xử lý các trường hợp khác nhau
    let code = ''
    
    if (typeof response === 'string') {
      // Trường hợp 1: Response là string trực tiếp
      code = response.trim()
    } else if (response && typeof response === 'object') {
      // Trường hợp 2: Response là object
      if (response.code && typeof response.code === 'string') {
        code = response.code.trim()
      } else if (response.data) {
        // Trường hợp 3: Response có property data
        if (typeof response.data === 'string') {
          code = response.data.trim()
        } else if (response.data.code && typeof response.data.code === 'string') {
          code = response.data.code.trim()
        }
      } else if (typeof response === 'object' && Object.keys(response).length === 0) {
        // Trường hợp 4: Response là object rỗng
        console.warn('getNewAssetCode: Received empty object response')
      }
    }
    
    // Validate mã: phải là string không rỗng và có format hợp lệ (TS + số)
    if (!code || code.trim() === '') {
      console.error('getNewAssetCode: Invalid response format, received:', response)
      throw new Error('Không thể lấy mã tài sản mới từ server')
    }
    
    // Kiểm tra format cơ bản (TS + số)
    if (!/^TS\d+$/.test(code)) {
      console.warn('getNewAssetCode: Code format may be invalid:', code)
      // Vẫn trả về nếu có giá trị, để backend validate
    }
    
    return code
  } catch (error) {
    console.error('Error in getNewAssetCode:', error)
    // Re-throw để caller có thể handle
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

/**
 * Xóa nhiều tài sản
 * @param {Array<string>} ids - Danh sách ID của các tài sản cần xóa
 * @returns {Promise} - Kết quả xóa
 * CreatedBy: DDToan - (17/1/2026)
 */
export const deleteMultipleFixedAssets = async (ids) => {
  try {
    const response = await axiosClient.delete('/api/v1/fixed-assets/batch-delete', {
      data: ids // Axios DELETE với body cần dùng data property
    })
    return response
  } catch (error) {
    console.error('Error in deleteMultipleFixedAssets:', error)
    throw error
  }
}

/**
 * Nhân bản tài sản - Lấy dữ liệu từ tài sản gốc với mã mới
 * @param {string} sourceId - ID của tài sản gốc cần nhân bản
 * @returns {Promise} - Dữ liệu tài sản đã nhân bản (có mã mới, ID reset)
 * CreatedBy: DDToan - (17/1/2026)
 */
export const cloneFixedAsset = async (sourceId) => {
  try {
    const response = await axiosClient.get(`/api/v1/fixed-assets/${sourceId}/clone`)
    return response
  } catch (error) {
    console.error('Error in cloneFixedAsset:', error)
    throw error
  }
}
