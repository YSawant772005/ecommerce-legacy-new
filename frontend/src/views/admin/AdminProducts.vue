<template>
  <div class="admin-products">
    <h1>Manage Products</h1>
    <button @click="showModal = true" class="btn-primary">Add Product</button>

    <table class="data-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Price</th>
          <th>Stock</th>
          <th>Category</th>
          <th>Brand</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="product in products" :key="product.id">
          <td>{{ product.id }}</td>
          <td>{{ product.name }}</td>
          <td>₹{{ product.price }}</td>
          <td>{{ product.stock }}</td>
          <td>{{ product.categoryName }}</td>
          <td>{{ product.brandName }}</td>
          <td>
            <button @click="editProduct(product)" class="btn-small">Edit</button>
            <button @click="deleteProduct(product.id)" class="btn-small btn-danger">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="modal" v-if="showModal">
      <div class="modal-content">
        <h2>{{ editingProduct ? 'Edit Product' : 'Add Product' }}</h2>
        <form @submit.prevent="saveProduct">
          <div class="form-group">
            <label>Name</label>
            <input v-model="form.name" required />
          </div>
          <div class="form-group">
            <label>Slug</label>
            <input v-model="form.slug" required />
          </div>
          <div class="form-group">
            <label>Price</label>
            <input v-model.number="form.price" type="number" step="0.01" required />
          </div>
          <div class="form-group">
            <label>Compare Price</label>
            <input v-model.number="form.comparePrice" type="number" step="0.01" />
          </div>
          <div class="form-group">
            <label>Stock</label>
            <input v-model.number="form.stock" type="number" required />
          </div>
          <div class="form-group">
            <label>Category</label>
            <select v-model.number="form.categoryId" required>
              <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
            </select>
          </div>
          <div class="form-group">
            <label>Brand</label>
            <select v-model.number="form.brandId" required>
              <option v-for="brand in brands" :key="brand.id" :value="brand.id">{{ brand.name }}</option>
            </select>
          </div>
          <div class="form-group">
            <label>Image URL</label>
            <input v-model="form.imageUrl" />
          </div>
          <div class="form-group">
            <label>Description</label>
            <textarea v-model="form.description" rows="3"></textarea>
          </div>
          <div class="form-group">
            <label>
              <input type="checkbox" v-model="form.isFeatured" />
              Featured
            </label>
          </div>
          <div class="form-group">
            <label>
              <input type="checkbox" v-model="form.isDeal" />
              Deal
            </label>
          </div>
          <div class="form-group">
            <label>
              <input type="checkbox" v-model="form.isNew" />
              New
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

const products = ref<any[]>([])
const categories = ref<any[]>([])
const brands = ref<any[]>([])
const showModal = ref(false)
const editingProduct = ref<any>(null)
const form = ref({
  id: 0,
  name: '',
  slug: '',
  price: 0,
  comparePrice: 0,
  stock: 0,
  categoryId: 0,
  brandId: 0,
  imageUrl: '',
  description: '',
  isFeatured: false,
  isDeal: false,
  isNew: false
})

async function loadProducts() {
  const response = await api.get('/admin/products')
  products.value = response.data.products
}

async function loadCategories() {
  const response = await api.get('/admin/categories')
  categories.value = response.data
}

async function loadBrands() {
  const response = await api.get('/admin/brands')
  brands.value = response.data
}

function editProduct(product: any) {
  editingProduct.value = product
  form.value = { ...product }
  showModal.value = true
}

async function saveProduct() {
  if (editingProduct.value) {
    await api.put(`/admin/products/${form.value.id}`, form.value)
  } else {
    await api.post('/admin/products', form.value)
  }
  closeModal()
  loadProducts()
}

async function deleteProduct(id: number) {
  if (confirm('Are you sure?')) {
    await api.delete(`/admin/products/${id}`)
    loadProducts()
  }
}

function closeModal() {
  showModal.value = false
  editingProduct.value = null
  form.value = {
    id: 0,
    name: '',
    slug: '',
    price: 0,
    comparePrice: 0,
    stock: 0,
    categoryId: 0,
    brandId: 0,
    imageUrl: '',
    description: '',
    isFeatured: false,
    isDeal: false,
    isNew: false
  }
}

onMounted(() => {
  loadProducts()
  loadCategories()
  loadBrands()
})
</script>

<style scoped>
.admin-products {
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
  max-width: 600px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
}

.form-group input,
.form-group select,
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
