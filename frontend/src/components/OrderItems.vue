<template>
  <div class="order-items">
    <div v-for="item in items" :key="item.id" class="order-item">
      <img :src="item.imageUrl || fallbackImage" :alt="item.productName" @error="useFallbackImage" />
      <div class="order-item-info">
        <strong>{{ item.productName }}</strong>
        <span v-if="item.variant">Variant: {{ item.variant }}</span>
        <span>Quantity: {{ item.quantity }}</span>
      </div>
      <div class="order-item-pricing">
        <span>₹{{ item.unitPrice }} each</span>
        <strong>₹{{ item.lineTotal }}</strong>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { OrderItem } from '../types'

defineProps<{ items: OrderItem[] }>()

const fallbackImage = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" width="160" height="160" viewBox="0 0 160 160"%3E%3Crect width="160" height="160" fill="%23eeeaf7"/%3E%3Ctext x="80" y="86" text-anchor="middle" font-family="Arial" font-size="16" fill="%2324145f"%3ENovaKart%3C/text%3E%3C/svg%3E'

function useFallbackImage(event: Event) {
  const image = event.target as HTMLImageElement
  if (image.src !== fallbackImage) image.src = fallbackImage
}
</script>

<style scoped>
.order-items { display: grid; gap: .75rem; }
.order-item {
  display: grid;
  grid-template-columns: 72px 1fr auto;
  gap: 1rem;
  align-items: center;
  padding: .75rem;
  border: 1px solid #eeeaf7;
  border-radius: 10px;
  background: #fff;
}
.order-item img {
  width: 72px;
  height: 72px;
  border-radius: 7px;
  object-fit: cover;
  background: #eeeaf7;
}
.order-item-info, .order-item-pricing { display: grid; gap: .3rem; }
.order-item-info span, .order-item-pricing span { color: #766e8f; font-size: .78rem; }
.order-item-pricing { text-align: right; white-space: nowrap; }
@media (max-width: 520px) {
  .order-item { grid-template-columns: 60px 1fr; gap: .75rem; }
  .order-item img { width: 60px; height: 60px; }
  .order-item-pricing { grid-column: 2; text-align: left; display: flex; justify-content: space-between; }
}
</style>
