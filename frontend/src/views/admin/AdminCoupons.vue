<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-lime">PROMOTIONAL ENGINE</span>
          <h1 class="admin-view-title">Manage Coupons</h1>
        </div>
        <button @click="showModal = true" class="btn-pill btn-pill-primary">
          <span>+ Add Coupon</span>
        </button>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>Promo Code</th>
              <th>Description</th>
              <th>Min Order</th>
              <th>Discount Value</th>
              <th>Free Shipping</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="coupon in coupons" :key="coupon.id">
              <td><strong><code>{{ coupon.code }}</code></strong></td>
              <td>{{ coupon.description || '—' }}</td>
              <td>₹{{ coupon.minOrderValue.toLocaleString('en-IN') }}</td>
              <td>
                <span v-if="coupon.fixedAmount > 0" class="badge-pill badge-lime">₹{{ coupon.fixedAmount }} OFF</span>
                <span v-else-if="coupon.percentDiscount > 0" class="badge-pill badge-purple">{{ coupon.percentDiscount }}% OFF</span>
                <span v-else>—</span>
              </td>
              <td>{{ coupon.freeShipping ? '✓ Yes' : 'No' }}</td>
              <td>
                <span class="badge-pill" :class="coupon.isActive ? 'badge-lime' : 'badge-discount'">
                  {{ coupon.isActive ? 'Active' : 'Inactive' }}
                </span>
              </td>
              <td>
                <button @click="editCoupon(coupon)" class="admin-action-btn">Edit</button>
                <button @click="deleteCoupon(coupon.id)" class="admin-action-btn delete">Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <!-- Modal -->
      <div class="admin-modal-backdrop" v-if="showModal" @click.self="closeModal">
        <div class="admin-modal-window">
          <div class="admin-modal-header">
            <h2>{{ editingCoupon ? 'Edit Coupon' : 'Add Promo Coupon' }}</h2>
            <button class="admin-action-btn" @click="closeModal">✕</button>
          </div>

          <form @submit.prevent="saveCoupon">
            <div class="admin-form-group">
              <label>Promo Code</label>
              <input v-model="form.code" required placeholder="e.g. NOVA20" />
            </div>

            <div class="admin-form-group">
              <label>Description</label>
              <input v-model="form.description" placeholder="e.g. 20% off on all autumn drops" />
            </div>

            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
              <div class="admin-form-group">
                <label>Min Order Value (₹)</label>
                <input v-model.number="form.minOrderValue" type="number" />
              </div>

              <div class="admin-form-group">
                <label>Fixed Discount (₹)</label>
                <input v-model.number="form.fixedAmount" type="number" />
              </div>
            </div>

            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
              <div class="admin-form-group">
                <label>Percent Discount (%)</label>
                <input v-model.number="form.percentDiscount" type="number" />
              </div>

              <div class="admin-form-group">
                <label>Max Discount Amount (₹)</label>
                <input v-model.number="form.maxDiscountAmount" type="number" />
              </div>
            </div>

            <div style="display: flex; gap: 1.5rem; margin: 0.8rem 0;">
              <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer; font-size: 0.88rem;">
                <input type="checkbox" v-model="form.freeShipping" />
                Include Free Shipping
              </label>
              <label style="display: flex; align-items: center; gap: 0.5rem; cursor: pointer; font-size: 0.88rem;">
                <input type="checkbox" v-model="form.isActive" />
                Active on Checkout
              </label>
            </div>

            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 1rem;">
              <div class="admin-form-group">
                <label>Start Date</label>
                <input v-model="form.startsOn" type="date" />
              </div>

              <div class="admin-form-group">
                <label>End Date</label>
                <input v-model="form.endsOn" type="date" />
              </div>
            </div>

            <div class="admin-form-actions">
              <button type="button" @click="closeModal" class="btn-pill btn-pill-outline">Cancel</button>
              <button type="submit" class="btn-pill btn-pill-primary">Save Coupon</button>
            </div>
          </form>
        </div>
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
  if (confirm('Are you sure you want to delete this coupon?')) {
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
