import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { Cart } from '../types'
import { cartApi } from '../api/cart'
import { useNotificationsStore } from './notifications'

export const useCartStore = defineStore('cart', () => {
  const cart = ref<Cart | null>(null)
  const loading = ref(false)
  const addingProductIds = ref(new Set<number>())
  const notificationsStore = useNotificationsStore()

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

  async function addToCart(productId: number, qty: number = 1, variant?: string, productName?: string) {
    if (addingProductIds.value.has(productId)) return false
    addingProductIds.value.add(productId)
    try {
      await cartApi.addToCart({ productId, qty, variant })
      await fetchCart()
      notificationsStore.show(`${productName || 'Product'} added to cart successfully`)
      return true
    } catch (error: any) {
      const status = error.response?.status
      const message = status === 401
        ? 'Please sign in to add products to your cart.'
        : error.response?.data?.error || 'Unable to add this product to your cart. Please try again.'
      notificationsStore.show(message, 'error')
      return false
    } finally {
      addingProductIds.value.delete(productId)
    }
  }

  function isAdding(productId: number) {
    return addingProductIds.value.has(productId)
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
    isAdding,
    fetchCart,
    addToCart,
    updateCartItem,
    removeCartItem,
    clearCart,
    applyCoupon
  }
})
