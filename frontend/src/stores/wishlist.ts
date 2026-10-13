import { defineStore } from 'pinia'
import { ref } from 'vue'
import { wishlistApi } from '../api/wishlist'

export const useWishlistStore = defineStore('wishlist', () => {
  const items = ref<any[]>([])
  const loading = ref(false)

  async function fetchWishlist() {
    loading.value = true
    try {
      const response = await wishlistApi.getWishlist()
      items.value = response.data
    } finally {
      loading.value = false
    }
  }

  async function toggleWishlist(productId: number) {
    await wishlistApi.addToWishlist(productId)
    await fetchWishlist()
  }

  async function removeFromWishlist(productId: number) {
    await wishlistApi.removeFromWishlist(productId)
    await fetchWishlist()
  }

  return {
    items,
    loading,
    fetchWishlist,
    toggleWishlist,
    removeFromWishlist
  }
})
