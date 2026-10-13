export interface User {
  id: number
  fullName: string
  email: string
  role?: string
  phone?: string
}

export interface Product {
  id: number
  name: string
  slug: string
  price: number
  comparePrice?: number
  stock: number
  rating: number
  reviewCount: number
  imageUrl?: string
  categoryId: number
  categoryName?: string
  brandName?: string
  description?: string
  discountPercent: number
  inStock: boolean
  isFeatured: boolean
  isDeal: boolean
  isNew: boolean
}

export interface Category {
  id: number
  name: string
  slug: string
  tagline?: string
  imageUrl?: string
}

export interface CartItem {
  productId: number
  qty: number
  variant?: string
  productName: string
  imageUrl?: string
  unitPrice: number
  lineTotal: number
}

export interface Cart {
  items: CartItem[]
  couponCode?: string
  subtotal: number
  discount: number
  shipping: number
  tax: number
  total: number
}

export interface Order {
  id: number
  orderNumber: string
  email: string
  fullName: string
  subtotal: number
  discount: number
  shipping: number
  tax: number
  total: number
  status: string
  paymentMethod?: string
  couponCode?: string
  createdOn: string
  items: OrderItem[]
}

export interface OrderItem {
  id: number
  productName: string
  imageUrl?: string
  variant?: string
  unitPrice: number
  quantity: number
  lineTotal: number
}

export interface AuthResponse {
  accessToken: string
  refreshToken: string
  user: User
}
