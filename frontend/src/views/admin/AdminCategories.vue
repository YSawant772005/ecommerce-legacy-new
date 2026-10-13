<template>
  <div class="admin-categories">
    <h1>Manage Categories</h1>
    <button @click="showModal = true" class="-btn-primary">Add Category</button>

    <table class="data-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Slug</th>
          <th>Tagline</th>
          <th>Order</th>
          <th>Active</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="category in categories" :key="category.id">
          <td>{{ category.id }}</td>
          <td>{{ category.name }}</td>
          <td>{{ category.slug }}</td>
          <td>{{ category.tagline }}</td>
          <td>{{ category.displayOrder }}</td>
          <td>{{ category.isActive ? 'Yes' : 'No' }}</td>
          <td>
            <button @click="editCategory(category)" class="btn-small">Edit</button>
            <button @click="deleteCategory(category.id)" class="btn-small btn-danger">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="modal" v-if="showModal">
      <div class="modal-content">
        <h2>{{ editingCategory ? 'Edit Category' : 'Add Category' }}</h2>
        <form @submit.prevent="saveCategory">
          <div class="form-group">
            <label>Name</label>
            <input v-model="form.name" required />
          </div>
          <div class="form-group">
            <label>Slug</label>
            <input v-model="form.slug" required />
          </div>
          <div class="form-group">
            <label>Tagline</label>
            <input v-model="form.tagline" />
          </div>
          <div class="form-group">
            <label>Description</label>
            <textarea v-model="form.description" rows="3"></textarea>
          </div>
          <div class="form-group">
            <label>Image URL</label>
            <input v-model="form.imageUrl" />
          </div>
          <div class="form-group">
            <label>Display Order</label>
            <input v-model.number="form.displayOrder" type="number" />
          </div>
          <div class="form-group">
            <label>
              <input type="checkbox" v-model="form.isActive" />
              Active
            </label>
          </div>
          <div class="form-actions">
            <button type="submit">Save</button>
            <button type="button" @click="closeModal">Cancel</button>
          </div>
        </form>
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
  if (confirm('Are you sure?')) {
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

<style scoped>
.admin-categories {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

.btn-primary {
  background: #e63946;
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
}

.data-table th,
.data-table td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.data-table th {
  background: #667eea;
  color: white;
}

.btn-small {
  padding: 0.25rem 0.5rem;
  margin-right: 0.5rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

.btn-danger {
  background: #e63946;
  color: white;
}

.modal {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-content {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  max-width: 500px;
  width: 90%;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.form-actions {
  display: flex;
  gap: 1rem;
  margin-top: 1rem;
}

.form-actions button {
  padding: 0.75rem 1.5rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}
</style>
