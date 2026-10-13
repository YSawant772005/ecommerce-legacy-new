import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Order } from '../types'
import { ordersApi } from '../api/orders'

export const useOrdersStore = defineStore('orders', () => {
  const orders = ref<Order[]>([])
  const currentOrder = ref<Order | null>(null)
  const loading = ref(false)

  async function fetchOrders() {
    loading.value = true
    try {
      const response = await ordersApi.getOrders()
      orders.value = response.data
    } finally {
      loading.value = false
    }
  }

  async function fetchOrder(id: number) {
    loading.value = true
    try {
      const response = await ordersApi.getOrder(id)
      currentOrder.value = response.data
    } finally {
      loading.value = false
    }
  }

  async function createOrder(data: {
    email: string
    fullName: string
    phone?: string
    addressLine: string
    city: string
    state: string
    postalCode: string
    paymentMethod: string
    couponCode?: string
  }) {
    const response = await ordersApi.createOrder(data)
    return response.data
  }

  return {
    orders,
    currentOrder,
    loading,
    fetchOrders,
    fetchOrder,
    createOrder
  }
})
