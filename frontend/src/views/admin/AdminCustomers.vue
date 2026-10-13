<template>
  <div class="admin-customers">
    <h1>Manage Customers</h1>

    <table class="data-table">
      <thead>
        <tr>
          <th>ID</th>
          <th>Name</th>
          <th>Email</th>
          <th>Phone</th>
          <th>Orders</th>
          <th>Total Spend</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="customer in customers" :key="customer.id">
          <td>{{ customer.id }}</td>
          <td>{{ customer.fullName }}</td>
          <td>{{ customer.email }}</td>
          <td>{{ customer.phone || '-' }}</td>
          <td>{{ customer.orderCount }}</td>
          <td>₹{{ customer.totalSpend }}</td>
        </tr>
      </tbody>
    </table>
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

<style scoped>
.admin-customers {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
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
</style>
