<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-purple">BRAND REGISTRY</span>
          <h1 class="admin-view-title">Manage Brands</h1>
        </div>
        <button @click="showModal = true" class="btn-pill btn-pill-primary">
          <span>+ Add Brand</span>
        </button>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Brand Name</th>
              <th>Slug</th>
              <th>Internal Code</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="brand in brands" :key="brand.id">
              <td><strong>#{{ brand.id }}</strong></td>
              <td><strong>{{ brand.name }}</strong></td>
              <td>{{ brand.slug }}</td>
              <td><code>{{ brand.code || '—' }}</code></td>
              <td>
                <button @click="editBrand(brand)" class="admin-action-btn">Edit</button>
                <button @click="deleteBrand(brand.id)" class="admin-action-btn delete">Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Modal -->
      <div class="admin-modal-backdrop" v-if="showModal" @click.self="closeModal">
        <div class="admin-modal-window">
          <div class="admin-modal-header">
            <h2>{{ editingBrand ? 'Edit Brand' : 'Add Brand' }}</h2>
            <button class="admin-action-btn" @click="closeModal">✕</button>
          </div>

          <form @submit.prevent="saveBrand">
            <div class="admin-form-group">
              <label>Brand Name</label>
              <input v-model="form.name" required placeholder="e.g. Bang & Olufsen" />
            </div>

            <div class="admin-form-group">
              <label>Slug</label>
              <input v-model="form.slug" required placeholder="e.g. bang-olufsen" />
            </div>

            <div class="admin-form-group">
              <label>Brand Code</label>
              <input v-model="form.code" placeholder="e.g. BO-01" />
            </div>

            <div class="admin-form-actions">
              <button type="button" @click="closeModal" class="btn-pill btn-pill-outline">Cancel</button>
              <button type="submit" class="btn-pill btn-pill-primary">Save Brand</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../../api/client'

const brands = ref<any[]>([])
const showModal = ref(false)
const editingBrand = ref<any>(null)
const form = ref({
  id: 0,
  name: '',
  slug: '',
  code: ''
})

async function loadBrands() {
  const response = await api.get('/admin/brands')
  brands.value = response.data
}

function editBrand(brand: any) {
  editingBrand.value = brand
  form.value = { ...brand }
  showModal.value = true
}

async function saveBrand() {
  if (editingBrand.value) {
    await api.put(`/admin/brands/${form.value.id}`, form.value)
  } else {
    await api.post('/admin/brands', form.value)
  }
  closeModal()
  loadBrands()
}

async function deleteBrand(id: number) {
  if (confirm('Are you sure you want to delete this brand?')) {
    await api.delete(`/admin/brands/${id}`)
    loadBrands()
  }
}

function closeModal() {
  showModal.value = false
  editingBrand.value = null
  form.value = { id: 0, name: '', slug: '', code: '' }
}

onMounted(() => {
  loadBrands()
})
</script>
