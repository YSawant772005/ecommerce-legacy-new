<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-lime">AUDIENCE AUDIT</span>
          <h1 class="admin-view-title">Newsletter Subscribers</h1>
        </div>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>Subscriber Email</th>
              <th>Subscribed On</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="subscriber in subscribers" :key="subscriber.id">
              <td><strong>{{ subscriber.email }}</strong></td>
              <td>{{ new Date(subscriber.subscribedOn).toLocaleDateString() }}</td>
              <td>
                <span class="badge-pill badge-lime">Subscribed</span>
              </td>
              <td>
                <button @click="deleteSubscriber(subscriber.id)" class="admin-action-btn delete">Unsubscribe / Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../../api/client'

const subscribers = ref<any[]>([])

async function loadSubscribers() {
  const response = await api.get('/admin/newsletter')
  subscribers.value = response.data
}

async function deleteSubscriber(id: number) {
  if (confirm('Are you sure you want to remove this subscriber?')) {
    await api.delete(`/admin/newsletter/${id}`)
    loadSubscribers()
  }
}

onMounted(() => {
  loadSubscribers()
})
</script>
