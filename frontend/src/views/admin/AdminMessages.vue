<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-purple">SUPPORT INBOX</span>
          <h1 class="admin-view-title">Contact Messages</h1>
        </div>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>From</th>
              <th>Email</th>
              <th>Subject</th>
              <th>Message Content</th>
              <th>Received</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="message in messages" :key="message.id">
              <td><strong>{{ message.fullName }}</strong></td>
              <td>{{ message.email }}</td>
              <td><strong>{{ message.subject }}</strong></td>
              <td style="max-width: 320px; color: var(--text-muted);">{{ message.body }}</td>
              <td>{{ new Date(message.createdOn).toLocaleDateString() }}</td>
              <td>
                <button @click="deleteMessage(message.id)" class="admin-action-btn delete">Delete</button>
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

const messages = ref<any[]>([])

async function loadMessages() {
  const response = await api.get('/admin/messages')
  messages.value = response.data
}

async function deleteMessage(id: number) {
  if (confirm('Are you sure you want to delete this message?')) {
    await api.delete(`/admin/messages/${id}`)
    loadMessages()
  }
}

onMounted(() => {
  loadMessages()
})
</script>
