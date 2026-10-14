<template>
  <div class="addresses-page">
    <div class="container">
      <router-link to="/account" class="back-link-pill">
        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
          <line x1="19" y1="12" x2="5" y2="12"></line>
          <polyline points="12 19 5 12 12 5"></polyline>
        </svg>
        <span>Back to Account</span>
      </router-link>

      <header class="addresses-header">
        <div>
          <div class="addresses-eyebrow">
            <span class="eyebrow-dot"></span>
            <span>SHIPPING BOOK</span>
          </div>
          <h1 class="addresses-title">
            My Addresses<span class="dot-accent">.</span>
          </h1>
          <p class="addresses-subtitle">Saved shipping destinations for expedited, friction-free checkout.</p>
        </div>

        <button @click="showModal = true" class="btn-pill btn-pill-primary add-addr-btn">
          <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5">
            <line x1="12" y1="5" x2="12" y2="19"></line>
            <line x1="5" y1="12" x2="19" y2="12"></line>
          </svg>
          <span>Add New Address</span>
        </button>
      </header>

      <!-- Addresses Grid -->
      <div class="addresses-grid" v-if="addresses.length > 0">
        <div v-for="address in addresses" :key="address.id" class="address-tile glass-card">
          <div class="address-tile-top">
            <div class="name-block">
              <h3>{{ address.fullName }}</h3>
              <span v-if="address.isDefault" class="badge-pill default-pill">Default</span>
            </div>
          </div>

          <div class="address-body-lines">
            <p class="phone-line">📞 {{ address.phone || 'No phone provided' }}</p>
            <p class="street-line">{{ address.addressLine }}</p>
            <p class="region-line">{{ address.city }}, {{ address.state }} {{ address.postalCode }}</p>
            <p class="country-line">{{ address.country }}</p>
          </div>

          <div class="address-actions-bar">
            <button @click="editAddress(address)" class="addr-action-btn edit">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
              </svg>
              <span>Edit</span>
            </button>
            <button @click="deleteAddress(address.id)" class="addr-action-btn delete">
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                <polyline points="3 6 5 6 21 6"></polyline>
                <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
              </svg>
              <span>Delete</span>
            </button>
          </div>
        </div>
      </div>

      <div v-else class="empty-addr-card glass-panel">
        <div class="empty-icon">📍</div>
        <h2>No addresses saved yet</h2>
        <p>Add a primary address to speed through checkout on future purchases.</p>
        <button @click="showModal = true" class="btn-pill btn-pill-primary">Add Your First Address</button>
      </div>

      <!-- Modal Dialog -->
      <div class="modal-backdrop" v-if="showModal" @click.self="closeModal">
        <div class="modal-window glass-panel">
          <div class="modal-header">
            <h2>{{ editingAddress ? 'Edit Address' : 'Add New Address' }}</h2>
            <button class="close-modal-btn" @click="closeModal">✕</button>
          </div>

          <form @submit.prevent="saveAddress" class="modal-form">
            <div class="form-row-2">
              <div class="field-item">
                <label>Full Name</label>
                <input v-model="form.fullName" required placeholder="Full Name" />
              </div>
              <div class="field-item">
                <label>Phone Number</label>
                <input v-model="form.phone" placeholder="+91..." />
              </div>
            </div>

            <div class="field-item">
              <label>Street Address</label>
              <input v-model="form.addressLine" required placeholder="Flat / Building, Street" />
            </div>

            <div class="form-row-2">
              <div class="field-item">
                <label>City</label>
                <input v-model="form.city" required placeholder="City" />
              </div>
              <div class="field-item">
                <label>State</label>
                <input v-model="form.state" required placeholder="State" />
              </div>
            </div>

            <div class="form-row-2">
              <div class="field-item">
                <label>Postal Code</label>
                <input v-model="form.postalCode" required placeholder="PIN Code" />
              </div>
              <div class="field-item">
                <label>Country</label>
                <input v-model="form.country" required placeholder="Country" />
              </div>
            </div>

            <div class="checkbox-field">
              <label class="custom-checkbox-label">
                <input type="checkbox" v-model="form.isDefault" />
                <span>Set as default shipping address</span>
              </label>
            </div>

            <div class="modal-actions-row">
              <button type="button" @click="closeModal" class="btn-pill btn-pill-outline">Cancel</button>
              <button type="submit" class="btn-pill btn-pill-primary">Save Address</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { accountApi } from '../api/account'

const addresses = ref<any[]>([])
const showModal = ref(false)
const editingAddress = ref<any>(null)
const form = ref({
  id: 0,
  fullName: '',
  phone: '',
  addressLine: '',
  city: '',
  state: '',
  postalCode: '',
  country: 'India',
  isDefault: false
})

async function loadAddresses() {
  const response = await accountApi.getAddresses()
  addresses.value = response.data
}

function editAddress(address: any) {
  editingAddress.value = address
  form.value = { ...address }
  showModal.value = true
}

async function saveAddress() {
  if (editingAddress.value) {
    await accountApi.updateAddress(form.value.id, form.value)
  } else {
    await accountApi.addAddress(form.value)
  }
  closeModal()
  loadAddresses()
}

async function deleteAddress(id: number) {
  if (confirm('Are you sure you want to delete this address?')) {
    await accountApi.deleteAddress(id)
    loadAddresses()
  }
}

function closeModal() {
  showModal.value = false
  editingAddress.value = null
  form.value = {
    id: 0,
    fullName: '',
    phone: '',
    addressLine: '',
    city: '',
    state: '',
    postalCode: '',
    country: 'India',
    isDefault: false
  }
}

onMounted(() => {
  loadAddresses()
})
</script>

<style scoped>
.addresses-page {
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

.addresses-header {
  display: flex;
  align-items: flex-end;
  justify-content: space-between;
  margin-bottom: 3.5rem;
  gap: 2rem;
}

.addresses-eyebrow {
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

.addresses-title {
  font-family: var(--font-display);
  font-size: clamp(2.8rem, 5.5vw, 4.4rem);
  font-weight: 900;
  line-height: 0.95;
  letter-spacing: -0.04em;
  color: #ffffff;
  margin-bottom: 0.8rem;
}

.addresses-subtitle {
  font-size: 1.05rem;
  color: var(--text-secondary);
}

.add-addr-btn {
  padding: 0.85rem 1.6rem;
  white-space: nowrap;
}

/* Grid */
.addresses-grid {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1.75rem;
}

.address-tile {
  padding: 2rem;
  border-radius: var(--radius-xl);
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  min-height: 240px;
}

.name-block {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 1.25rem;
}

.name-block h3 {
  font-size: 1.25rem;
  font-weight: 800;
  color: #ffffff;
}

.default-pill {
  background: rgba(34, 197, 94, 0.15);
  color: #4ade80;
  border: 1px solid rgba(34, 197, 94, 0.35);
  font-size: 0.68rem;
}

.address-body-lines {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
  font-size: 0.88rem;
  color: var(--text-secondary);
  line-height: 1.5;
  margin-bottom: 1.8rem;
}

.phone-line {
  color: var(--accent-purple-bright);
  font-weight: 600;
  font-size: 0.84rem;
}

.address-actions-bar {
  display: flex;
  gap: 0.75rem;
  padding-top: 1.25rem;
  border-top: 1px solid var(--border-subtle);
}

.addr-action-btn {
  display: inline-flex;
  align-items: center;
  gap: 0.4rem;
  padding: 0.45rem 0.95rem;
  border-radius: var(--radius-pill);
  font-size: 0.78rem;
  font-weight: 700;
  border: 1px solid var(--border-subtle);
  background: rgba(255, 255, 255, 0.04);
  color: var(--text-secondary);
  cursor: pointer;
  transition: all var(--transition-fast);
}

.addr-action-btn.edit:hover {
  background: rgba(255, 255, 255, 0.12);
  color: #ffffff;
}

.addr-action-btn.delete:hover {
  background: rgba(239, 68, 68, 0.15);
  color: #f87171;
  border-color: rgba(239, 68, 68, 0.3);
}

/* Empty State */
.empty-addr-card {
  max-width: 500px;
  margin: 3rem auto;
  padding: 4.5rem 2rem;
  text-align: center;
  border-radius: var(--radius-xl);
}

.empty-icon {
  font-size: 2.5rem;
  margin-bottom: 1rem;
}

.empty-addr-card h2 {
  font-size: 1.6rem;
  color: #ffffff;
  margin-bottom: 0.6rem;
}

.empty-addr-card p {
  color: var(--text-secondary);
  font-size: 0.92rem;
  margin-bottom: 2rem;
}

/* Modal */
.modal-backdrop {
  position: fixed;
  inset: 0;
  z-index: 1000;
  background: rgba(0, 0, 0, 0.75);
  backdrop-filter: blur(12px);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 1.5rem;
}

.modal-window {
  width: 100%;
  max-width: 540px;
  padding: 2.5rem;
  border-radius: var(--radius-xl);
  box-shadow: 0 30px 80px rgba(0, 0, 0, 0.6);
}

.modal-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 2rem;
}

.modal-header h2 {
  font-size: 1.45rem;
  font-weight: 850;
  color: #ffffff;
}

.close-modal-btn {
  width: 32px;
  height: 32px;
  border-radius: 50%;
  border: 0;
  background: rgba(255, 255, 255, 0.08);
  color: #ffffff;
  cursor: pointer;
}

.modal-form {
  display: flex;
  flex-direction: column;
  gap: 1.2rem;
}

.form-row-2 {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1rem;
}

.field-item {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.field-item label {
  font-size: 0.8rem;
  font-weight: 600;
  color: var(--text-secondary);
}

.field-item input {
  padding: 0.8rem 1rem;
  border-radius: var(--radius-md);
  background: rgba(255, 255, 255, 0.04);
  border: 1px solid var(--border-medium);
  color: #ffffff;
  font-family: inherit;
  font-size: 0.88rem;
  outline: none;
}

.field-item input:focus {
  border-color: var(--accent-purple-bright);
}

.checkbox-field {
  padding: 0.5rem 0;
}

.custom-checkbox-label {
  display: flex;
  align-items: center;
  gap: 0.65rem;
  font-size: 0.85rem;
  color: var(--text-secondary);
  cursor: pointer;
}

.modal-actions-row {
  display: flex;
  justify-content: flex-end;
  gap: 1rem;
  margin-top: 1rem;
}

@media (max-width: 1024px) {
  .addresses-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 640px) {
  .addresses-header {
    flex-direction: column;
    align-items: flex-start;
  }
  .addresses-grid {
    grid-template-columns: 1fr;
  }
  .form-row-2 {
    grid-template-columns: 1fr;
  }
}
</style>
