import api from './client'

export const reviewsApi = {
  getProductReviews: (productId: number) => api.get(`/reviews/product/${productId}`),

  submitReview: (data: { productId: number; rating: number; title?: string; body?: string }) =>
    api.post('/reviews', data)
}
