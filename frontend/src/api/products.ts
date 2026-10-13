import api from './client'
import type { Product, Category } from '../types'

export interface ProductFilters {
  page?: number
  pageSize?: number
  categoryId?: number
  search?: string
  sort?: string
}

export interface PagedResponse<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export const productsApi = {
  getCategories: () => api.get<Category[]>('/products/categories'),

  getProducts: (filters: ProductFilters = {}) =>
    api.get<PagedResponse<Product>>('/products', { params: filters }),

  getProduct: (id: number) => api.get<Product>(`/products/${id}`)
}
