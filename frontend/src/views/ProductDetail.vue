<template>
  <div class="product-detail">
    <div v-if="product" class="product-container">
      <div class="product-image">
        <img :src="productImage(product)" :alt="product.name" />
      </div>
      <div class="product-info">
        <h1>{{ product.name }}</h1>
        <p class="price">₹{{ product.price }}</p>
        <p v-if="product.comparePrice" class="compare-price">₹{{ product.comparePrice }}</p>
        <span v-if="product.discountPercent > 0" class="discount-badge">
          {{ product.discountPercent }}% OFF
        </span>
        <div class="rating">⭐ {{ product.rating }} ({{ product.reviewCount }} reviews)</div>
        <p class="stock">{{ product.inStock ? 'In Stock' : 'Out of Stock' }}</p>
        <p class="description">{{ product.description }}</p>
        <button :class="{ 'is-adding': product && cartStore.isAdding(product.id) }" @click="addToCart" :disabled="!product.inStock || cartStore.isAdding(product.id)">
          {{ cartStore.isAdding(product.id) ? 'Adding…' : product.inStock ? 'Add to Cart' : 'Out of Stock' }}
        </button>
        <button @click="toggleWishlist" class="wishlist-btn">
          {{ isInWishlist ? 'Remove from Wishlist' : 'Add to Wishlist' }}
        </button>
      </div>
    </div>

    <div class="reviews-section" v-if="product">
      <h2>Reviews</h2>
      <div v-if="reviews.length > 0" class="reviews-list">
        <div v-for="review in reviews" :key="review.id" class="review-card">
          <div class="review-header">
            <strong>{{ review.authorName }}</strong>
            <span v-if="review.verifiedPurchase" class="verified-badge">✓ Verified Purchase</span>
          </div>
          <div class="review-rating">⭐ {{ review.rating }}</div>
          <h4>{{ review.title }}</h4>
          <p>{{ review.body }}</p>
          <p class="review-date">{{ new Date(review.createdOn).toLocaleDateString() }}</p>
        </div>
      </div>
      <p v-else>No reviews yet</p>

      <div class="review-form" v-if="authStore.isAuthenticated">
        <h3>Write a Review</h3>
        <form @submit.prevent="submitReview">
          <div class="form-group">
            <label>Rating</label>
            <select v-model="reviewForm.rating" required>
              <option value="5">⭐⭐⭐⭐⭐</option>
              <option value="4">⭐⭐⭐⭐</option>
              <option value="3">⭐⭐⭐</option>
              <option value="2">⭐⭐</option>
              <option value="1">⭐</option>
            </select>
          </div>
          <div class="form-group">
            <label>Title</label>
            <input v-model="reviewForm.title" />
          </div>
          <div class="form-group">
            <label>Review</label>
            <textarea v-model="reviewForm.body" rows="4"></textarea>
          </div>
          <button type="submit">Submit Review</button>
        </form>
      </div>
      <p v-else>
        <router-link to="/login">Login to write a review</router-link>
      </p>
    </div>

    <p v-else>Loading...</p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useCatalogStore } from '../stores/catalog'
import { useCartStore } from '../stores/cart'
import { useWishlistStore } from '../stores/wishlist'
import { useAuthStore } from '../stores/auth'
import { reviewsApi } from '../api/reviews'
import type { Product } from '../types'
import { productImage } from '../utils/productImage'

const route = useRoute()
const catalogStore = useCatalogStore()
const cartStore = useCartStore()
const wishlistStore = useWishlistStore()
const authStore = useAuthStore()

const product = ref<Product | null>(null)
const reviews = ref<any[]>([])
const reviewForm = ref({ rating: 5, title: '', body: '' })

const isInWishlist = computed(() => 
  wishlistStore.items.some((item: any) => item.productId === product.value?.id)
)

async function loadProduct() {
  const id = parseInt(route.params.id as string)
  product.value = await catalogStore.fetchProduct(id)
}

async function loadReviews() {
  if (product.value) {
    const response = await reviewsApi.getProductReviews(product.value.id)
    reviews.value = response.data
  }
}

function addToCart() {
  if (product.value) {
    cartStore.addToCart(product.value.id, 1, undefined, product.value.name)
  }
}

function toggleWishlist() {
  if (product.value) {
    wishlistStore.toggleWishlist(product.value.id)
  }
}

async function submitReview() {
  if (product.value) {
    await reviewsApi.submitReview({
      productId: product.value.id,
      rating: reviewForm.value.rating,
      title: reviewForm.value.title,
      body: reviewForm.value.body
    })
    reviewForm.value = { rating: 5, title: '', body: '' }
    loadReviews()
    loadProduct() // Refresh to update rating
  }
}

onMounted(async () => {
  await loadProduct()
  await loadReviews()
  if (authStore.isAuthenticated) {
    await wishlistStore.fetchWishlist()
  }
})
</script>

<style scoped>
.product-detail {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.product-container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2rem;
}

.product-image img {
  width: 100%;
  height: 400px;
  object-fit: cover;
  border-radius: 8px;
}

.product-info h1 {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.price {
  font-size: 2rem;
  font-weight: bold;
  color: #e63946;
}

.compare-price {
  text-decoration: line-through;
  color: #999;
  font-size: 1.25rem;
}

.discount-badge {
  display: inline-block;
  background: #e63946;
  color: white;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  margin-left: 1rem;
}

.rating {
  color: #f4b400;
  margin: 1rem 0;
}

.stock {
  font-weight: bold;
  margin: 1rem 0;
}

.stock {
  color: #2ecc71;
}

.description {
  margin: 1rem 0;
  line-height: 1.6;
}

.product-info button {
  background: #e63946;
  color: white;
  border: none;
  padding: 1rem 2rem;
  cursor: pointer;
  border-radius: 4px;
  font-size: 1rem;
}

.product-info button:disabled {
  background: #ccc;
  cursor: not-allowed;
}

.wishlist-btn {
  background: #667eea;
  margin-top: 0.5rem;
}

.reviews-section {
  margin-top: 3rem;
  padding-top: 2rem;
  border-top: 1px solid #ddd;
}

.reviews-list {
  display: grid;
  gap: 1rem;
  margin-bottom: 2rem;
}

.review-card {
  background: #f5f5f5;
  padding: 1rem;
  border-radius: 8px;
}

.review-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}

.verified-badge {
  background: #2ecc71;
  color: white;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
}

.review-rating {
  color: #f4b400;
  margin: 0.5rem 0;
}

.review-card h4 {
  margin: 0.5rem 0;
}

.review-date {
  color: #666;
  font-size: 0.9rem;
  margin-top: 0.5rem;
}

.review-form {
  background: #f5f5f5;
  padding: 2rem;
  border-radius: 8px;
}

.review-form .form-group {
  margin-bottom: 1rem;
}

.review-form label {
  display: block;
  margin-bottom: 0.5rem;
}

.review-form input,
.review-form select,
.review-form textarea {
  width: 100%;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.review-form button {
  background: #e63946;
  color: white;
  border: none;
  padding: 0.75rem 2rem;
  cursor: pointer;
  border-radius: 4px;
}
</style>
