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
        <span class="toast-icon" aria-hidden="true">{{ notification.type === 'success' ? '✓' : '!' }}</span>
        <span>{{ notification.message }}</span>
        <button type="button" aria-label="Dismiss notification" @click="notificationsStore.dismiss(notification.id)">×</button>
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
  top: 1rem;
  right: 1rem;
  z-index: 1000;
  display: grid;
  gap: .7rem;
  width: min(380px, calc(100vw - 2rem));
}

.toast {
  display: flex;
  align-items: center;
  gap: .7rem;
  padding: .9rem 1rem;
  border-radius: 10px;
  color: #fff;
  box-shadow: 0 10px 28px rgba(36, 20, 95, .2);
  font-size: .85rem;
  font-weight: 700;
}

.toast-success { background: #217a54; }
.toast-error { background: #b42318; }
.toast-icon {
  display: grid;
  flex: 0 0 1.4rem;
  place-items: center;
  width: 1.4rem;
  height: 1.4rem;
  border: 1px solid rgba(255,255,255,.7);
  border-radius: 50%;
}
.toast button {
  margin-left: auto;
  padding: 0 .2rem;
  border: 0;
  background: transparent;
  color: inherit;
  cursor: pointer;
  font-size: 1.25rem;
  line-height: 1;
}
.toast-enter-active, .toast-leave-active { transition: all .2s ease; }
.toast-enter-from, .toast-leave-to { opacity: 0; transform: translateY(-8px); }
</style>
