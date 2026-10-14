<template>
  <div class="auth-page">
    <div class="auth-ambient-glow" aria-hidden="true"></div>

    <div class="auth-container glass-panel">
      <div class="auth-header">
        <span class="auth-eyebrow">JOIN THE COMMUNITY</span>
        <h1 class="auth-title">Register<span class="dot-accent">.</span></h1>
        <p class="auth-subtitle">Create your NovaKart member profile to unlock curated drops and fast checkout.</p>
      </div>

      <form @submit.prevent="handleRegister" class="auth-form">
        <div class="form-group">
          <label for="reg-name">Full Name</label>
          <div class="input-wrap">
            <svg class="field-icon" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
              <circle cx="12" cy="7" r="4"></circle>
            </svg>
            <input 
              id="reg-name"
              v-model="fullName" 
              type="text" 
              placeholder="e.g. Maya Chen" 
              required 
              autocomplete="name"
            />
          </div>
        </div>

        <div class="form-group">
          <label for="reg-email">Email Address</label>
          <div class="input-wrap">
            <svg class="field-icon" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
              <polyline points="22,6 12,13 2,6"></polyline>
            </svg>
            <input 
              id="reg-email"
              v-model="email" 
              type="email" 
              placeholder="you@domain.com" 
              required 
              autocomplete="email"
            />
          </div>
        </div>

        <div class="form-group">
          <label for="reg-password">Create Password</label>
          <div class="input-wrap">
            <svg class="field-icon" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <rect x="3" y="11" width="18" height="11" rx="2" ry="2"></rect>
              <path d="M7 11V7a5 5 0 0 1 10 0v4"></path>
            </svg>
            <input 
              id="reg-password"
              v-model="password" 
              :type="showPassword ? 'text' : 'password'" 
              placeholder="At least 6 characters" 
              required 
              autocomplete="new-password"
            />
            <button 
              type="button" 
              class="toggle-pw-btn" 
              @click="showPassword = !showPassword"
              :aria-label="showPassword ? 'Hide password' : 'Show password'"
            >
              {{ showPassword ? 'Hide' : 'Show' }}
            </button>
          </div>
        </div>

        <div v-if="error" class="auth-error-banner" role="alert">
          <span>⚠️ {{ error }}</span>
        </div>

        <button type="submit" class="btn-pill btn-pill-primary submit-auth-btn" :disabled="loading">
          <span v-if="loading" class="loading-spin"></span>
          <span v-else class="btn-row">
            <span>Create Account</span>
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <line x1="5" y1="12" x2="19" y2="12"></line>
              <polyline points="12 5 19 12 12 19"></polyline>
            </svg>
          </span>
        </button>
      </form>

      <div class="auth-footer-links">
        <p class="register-prompt">
          Already have an account? 
          <router-link to="/login" class="auth-switch-link">Sign In</router-link>
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const authStore = useAuthStore()

const fullName = ref('')
const email = ref('')
const password = ref('')
const loading = ref(false)
const error = ref('')
const showPassword = ref(false)

async function handleRegister() {
  loading.value = true
  error.value = ''
  try {
    await authStore.register({ fullName: fullName.value, email: email.value, password: password.value })
    router.push('/')
  } catch (e: any) {
    error.value = e.response?.data?.error || 'Registration failed. Please check your information.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-page {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  min-height: calc(100vh - 108px);
  padding: 3rem 1.5rem;
  background-color: var(--bg-main);
  overflow: hidden;
}

.auth-ambient-glow {
  position: absolute;
  top: 20%;
  left: 50%;
  transform: translateX(-50%);
  width: 600px;
  height: 500px;
  background: radial-gradient(circle, rgba(168, 85, 247, 0.22) 0%, rgba(34, 197, 94, 0.08) 50%, transparent 70%);
  filter: blur(80px);
  pointer-events: none;
}

.auth-container {
  position: relative;
  z-index: 1;
  width: 100%;
  max-width: 460px;
  padding: 3rem 2.5rem;
  border-radius: var(--radius-xl);
  box-shadow: 0 30px 70px rgba(0, 0, 0, 0.5);
}

.auth-header {
  margin-bottom: 2rem;
  text-align: center;
}

.auth-eyebrow {
  display: inline-block;
  font-size: 0.72rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: var(--accent-lime);
  margin-bottom: 0.6rem;
}

.auth-title {
  font-family: var(--font-display);
  font-size: 2.5rem;
  font-weight: 900;
  letter-spacing: -0.03em;
  color: #ffffff;
  margin-bottom: 0.6rem;
}

.auth-subtitle {
  font-size: 0.88rem;
  color: var(--text-secondary);
  line-height: 1.5;
}

.auth-form {
  display: flex;
  flex-direction: column;
  gap: 1.4rem;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.form-group label {
  font-size: 0.82rem;
  font-weight: 600;
  color: var(--text-secondary);
}

.input-wrap {
  position: relative;
  display: flex;
  align-items: center;
}

.field-icon {
  position: absolute;
  left: 1rem;
  color: var(--text-muted);
  pointer-events: none;
}

.input-wrap input {
  width: 100%;
  padding: 0.85rem 3.5rem 0.85rem 2.8rem;
  border-radius: var(--radius-md);
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-medium);
  color: #ffffff;
  font-family: inherit;
  font-size: 0.92rem;
  outline: none;
  transition: all var(--transition-fast);
}

.input-wrap input:focus {
  border-color: var(--accent-purple-bright);
  background: rgba(255, 255, 255, 0.08);
  box-shadow: 0 0 15px rgba(168, 85, 247, 0.25);
}

.toggle-pw-btn {
  position: absolute;
  right: 0.85rem;
  background: transparent;
  border: 0;
  color: var(--text-muted);
  font-size: 0.75rem;
  font-weight: 700;
  cursor: pointer;
}

.toggle-pw-btn:hover {
  color: #ffffff;
}

.auth-error-banner {
  padding: 0.8rem 1rem;
  border-radius: var(--radius-md);
  background: rgba(239, 68, 68, 0.15);
  border: 1px solid rgba(239, 68, 68, 0.3);
  color: #fca5a5;
  font-size: 0.82rem;
}

.submit-auth-btn {
  width: 100%;
  padding: 1rem;
  font-size: 0.98rem;
  margin-top: 0.5rem;
}

.btn-row {
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.auth-footer-links {
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid var(--border-subtle);
  text-align: center;
}

.register-prompt {
  font-size: 0.88rem;
  color: var(--text-muted);
  margin: 0;
}

.auth-switch-link {
  color: #ffffff;
  font-weight: 700;
  text-decoration: underline;
  margin-left: 0.3rem;
}

.auth-switch-link:hover {
  color: var(--accent-lime);
}
</style>
