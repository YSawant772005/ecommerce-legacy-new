import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { User } from '../types'
import { authApi } from '../api/auth'

export const useAuthStore = defineStore('auth', () => {
  const user = ref<User | null>(null)
  const accessToken = ref<string | null>(localStorage.getItem('accessToken'))
  const refreshToken = ref<string | null>(localStorage.getItem('refreshToken'))

  const isAuthenticated = computed(() => !!user.value)
  const isAdmin = computed(() => user.value?.role === 'Admin')

  async function register(data: { fullName: string; email: string; password: string }) {
    const response = await authApi.register(data)
    setAuth(response.data)
  }

  async function login(data: { email: string; password: string }) {
    const response = await authApi.login(data)
    setAuth(response.data)
  }

  async function logout() {
    try {
      await authApi.logout()
    } finally {
      clearAuth()
    }
  }

  async function fetchCurrentUser() {
    if (!accessToken.value) return
    const response = await authApi.getCurrentUser()
    user.value = response.data
  }

  function setAuth(data: { accessToken: string; refreshToken: string; user: User }) {
    accessToken.value = data.accessToken
    refreshToken.value = data.refreshToken
    user.value = data.user
    localStorage.setItem('accessToken', data.accessToken)
    localStorage.setItem('refreshToken', data.refreshToken)
  }

  function clearAuth() {
    user.value = null
    accessToken.value = null
    refreshToken.value = null
    localStorage.removeItem('accessToken')
    localStorage.removeItem('refreshToken')
  }

  // Initialize from localStorage
  if (accessToken.value) {
    fetchCurrentUser()
  }

  return {
    user,
    accessToken,
    isAuthenticated,
    isAdmin,
    register,
    login,
    logout,
    fetchCurrentUser
  }
})
