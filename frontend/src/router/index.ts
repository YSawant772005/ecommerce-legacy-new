import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('../views/Home.vue')
    },
    {
      path: '/products',
      name: 'products',
      component: () => import('../views/Products.vue')
    },
    {
      path: '/products/:id',
      name: 'product-detail',
      component: () => import('../views/ProductDetail.vue')
    },
    {
      path: '/cart',
      name: 'cart',
      component: () => import('../views/Cart.vue')
    },
    {
      path: '/checkout',
      name: 'checkout',
      meta: { requiresAuth: true },
      component: () => import('../views/Checkout.vue')
    },
    {
      path: '/login',
      name: 'login',
      meta: { guestOnly: true },
      component: () => import('../views/Login.vue')
    },
    {
      path: '/register',
      name: 'register',
      meta: { guestOnly: true },
      component: () => import('../views/Register.vue')
    },
    {
      path: '/account',
      name: 'account',
      meta: { requiresAuth: true },
      component: () => import('../views/Account.vue')
    },
    {
      path: '/account/profile',
      name: 'account-profile',
      meta: { requiresAuth: true },
      component: () => import('../views/AccountProfile.vue')
    },
    {
      path: '/account/orders',
      name: 'orders',
      meta: { requiresAuth: true },
      component: () => import('../views/Orders.vue')
    },
    {
      path: '/account/orders/:id',
      name: 'order-detail',
      meta: { requiresAuth: true },
      component: () => import('../views/OrderDetail.vue')
    },
    {
      path: '/account/addresses',
      name: 'account-addresses',
      meta: { requiresAuth: true },
      component: () => import('../views/AccountAddresses.vue')
    },
    {
      path: '/contact',
      name: 'contact',
      component: () => import('../views/Contact.vue')
    },
    {
      path: '/wishlist',
      name: 'wishlist',
      meta: { requiresAuth: true },
      component: () => import('../views/Wishlist.vue')
    },
    {
      path: '/admin',
      name: 'admin',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/Admin.vue')
    },
    {
      path: '/admin/products',
      name: 'admin-products',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminProducts.vue')
    },
    {
      path: '/admin/categories',
      name: 'admin-categories',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminCategories.vue')
    },
    {
      path: '/admin/brands',
      name: 'admin-brands',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminBrands.vue')
    },
    {
      path: '/admin/orders',
      name: 'admin-orders',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminOrders.vue')
    },
    {
      path: '/admin/customers',
      name: 'admin-customers',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminCustomers.vue')
    },
    {
      path: '/admin/coupons',
      name: 'admin-coupons',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminCoupons.vue')
    },
    {
      path: '/admin/reviews',
      name: 'admin-reviews',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminReviews.vue')
    },
    {
      path: '/admin/messages',
      name: 'admin-messages',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminMessages.vue')
    },
    {
      path: '/admin/newsletter',
      name: 'admin-newsletter',
      meta: { requiresAuth: true, requiresAdmin: true },
      component: () => import('../views/admin/AdminNewsletter.vue')
    }
  ]
})

router.beforeEach((to, _from, next) => {
  const authStore = useAuthStore()

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next('/login')
  } else if (to.meta.guestOnly && authStore.isAuthenticated) {
    next('/')
  } else if (to.meta.requiresAdmin && !authStore.isAdmin) {
    next('/')
  } else {
    next()
  }
})

export default router
