<template>
  <div class="admin-messages">
    <h1>Contact Messages</h1>

    <table class="data-table">
      <thead>
        <tr>
          <th>Name</th>
          <th>Email</th>
          <th>Subject</th>
          <th>Message</th>
          <th>Date</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="message in messages" :key="message.id">
          <td>{{ message.fullName }}</td>
          <td>{{ message.email }}</td>
          <td>{{ message.subject }}</td>
          <td>{{ message.body }}</td>
          <td>{{ new Date(message.createdOn).toLocaleDateString() }}</td>
          <td>
            <button @click="deleteMessage(message.id)" class="btn-small btn-danger">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
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
  if (confirm('Are you sure?')) {
    await api.delete(`/admin/messages/${id}`)
    loadMessages()
  }
}

onMounted(() => {
  loadMessages()
})
</script>

<style scoped>
.admin-messages {
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
}

.data-table {
  width: 100%;
  border-collapse: collapse;
}

.data-table th,
.data-table td {
  padding: 0.75rem;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.data-table th {
  background: #667eea;
  color: white;
}

.btn-small {
  padding: 0.25rem 0.5rem;
  margin-right: 0.5rem;
  border: none;
  cursor: pointer;
  border-radius: 4px;
}

.btn-danger {
  background: #e63946;
  color: white;
}
</style>
