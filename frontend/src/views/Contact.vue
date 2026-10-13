<template>
  <div class="contact">
    <h1>Contact Us</h1>
    <div class="contact-container">
      <div class="contact-info">
        <h2>Get in Touch</h2>
        <p>Have questions? We'd love to hear from you.</p>
        <div class="info-item">
          <strong>Email:</strong>
          <p>support@novakart.com</p>
        </div>
        <div class="info-item">
          <strong>Phone:</strong>
          <p>+91 123 456 7890</p>
        </div>
        <div class="info-item">
          <strong>Address:</strong>
          <p>123 Commerce Street, Tech City, India</p>
        </div>
      </div>
      <div class="contact-form">
        <h2>Send us a Message</h2>
        <form @submit.prevent="submitForm">
          <div class="form-group">
            <label>Name</label>
            <input v-model="form.fullName" required />
          </div>
          <div class="form-group">
            <label>Email</label>
            <input v-model="form.email" type="email" required />
          </div>
          <div class="form-group">
            <label>Subject</label>
            <input v-model="form.subject" required />
          </div>
          <div class="form-group">
            <label>Message</label>
            <textarea v-model="form.body" rows="5" required></textarea>
          </div>
          <button type="submit" :disabled="loading">
            {{ loading ? 'Sending...' : 'Send Message' }}
          </button>
        </form>
        <p v-if="success" class="success">Message sent successfully!</p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import api from '../api/client'

const form = ref({
  fullName: '',
  email: '',
  subject: '',
  body: ''
})
const loading = ref(false)
const success = ref(false)

async function submitForm() {
  loading.value = true
  success.value = false
  try {
    await api.post('/contact', form.value)
    success.value = true
    form.value = { fullName: '', email: '', subject: '', body: '' }
  } catch (e) {
    alert('Failed to send message')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.contact {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.contact-container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 3rem;
  margin-top: 2rem;
}

.contact-info h2,
.contact-form h2 {
  margin-bottom: 1rem;
}

.info-item {
  margin: 1.5rem 0;
}

.info-item strong {
  display: block;
  margin-bottom: 0.25rem;
}

.contact-form {
  background: #f5f5f5;
  padding: 2rem;
  border-radius: 8px;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
}

.form-group input,
.form-group textarea {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.contact-form button {
  background: #e63946;
  color: white;
  padding: 0.75rem 2rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  width: 100%;
}

.contact-form button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.success {
  color: #2ecc71;
  margin-top: 1rem;
  text-align: center;
}
</style>
