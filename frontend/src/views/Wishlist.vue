<template>
  <div class="wishlist">
    <h1>My Wishlist</h1>

    <div v-if="!wishlistStore.loading && wishlistStore.items.length > 0" class="grid">
      <div v-for="item in wishlistStore.items" :key="item.productId" class="card">
        <img v-if="item.imageUrl" :src="item.imageUrl" :alt="item.productName" />
        <h3>{{ item.productName }}</h3>
        <p class="price">₹{{ item.price }}</p>
        <button @click="addToCart(item.productId)">Add to Cart</button>
        <button @click="removeItem(item.productId)" class="remove-btn">Remove</button>
      </div>
    </div>

    <p v-else-if="!wishlistStore.loading">Your wishlist is empty</p>
    <p v-else>Loading...</p>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useWishlistStore } from '../stores/wishlist'
import { useCartStore } from '../stores/cart'

const wishlistStore = useWishlistStore()
const cartStore = useCartStore()

function addToCart(productId: number) {
  cartStore.addToCart(productId)
}

function removeItem(productId: number) {
  wishlistStore.removeFromWishlist(productId)
}

onMounted(() => {
  wishlistStore.fetchWishlist()
})
</script>

<style scoped>
.wishlist {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
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

.remove-btn {
  background: #666;
  margin-top: 0.5rem;
}
</style>
