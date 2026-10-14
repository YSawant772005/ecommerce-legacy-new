<template>
  <section class="newsletter-section" aria-label="Newsletter Subscription">
    <div class="container">
      <div class="newsletter-card glass-panel">
        <div class="newsletter-glow-orb" aria-hidden="true"></div>

        <div class="newsletter-header">
          <span class="newsletter-eyebrow">JOIN THE INNER CIRCLE</span>
          <h2 class="newsletter-heading">
            First access to <em class="serif-accent">limited drops.</em>
          </h2>
          <p class="newsletter-subtext">
            Be the first to know about private archival sales, new product drops, and editorial features. No spam, unsubscribe anytime.
          </p>
        </div>

        <form @submit.prevent="subscribe" class="newsletter-form">
          <div class="input-container">
            <svg class="mail-icon" width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
              <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
              <polyline points="22,6 12,13 2,6"></polyline>
            </svg>
            <input 
              v-model="email" 
              type="email" 
              placeholder="Enter your email address" 
              required 
              aria-label="Email address"
            />
          </div>
          <button type="submit" class="btn-pill btn-pill-primary submit-btn" :disabled="loading">
            <span>{{ loading ? 'Subscribing…' : 'Subscribe' }}</span>
            <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <line x1="5" y1="12" x2="19" y2="12"></line>
              <polyline points="12 5 19 12 12 19"></polyline>
            </svg>
          </button>
        </form>

        <Transition name="fade">
          <div v-if="success" class="alert-box alert-success" role="alert">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <polyline points="20 6 9 17 4 12"></polyline>
            </svg>
            <span>You're in. Welcome to the Nova community.</span>
          </div>
        </Transition>

        <Transition name="fade">
          <div v-if="error" class="alert-box alert-error" role="alert">
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
              <circle cx="12" cy="12" r="10"></circle>
              <line x1="12" y1="8" x2="12" y2="12"></line>
              <line x1="12" y1="16" x2="12.01" y2="16"></line>
            </svg>
            <span>{{ error }}</span>
          </div>
        </Transition>
      </div>
    </div>
  </section>
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
    error.value = e.response?.data?.error || 'Subscription failed. Please try again.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.newsletter-section {
  padding: 2rem 0 6rem;
}

.newsletter-card {
  position: relative;
  overflow: hidden;
  padding: 5rem 3rem;
  text-align: center;
  border-radius: var(--radius-xl);
  background: linear-gradient(135deg, rgba(25, 23, 38, 0.9) 0%, rgba(13, 12, 20, 0.95) 100%);
  border: 1px solid var(--border-medium);
  box-shadow: 0 30px 70px rgba(0, 0, 0, 0.5);
}

.newsletter-glow-orb {
  position: absolute;
  top: -50%;
  left: 50%;
  transform: translateX(-50%);
  width: 600px;
  height: 400px;
  background: radial-gradient(circle, rgba(168, 85, 247, 0.25) 0%, rgba(34, 197, 94, 0.1) 40%, transparent 70%);
  filter: blur(80px);
  pointer-events: none;
}

.newsletter-header {
  position: relative;
  z-index: 1;
  max-width: 600px;
  margin: 0 auto 2.5rem;
}

.newsletter-eyebrow {
  display: block;
  font-size: 0.75rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: var(--accent-lime);
  margin-bottom: 0.8rem;
}

.newsletter-heading {
  font-family: var(--font-display);
  font-size: clamp(2.4rem, 4.5vw, 3.8rem);
  font-weight: 850;
  line-height: 1.05;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 1.2rem;
}

.newsletter-subtext {
  font-size: 1rem;
  line-height: 1.6;
  color: var(--text-secondary);
}

.newsletter-form {
  position: relative;
  z-index: 1;
  display: flex;
  align-items: center;
  gap: 0.75rem;
  max-width: 520px;
  margin: 0 auto;
}

.input-container {
  position: relative;
  flex: 1;
  display: flex;
  align-items: center;
}

.mail-icon {
  position: absolute;
  left: 1.2rem;
  color: var(--text-muted);
  pointer-events: none;
}

.input-container input {
  width: 100%;
  padding: 0.95rem 1.2rem 0.95rem 3rem;
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-medium);
  border-radius: var(--radius-pill);
  color: #ffffff;
  font-size: 0.92rem;
  outline: none;
  transition: all var(--transition-fast);
}

.input-container input:focus {
  border-color: var(--accent-purple-bright);
  background: rgba(255, 255, 255, 0.08);
  box-shadow: 0 0 20px rgba(168, 85, 247, 0.3);
}

.submit-btn {
  padding: 0.95rem 1.8rem;
  white-space: nowrap;
}

.alert-box {
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  padding: 0.75rem 1.4rem;
  border-radius: var(--radius-pill);
  font-size: 0.88rem;
  font-weight: 600;
  margin-top: 1.8rem;
}

.alert-success {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
  border: 1px solid rgba(34, 197, 94, 0.3);
}

.alert-error {
  background: rgba(244, 63, 94, 0.15);
  color: #fb7185;
  border: 1px solid rgba(244, 63, 94, 0.3);
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.3s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

@media (max-width: 640px) {
  .newsletter-card {
    padding: 3.5rem 1.5rem;
  }
  .newsletter-form {
    flex-direction: column;
  }
  .submit-btn {
    width: 100%;
  }
}
</style>
