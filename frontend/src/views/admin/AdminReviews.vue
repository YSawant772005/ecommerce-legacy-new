<template>
  <div class="admin-reviews">
    <h1>Manage Reviews</h1>

    <table class="data-table">
      <thead>
        <tr>
          <th>Product</th>
          <th>Author</th>
          <th>Rating</th>
          <th>Title</th>
          <th>Verified</th>
          <th>Approved</th>
          <th>Date</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="review in reviews" :key="review.id">
          <td>{{ review.product?.name }}</td>
          <td>{{ review.authorName }}</td>
          <td>⭐ {{ review.rating }}</td>
          <td>{{ review.title }}</td>
          <td>{{ review.verifiedPurchase ? 'Yes' : 'No' }}</td>
          <td>{{ review.isApproved ? 'Yes' : 'No' }}</td>
          <td>{{ new Date(review.createdOn).toLocaleDateString() }}</td>
          <td>
            <button v-if="!review.isApproved" @click="approveReview(review.id)" class="btn-small">Approve</button>
            <button v-if="review.isApproved" @click="hideReview(review.id)" class="btn-small">Hide</button>
            <button @click="deleteReview(review.id)" class="btn-small btn-danger">Delete</button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import api from '../../api/client'

const reviews = ref<any[]>([])

async function loadReviews() {
  const response = await api.get('/admin/reviews')
  reviews.value = response.data
}

async function approveReview(id: number) {
  await api.put(`/admin/reviews/${id}/approve`)
  loadReviews()
}

async function hideReview(id: number) {
  await api.put(`/admin/reviews/${id}/hide`)
  loadReviews()
}

async function deleteReview(id: number) {
  if (confirm('Are you sure?')) {
    await api.delete(`/admin/reviews/${id}`)
    loadReviews()
  }
}

onMounted(() => {
  loadReviews()
})
</script>

<style scoped>
.admin-reviews {
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
