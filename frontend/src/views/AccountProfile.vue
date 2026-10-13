<template>
  <div class="account-profile">
    <h1>My Profile</h1>
    <div class="profile-section">
      <h2>Personal Information</h2>
      <form @submit.prevent="updateProfile">
        <div class="form-group">
          <label>Full Name</label>
          <input v-model="form.fullName" required />
        </div>
        <div class="form-group">
          <label>Email</label>
          <input v-model="form.email" disabled />
        </div>
        <div class="form-group">
          <label>Phone</label>
          <input v-model="form.phone" />
        </div>
        <button type="submit" :disabled="loading">
          {{ loading ? 'Saving...' : 'Save Changes' }}
        </button>
      </form>
    </div>

    <div class="profile-section">
      <h2>Change Password</h2>
      <form @submit.prevent="changePassword">
        <div class="form-group">
          <label>Current Password</label>
          <input v-model="passwordForm.currentPassword" type="password" required />
        </div>
        <div class="form-group">
          <label>New Password</label>
          <input v-model="passwordForm.newPassword" type="password" required />
        </div>
        <button type="submit" :disabled="passwordLoading">
          {{ passwordLoading ? 'Changing...' : 'Change Password' }}
        </button>
      </form>
    </div>

    <div class="links">
      <router-link to="/account/addresses" class="btn">Manage Addresses</router-link>
      <router-link to="/account/orders" class="btn">View Orders</router-link>
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

async function loadProfile() {
  const response = await accountApi.getProfile()
  form.value = response.data
}

async function updateProfile() {
  loading.value = true
  try {
    await accountApi.updateProfile({
      fullName: form.value.fullName,
      phone: form.value.phone
    })
    alert('Profile updated successfully')
  } catch (e) {
    alert('Failed to update profile')
  } finally {
    loading.value = false
  }
}

async function changePassword() {
  passwordLoading.value = true
  try {
    await accountApi.changePassword(passwordForm.value)
    alert('Password changed successfully')
    passwordForm.value = { currentPassword: '', newPassword: '' }
  } catch (e) {
    alert('Failed to change password')
  } finally {
    passwordLoading.value = false
  }
}

onMounted(() => {
  loadProfile()
})
</script>

<style scoped>
.account-profile {
  padding: 2rem;
  max-width: 800px;
  margin: 0 auto;
}

.profile-section {
  background: #f5f5f5;
  padding: 2rem;
  border-radius: 8px;
  margin-bottom: 2rem;
}

.profile-section h2 {
  margin-bottom: 1rem;
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
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.form-group input:disabled {
  background: #e0e0e0;
}

.profile-section button {
  background: #e63946;
  color: white;
  padding: 0.75rem 2rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

.profile-section button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.links {
  display: flex;
  gap: 1rem;
}

.btn {
  background: #667eea;
  color: white;
  padding: 0.75rem 1.5rem;
  text-decoration: none;
  border-radius: 4px;
}
</style>
