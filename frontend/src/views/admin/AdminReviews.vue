<template>
  <div class="admin-view-page">
    <div class="container">
      <router-link to="/admin" class="admin-back-btn">
        <span>← Back to Mission Control</span>
      </router-link>

      <header class="admin-view-header">
        <div>
          <span class="badge-pill badge-lime">TESTIMONIAL MODERATION</span>
          <h1 class="admin-view-title">Manage Reviews</h1>
        </div>
      </header>

      <div class="admin-table-panel">
        <table class="data-table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Reviewer</th>
              <th>Rating</th>
              <th>Review Title</th>
              <th>Verified Buyer</th>
              <th>Status</th>
              <th>Date</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="review in reviews" :key="review.id">
              <td><strong>{{ review.product?.name || 'Item #' + review.productId }}</strong></td>
              <td>{{ review.authorName }}</td>
              <td><span style="color: #fbbf24; font-weight: 700;">★ {{ review.rating }}</span></td>
              <td>{{ review.title }}</td>
              <td>
                <span class="badge-pill" :class="review.verifiedPurchase ? 'badge-lime' : 'badge-discount'">
                  {{ review.verifiedPurchase ? '✓ Verified' : 'Standard' }}
                </span>
              </td>
              <td>
                <span class="badge-pill" :class="review.isApproved ? 'badge-lime' : 'badge-discount'">
                  {{ review.isApproved ? 'Approved' : 'Hidden' }}
                </span>
              </td>
              <td>{{ new Date(review.createdOn).toLocaleDateString() }}</td>
              <td>
                <button v-if="!review.isApproved" @click="approveReview(review.id)" class="admin-action-btn">Approve</button>
                <button v-if="review.isApproved" @click="hideReview(review.id)" class="admin-action-btn">Hide</button>
                <button @click="deleteReview(review.id)" class="admin-action-btn delete">Delete</button>
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
  if (confirm('Are you sure you want to delete this review?')) {
    await api.delete(`/admin/reviews/${id}`)
    loadReviews()
  }
}

onMounted(() => {
  loadReviews()
})
</script>
