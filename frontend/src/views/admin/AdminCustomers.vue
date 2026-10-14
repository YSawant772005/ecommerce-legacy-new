<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-purple">CUSTOMER DIRECTORY</span>
          <h1 class="admin-view-title">Manage Customers</h1>
        </div>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>ID</th>
              <th>Customer Name</th>
              <th>Email</th>
              <th>Phone</th>
              <th>Orders Count</th>
              <th>Lifetime Spend</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="customer in customers" :key="customer.id">
              <td><strong>#{{ customer.id }}</strong></td>
              <td><strong>{{ customer.fullName }}</strong></td>
              <td>{{ customer.email }}</td>
              <td>{{ customer.phone || '—' }}</td>
              <td>
                <span class="badge-pill badge-lime">{{ customer.orderCount }} orders</span>
              </td>
              <td><strong>₹{{ customer.totalSpend.toLocaleString('en-IN') }}</strong></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../../api/client'

const customers = ref<any[]>([])

async function loadCustomers() {
  const response = await api.get('/admin/customers')
  customers.value = response.data.customers
}

onMounted(() => {
  loadCustomers()
})
</script>
