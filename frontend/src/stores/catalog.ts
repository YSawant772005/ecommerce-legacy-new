import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Product, Category } from '../types'
import { productsApi } from '../api/products'

export const useCatalogStore = defineStore('catalog', () => {
  const products = ref<Product[]>([])
  const categories = ref<Category[]>([])
  const loading = ref(false)
  const totalCount = ref(0)
  const currentPage = ref(1)
  const totalPages = ref(0)
  const error = ref('')
  let requestId = 0

  async function fetchCategories() {
    const response = await productsApi.getCategories()
    categories.value = response.data
  }

  async function fetchProducts(filters: {
    page?: number
    pageSize?: number
    categoryId?: number
    search?: string
    sort?: string
  } = {}) {
    loading.value = true
    error.value = ''
    const currentRequest = ++requestId
    try {
      const response = await productsApi.getProducts(filters)
      if (currentRequest !== requestId) return false
      products.value = response.data.items
      totalCount.value = response.data.totalCount
      currentPage.value = response.data.page
      totalPages.value = response.data.totalPages
      return true
    } catch (requestError) {
      error.value = 'We could not load these products.'
      throw requestError
    } finally {
      if (currentRequest === requestId) loading.value = false
    }

  }

  async function appendProducts(filters: {
    page: number
    pageSize?: number
    categoryId?: number
    search?: string
    sort?: string
  }) {
    if (loading.value || (filters.page > totalPages.value && totalPages.value > 0)) return false
    loading.value = true
    error.value = ''
    const currentRequest = ++requestId
    try {
      const response = await productsApi.getProducts(filters)
      if (currentRequest !== requestId) return false
      const existingIds = new Set(products.value.map(product => product.id))
      const newItems = response.data.items.filter(product => !existingIds.has(product.id))
      products.value = [...products.value, ...newItems]
      totalCount.value = response.data.totalCount
      currentPage.value = response.data.page
      totalPages.value = response.data.totalPages
      return true
    } catch (requestError) {
      error.value = 'We could not load more products.'
      throw requestError
    } finally {
      if (currentRequest === requestId) loading.value = false
    }

  }

  function clearError() {
    error.value = ''
  }

  async function fetchProduct(id: number) {
    const response = await productsApi.getProduct(id)
    return response.data
  }

  return {
    products,
    categories,
    loading,
    totalCount,
    currentPage,
    totalPages,
    error,
    fetchCategories,
    fetchProducts,
    appendProducts,
    clearError,
    fetchProduct
  }
})
