<template>
  <div class="products">
    <h1>Products</h1>

    <div class="filters">
      <select v-model="selectedCategory" @change="loadProducts">
        <option :value="null">All Categories</option>
        <option v-for="category in catalogStore.categories" :key="category.id" :value="category.id">
          {{ category.name }}
        </option>
      </select>

      <input v-model="searchQuery" placeholder="Search products..." @input="loadProducts" />

      <select v-model="sortBy" @change="loadProducts">
        <option value="popular">Popular</option>
        <option value="price-asc">Price: Low to High</option>
        <option value="price-desc">Price: High to Low</option>
        <option value="rating">Rating</option>
        <option value="new">Newest</option>
      </select>
    </div>

    <div class="grid" v-if="!catalogStore.loading">
      <div v-for="product in catalogStore.products" :key="product.id" class="card">
        <img v-if="product.imageUrl" :src="product.imageUrl" :alt="product.name" />
        <h3>{{ product.name }}</h3>
        <p class="category">{{ product.categoryName }}</p>
        <p class="price">₹{{ product.price }}</p>
        <p v-if="product.comparePrice" class="compare-price">₹{{ product.comparePrice }}</p>
        <span v-if="product.discountPercent > 0"
          class="discount-badge">{{ product.discountPercent }}% OFF</span>
        <div class="rating">
          ⭐ {{ product.rating }} ({{ product.reviewCount }})
        </div>
        <button @click="addToCart(product.id)" :disabled="!product.inStock">
          {{ product.inStock ? 'Add to Cart' : 'Out of Stock' }}
        </button>
        <router-link :to="`/products/${product.id}`" class="view-link">View Details</router-link>
      </div>
    </div>
    <p v-else>Loading...</p>

    <div class="pagination" v-if="catalogStore.totalPages > 1">
      <button @click="prevPage" :disabled="currentPage === 1">Previous</button>
      <span>Page {{ currentPage }} of {{ catalogStore.totalPages }}</span>
      <button @click="nextPage" :disabled="currentPage === catalogStore.totalPages">Next</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useCatalogStore } from '../stores/catalog'
import { useCartStore } from '../stores/cart'

const catalogStore = useCatalogStore()
const cartStore = useCartStore()

const selectedCategory = ref<number | null>(null)
const searchQuery = ref('')
const sortBy = ref('popular')
const currentPage = ref(1)

function loadProducts() {
  catalogStore.fetchProducts({
    page: currentPage.value,
    pageSize: 24,
    categoryId: selectedCategory.value || undefined,
    search: searchQuery.value || undefined,
    sort: sortBy.value
  })
}

function addToCart(productId: number) {
  cartStore.addToCart(productId)
}

function prevPage() {
  if (currentPage.value > 1) {
    currentPage.value--
    loadProducts()
  }
}

function nextPage() {
  if (currentPage.value < catalogStore.totalPages) {
    currentPage.value++
    loadProducts()
  }
}

onMounted(() => {
  catalogStore.fetchCategories()
  loadProducts()
})
</script>

<style scoped>
.products {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.filters {
  display: flex;
  gap: 1rem;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.filters select,
.filters input {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 2rem;
}

.card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1rem;
  text-align: center;
  position: relative;
}

.card img {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 4px;
}

.category {
  color: #666;
  font-size: 0.9rem;
}

.price {
  font-size: 1.25rem;
  font-weight: bold;
  color: #e63946;
}

.compare-price {
  text-decoration: line-through;
  color: #999;
  font-size: 0.9rem;
}

.discount-badge {
  position: absolute;
  top: 1rem;
  right: 1rem;
  background: #e63946;
  color: white;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
}

.rating {
  color: #f4b400;
  margin: 0.5rem 0;
}

.card button {
  background: #e63946;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  cursor: pointer;
  border-radius: 4px;
  margin-top: 1rem;
  width: 100%;
}

.card button:disabled {
  background: #ccc;
  cursor: not-allowed;
}

.view-link {
  display: block;
  margin-top: 0.5rem;
  color: #667eea;
  text-decoration: none;
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  margin-top: 2rem;
}

.pagination button {
  padding: 0.5rem 1rem;
  border: 1px solid #ddd;
  background: white;
  cursor: pointer;
  border-radius: 4px;
}

.pagination button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
