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
  min-height: calc(100vh - 100px);
  padding: 4rem max(1.5rem, calc((100% - 1120px) / 2));
  margin: 0 auto;
  color: #24145f;
  background: #fbfaff;
}

.wishlist h1 { margin: 0 0 .55rem; font-size: clamp(2.7rem, 5vw, 4.5rem); letter-spacing: -.08em; }
.wishlist > p { color: #8b8499; }
.grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1rem;
}

.card {
  overflow: hidden;
  border: 1px solid #e8e3f1;
  border-radius: 14px;
  background: #fff;
  box-shadow: 0 8px 24px rgba(36,20,95,.05);
}

.card img {
  width: 100%;
  height: 220px;
  object-fit: cover;
}

.card h3, .card p { padding: 0 1rem; }
.card h3 { min-height: 2.5rem; margin: 1rem 0 .25rem; font-size: .92rem; }
.price {
  font-size: 1.05rem;
  font-weight: bold;
  color: #24145f;
}

.card button {
  background: #281568;
  color: white;
  border: none;
  padding: .65rem 1rem;
  cursor: pointer;
  border-radius: 8px;
  margin: .4rem 1rem 1rem;
  width: 100%;
}

.remove-btn {
  background: #f0edf5 !important;
  color: #6c647c !important;
  margin-top: 0 !important;
}

@media (max-width: 850px) { .grid { grid-template-columns: repeat(2, 1fr); } }
@media (max-width: 560px) { .grid { grid-template-columns: 1fr; } }
</style>
