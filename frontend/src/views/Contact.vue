<template>
  <div class="contact-page">
    <div class="container">
      <div class="contact-grid">
        <!-- Left: Brand Info & Channels -->
        <div class="contact-intro-card">
          <div class="contact-eyebrow">
            <span class="eyebrow-dot"></span>
            <span>CUSTOMER CARE & CONCIERGE</span>
          </div>

          <h1 class="contact-title">
            Contact Us<span class="dot-accent">.</span><br />
            <em class="serif-accent">Let's connect.</em>
          </h1>

          <p class="contact-description">
            Have a question about an archival drop, order tracking, or brand partnership? Our dedicated concierge team is here to assist.
          </p>

          <div class="contact-channels-list">
            <a href="mailto:support@novakart.com" class="channel-card glass-panel">
              <div class="channel-icon">✉</div>
              <div>
                <strong>Email Support</strong>
                <span>support@novakart.com</span>
              </div>
            </a>

            <a href="tel:+911234567890" class="channel-card glass-panel">
              <div class="channel-icon">↗</div>
              <div>
                <strong>Phone & WhatsApp</strong>
                <span>+91 123 456 7890</span>
              </div>
            </a>

            <div class="channel-card glass-panel">
              <div class="channel-icon">⌖</div>
              <div>
                <strong>Headquarters</strong>
                <span>Tech City, India</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Right: Message Form -->
        <div class="contact-form-card glass-panel">
          <div class="form-header-row">
            <span class="form-counter">01</span>
            <h2>Send us a direct message</h2>
            <span class="header-glyph">↗</span>
          </div>

          <form @submit.prevent="submitForm" class="contact-form-body">
            <div class="form-field">
              <label for="c-name">Your Full Name</label>
              <input id="c-name" v-model="form.fullName" placeholder="e.g. Maya Chen" required />
            </div>

            <div class="form-field">
              <label for="c-email">Email Address</label>
              <input id="c-email" v-model="form.email" type="email" placeholder="you@domain.com" required />
            </div>

            <div class="form-field">
              <label for="c-subject">Subject</label>
              <input id="c-subject" v-model="form.subject" placeholder="What can we help you with?" required />
            </div>

            <div class="form-field">
              <label for="c-message">Your Message</label>
              <textarea id="c-message" v-model="form.body" rows="4" placeholder="Tell us how we can assist..." required></textarea>
            </div>

            <button type="submit" class="btn-pill btn-pill-primary submit-contact-btn" :disabled="loading">
              <span v-if="loading" class="loading-spin"></span>
              <span v-else class="btn-text">
                <span>Send Message</span>
                <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                  <line x1="5" y1="12" x2="19" y2="12"></line>
                  <polyline points="12 5 19 12 12 19"></polyline>
                </svg>
              </span>
            </button>
          </form>

          <Transition name="fade">
            <div v-if="success" class="contact-success-alert" role="alert">
              <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
                <polyline points="20 6 9 17 4 12"></polyline>
              </svg>
              <span>Thank you — your message has been received. Our team will get back to you shortly.</span>
            </div>
          </Transition>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import api from '../api/client'

const form = ref({ fullName: '', email: '', subject: '', body: '' })
const loading = ref(false)
const success = ref(false)

async function submitForm() {
  loading.value = true
  success.value = false
  try {
    await api.post('/contact', form.value)
    success.value = true
    form.value = { fullName: '', email: '', subject: '', body: '' }
  } catch {
    alert('Failed to send message. Please try again.')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.contact-page {
  min-height: 100vh;
  padding: 4.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.contact-grid {
  display: grid;
  grid-template-columns: 1fr 1.15fr;
  gap: 4.5rem;
  align-items: center;
}

.contact-eyebrow {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  font-size: 0.75rem;
  font-weight: 800;
  letter-spacing: 0.15em;
  color: var(--accent-lime);
  margin-bottom: 0.8rem;
}

.eyebrow-dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: var(--accent-lime);
}

.contact-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5vw, 4.4rem);
  font-weight: 900;
  line-height: 1;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 1.25rem;
}

.contact-title em {
  color: var(--accent-purple-bright);
}

.contact-description {
  font-size: 1.05rem;
  line-height: 1.65;
  color: var(--text-secondary);
  max-width: 480px;
  margin-bottom: 2.5rem;
}

.contact-channels-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.channel-card {
  display: flex;
  align-items: center;
  gap: 1.2rem;
  padding: 1.25rem 1.6rem;
  border-radius: var(--radius-lg);
  text-decoration: none;
  color: inherit;
  transition: all var(--transition-fast);
}

.channel-card:hover {
  transform: translateX(4px);
  border-color: rgba(255, 255, 255, 0.25);
  background: rgba(255, 255, 255, 0.05);
}

.channel-icon {
  width: 42px;
  height: 42px;
  border-radius: 50%;
  background: rgba(168, 85, 247, 0.15);
  color: var(--accent-purple-bright);
  display: grid;
  place-items: center;
  font-size: 1.2rem;
  flex-shrink: 0;
}

.channel-card strong {
  display: block;
  font-size: 0.95rem;
  color: #ffffff;
  margin-bottom: 0.15rem;
}

.channel-card span {
  font-size: 0.84rem;
  color: var(--text-muted);
}

/* Form Card */
.contact-form-card {
  padding: 3rem;
  border-radius: var(--radius-xl);
}

.form-header-row {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 2rem;
  padding-bottom: 1.2rem;
  border-bottom: 1px solid var(--border-subtle);
}

.form-counter {
  font-size: 0.8rem;
  font-weight: 800;
  color: var(--accent-lime);
}

.form-header-row h2 {
  font-size: 1.35rem;
  font-weight: 800;
  color: #ffffff;
}

.header-glyph {
  color: var(--text-muted);
  font-size: 1.2rem;
}

.contact-form-body {
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.form-field {
  display: flex;
  flex-direction: column;
  gap: 0.45rem;
}

.form-field label {
  font-size: 0.82rem;
  font-weight: 600;
  color: var(--text-secondary);
}

.form-field input,
.form-field textarea {
  width: 100%;
  padding: 0.85rem 1.1rem;
  border-radius: var(--radius-md);
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-medium);
  color: #ffffff;
  font-family: inherit;
  font-size: 0.92rem;
  outline: none;
  transition: all var(--transition-fast);
}

.form-field input:focus,
.form-field textarea:focus {
  border-color: var(--accent-purple-bright);
  background: rgba(255, 255, 255, 0.08);
  box-shadow: 0 0 15px rgba(168, 85, 247, 0.25);
}

.submit-contact-btn {
  width: 100%;
  padding: 1.05rem;
  font-size: 0.98rem;
  margin-top: 0.5rem;
}

.btn-text {
  display: flex;
  align-items: center;
  gap: 0.6rem;
}

.contact-success-alert {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  margin-top: 1.5rem;
  padding: 1rem 1.25rem;
  border-radius: var(--radius-md);
  background: rgba(34, 197, 94, 0.15);
  border: 1px solid rgba(34, 197, 94, 0.35);
  color: #4ade80;
  font-size: 0.88rem;
  font-weight: 600;
}

@media (max-width: 900px) {
  .contact-grid {
    grid-template-columns: 1fr;
    gap: 3rem;
  }
}

@media (max-width: 600px) {
  .contact-form-card {
    padding: 2rem 1.5rem;
  }
}
</style>
