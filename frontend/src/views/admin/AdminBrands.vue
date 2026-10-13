<template>
  <div class="admin-brands">
    <h1>Manage Brands</h1>
    <button @click="showModal = true" class="btn-primary">Add Brand</button>

    <table class="data-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Slug</th>
          <th>Code</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="brand in brands" :key="brand.id">
          <td>{{ brand.id }}</td>
          <td>{{ brand.name }}</td>
          <td>{{ brand.slug }}</td>
          <td>{{ brand.code }}</td>
          <td>
            <button @click="editBrand(brand)" class="btn-small">Edit</button>
            <button @click="deleteBrand(brand.id)" class="btn-small btn-danger">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="modal" v-if="showModal">
      <div class="modal-content">
        <h2>{{ editingBrand ? 'Edit Brand' : 'Add Brand' }}</h2>
        <form @submit.prevent="saveBrand">
          <div class="form-group">
            <label>Name</label>
            <input v-model="form.name" required />
          </div>
          <div class="form-group">
            <label>Slug</label>
            <input v-model="form.slug" required />
          </div>
          <div class="form-group">
            <label>Code</label>
            <input v-model="form.code" />
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
  if (confirm('Are you sure?')) {
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

<style scoped>
.admin-brands {
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
  max-width: 400px;
  width: 90%;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
}

.form-group input {
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
