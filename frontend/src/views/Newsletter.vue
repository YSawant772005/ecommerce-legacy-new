<template>
  <div class="newsletter">
    <h2>Subscribe to our Newsletter</h2>
    <p>Get the latest updates on new products and upcoming sales.</p>
    <form @submit.prevent="subscribe" class="newsletter-form">
      <input v-model="email" type="email" placeholder="Enter your email" required />
      <button type="submit" :disabled="loading">
        {{ loading ? 'Subscribing...' : 'Subscribe' }}
      </button>
    </form>
    <p v-if="success" class="success">Successfully subscribed!</p>
    <p v-if="error" class="error">{{ error }}</p>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import api from '../api/client'

const email = ref('')
const loading = ref(false)
const success = ref(false)
const error = ref('')

async function subscribe() {
  loading.value = true
  success.value = false
  error.value = ''
  try {
    await api.post('/newsletter/subscribe', { email: email.value })
    success.value = true
    email.value = ''
  } catch (e: any) {
    error.value = e.response?.data?.error || 'Subscription failed'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.newsletter {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  padding: 3rem 2rem;
  text-align: center;
}

.newsletter h2 {
  margin-bottom: 0.5rem;
}

.newsletter p {
  margin-bottom: 1.5rem;
  opacity: 0.9;
}

.newsletter-form {
  display: flex;
  gap: 0.5rem;
  justify-content: center;
  max-width: 500px;
  margin: 0 auto;
}

.newsletter-form input {
  flex: 1;
  padding: 0.75rem;
  border: none;
  border-radius: 4px;
}

.newsletter-form button {
  padding: 0.75rem 2rem;
  background: #e63946;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

.newsletter-form button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.success {
  color: #2ecc71;
  margin-top: 1rem;
}

.error {
  color: #e63946;
  margin-top: 1rem;
}
</style>
