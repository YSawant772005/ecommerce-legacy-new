<template>
  <div class="auth-page">
    <div class="auth-container">
      <p class="eyebrow">WELCOME TO NOVAKART</p><h1>Come on in.</h1>
      <form @submit.prevent="handleLogin">
        <div class="form-group">
          <label>Email</label>
          <input v-model="email" type="email" required />
        </div>
        <div class="form-group">
          <label>Password</label>
          <input v-model="password" type="password" required />
        </div>
        <button type="submit" :disabled="loading">Login</button>
      </form>
      <p v-if="error" class="error">{{ error }}</p>
      <p class="link">
        Don't have an account? <router-link to="/register">Register</router-link>
      </p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const email = ref('')
const password = ref('')
const loading = ref(false)
const error = ref('')

async function handleLogin() {
  loading.value = true
  error.value = ''
  try {
    await authStore.login({ email: email.value, password: password.value })
    router.push('/')
  } catch (e: any) {
    error.value = e.response?.data?.error || 'Login failed'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-page {
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: calc(100vh - 120px);
  padding: 2rem;
  background:linear-gradient(135deg,#fbfaff 50%,#0fe1d1 50%);
}

.auth-container {
  background: white;
  padding: 3rem;
  border-radius: 20px;
  box-shadow: 0 20px 50px rgba(36,20,95,.14);
  width: 100%;
  max-width: 400px;
}
.eyebrow { color:#9654db; font-size:.68rem; font-weight:800; letter-spacing:.18em; }
.auth-container h1 { color:#24145f; font-size:3.6rem; letter-spacing:-.08em; margin:.5rem 0 2rem; }

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

button {
  width: 100%;
  padding: 0.75rem;
  background: #24145f;
  color: white;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.error {
  color: #e63946;
  margin-top: 1rem;
}

.link {
  margin-top: 1rem;
  text-align: center;
}

.link a {
  color: #9654db;
  text-decoration: none;
}
</style>
