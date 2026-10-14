<template>
  <div class="profile-settings-page">
    <div class="container">
      <router-link to="/account" class="back-link-pill">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <line x1="19" y1="12" x2="5" y2="12"></line>
          <polyline points="12 19 5 12 12 19"></polyline>
        </svg>
        <span>Back to Account</span>
      </router-link>

      <header class="profile-header">
        <div class="profile-eyebrow">
          <span class="eyebrow-dot"></span>
          <span>SECURITY & PREFERENCES</span>
        </div>
        <h1 class="profile-title">
          My Profile<span class="dot-accent">.</span>
        </h1>
        <p class="profile-subtitle">Keep your contact details and account credentials up to date.</p>
      </header>

      <div class="profile-cards-grid">
        <!-- Personal Information Section -->
        <section class="settings-card glass-panel">
          <div class="card-title-row">
            <span class="card-icon">👤</span>
            <div>
              <h2>Personal Information</h2>
              <p>Your primary contact and profile identity.</p>
            </div>
          </div>

          <form @submit.prevent="updateProfile" class="settings-form">
            <div class="form-field">
              <label for="p-fullname">Full Name</label>
              <input id="p-fullname" v-model="form.fullName" required placeholder="e.g. Maya Chen" />
            </div>

            <div class="form-field">
              <label for="p-email">Email Address (Read-only)</label>
              <input id="p-email" v-model="form.email" disabled />
              <span class="field-hint">Email cannot be modified directly for security reasons.</span>
            </div>

            <div class="form-field">
              <label for="p-phone">Phone Number</label>
              <input id="p-phone" v-model="form.phone" placeholder="+91 98765 43210" />
            </div>

            <button type="submit" class="btn-pill btn-pill-primary save-btn" :disabled="loading">
              <span v-if="loading" class="loading-spin"></span>
              <span v-else>Save Changes</span>
            </button>
          </form>

          <div v-if="profileMessage" class="feedback-banner success" role="alert">
            ✓ {{ profileMessage }}
          </div>
        </section>

        <!-- Change Password Section -->
        <section class="settings-card glass-panel">
          <div class="card-title-row">
            <span class="card-icon">🔒</span>
            <div>
              <h2>Change Password</h2>
              <p>Ensure your account is protected with a strong password.</p>
            </div>
          </div>

          <form @submit.prevent="changePassword" class="settings-form">
            <div class="form-field">
              <label for="pw-current">Current Password</label>
              <input id="pw-current" v-model="passwordForm.currentPassword" type="password" required placeholder="Current password" />
            </div>

            <div class="form-field">
              <label for="pw-new">New Password</label>
              <input id="pw-new" v-model="passwordForm.newPassword" type="password" required placeholder="At least 6 characters" />
            </div>

            <button type="submit" class="btn-pill btn-pill-primary save-btn" :disabled="passwordLoading">
              <span v-if="passwordLoading" class="loading-spin"></span>
              <span v-else>Change Password</span>
            </button>
          </form>

          <div v-if="passwordMessage" class="feedback-banner success" role="alert">
            ✓ {{ passwordMessage }}
          </div>
          <div v-if="passwordError" class="feedback-banner error" role="alert">
            ⚠️ {{ passwordError }}
          </div>
        </section>
      </div>

      <!-- Quick Navigation links -->
      <div class="quick-links-strip">
        <router-link to="/account/addresses" class="quick-link-chip glass-card">
          <span>📍 Manage Addresses →</span>
        </router-link>
        <router-link to="/account/orders" class="quick-link-chip glass-card">
          <span>📦 View Order History →</span>
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { accountApi } from '../api/account'

const form = ref({
  fullName: '',
  email: '',
  phone: ''
})
const passwordForm = ref({
  currentPassword: '',
  newPassword: ''
})
const loading = ref(false)
const passwordLoading = ref(false)
const profileMessage = ref('')
const passwordMessage = ref('')
const passwordError = ref('')

async function loadProfile() {
  const response = await accountApi.getProfile()
  form.value = response.data
}

async function updateProfile() {
  loading.value = true
  profileMessage.value = ''
  try {
    await accountApi.updateProfile({
      fullName: form.value.fullName,
      phone: form.value.phone
    })
    profileMessage.value = 'Profile updated successfully.'
    setTimeout(() => { profileMessage.value = '' }, 4000)
  } catch (e) {
    alert('Failed to update profile')
  } finally {
    loading.value = false
  }
}

async function changePassword() {
  passwordLoading.value = true
  passwordMessage.value = ''
  passwordError.value = ''
  try {
    await accountApi.changePassword(passwordForm.value)
    passwordMessage.value = 'Password changed successfully.'
    passwordForm.value = { currentPassword: '', newPassword: '' }
    setTimeout(() => { passwordMessage.value = '' }, 4000)
  } catch (e: any) {
    passwordError.value = e.response?.data?.error || 'Failed to change password. Please check your current password.'
  } finally {
    passwordLoading.value = false
  }
}

onMounted(() => {
  loadProfile()
})
</script>

<style scoped>
.profile-settings-page {
  min-height: 100vh;
  padding: 3.5rem 0 6rem;
  background-color: var(--bg-main);
  color: var(--text-primary);
}

.back-link-pill {
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  padding: 0.5rem 1rem;
  border-radius: var(--radius-pill);
  background: rgba(255, 255, 255, 0.05);
  border: 1px solid var(--border-subtle);
  color: var(--text-secondary);
  text-decoration: none;
  font-size: 0.84rem;
  font-weight: 600;
  margin-bottom: 2rem;
  transition: all var(--transition-fast);
}

.back-link-pill:hover {
  background: rgba(255, 255, 255, 0.1);
  color: #ffffff;
}

.profile-header {
  margin-bottom: 3.5rem;
}

.profile-eyebrow {
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

.profile-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5.5vw, 4.4rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 0.8rem;
}

.profile-subtitle {
  font-size: 1.05rem;
  color: var(--text-secondary);
}

.profile-cards-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 2.5rem;
  margin-bottom: 3rem;
}

.settings-card {
  padding: 2.5rem;
  border-radius: var(--radius-xl);
}

.card-title-row {
  display: flex;
  align-items: flex-start;
  gap: 1.2rem;
  margin-bottom: 2rem;
}

.card-icon {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.06);
  display: grid;
  place-items: center;
  font-size: 1.2rem;
  flex-shrink: 0;
}

.card-title-row h2 {
  font-size: 1.35rem;
  font-weight: 800;
  letter-spacing: -0.02em;
  color: #ffffff;
  margin-bottom: 0.25rem;
}

.card-title-row p {
  font-size: 0.84rem;
  color: var(--text-muted);
  margin: 0;
}

.settings-form {
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

.form-field input {
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

.form-field input:focus {
  border-color: var(--accent-purple-bright);
  background: rgba(255, 255, 255, 0.08);
}

.form-field input:disabled {
  opacity: 0.5;
  cursor: not-allowed;
  background: rgba(255, 255, 255, 0.02);
}

.field-hint {
  font-size: 0.72rem;
  color: var(--text-muted);
}

.save-btn {
  margin-top: 0.5rem;
  padding: 0.85rem;
}

.feedback-banner {
  margin-top: 1.25rem;
  padding: 0.75rem 1rem;
  border-radius: var(--radius-md);
  font-size: 0.84rem;
  font-weight: 600;
}

.feedback-banner.success {
  background: rgba(34, 197, 94, 0.15);
  border: 1px solid rgba(34, 197, 94, 0.35);
  color: #4ade80;
}

.feedback-banner.error {
  background: rgba(239, 68, 68, 0.15);
  border: 1px solid rgba(239, 68, 68, 0.35);
  color: #fca5a5;
}

.quick-links-strip {
  display: flex;
  gap: 1.5rem;
}

.quick-link-chip {
  padding: 1rem 1.6rem;
  border-radius: var(--radius-md);
  text-decoration: none;
  color: #ffffff;
  font-size: 0.88rem;
  font-weight: 600;
}

@media (max-width: 900px) {
  .profile-cards-grid {
    grid-template-columns: 1fr;
  }
}
</style>
