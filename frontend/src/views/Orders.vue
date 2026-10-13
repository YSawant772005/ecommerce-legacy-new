<template>
  <div class="orders">
    <h1>My Orders</h1>

    <div v-if="!ordersStore.loading && ordersStore.orders.length > 0">
      <div v-for="order in ordersStore.orders" :key="order.id" class="order-card">
        <div class="order-header">
          <h3>Order #{{ order.orderNumber }}</h3>
          <span class="status">{{ order.status }}</span>
        </div>
        <p><strong>Date:</strong> {{ new Date(order.createdOn).toLocaleDateString() }}</p>
        <p><strong>Total:</strong> ₹{{ order.total }}</p>
        <p><strong>Items:</strong> {{ order.items.length }}</p>
      </div>
    </div>

    <p v-else-if="!ordersStore.loading">No orders yet</p>
    <p v-else>Loading...</p>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useOrdersStore } from '../stores/orders'

const ordersStore = useOrdersStore()

onMounted(() => {
  ordersStore.fetchOrders()
})
</script>

<style scoped>
.orders {
  padding: 2rem;
  max-width: 800px;
  margin: 0 auto;
}

.order-card {
  background: #f5f5f5;
  padding: 1.5rem;
  border-radius: 8px;
  margin-bottom: 1rem;
}

.order-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.status {
  background: #667eea;
  color: white;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
}

.order-card p {
  margin: 0.5rem 0;
}
</style>
