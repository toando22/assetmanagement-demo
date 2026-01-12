<!--
  Mô tả: Demo page cho tất cả components
  
  CreatedBy: DDToan - (09/1/2026)
-->

<template>
  <div class="component-demo">
    <div class="demo-container">
      <h1>Component Library</h1>

      <!-- Search Component -->
      <section class="demo-section">
        <h2>Search</h2>
        <div class="demo-grid">
          <div class="demo-item">
            <label>Default</label>
            <MsSearch v-model="searchValue" placeholder="Tìm kiếm tài sản" />
          </div>
          <div class="demo-item">
            <label>Hover</label>
            <MsSearch v-model="searchValue" placeholder="Tìm kiếm tài sản" />
          </div>
          <div class="demo-item">
            <label>Active/Focused</label>
            <MsSearch v-model="searchActiveValue" placeholder="I am searching" />
          </div>
          <div class="demo-item">
            <label>Disable</label>
            <MsSearch v-model="searchValue" placeholder="Tìm kiếm tài sản" :disabled="true" />
          </div>
        </div>
      </section>

      <!-- Button Component -->
      <section class="demo-section">
        <h2>Button</h2>
        
        <div class="demo-subsection">
          <h3>Main button</h3>
          <div class="demo-row">
            <MsButton variant="primary">Button</MsButton>
            <MsButton variant="primary">Button</MsButton>
            <MsButton variant="primary">Button</MsButton>
            <MsButton variant="primary">Button</MsButton>
            <MsButton variant="primary" :disabled="true">Button</MsButton>
          </div>
          <div class="demo-labels">
            <span>Default</span>
            <span>Hover</span>
            <span>Active</span>
            <span>Focus</span>
            <span>Disable</span>
          </div>
        </div>

        <div class="demo-subsection">
          <h3>Sub button</h3>
          <div class="demo-row">
            <MsButton variant="secondary">Button</MsButton>
            <MsButton variant="secondary">Button</MsButton>
            <MsButton variant="secondary">Button</MsButton>
            <MsButton variant="secondary">Button</MsButton>
            <MsButton variant="secondary" :disabled="true">Button</MsButton>
          </div>
        </div>

        <div class="demo-subsection">
          <h3>Outline button</h3>
          <div class="demo-row">
            <MsButton variant="outline">Button</MsButton>
            <MsButton variant="outline">Button</MsButton>
            <MsButton variant="outline">Button</MsButton>
            <MsButton variant="outline">Button</MsButton>
            <MsButton variant="outline" :disabled="true">Button</MsButton>
          </div>
        </div>
      </section>

      <!-- Dropdown Component -->
      <section class="demo-section">
        <h2>Dropdown</h2>
        <div class="demo-grid">
          <div class="demo-item">
            <label>Regular Dropdown</label>
            <MsDropdown 
              v-model="selectedDropdown1"
              :options="dropdownOptions"
              placeholder="Roboto - 14pt - Regular"
            />
          </div>
          <div class="demo-item">
            <label>Dropdown Open</label>
            <MsDropdown 
              v-model="selectedDropdown2"
              :options="dropdownOptions"
              placeholder="Drilldown"
            />
          </div>
          <div class="demo-item">
            <label>With Checkbox</label>
            <MsDropdown 
              v-model="selectedCheckboxes"
              :options="dropdownOptions"
              :with-checkbox="true"
              placeholder="Drilldown"
            />
          </div>
          <div class="demo-item">
            <label>Table Dropdown</label>
            <MsDropdown 
              v-model="selectedTable"
              :options="tableOptions"
              :with-table="true"
              placeholder="Select item"
            />
          </div>
        </div>
      </section>

      <!-- Text field Component -->
      <section class="demo-section">
        <h2>Text field</h2>
        <div class="demo-grid">
          <div class="demo-item">
            <label>Default</label>
            <MsInput v-model="inputValue" placeholder="Roboto - 14pt - Regular" />
          </div>
          <div class="demo-item">
            <label>Hover</label>
            <MsInput v-model="inputValue" placeholder="Roboto - 14pt - Regular" />
          </div>
          <div class="demo-item">
            <label>Active</label>
            <MsInput v-model="inputActiveValue" placeholder="Roboto - 14pt - Regular" />
          </div>
          <div class="demo-item">
            <label>Focus</label>
            <MsInput v-model="inputFocusValue" placeholder="Roboto - 14pt - Regular" />
          </div>
          <div class="demo-item">
            <label>Disable</label>
            <MsInput v-model="inputValue" placeholder="Roboto - 14pt - Regular" :disabled="true" />
          </div>
          <div class="demo-item">
            <label>Error</label>
            <MsInput 
              v-model="inputErrorValue" 
              placeholder="Roboto - 14pt - Regular" 
              :error="true"
              error-message="Error message"
            />
          </div>
        </div>
      </section>
    </div>
  </div>
</template>

<script setup>
/*
  Mô tả: Script setup cho ComponentDemo view
  
  CreatedBy: DDToan - (09/1/2026)
*/

import { ref } from 'vue'
import { MsInput, MsButton, MsSearch, MsDropdown } from '@/components'

// Search states
const searchValue = ref('')
const searchActiveValue = ref('I am searching')

// Button states
const buttonState = ref('default')

// Dropdown states
const selectedDropdown1 = ref('')
const selectedDropdown2 = ref('1')
const selectedCheckboxes = ref([])
const selectedTable = ref('')

const dropdownOptions = ref([
  { value: '1', label: 'Drilldown' },
  { value: '2', label: 'Drilldown' },
  { value: '3', label: 'Drilldown' },
  { value: '4', label: 'Drilldown' },
  { value: '5', label: 'Drilldown' },
  { value: '6', label: 'Drilldown' }
])

const tableOptions = ref([
  { value: '1', label: 'Drilldown' },
  { value: '2', label: 'Drilldown' },
  { value: '3', label: 'Drilldown' },
  { value: '4', label: 'Drilldown' },
  { value: '5', label: 'Drilldown' },
  { value: '6', label: 'Drilldown' }
])

// Text field states
const inputValue = ref('')
const inputActiveValue = ref('Roboto - 14pt - Regular')
const inputFocusValue = ref('Roboto - 14pt - Regular')
const inputErrorValue = ref('Roboto - 14pt - Regular')
</script>

<style scoped>
.component-demo {
  min-height: 100vh;
  background-color: #f0f2f5;
  padding: 24px;
}

.demo-container {
  max-width: 1200px;
  margin: 0 auto;
}

h1 {
  font-family: 'Roboto', sans-serif;
  font-size: 32px;
  font-weight: 700;
  color: #182026;
  margin-bottom: 32px;
}

.demo-section {
  background-color: #ffffff;
  border-radius: 4px;
  padding: 24px;
  margin-bottom: 24px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

h2 {
  font-family: 'Roboto', sans-serif;
  font-size: 24px;
  font-weight: 700;
  color: #182026;
  margin-bottom: 16px;
  border-bottom: 2px solid #019160;
  padding-bottom: 8px;
}

h3 {
  font-family: 'Roboto', sans-serif;
  font-size: 16px;
  font-weight: 600;
  color: #182026;
  margin-bottom: 12px;
}

.demo-subsection {
  margin-bottom: 24px;
}

.demo-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 16px;
}

.demo-item {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.demo-item > label {
  font-family: 'Roboto', sans-serif;
  font-size: 13px;
  font-weight: 600;
  color: #595959;
}

.demo-row {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
}

.demo-labels {
  display: flex;
  gap: 12px;
  margin-top: 8px;
  flex-wrap: wrap;
}

.demo-labels span {
  font-family: 'Roboto', sans-serif;
  font-size: 12px;
  color: #595959;
  width: 80px;
  text-align: center;
}
</style>
