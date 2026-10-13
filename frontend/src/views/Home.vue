<template>
  <div class="home">
    <div class="hero">
      <h1>Welcome to NovaKart</h1>
      <p>Modern E-commerce Platform</p>
      <router-link to="/products" class="btn">Shop Now</router-link>
    </div>

    <div class="section">
      <h2>Categories</h2>
      <div class="grid" v-if="!catalogStore.loading">
        <div v-for="category in catalogStore.categories" :key="category.id" class="card">
          <img v-if="category.imageUrl" :src="category.imageUrl" :alt="category.name" />
          <h3>{{ category.name }}</h3>
          <p>{{ category.tagline }}</p>
          <router-link :to="`/products?categoryId=${category.id}`">View Products</router-link>
        </div>
      </div>
      <p v-else>Loading...</p>
    </div>

    <div class="section">
      <h2>Featured Products</h2>
      <div class="grid" v-if="!catalogStore.loading">
        <div v-for="product in featuredProducts" :key="product.id" class="card">
          <img v-if="product.imageUrl" :src="product.imageUrl" :alt="product.name" />
          <h3>{{ product.name }}</h3>
          <p class="price">₹{{ product.price }}</p>
          <p v-if="product.comparePrice" class="compare-price">₹{{ product.comparePrice }}</p>
          <button @click="addToCart(product.id)">Add to Cart</button>
        </div>
      </div>
      <p v-else>Loading...</p>
    </div>

    <Newsletter />
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted } from 'vue'
import { useCatalogStore } from '../stores/catalog'
import { useCartStore } from '../stores/cart'
import Newsletter from './Newsletter.vue'

const catalogStore = useCatalogStore()
const cartStore = useCartStore()

const featuredProducts = computed(() => 
  catalogStore.products.filter(p => p.isFeatured).slice(0, 8)
)

function addToCart(productId: number) {
  cartStore.addToCart(productId)
}

onMounted(() => {
  catalogStore.fetchCategories()
  catalogStore.fetchProducts({ pageSize: 24 })
})
</script>

<style scoped>
.hero {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 4rem 2rem;
  text-align: center;
}

.hero h1 {
  font-size: 3rem;
  margin-bottom: 1rem;
}

.btn {
  display: inline-block;
  background: white;
  color: #667eea;
  padding: 0.75rem 2rem;
  text-decoration: none;
  border-radius: 4px;
  margin-top: 1rem;
}

.section {
  padding: 3rem 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 2rem;
  margin-top: 2rem;
}

.card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1rem;
  text-align: center;
}

.card img {
  width: 100%;
  height: 200px;
  object-fit: cover;
  border-radius: 4px;
}

.price {
  font-size: 1.25rem;
  font-weight: bold;
  color: #e63946;
}

.compare-price {
  text-decoration: line-through;
  color: #999;
}

.card button {
  background: #e63946;
  color: white;
  border: none;
  padding: 0.5rem 1rem;
  cursor: pointer;
  border-radius: 4px;
  margin-top: 1rem;
}
</style>
