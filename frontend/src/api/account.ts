import api from './client'

export const accountApi = {
  getProfile: () => api.get('/account/profile'),

  updateProfile: (data: { fullName: string; phone?: string }) =>
    api.put('/account/profile', data),

  getAddresses: () => api.get('/account/addresses'),

  addAddress: (data: any) => api.post('/account/addresses', data),

  updateAddress: (id: number, data: any) => api.put(`/account/addresses/${id}`, data),

  deleteAddress: (id: number) => api.delete(`/account/addresses/${id}`),

  changePassword: (data: { currentPassword: string; newPassword: string }) =>
    api.post('/account/change-password', data)
}
