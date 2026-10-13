<template>
  <div class="admin-newsletter">
    <h1>Newsletter Subscribers</h1>

    <table class="data-table">
      <thead>
        <tr>
          <th>Email</th>
          <th>Subscribed On</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="subscriber in subscribers" :key="subscriber.id">
          <td>{{ subscriber.email }}</td>
          <td>{{ new Date(subscriber.subscribedOn).toLocaleDateString() }}</td>
          <td>
            <button @click="deleteSubscriber(subscriber.id)" class="btn-small btn-danger">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
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
  if (confirm('Are you sure?')) {
    await api.delete(`/admin/newsletter/${id}`)
    loadSubscribers()
  }
}

onMounted(() => {
  loadSubscribers()
})
</script>

<style scoped>
.admin-newsletter {
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
