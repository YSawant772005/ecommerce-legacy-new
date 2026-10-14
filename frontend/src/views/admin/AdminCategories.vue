<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-purple">TAXONOMY CONTROL</span>
          <h1 class="admin-view-title">Manage Categories</h1>
        </div>
        <button @click="showModal = true" class="btn-pill btn-pill-primary">
          <span>+ Add Category</span>
        </button>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Name</th>
              <th>Slug</th>
              <th>Tagline</th>
              <th>Order</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="category in categories" :key="category.id">
              <td><strong>#{{ category.id }}</strong></td>
              <td><strong>{{ category.name }}</strong></td>
              <td>{{ category.slug }}</td>
              <td>{{ category.tagline || '—' }}</td>
              <td>{{ category.displayOrder }}</td>
              <td>
                <span class="badge-pill" :class="category.isActive ? 'badge-lime' : 'badge-discount'">
                  {{ category.isActive ? 'Active' : 'Inactive' }}
                </span>
              </td>
              <td>
                <button @click="editCategory(category)" class="admin-action-btn">Edit</button>
                <button @click="deleteCategory(category.id)" class="admin-action-btn delete">Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Modal -->
      <div class="admin-modal-backdrop" v-if="showModal" @click.self="closeModal">
        <div class="admin-modal-window">
          <div class="admin-modal-header">
            <h2>{{ editingCategory ? 'Edit Category' : 'Add Category' }}</h2>
            <button class="admin-action-btn" @click="closeModal">✕</button>
          </div>

          <form @submit.prevent="saveCategory">
            <div class="admin-form-group">
              <label>Category Name</label>
              <input v-model="form.name" required placeholder="e.g. Modern Audio" />
            </div>

            <div class="admin-form-group">
              <label>Slug</label>
              <input v-model="form.slug" required placeholder="e.g. modern-audio" />
            </div>

            <div class="admin-form-group">
              <label>Tagline</label>
              <input v-model="form.tagline" placeholder="Short descriptive tagline" />
            </div>

            <div class="admin-form-group">
              <label>Description</label>
              <textarea v-model="form.description" rows="3"></textarea>
            </div>

            <div class="admin-form-group">
              <label>Image URL</label>
              <input v-model="form.imageUrl" placeholder="https://..." />
            </div>

            <div class="admin-form-group">
              <label>Display Order</label>
              <input v-model.number="form.displayOrder" type="number" />
            </div>

            <div class="admin-form-group">
              <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer;">
                <input type="checkbox" v-model="form.isActive" />
                Active on Storefront
              </label>
            </div>

            <div class="admin-form-actions">
              <button type="button" @click="closeModal" class="btn-pill btn-pill-outline">Cancel</button>
              <button type="submit" class="btn-pill btn-pill-primary">Save Category</button>
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

const categories = ref<any[]>([])
const showModal = ref(false)
const editingCategory = ref<any>(null)
const form = ref({
  id: 0,
  name: '',
  slug: '',
  tagline: '',
  description: '',
  imageUrl: '',
  displayOrder: 0,
  isActive: true
})

async function loadCategories() {
  const response = await api.get('/admin/categories')
  categories.value = response.data
}

function editCategory(category: any) {
  editingCategory.value = category
  form.value = { ...category }
  showModal.value = true
}

async function saveCategory() {
  if (editingCategory.value) {
    await api.put(`/admin/categories/${form.value.id}`, form.value)
  } else {
    await api.post('/admin/categories', form.value)
  }
  closeModal()
  loadCategories()
}

async function deleteCategory(id: number) {
  if (confirm('Are you sure you want to delete this category?')) {
    await api.delete(`/admin/categories/${id}`)
    loadCategories()
  }
}

function closeModal() {
  showModal.value = false
  editingCategory.value = null
  form.value = {
    id: 0,
    name: '',
    slug: '',
    tagline: '',
    description: '',
    imageUrl: '',
    displayOrder: 0,
    isActive: true
  }
}

onMounted(() => {
  loadCategories()
})
</script>
