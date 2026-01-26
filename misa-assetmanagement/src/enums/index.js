/*
  Mô tả: Enums cho ứng dụng
  
  CreatedBy: DDToan - (09/1/2026)
  
  EditBy: 
*/

/**
 * Enum cho trạng thái form
 * CreatedBy: DDToan - (09/1/2026)
 */
export const FormMode = {
  ADD: 'add',
  EDIT: 'edit',
  VIEW: 'view',
  DUPLICATE: 'duplicate'
}

/**
 * Enum cho trạng thái API request
 * CreatedBy: DDToan - (09/1/2026)
 */
export const RequestStatus = {
  IDLE: 'idle',
  LOADING: 'loading',
  SUCCESS: 'success',
  ERROR: 'error'
}

/**
 * Enum cho loại thông báo
 * CreatedBy: DDToan - (09/1/2026)
 */
export const NotificationType = {
  SUCCESS: 'success',
  ERROR: 'error',
  WARNING: 'warning',
  INFO: 'info'
}

/**
 * Enum cho kích thước component
 * CreatedBy: DDToan - (09/1/2026)
 */
export const ComponentSize = {
  XS: 'xs',
  SM: 'sm',
  MD: 'md',
  LG: 'lg',
  XL: 'xl'
}

/**
 * Enum cho căn lề
 * CreatedBy: DDToan - (09/1/2026)
 */
export const TextAlign = {
  LEFT: 'left',
  CENTER: 'center',
  RIGHT: 'right'
}
