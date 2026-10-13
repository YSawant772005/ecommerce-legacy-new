import api from './client'
import type { Order } from '../types'

export const ordersApi = {
  createOrder: (data: {
    email: string
    fullName: string
    phone?: string
    addressLine: string
    city: string
    state: string
    postalCode: string
    paymentMethod: string
    couponCode?: string
  }) => api.post<Order>('/orders', data),

  getOrders: () => api.get<Order[]>('/orders'),

  getOrder: (id: number) => api.get<Order>(`/orders/${id}`)
}
