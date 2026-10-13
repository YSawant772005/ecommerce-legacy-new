<template>
  <div class="checkout">
    <p class="eyebrow">FINAL STEP</p><h1>Make it yours.</h1>

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
  padding: 4rem max(1.5rem, calc((100% - 1000px) / 2));
  color:#24145f; background:#fbfaff; min-height:calc(100vh - 100px);
  margin: 0 auto;
}
.eyebrow { color:#9654db; font-size:.68rem; font-weight:800; letter-spacing:.18em; }
.checkout h1 { font-size:clamp(2.8rem,6vw,5rem); letter-spacing:-.08em; margin:.5rem 0 3rem; }
.checkout form { display:grid; grid-template-columns:1fr .8fr; gap:2rem; }

.form-section {
  margin-bottom: 2rem; background:#fff; border:1px solid #e8e3f1; border-radius:16px; padding:1.5rem;
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
  padding: 0.8rem;
  border: 1px solid #e8e3f1;
  border-radius: 4px;
}

.order-summary {
  background: #24145f; color:white;
  padding: 2rem;
  border-radius: 8px;
  margin-bottom: 2rem;
}
.checkout > form > button { grid-column:2; border:0; background:#0fe1d1; color:#24145f; padding:1rem; border-radius:999px; font-weight:800; cursor:pointer; }
@media (max-width:760px) { .checkout form { grid-template-columns:1fr; } .checkout > form > button { grid-column:auto; } }

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
