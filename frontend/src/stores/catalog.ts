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
    try {
      const response = await productsApi.getProducts(filters)
      products.value = response.data.items
      totalCount.value = response.data.totalCount
      currentPage.value = response.data.page
      totalPages.value = response.data.totalPages
    } finally {
      loading.value = false
    }
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
    fetchCategories,
    fetchProducts,
    fetchProduct
  }
})
