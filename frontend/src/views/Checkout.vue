<template>
  <div class="checkout">
    <h1>Checkout</h1>

    <form @submit.prevent="handleCheckout" v-if="cartStore.cart && cartStore.cart.items.length > 0">
      <div class="form-section">
        <h2>Shipping Information</h2>
        <div class="form-group">
          <label>Full Name</label>
          <input v-model="form.fullName" required />
        </div>
        <div class="form-group">
          <label>Email</label>
          <input v-model="form.email" type="email" required />
        </div>
        <div class="form-group">
          <label>Phone</label>
          <input v-model="form.phone" />
        </div>
        <div class="form-group">
          <label>Address</label>
          <input v-model="form.addressLine" required />
        </div>
        <div class="form-group">
          <label>City</label>
          <input v-model="form.city" required />
        </div>
        <div class="form-group">
          <label>State</label>
          <input v-model="form.state" required />
        </div>
        <div class="form-group">
          <label>Postal Code</label>
          <input v-model="form.postalCode" required />
        </div>
      </div>

      <div class="form-section">
        <h2>Payment Method</h2>
        <div class="form-group">
          <label>
            <input type="radio" v-model="form.paymentMethod" value="UPI" required />
            UPI
          </label>
        </div>
        <div class="form-group">
          <label>
            <input type="radio" v-model="form.paymentMethod" value="Card" />
            Credit/Debit Card
          </label>
        </div>
        <div class="form-group">
          <label>
            <input type="radio" v-model="form.paymentMethod" value="COD" />
            Cash on Delivery
          </label>
        </div>
      </div>

      <div class="order-summary">
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
      </div>

      <button type="submit" :disabled="loading">Place Order</button>
    </form>

    <p v-else>Your cart is empty</p>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useCartStore } from '../stores/cart'
import { useAuthStore } from '../stores/auth'
import { useOrdersStore } from '../stores/orders'

const router = useRouter()
const cartStore = useCartStore()
const authStore = useAuthStore()
const ordersStore = useOrdersStore()

const form = ref({
  fullName: authStore.user?.fullName || '',
  email: authStore.user?.email || '',
  phone: authStore.user?.phone || '',
  addressLine: '',
  city: '',
  state: '',
  postalCode: '',
  paymentMethod: 'UPI'
})

const loading = ref(false)

async function handleCheckout() {
  loading.value = true
  try {
    await ordersStore.createOrder({
      ...form.value,
      couponCode: cartStore.cart?.couponCode
    })
    router.push(`/account/orders`)
  } catch (e: any) {
    alert(e.response?.data?.error || 'Order failed')
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  cartStore.fetchCart()
})
</script>

<style scoped>
.checkout {
  padding: 2rem;
  max-width: 800px;
  margin: 0 auto;
}

.form-section {
  margin-bottom: 2rem;
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

.order-summary {
  background: #f5f5f5;
  padding: 2rem;
  border-radius: 8px;
  margin-bottom: 2rem;
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

button {
  width: 100%;
  padding: 1rem;
  background: #e63946;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  font-size: 1rem;
}

button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}
</style>
