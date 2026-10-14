<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-purple">CATALOGUE CONTROL</span>
          <h1 class="admin-view-title">Manage Products</h1>
        </div>
        <button @click="showModal = true" class="btn-pill btn-pill-primary">
          <span>+ Add Product</span>
        </button>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Product Name</th>
              <th>Price</th>
              <th>Stock</th>
              <th>Category</th>
              <th>Brand</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="product in products" :key="product.id">
              <td><strong>#{{ product.id }}</strong></td>
              <td>{{ product.name }}</td>
              <td>₹{{ product.price.toLocaleString('en-IN') }}</td>
              <td>
                <span class="badge-pill" :class="product.stock > 5 ? 'badge-lime' : 'badge-discount'">
                  {{ product.stock }} units
                </span>
              </td>
              <td>{{ product.categoryName }}</td>
              <td>{{ product.brandName }}</td>
              <td>
                <button @click="editProduct(product)" class="admin-action-btn">Edit</button>
                <button @click="deleteProduct(product.id)" class="admin-action-btn delete">Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Add/Edit Product Modal -->
      <div class="admin-modal-backdrop" v-if="showModal" @click.self="closeModal">
        <div class="admin-modal-window">
          <div class="admin-modal-header">
            <h2>{{ editingProduct ? 'Edit Product' : 'Add New Product' }}</h2>
            <button class="admin-action-btn" @click="closeModal">✕</button>
          </div>

          <form @submit.prevent="saveProduct">
            <div class="admin-form-group">
              <label>Product Name</label>
              <input v-model="form.name" required placeholder="e.g. Wireless Noise-Cancelling Headphones" />
            </div>

            <div class="admin-form-group">
              <label>Slug URL Identifier</label>
              <input v-model="form.slug" required placeholder="e.g. wireless-headphones" />
            </div>

            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
              <div class="admin-form-group">
                <label>Price (₹)</label>
                <input v-model.number="form.price" type="number" step="0.01" required />
              </div>

              <div class="admin-form-group">
                <label>Compare Price (₹)</label>
                <input v-model.number="form.comparePrice" type="number" step="0.01" />
              </div>
            </div>

            <div class="admin-form-group">
              <label>Inventory Stock Units</label>
              <input v-model.number="form.stock" type="number" required />
            </div>

            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
              <div class="admin-form-group">
                <label>Category</label>
                <select v-model.number="form.categoryId" required>
                  <option v-for="cat in categories" :key="cat.id" :value="cat.id">{{ cat.name }}</option>
                </select>
              </div>

              <div class="admin-form-group">
                <label>Brand</label>
                <select v-model.number="form.brandId" required>
                  <option v-for="brand in brands" :key="brand.id" :value="brand.id">{{ brand.name }}</option>
                </select>
              </div>
            </div>

            <div class="admin-form-group">
              <label>Image URL</label>
              <input v-model="form.imageUrl" placeholder="https://..." />
            </div>

            <div class="admin-form-group">
              <label>Description</label>
              <textarea v-model="form.description" rows="3"></textarea>
            </div>

            <div style="display: flex; gap: 1.5rem; margin: 1rem 0;">
              <label style="display: flex; align-items: center; gap: 0.5rem; font-size: 0.88rem; cursor: pointer;">
                <input type="checkbox" v-model="form.isFeatured" />
                Featured
              </label>
              <label style="display: flex; align-items: center; gap: 0.5rem; font-size: 0.88rem; cursor: pointer;">
                <input type="checkbox" v-model="form.isDeal" />
                Deal of the Day
              </label>
              <label style="display: flex; align-items: center; gap: 0.5rem; font-size: 0.88rem; cursor: pointer;">
                <input type="checkbox" v-model="form.isNew" />
                New Arrival
              </label>
            </div>

            <div class="admin-form-actions">
              <button type="button" @click="closeModal" class="btn-pill btn-pill-outline">Cancel</button>
              <button type="submit" class="btn-pill btn-pill-primary">Save Product</button>
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
  if (confirm('Are you sure you want to delete this product?')) {
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
