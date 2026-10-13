<template>
  <div class="admin-orders">
    <h1>Manage Orders</h1>

    <div class="filters">
      <select v-model="statusFilter" @change="loadOrders">
        <option value="">All Status</option>
        <option value="Placed">Placed</option>
        <option value="Packed">Packed</option>
        <option value="Shipped">Shipped</option>
        <option value="Delivered">Delivered</option>
        <option value="Cancelled">Cancelled</option>
      </select>
    </div>

    <table class="data-table">
      <thead>
        <tr>
          <th>Order #</th>
          <th>Email</th>
          <th>Total</th>
          <th>Status</th>
          <th>Date</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="order in orders" :key="order.id">
          <td>{{ order.orderNumber }}</td>
          <td>{{ order.email }}</td>
          <td>₹{{ order.total }}</td>
          <td>{{ order.status }}</td>
          <td>{{ new Date(order.createdOn).toLocaleDateString() }}</td>
          <td>
            <button @click="viewOrder(order)" class="btn-small">View</button>
            <select v-model="order.status" @change="updateStatus(order)" class="status-select">
              <option value="Placed">Placed</option>
              <option value="Packed">Packed</option>
              <option value="Shipped">Shipped</option>
              <option value="Delivered">Delivered</option>
              <option value="Cancelled">Cancelled</option>
            </select>
          </td>
        </tr>
      </tbody>
    </table>

    <div class="modal" v-if="selectedOrder">
      <div class="modal-content">
        <h2>Order #{{ selectedOrder.orderNumber }}</h2>
        <p><strong>Email:</strong> {{ selectedOrder.email }}</p>
        <p><strong>Total:</strong> ₹{{ selectedOrder.total }}</p>
        <p><strong>Status:</strong> {{ selectedOrder.status }}</p>
        <h3>Items</h3>
        <table class="items-table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Quantity</th>
              <th>Price</th>
              <th>Total</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="item in selectedOrder.items" :key="item.id">
              <td>{{ item.productName }}</td>
              <td>{{ item.quantity }}</td>
              <td>₹{{ item.unitPrice }}</td>
              <td>₹{{ item.lineTotal }}</td>
            </tr>
          </tbody>
        </table>
        <button @click="selectedOrder = null" class="btn-close">Close</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../../api/client'

const orders = ref<any[]>([])
const statusFilter = ref('')
const selectedOrder = ref<any>(null)

async function loadOrders() {
  const params: any = {}
  if (statusFilter.value) params.status = statusFilter.value
  const response = await api.get('/admin/orders', { params })
  orders.value = response.data.orders
}

async function updateStatus(order: any) {
  await api.put(`/admin/orders/${order.id}/status`, { status: order.status })
}

function viewOrder(order: any) {
  selectedOrder.value = order
}

onMounted(() => {
  loadOrders()
})
</script>

<style scoped>
.admin-orders {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

.filters {
  margin-bottom: 1rem;
}

.filters select {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
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

.status-select {
  padding: 0.25rem;
  border: 1px solid #ddd;
  border-radius: 4px;
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
  max-width: 600px;
  width: 90%;
  max-height: 90vh;
  overflow-y: auto;
}

.items-table {
  width: 100%;
  border-collapse: collapse;
  margin: 1rem 0;
}

.items-table th,
.items-table td {
  padding: 0.5rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.btn-close {
  background: #667eea;
  color: white;
  padding: 0.75rem 1.5rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}
</style>
