import api from './client'

export const wishlistApi = {
  getWishlist: () => api.get('/wishlist'),

  addToWishlist: (productId: number) => api.post(`/wishlist/${productId}`),

  removeFromWishlist: (productId: number) => api.delete(`/wishlist/${productId}`)
}
