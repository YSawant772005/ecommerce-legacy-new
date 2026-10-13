<template>
  <div class="cart">
    <h1>Shopping Cart</h1>

    <div v-if="!cartStore.loading && cartStore.cart && cartStore.cart.items.length > 0">
      <div class="cart-items">
        <div v-for="item in cartStore.cart.items" :key="item.productId" class="cart-item">
          <img v-if="item.imageUrl" :src="item.imageUrl" :alt="item.productName" />
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

    <div v-else-if="!cartStore.loading">
      <p>Your cart is empty</p>
      <router-link to="/products" class="btn">Continue Shopping</router-link>
    </div>

    <p v-else>Loading cart...</p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useCartStore } from '../stores/cart'

const cartStore = useCartStore()
const couponCode = ref('')

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
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.cart-items {
  display: grid;
  gap: 1rem;
  margin-bottom: 2rem;
}

.cart-item {
  display: flex;
  gap: 1rem;
  padding: 1rem;
  border: 1px solid #ddd;
  border-radius: 8px;
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
  background: #f5f5f5;
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

.coupon-section {
  display: flex;
  gap: 0.5rem;
  margin: 1rem 0;
}

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
