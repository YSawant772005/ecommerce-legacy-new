<template>
  <div class="toast-region" aria-live="polite" aria-atomic="true">
    <TransitionGroup name="toast">
      <div
        v-for="notification in notificationsStore.notifications"
        :key="notification.id"
        class="toast"
        :class="`toast-${notification.type}`"
        role="status"
      >
        <span class="toast-icon" aria-hidden="true">
          <svg v-if="notification.type === 'success'" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
            <polyline points="20 6 9 17 4 12"></polyline>
          </svg>
          <svg v-else width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="3" stroke-linecap="round" stroke-linejoin="round">
            <circle cx="12" cy="12" r="10"></circle>
            <line x1="12" y1="8" x2="12" y2="12"></line>
            <line x1="12" y1="16" x2="12.01" y2="16"></line>
          </svg>
        </span>
        <span class="toast-message">{{ notification.message }}</span>
        <button type="button" class="dismiss-btn" aria-label="Dismiss notification" @click="notificationsStore.dismiss(notification.id)">
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" stroke-linecap="round" stroke-linejoin="round">
            <line x1="18" y1="6" x2="6" y2="18"></line>
            <line x1="6" y1="6" x2="18" y2="18"></line>
          </svg>
        </button>
      </div>
    </TransitionGroup>
  </div>
</template>

<script setup lang="ts">
import { useNotificationsStore } from '../stores/notifications'

const notificationsStore = useNotificationsStore()
</script>

<style scoped>
.toast-region {
  position: fixed;
  bottom: 1.5rem;
  right: 1.5rem;
  z-index: 9999;
  display: grid;
  gap: 0.75rem;
  width: min(400px, calc(100vw - 2rem));
  pointer-events: none;
}

.toast {
  pointer-events: auto;
  display: flex;
  align-items: center;
  gap: 0.85rem;
  padding: 0.95rem 1.2rem;
  border-radius: var(--radius-md);
  color: #ffffff;
  backdrop-filter: blur(20px);
  -webkit-backdrop-filter: blur(20px);
  font-size: 0.86rem;
  font-weight: 600;
  letter-spacing: -0.01em;
  box-shadow: 0 20px 40px rgba(0, 0, 0, 0.4), 0 0 0 1px rgba(255, 255, 255, 0.1);
  transition: all 0.3s cubic-bezier(0.16, 1, 0.3, 1);
}

.toast-success {
  background: rgba(14, 28, 22, 0.88);
  border: 1px solid rgba(34, 197, 94, 0.35);
  box-shadow: 0 16px 36px rgba(0, 0, 0, 0.45), 0 0 24px rgba(34, 197, 94, 0.2);
}

.toast-error {
  background: rgba(38, 14, 20, 0.88);
  border: 1px solid rgba(244, 63, 94, 0.35);
  box-shadow: 0 16px 36px rgba(0, 0, 0, 0.45), 0 0 24px rgba(244, 63, 94, 0.2);
}

.toast-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 24px;
  height: 24px;
  border-radius: 50%;
  flex-shrink: 0;
}

.toast-success .toast-icon {
  background: rgba(34, 197, 94, 0.2);
  color: #4ade80;
  border: 1px solid rgba(34, 197, 94, 0.4);
}

.toast-error .toast-icon {
  background: rgba(244, 63, 94, 0.2);
  color: #fb7185;
  border: 1px solid rgba(244, 63, 94, 0.4);
}

.toast-message {
  flex: 1;
  line-height: 1.4;
}

.dismiss-btn {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 24px;
  height: 24px;
  padding: 0;
  border: 0;
  background: rgba(255, 255, 255, 0.08);
  color: rgba(255, 255, 255, 0.7);
  border-radius: 50%;
  cursor: pointer;
  transition: all 0.2s ease;
}

.dismiss-btn:hover {
  background: rgba(255, 255, 255, 0.2);
  color: #ffffff;
}

.toast-enter-active,
.toast-leave-active {
  transition: all 0.35s cubic-bezier(0.16, 1, 0.3, 1);
}

.toast-enter-from {
  opacity: 0;
  transform: translateY(20px) scale(0.95);
}

.toast-leave-to {
  opacity: 0;
  transform: translateX(30px) scale(0.95);
}
</style>
