<template>
  <div class="account-addresses">
    <h1>My Addresses</h1>
    <button @click="showModal = true" class="btn-primary">Add Address</button>

    <div class="addresses-grid" v-if="addresses.length > 0">
      <div v-for="address in addresses" :key="address.id" class="address-card">
        <div class="address-header">
          <h3>{{ address.fullName }}</h3>
          <span v-if="address.isDefault" class="default-badge">Default</span>
        </div>
        <p>{{ address.phone }}</p>
        <p>{{ address.addressLine }}</p>
        <p>{{ address.city }}, {{ address.state }}</p>
        <p>{{ address.postalCode }}</p>
        <p>{{ address.country }}</p>
        <div class="address-actions">
          <button @click="editAddress(address)" class="btn-small">Edit</button>
          <button @click="deleteAddress(address.id)" class="btn-small btn-danger">Delete</button>
        </div>
      </div>
    </div>
    <p v-else>No addresses saved</p>

    <div class="modal" v-if="showModal">
      <div class="modal-content">
        <h2>{{ editingAddress ? 'Edit Address' : 'Add Address' }}</h2>
        <form @submit.prevent="saveAddress">
          <div class="form-group">
            <label>Full Name</label>
            <input v-model="form.fullName" required />
          </div>
          <div class="form-group">
            <label>Phone</label>
            <input v-model="form.phone" />
          </div>
          <div class="form-group">
            <label>Address Line</label>
            <input v-model="form.addressLine" required />
          </div>
          <div class="form-group">
            <label>City</label>
            <input v-model="form.city" required />
          </div>
          <div class="form-group">
            <label>State</label>
            <input v-model="form.state" required />
          </div>
          <div class="form-group">
            <label>Postal Code</label>
            <input v-model="form.postalCode" required />
          </div>
          <div class="form-group">
            <label>Country</label>
            <input v-model="form.country" required />
          </div>
          <div class="form-group">
            <label>
              <input type="checkbox" v-model="form.isDefault" />
              Set as Default
            </label>
          </div>
          <div class="form-actions">
            <button type="submit">Save</button>
            <button type="button" @click="closeModal">Cancel</button>
          </div>
        </form>
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
  country: '',
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
  if (confirm('Are you sure?')) {
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
    country: '',
    isDefault: false
  }
}

onMounted(() => {
  loadAddresses()
})
</script>

<style scoped>
.account-addresses {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.btn-primary {
  background: #e63946;
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
  margin-bottom: 1rem;
}

.addresses-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

.address-card {
  background: #f5f5f5;
  padding: 1.5rem;
  border-radius: 8px;
}

.address-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.default-badge {
  background: #2ecc71;
  color: white;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
}

.address-card p {
  margin: 0.25rem 0;
}

.address-actions {
  margin-top: 1rem;
  display: flex;
  gap: 0.5rem;
}

.btn-small {
  padding: 0.25rem 0.5rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

.btn-danger {
  background: #e63946;
  color: white;
}

.modal {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
}

.modal-content {
  background: white;
  padding: 2rem;
  border-radius: 8px;
  max-width: 500px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
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
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.form-actions {
  display: flex;
  gap: 1rem;
  margin-top: 1rem;
}

.form-actions button {
  padding: 0.75rem 1.5rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}
</style>
