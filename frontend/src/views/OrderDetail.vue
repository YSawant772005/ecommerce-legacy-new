<template>
  <div class="order-detail">
    <router-link to="/account/orders" class="back-link">← Back to orders</router-link>
    <p v-if="ordersStore.loading">Loading order...</p>
    <template v-else-if="ordersStore.currentOrder">
      <p class="eyebrow">ORDER DETAILS</p>
      <div class="heading-row">
        <div>
          <h1>#{{ ordersStore.currentOrder.orderNumber }}</h1>
          <p>Placed {{ new Date(ordersStore.currentOrder.createdOn).toLocaleDateString() }}</p>
        </div>
        <span class="status">{{ ordersStore.currentOrder.status }}</span>
      </div>
      <section class="order-card">
        <h2>Items</h2>
        <OrderItems :items="ordersStore.currentOrder.items" />
        <div class="total-row"><span>Total</span><strong>₹{{ ordersStore.currentOrder.total }}</strong></div>
      </section>
    </template>
    <p v-else>Order not found.</p>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useOrdersStore } from '../stores/orders'
import OrderItems from '../components/OrderItems.vue'

const route = useRoute()
const ordersStore = useOrdersStore()

onMounted(() => ordersStore.fetchOrder(Number(route.params.id)))
</script>

<style scoped>
.order-detail {
  min-height: calc(100vh - 100px);
  padding: 4rem max(1.5rem, calc((100% - 900px) / 2));
  color: #24145f;
  background: #fbfaff;
}
.back-link { color: #6d6389; text-decoration: none; font-size: .8rem; }
.eyebrow { color: #9654db; font-size: .68rem; font-weight: 800; letter-spacing: .18em; }
.heading-row { display: flex; justify-content: space-between; align-items: center; margin: 1rem 0 2rem; }
h1 { margin: .3rem 0; font-size: clamp(2.5rem, 6vw, 4.5rem); letter-spacing: -.08em; }
.heading-row p { color: #766e8f; }
.status { padding: .35rem .7rem; border-radius: 999px; background: #0fe1d1; font-size: .75rem; font-weight: 800; }
.order-card { padding: 1.5rem; border: 1px solid #e8e3f1; border-radius: 12px; background: #fff; }
.order-card h2 { margin-top: 0; }
.total-row { display: flex; justify-content: space-between; margin-top: 1.5rem; padding-top: 1rem; border-top: 1px solid #e8e3f1; font-size: 1.1rem; }
@media (max-width: 520px) { .heading-row { align-items: flex-start; gap: 1rem; } }
</style>
