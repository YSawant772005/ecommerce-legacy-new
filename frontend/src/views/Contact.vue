<template>
  <div class="contact">
    <section class="contact-intro">
      <span class="eyebrow">WE'RE HERE TO HELP</span>
      <h1>Let's make<br /><em>something good.</em></h1>
      <p>Have a question about an order, a product or simply want to say hello? Our small, human team would love to hear from you.</p>
      <div class="contact-links">
        <a href="mailto:support@novakart.com"><span>✉</span> support@novakart.com</a>
        <a href="tel:+911234567890"><span>↗</span> +91 123 456 7890</a>
        <span><span>⌖</span> Tech City, India</span>
      </div>
    </section>
    <section class="contact-form">
      <div class="form-header"><span>01</span><h2>Send us a message</h2><span>↗</span></div>
      <form @submit.prevent="submitForm">
        <label>Name<input v-model="form.fullName" placeholder="Your name" required /></label>
        <label>Email<input v-model="form.email" type="email" placeholder="you@example.com" required /></label>
        <label>Subject<input v-model="form.subject" placeholder="How can we help?" required /></label>
        <label>Message<textarea v-model="form.body" rows="5" placeholder="Tell us a little more..." required></textarea></label>
        <button type="submit" :disabled="loading">{{ loading ? 'Sending...' : 'Send message  ↗' }}</button>
      </form>
      <p v-if="success" class="success">Thanks — your message is on its way.</p>
    </section>
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
    alert('Failed to send message')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.contact { min-height: calc(100vh - 100px); padding: 5rem max(1.5rem, calc((100% - 1050px) / 2)); background: #fbfaff; color: #24145f; display: grid; grid-template-columns: .9fr 1.1fr; gap: 8rem; align-items: center; }
.eyebrow { color: #9654db; font-size: .68rem; font-weight: 800; letter-spacing: .18em; }
.contact-intro h1 { margin: 1.2rem 0; font-size: clamp(3rem, 5vw, 5rem); line-height: .9; letter-spacing: -.08em; }
.contact-intro h1 em { color: #9654db; font-family: Georgia, serif; font-weight: 400; }
.contact-intro > p { max-width: 370px; color: #837d97; font-size: .87rem; line-height: 1.7; }
.contact-links { display: grid; gap: 1rem; margin-top: 3rem; font-size: .78rem; font-weight: 700; }
.contact-links a, .contact-links > span { color: #24145f; text-decoration: none; }
.contact-links span { margin-right: .55rem; color: #9654db; }
.contact-form { padding: 2rem; border: 1px solid #e4dfee; border-radius: 18px; background: #fff; box-shadow: 0 18px 50px rgba(36,20,95,.08); }
.form-header { display: flex; align-items: center; justify-content: space-between; margin-bottom: 2rem; color: #a19aaa; font-size: .7rem; }
.form-header h2 { margin: 0; color: #24145f; font-size: 1.25rem; }
.contact-form form { display: grid; gap: 1.1rem; }
.contact-form label { display: grid; gap: .45rem; color: #6e6780; font-size: .7rem; font-weight: 700; }
.contact-form input, .contact-form textarea { width: 100%; padding: .8rem .9rem; border: 1px solid #e4dfee; border-radius: 8px; outline: 0; background: #fcfbfe; color: #24145f; font: inherit; resize: vertical; }
.contact-form input:focus, .contact-form textarea:focus { border-color: #9654db; box-shadow: 0 0 0 3px rgba(150,84,219,.12); }
.contact-form button { padding: .85rem 1rem; border: 0; border-radius: 999px; background: #281568; color: #fff; cursor: pointer; font-size: .75rem; font-weight: 700; }
.success { color: #159f8d; font-size: .75rem; }
@media (max-width: 760px) { .contact { grid-template-columns: 1fr; gap: 3rem; padding-top: 3rem; } }
</style>
