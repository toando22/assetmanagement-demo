<!--
  Mô tả: Date input component với calendar icon
  States: Default, Hover, Active, Focus, Disable, Error
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div class="ms-input-date" :class="{ 'ms-input-date--error': error, 'ms-input-date--disabled': disabled }">
    <label v-if="label" class="ms-input-date__label" :for="inputId">
      {{ label }}
      <span v-if="required" class="ms-input-date__required">*</span>
    </label>
    <div class="ms-input-date__wrapper">
      <input
        :id="inputId"
        class="ms-input-date__field"
        type="text"
        :value="modelValue"
        :placeholder="placeholder || 'dd/mm/yyyy'"
        :disabled="disabled"
        @input="handleInput"
        @focus="$emit('focus', $event)"
        @blur="handleBlur"
      />
      <i 
        class="ms-input-date__icon icon icon-calendar" 
        @click="handleIconClick"
        :class="{ 'ms-input-date__icon--clickable': !disabled }"
      ></i>
      
      <!-- Date Picker Popup -->
      <teleport to="body">
        <transition name="date-picker-fade">
          <div 
            v-if="showDatePicker" 
            class="ms-date-picker-overlay"
            @click.self="closeDatePicker"
          >
            <div class="ms-date-picker" ref="datePickerRef">
              <div class="ms-date-picker__header">
                <button 
                  class="ms-date-picker__nav-btn"
                  @click="previousMonth"
                  type="button"
                >
                  <i class="icon icon-chevron-left"></i>
                </button>
                <div class="ms-date-picker__month-year">
                  {{ currentMonthYear }}
                </div>
                <button 
                  class="ms-date-picker__nav-btn"
                  @click="nextMonth"
                  type="button"
                >
                  <i class="icon icon-chevron-right"></i>
                </button>
              </div>
              
              <div class="ms-date-picker__weekdays">
                <div 
                  v-for="day in weekdays" 
                  :key="day"
                  class="ms-date-picker__weekday"
                >
                  {{ day }}
                </div>
              </div>
              
              <div class="ms-date-picker__days">
                <button
                  v-for="day in calendarDays"
                  :key="day.key"
                  class="ms-date-picker__day"
                  :class="{
                    'ms-date-picker__day--other-month': day.isOtherMonth,
                    'ms-date-picker__day--today': day.isToday,
                    'ms-date-picker__day--selected': day.isSelected
                  }"
                  @click="selectDate(day.date)"
                  type="button"
                >
                  {{ day.day }}
                </button>
              </div>
            </div>
          </div>
        </transition>
      </teleport>
    </div>
    <span v-if="error && errorMessage" class="ms-input-date__error-message">
      {{ errorMessage }}
    </span>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho MsInputDate component
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { computed, ref, onMounted, onUnmounted, nextTick, watch } from 'vue'

const props = defineProps({
  modelValue: {
    type: String,
    default: ''
  },
  label: {
    type: String,
    default: ''
  },
  placeholder: {
    type: String,
    default: ''
  },
  disabled: {
    type: Boolean,
    default: false
  },
  error: {
    type: Boolean,
    default: false
  },
  errorMessage: {
    type: String,
    default: 'Error message'
  },
  required: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['update:modelValue', 'focus', 'blur'])

const inputId = computed(() => `input-date-${Math.random().toString(36).substr(2, 9)}`)

// Date Picker State
const showDatePicker = ref(false)
const datePickerRef = ref(null)
const currentDate = ref(new Date())

// Weekdays
const weekdays = ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7']

// Format date to dd/mm/yyyy
const formatDate = (date) => {
  if (!date) return ''
  const d = new Date(date)
  if (isNaN(d.getTime())) return ''
  const day = String(d.getDate()).padStart(2, '0')
  const month = String(d.getMonth() + 1).padStart(2, '0')
  const year = d.getFullYear()
  return `${day}/${month}/${year}`
}

// Parse date from dd/mm/yyyy
const parseDate = (dateString) => {
  if (!dateString) return null
  const parts = dateString.split('/')
  if (parts.length !== 3) return null
  const day = parseInt(parts[0], 10)
  const month = parseInt(parts[1], 10) - 1
  const year = parseInt(parts[2], 10)
  const date = new Date(year, month, day)
  if (isNaN(date.getTime())) return null
  return date
}

// Initialize currentDate from modelValue or use today
watch(() => props.modelValue, (newValue) => {
  if (newValue) {
    const parsed = parseDate(newValue)
    if (parsed) {
      currentDate.value = parsed
    }
  }
}, { immediate: true })

// Computed: Current month and year display
const currentMonthYear = computed(() => {
  const months = [
    'Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5', 'Tháng 6',
    'Tháng 7', 'Tháng 8', 'Tháng 9', 'Tháng 10', 'Tháng 11', 'Tháng 12'
  ]
  return `${months[currentDate.value.getMonth()]} ${currentDate.value.getFullYear()}`
})

// Computed: Calendar days
const calendarDays = computed(() => {
  const year = currentDate.value.getFullYear()
  const month = currentDate.value.getMonth()
  
  // First day of month
  const firstDay = new Date(year, month, 1)
  const firstDayOfWeek = firstDay.getDay() // 0 = Sunday
  
  // Last day of month
  const lastDay = new Date(year, month + 1, 0)
  const daysInMonth = lastDay.getDate()
  
  // Selected date
  const selectedDate = props.modelValue ? parseDate(props.modelValue) : null
  const today = new Date()
  today.setHours(0, 0, 0, 0)
  
  const days = []
  
  // Previous month days
  const prevMonth = new Date(year, month, 0)
  const daysInPrevMonth = prevMonth.getDate()
  for (let i = firstDayOfWeek - 1; i >= 0; i--) {
    const day = daysInPrevMonth - i
    const date = new Date(year, month - 1, day)
    days.push({
      day,
      date,
      isOtherMonth: true,
      isToday: false,
      isSelected: false,
      key: `prev-${day}`
    })
  }
  
  // Current month days
  for (let day = 1; day <= daysInMonth; day++) {
    const date = new Date(year, month, day)
    date.setHours(0, 0, 0, 0)
    const isToday = date.getTime() === today.getTime()
    const isSelected = selectedDate && date.getTime() === selectedDate.getTime()
    
    days.push({
      day,
      date,
      isOtherMonth: false,
      isToday,
      isSelected,
      key: `current-${day}`
    })
  }
  
  // Next month days to fill the grid
  const remainingDays = 42 - days.length // 6 weeks * 7 days
  for (let day = 1; day <= remainingDays; day++) {
    const date = new Date(year, month + 1, day)
    days.push({
      day,
      date,
      isOtherMonth: true,
      isToday: false,
      isSelected: false,
      key: `next-${day}`
    })
  }
  
  return days
})

// Methods
const handleInput = (event) => {
  emit('update:modelValue', event.target.value)
}

const handleBlur = (event) => {
  emit('blur', event)
}

const handleIconClick = () => {
  if (props.disabled) return
  
  // Initialize currentDate from modelValue or use today
  if (props.modelValue) {
    const parsed = parseDate(props.modelValue)
    if (parsed) {
      currentDate.value = parsed
    } else {
      currentDate.value = new Date()
    }
  } else {
    currentDate.value = new Date()
  }
  
  showDatePicker.value = true
}

const closeDatePicker = () => {
  showDatePicker.value = false
}

const selectDate = (date) => {
  const formatted = formatDate(date)
  emit('update:modelValue', formatted)
  closeDatePicker()
}

const previousMonth = () => {
  const newDate = new Date(currentDate.value)
  newDate.setMonth(newDate.getMonth() - 1)
  currentDate.value = newDate
}

const nextMonth = () => {
  const newDate = new Date(currentDate.value)
  newDate.setMonth(newDate.getMonth() + 1)
  currentDate.value = newDate
}

// Close date picker when clicking outside
const handleClickOutside = (event) => {
  if (!showDatePicker.value) return
  
  if (datePickerRef.value && datePickerRef.value.contains(event.target)) {
    return
  }
  
  const inputWrapper = document.querySelector(`#${inputId.value}`)?.closest('.ms-input-date__wrapper')
  if (inputWrapper && inputWrapper.contains(event.target)) {
    return
  }
  
  closeDatePicker()
}

onMounted(() => {
  document.addEventListener('click', handleClickOutside)
})

onUnmounted(() => {
  document.removeEventListener('click', handleClickOutside)
})
</script>

<style scoped>
.ms-input-date {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.ms-input-date__label {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #182026;
  line-height: 18px;
}

.ms-input-date__required {
  color: #e74c3c;
}

.ms-input-date__wrapper {
  position: relative;
}

.ms-input-date__field {
  width: 100%;
  height: 36px;
  padding: 0 36px 0 12px;
  border: 1px solid #d9d9d9;
  border-radius: 2px;
  font-family: 'Roboto', sans-serif;
  font-size: 14px;
  font-weight: 400;
  color: #182026;
  background-color: #ffffff;
  transition: all 0.2s ease;
  box-sizing: border-box;
}

.ms-input-date__field::placeholder {
  color: #bfbfbf;
  font-style: italic;
}

/* Hover state */
.ms-input-date__field:hover:not(:disabled) {
  border-color: #40a9ff;
}

/* Focus state */
.ms-input-date__field:focus {
  border-color: #1aa4c8;
  outline: none;
  background-color: #ffffff;
}

/* Disabled state */
.ms-input-date--disabled .ms-input-date__field {
  background-color: #f5f5f5;
  color: #bfbfbf;
  cursor: not-allowed;
  border-color: #d9d9d9;
}

/* Error state */
.ms-input-date--error .ms-input-date__field {
  border-color: #e74c3c;
}

.ms-input-date--error .ms-input-date__field:focus {
  border-color: #e74c3c;
}

/* Calendar icon */
.ms-input-date__icon {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  width: 18px;
  height: 18px;
  color: #090f39;
  pointer-events: none;
  /* TODO: Thêm mask-position cho icon calendar từ SVG */
  -webkit-mask-position: -287px -67px; 
  mask-position: -287px -67px; 
}

.ms-input-date__icon--clickable {
  pointer-events: auto;
  cursor: pointer;
}

.ms-input-date__icon--clickable:hover {
  color: #1aa4c8;
}

.ms-input-date__error-message {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 400;
  color: #e74c3c;
  line-height: 18px;
}

/* ============================================ */
/* DATE PICKER POPUP                            */
/* ============================================ */

.ms-date-picker-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  z-index: 9999;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  padding-top: 10px;
}

.ms-date-picker {
  background-color: #ffffff;
  border-radius: 4px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.2);
  padding: 16px;
  min-width: 280px;
  font-family: 'Roboto', sans-serif;
}

.ms-date-picker__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 16px;
}

.ms-date-picker__nav-btn {
  width: 32px;
  height: 32px;
  border: none;
  background: transparent;
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 2px;
  color: #182026;
  transition: all 0.2s ease;
}

.ms-date-picker__nav-btn:hover {
  background-color: #f0f0f0;
  color: #1aa4c8;
}

.ms-date-picker__nav-btn .icon {
  width: 16px;
  height: 16px;
}

.ms-date-picker__month-year {
  font-size: 14px;
  font-weight: 600;
  color: #182026;
}

.ms-date-picker__weekdays {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 4px;
  margin-bottom: 8px;
}

.ms-date-picker__weekday {
  text-align: center;
  font-size: 12px;
  font-weight: 600;
  color: #595959;
  padding: 8px 0;
}

.ms-date-picker__days {
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 4px;
}

.ms-date-picker__day {
  width: 36px;
  height: 36px;
  border: none;
  background: transparent;
  cursor: pointer;
  border-radius: 2px;
  font-size: 14px;
  color: #182026;
  transition: all 0.2s ease;
  display: flex;
  align-items: center;
  justify-content: center;
}

.ms-date-picker__day:hover:not(.ms-date-picker__day--other-month) {
  background-color: #e6f7ff;
  color: #1aa4c8;
}

.ms-date-picker__day--other-month {
  color: #bfbfbf;
  cursor: default;
}

.ms-date-picker__day--today {
  background-color: #f0f0f0;
  font-weight: 600;
}

.ms-date-picker__day--selected {
  background-color: #1aa4c8;
  color: #ffffff;
  font-weight: 600;
}

.ms-date-picker__day--selected:hover {
  background-color: #1590b0;
}

/* Transition */
.date-picker-fade-enter-active,
.date-picker-fade-leave-active {
  transition: opacity 0.2s ease;
}

.date-picker-fade-enter-active .ms-date-picker,
.date-picker-fade-leave-active .ms-date-picker {
  transition: transform 0.2s ease, opacity 0.2s ease;
}

.date-picker-fade-enter-from,
.date-picker-fade-leave-to {
  opacity: 0;
}

.date-picker-fade-enter-from .ms-date-picker,
.date-picker-fade-leave-to .ms-date-picker {
  transform: translateY(-10px);
  opacity: 0;
}
</style>
