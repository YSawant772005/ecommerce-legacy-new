<template>
  <div class="order-items-list">
    <div v-for="item in items" :key="item.id" class="order-item-row glass-card">
      <div class="item-img-container">
        <img 
          :src="getItemImage(item)" 
          :alt="item.productName" 
          @error="useFallbackImage" 
          loading="lazy"
        />
      </div>

      <div class="item-details-box">
        <strong class="item-name">{{ item.productName }}</strong>
        <div class="item-attributes">
          <span v-if="item.variant" class="variant-chip">{{ item.variant }}</span>
          <span class="qty-tag">Qty: {{ item.quantity }}</span>
          <span class="unit-price-tag">₹{{ item.unitPrice.toLocaleString('en-IN') }} each</span>
        </div>
      </div>

      <div class="item-pricing-box">
        <span class="line-total-amount">₹{{ item.lineTotal.toLocaleString('en-IN') }}</span>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import type { OrderItem } from '../types'
import { productImage } from '../utils/productImage'

defineProps<{ items: OrderItem[] }>()

const fallbackImage = 'data:image/svg+xml,%3Csvg xmlns="http://www.w3.org/2000/svg" width="160" height="160" viewBox="0 0 160 160"%3E%3Crect width="160" height="160" fill="%23191827"/%3E%3Ctext x="80" y="86" text-anchor="middle" font-family="Arial" font-size="16" fill="%23ffffff"%3ENova%3C/text%3E%3C/svg%3E'

function getItemImage(item: OrderItem): string {
  if (item.imageUrl && !item.imageUrl.includes('placehold.co')) {
    return item.imageUrl
  }
  return productImage({
    id: item.id,
    name: item.productName,
    imageUrl: item.imageUrl
  })
}

function useFallbackImage(event: Event) {
  const image = event.target as HTMLImageElement
  if (image.src !== fallbackImage) image.src = fallbackImage
}
</script>

<style scoped>
.order-items-list {
  display: flex;
  flex-direction: column;
  gap: 0.85rem;
}

.order-item-row {
  display: grid;
  grid-template-columns: 76px 1fr auto;
  gap: 1.25rem;
  align-items: center;
  padding: 1rem 1.25rem;
  border-radius: var(--radius-md);
  background: rgba(255, 255, 255, 0.03);
  border: 1px solid var(--border-subtle);
}

.item-img-container {
  width: 76px;
  height: 76px;
  border-radius: var(--radius-sm);
  overflow: hidden;
  background: #0d0c15;
  flex-shrink: 0;
}

.item-img-container img {
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.item-details-box {
  display: flex;
  flex-direction: column;
  gap: 0.4rem;
}

.item-name {
  font-size: 0.98rem;
  font-weight: 700;
  color: #ffffff;
  letter-spacing: -0.01em;
}

.item-attributes {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex-wrap: wrap;
  font-size: 0.78rem;
  color: var(--text-muted);
}

.variant-chip {
  padding: 0.15rem 0.55rem;
  border-radius: var(--radius-pill);
  background: rgba(168, 85, 247, 0.15);
  border: 1px solid rgba(168, 85, 247, 0.3);
  color: #c084fc;
  font-weight: 600;
}

.qty-tag {
  color: var(--text-secondary);
  font-weight: 600;
}

.unit-price-tag {
  color: var(--text-muted);
}

.item-pricing-box {
  text-align: right;
}

.line-total-amount {
  font-size: 1.15rem;
  font-weight: 800;
  color: #ffffff;
}

@media (max-width: 560px) {
  .order-item-row {
    grid-template-columns: 60px 1fr;
    gap: 0.85rem;
  }
  .item-img-container {
    width: 60px;
    height: 60px;
  }
  .item-pricing-box {
    grid-column: 2;
    text-align: left;
    margin-top: 0.25rem;
  }
}
</style>
