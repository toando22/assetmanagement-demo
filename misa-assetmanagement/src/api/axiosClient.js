/*
  Mô tả: Axios client configuration cho API calls
  CreatedBy: DDToan - (13/1/2026)
*/

import axios from 'axios'

// Tạo axios instance với config mặc định
const axiosClient = axios.create({
  baseURL: 'https://localhost:7038', 
  timeout: 10000, // 10 giây
  headers: {
    'Content-Type': 'application/json'
  }
})

// Request interceptor - có thể thêm token, logging, etc.
axiosClient.interceptors.request.use(
  (config) => {
    // Có thể thêm token vào đây sau này
    // if (token) {
    //   config.headers.Authorization = `Bearer ${token}`
    // }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

// Response interceptor - xử lý response và error
axiosClient.interceptors.response.use(
  (response) => {
    // Trả về data trực tiếp từ response
    return response.data
  },
  (error) => {
    // Xử lý lỗi
    if (error.response) {
      // Lỗi từ server (4xx, 5xx)
      console.error('API Error:', error.response.data)
      return Promise.reject(error.response.data || { message: 'Có lỗi xảy ra từ server' })
    } else if (error.request) {
      // Không nhận được response (network error)
      console.error('Network Error:', error.request)
      return Promise.reject({ message: 'Không thể kết nối đến server. Vui lòng kiểm tra kết nối mạng.' })
    } else {
      // Lỗi khác
      console.error('Error:', error.message)
      return Promise.reject({ message: error.message || 'Có lỗi xảy ra' })
    }
  }
)

export default axiosClient
