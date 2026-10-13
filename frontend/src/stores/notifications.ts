import { defineStore } from 'pinia'
import { ref } from 'vue'

export type NotificationType = 'success' | 'error'

export interface Notification {
  id: number
  message: string
  type: NotificationType
}

export const useNotificationsStore = defineStore('notifications', () => {
  const notifications = ref<Notification[]>([])
  let nextId = 0

  function show(message: string, type: NotificationType = 'success', duration = 4000) {
    const id = ++nextId
    notifications.value.push({ id, message, type })
    window.setTimeout(() => dismiss(id), duration)
  }

  function dismiss(id: number) {
    notifications.value = notifications.value.filter(notification => notification.id !== id)
  }

  return { notifications, show, dismiss }
})
