import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Cart } from '../types'
import { cartApi } from '../api/cart'

export const useCartStore = defineStore('cart', () => {
  const cart = ref<Cart | null>(null)
  const loading = ref(false)

  const itemCount = computed(() => cart.value?.items.reduce((sum: number, item) => sum + item.qty, 0) || 0)
  const subtotal = computed(() => cart.value?.subtotal || 0)

  async function fetchCart() {
    loading.value = true
    try {
      const response = await cartApi.getCart()
      cart.value = response.data
    } finally {
      loading.value = false
    }
  }

  async function addToCart(productId: number, qty: number = 1, variant?: string) {
    await cartApi.addToCart({ productId, qty, variant })
    await fetchCart()
  }

  async function updateCartItem(productId: number, qty: number) {
    await cartApi.updateCartItem(productId, { qty })
    await fetchCart()
  }

  async function removeCartItem(productId: number) {
    await cartApi.removeCartItem(productId)
    await fetchCart()
  }

  async function clearCart() {
    await cartApi.clearCart()
    await fetchCart()
  }

  async function applyCoupon(couponCode: string) {
    await cartApi.applyCoupon({ couponCode })
    await fetchCart()
  }

  return {
    cart,
    loading,
    itemCount,
    subtotal,
    fetchCart,
    addToCart,
    updateCartItem,
    removeCartItem,
    clearCart,
    applyCoupon
  }
})
