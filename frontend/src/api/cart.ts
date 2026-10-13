import api from './client'
import type { Cart } from '../types'

export const cartApi = {
  getCart: () => api.get<Cart>('/cart'),

  addToCart: (data: { productId: number; qty: number; variant?: string }) =>
    api.post('/cart/items', data),

  updateCartItem: (productId: number, data: { qty: number }) =>
    api.put(`/cart/items/${productId}`, data),

  removeCartItem: (productId: number) =>
    api.delete(`/cart/items/${productId}`),

  clearCart: () => api.delete('/cart'),

  applyCoupon: (data: { couponCode: string }) =>
    api.post('/cart/coupon', data)
}
