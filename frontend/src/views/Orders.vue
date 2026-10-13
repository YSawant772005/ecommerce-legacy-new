<template>
  <div class="orders">
    <p class="eyebrow">YOUR NOVAKART</p><h1>Order history</h1>

    <div v-if="!ordersStore.loading && ordersStore.orders.length > 0">
      <div v-for="order in ordersStore.orders" :key="order.id" class="order-card">
        <div class="order-header">
        <div><small>ORDER PLACED {{ new Date(order.createdOn).toLocaleDateString() }}</small><h3>#{{ order.orderNumber }}</h3></div>
          <span class="status">{{ order.status }}</span>
        </div>
        <p>{{ order.items.length }} item{{ order.items.length === 1 ? '' : 's' }} <span>·</span> ₹{{ order.total }}</p>
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
  padding: 4rem max(1.5rem, calc((100% - 900px) / 2));
  margin: 0 auto;
  color:#24145f; background:#fbfaff; min-height:calc(100vh - 100px);
}
.eyebrow { color:#9654db; font-size:.68rem; font-weight:800; letter-spacing:.18em; }
.orders h1 { font-size:clamp(2.8rem,6vw,5rem); letter-spacing:-.08em; margin:.5rem 0 3rem; }

.order-card {
  background: #fff; border:1px solid #e8e3f1; padding:1.5rem 2rem;
  border-radius: 8px;
  margin-bottom: 1rem;
}

.order-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}
.order-header small { color:#9654db; font-size:.65rem; font-weight:800; letter-spacing:.12em; }
.order-header h3 { margin:.45rem 0 0; }

.status {
  background: #0fe1d1; color:#24145f;
  color: white;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-size: 0.8rem;
}

.order-card p {
  margin: 0.5rem 0;
}
.order-card p span { color:#9654db; margin:0 .4rem; }
</style>
