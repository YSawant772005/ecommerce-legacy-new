<template>
  <div class="admin-coupons">
    <h1>Manage Coupons</h1>
    <button @click="showModal = true" class="btn-primary">Add Coupon</button>

    <table class="data-table">
      <thead>
        <tr>
          <th>Code</th>
          <th>Description</th>
          <th>Min Order</th>
          <th>Discount</th>
          <th>Free Shipping</th>
          <th>Active</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="coupon in coupons" :key="coupon.id">
          <td>{{ coupon.code }}</td>
          <td>{{ coupon.description }}</td>
          <td>₹{{ coupon.minOrderValue }}</td>
          <td>
            <span v-if="coupon.fixedAmount > 0">₹{{ coupon.fixedAmount }}</span>
            <span v-else-if="coupon.percentDiscount > 0">{{ coupon.percentDiscount }}%</span>
          </td>
          <td>{{ coupon.freeShipping ? 'Yes' : 'No' }}</td>
          <td>{{ coupon.isActive ? 'Yes' : 'No' }}</td>
          <td>
            <button @click="editCoupon(coupon)" class="btn-small">Edit</button>
            <button @click="deleteCoupon(coupon.id)" class="btn-small btn-danger">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="modal" v-if="showModal">
      <div class="modal-content">
        <h2>{{ editingCoupon ? 'Edit Coupon' : 'Add Coupon' }}</h2>
        <form @submit.prevent="saveCoupon">
          <div class="form-group">
            <label>Code</label>
            <input v-model="form.code" required />
          </div>
          <div class="form-group">
            <label>Description</label>
            <input v-model="form.description" />
          </div>
          <div class="form-group">
            <label>Min Order Value</label>
            <input v-model.number="form.minOrderValue" type="number" />
          </div>
          <div class="form-group">
            <label>Fixed Amount (₹)</label>
            <input v-model.number="form.fixedAmount" type="number" />
          </div>
          <div class="form-group">
            <label>Percent Discount (%)</label>
            <input v-model.number="form.percentDiscount" type="number" />
          </div>
          <div class="form-group">
            <label>Max Discount Amount (₹)</label>
            <input v-model.number="form.maxDiscountAmount" type="number" />
          </div>
          <div class="form-group">
            <label>
              <input type="checkbox" v-model="form.freeShipping" />
              Free Shipping
            </label>
          </div>
          <div class="form-group">
            <label>
              <input type="checkbox" v-model="form.isActive" />
              Active
            </label>
          </div>
          <div class="form-group">
            <label>Start Date</label>
            <input v-model="form.startsOn" type="date" />
          </div>
          <div class="form-group">
            <label>End Date</label>
            <input v-model="form.endsOn" type="date" />
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
import api from '../../api/client'

const coupons = ref<any[]>([])
const showModal = ref(false)
const editingCoupon = ref<any>(null)
const form = ref({
  id: 0,
  code: '',
  description: '',
  minOrderValue: 0,
  fixedAmount: 0,
  percentDiscount: 0,
  maxDiscountAmount: 0,
  freeShipping: false,
  isActive: true,
  startsOn: '',
  endsOn: ''
})

async function loadCoupons() {
  const response = await api.get('/admin/coupons')
  coupons.value = response.data
}

function editCoupon(coupon: any) {
  editingCoupon.value = coupon
  form.value = { 
    ...coupon,
    startsOn: coupon.startsOn ? coupon.startsOn.split('T')[0] : '',
    endsOn: coupon.endsOn ? coupon.endsOn.split('T')[0] : ''
  }
  showModal.value = true
}

async function saveCoupon() {
  const data = { ...form.value }
  if (data.startsOn) data.startsOn = new Date(data.startsOn).toISOString()
  if (data.endsOn) data.endsOn = new Date(data.endsOn).toISOString()
  
  if (editingCoupon.value) {
    await api.put(`/admin/coupons/${form.value.id}`, data)
  } else {
    await api.post('/admin/coupons', data)
  }
  closeModal()
  loadCoupons()
}

async function deleteCoupon(id: number) {
  if (confirm('Are you sure?')) {
    await api.delete(`/admin/coupons/${id}`)
    loadCoupons()
  }
}

function closeModal() {
  showModal.value = false
  editingCoupon.value = null
  form.value = {
    id: 0,
    code: '',
    description: '',
    minOrderValue: 0,
    fixedAmount: 0,
    percentDiscount: 0,
    maxDiscountAmount: 0,
    freeShipping: false,
    isActive: true,
    startsOn: '',
    endsOn: ''
  }
}

onMounted(() => {
  loadCoupons()
})
</script>

<style scoped>
.admin-coupons {
  padding: 2rem;
  max-width: 1400px;
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

.data-table {
  width: 100%;
  border-collapse: collapse;
}

.data-table th,
.data-table td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.data-table th {
  background: #667eea;
  color: white;
}

.btn-small {
  padding: 0.25rem 0.5rem;
  margin-right: 0.5rem;
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
