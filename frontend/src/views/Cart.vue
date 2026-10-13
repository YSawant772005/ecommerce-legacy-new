<template>
  <div class="cart">
    <p class="eyebrow">YOUR NOVAKART BAG</p><h1>Ready when<br><span>you are.</span></h1>

    <div v-if="!cartStore.loading && cartStore.cart && cartStore.cart.items.length > 0">
      <div class="cart-items">
        <div v-for="item in cartStore.cart.items" :key="item.productId" class="cart-item">
          <img
            :src="productImage({ id: item.productId, name: item.productName, imageUrl: item.imageUrl })"
            :alt="item.productName"
            @error="useFallbackImage"
          />
          <div class="item-details">
            <h3>{{ item.productName }}</h3>
            <p>₹{{ item.unitPrice }}</p>
            <div class="quantity-controls">
              <button @click="updateQty(item.productId, item.qty - 1)">-</button>
              <span>{{ item.qty }}</span>
              <button @click="updateQty(item.productId, item.qty + 1)">+</button>
            </div>
          </div>
          <div class="item-total">
            <p>₹{{ item.lineTotal }}</p>
            <button @click="removeItem(item.productId)" class="remove-btn">Remove</button>
          </div>
        </div>
      </div>

      <div class="cart-summary">
        <h2>Order Summary</h2>
        <div class="summary-row">
          <span>Subtotal:</span>
          <span>₹{{ cartStore.cart.subtotal }}</span>
        </div>
        <div class="summary-row" v-if="cartStore.cart.discount > 0">
          <span>Discount:</span>
          <span>-₹{{ cartStore.cart.discount }}</span>
        </div>
        <div class="summary-row">
          <span>Shipping:</span>
          <span>{{ cartStore.cart.shipping === 0 ? 'FREE' : '₹' + cartStore.cart.shipping }}</span>
        </div>
        <div class="summary-row">
          <span>Tax (18%):</span>
          <span>₹{{ cartStore.cart.tax }}</span>
        </div>
        <div class="summary-row total">
          <span>Total:</span>
          <span>₹{{ cartStore.cart.total }}</span>
        </div>

        <div class="coupon-section">
          <input v-model="couponCode" placeholder="Enter coupon code" />
          <button @click="applyCoupon">Apply Coupon</button>
        </div>

        <router-link to="/checkout" class="checkout-btn">Proceed to Checkout</router-link>
      </div>
    </div>

    <div v-else-if="!cartStore.loading" class="empty-cart">
      <div class="empty-mark">NK</div><h2>Your bag is waiting</h2><p>Explore our latest edit and find something worth keeping.</p>
      <router-link to="/products" class="btn">Continue Shopping →</router-link>
    </div>

    <p v-else>Loading cart...</p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useCartStore } from '../stores/cart'
import { productImage } from '../utils/productImage'

const cartStore = useCartStore()
const couponCode = ref('')
const fallbackImage = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" width="400" height="400" viewBox="0 0 400 400"%3E%3Crect width="400" height="400" fill="%23eeeaf7"/%3E%3Ctext x="200" y="210" text-anchor="middle" font-family="Arial" font-size="28" fill="%2324145f"%3ENovaKart%3C/text%3E%3C/svg%3E'

function useFallbackImage(event: Event) {
  const image = event.target as HTMLImageElement
  if (image.src !== fallbackImage) image.src = fallbackImage
}

function updateQty(productId: number, qty: number) {
  if (qty <= 0) {
    cartStore.removeCartItem(productId)
  } else {
    cartStore.updateCartItem(productId, qty)
  }
}

function removeItem(productId: number) {
  cartStore.removeCartItem(productId)
}

function applyCoupon() {
  if (couponCode.value) {
    cartStore.applyCoupon(couponCode.value)
  }
}

onMounted(() => {
  cartStore.fetchCart()
})
</script>

<style scoped>
.cart {
  padding: 4rem max(1.5rem, calc((100% - 1120px) / 2));
  margin: 0 auto;
  color:#24145f; background:#fbfaff; min-height:calc(100vh - 100px);
}
.eyebrow { color:#9654db; font-size:.68rem; font-weight:800; letter-spacing:.18em; }
.cart h1 { font-size:clamp(2.8rem,6vw,5rem); line-height:.9; letter-spacing:-.08em; margin:.5rem 0 3rem; }
.cart h1 span { color:#9654db; }
.cart > div:first-of-type { display:grid; grid-template-columns:1.4fr .8fr; gap:2rem; align-items:start; }

.cart-items {
  display: grid;
  gap: 1rem;
  margin-bottom: 2rem;
}

.cart-item {
  display: flex;
  gap: 1rem;
  padding: 1rem;
  border: 1px solid #e8e3f1; border-radius:16px; background:#fff;
  align-items: center;
}

.cart-item img {
  width: 100px;
  height: 100px;
  object-fit: cover;
  border-radius: 4px;
}

.item-details {
  flex: 1;
}

.quantity-controls {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  margin-top: 0.5rem;
}

.quantity-controls button {
  padding: 0.25rem 0.5rem;
  border: 1px solid #ddd;
  background: white;
  cursor: pointer;
  border-radius: 4px;
}

.item-total {
  text-align: right;
}

.remove-btn {
  background: #e63946;
  color: white;
  border: none;
  padding: 0.25rem 0.5rem;
  cursor: pointer;
  border-radius: 4px;
  margin-top: 0.5rem;
}

.cart-summary {
  background:#24145f; color:white;
  padding: 2rem;
  border-radius: 8px;
}

.summary-row {
  display: flex;
  justify-content: space-between;
  margin-bottom: 1rem;
}

.summary-row.total {
  font-size: 1.25rem;
  font-weight: bold;
  border-top: 1px solid #ddd;
  padding-top: 1rem;
}
.summary-row { color:rgba(255,255,255,.8); }
.summary-row.total { color:#fff; }

.coupon-section {
  display: flex;
  gap: 0.5rem;
  margin: 1rem 0;
}
.coupon-section input { flex:1; min-width:0; border:0; padding:.8rem; border-radius:999px; }
.coupon-section button, .checkout-btn, .btn { border:0; background:#0fe1d1; color:#24145f; padding:.8rem 1.1rem; border-radius:999px; font-weight:800; text-decoration:none; cursor:pointer; }
.checkout-btn { display:block; text-align:center; margin-top:1.2rem; }
.empty-cart { text-align:center; padding:5rem 1rem; }
.empty-mark { margin:auto; display:grid; place-items:center; width:80px; height:80px; border-radius:50%; background:#0fe1d1; font-weight:900; }
.empty-cart p { color:#796e96; margin-bottom:1.5rem; }
@media (max-width:760px) { .cart > div:first-of-type { grid-template-columns:1fr; } }

.coupon-section input {
  flex: 1;
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.coupon-section button {
  padding: 0.5rem 1rem;
  background: #667eea;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

.checkout-btn {
  display: block;
  text-align: center;
  background: #e63946;
  color: white;
  padding: 1rem;
  text-decoration: none;
  border-radius: 4px;
  margin-top: 1rem;
}

.btn {
  display: inline-block;
  background: #667eea;
  color: white;
  padding: 0.75rem 2rem;
  text-decoration: none;
  border-radius: 4px;
}
</style>
