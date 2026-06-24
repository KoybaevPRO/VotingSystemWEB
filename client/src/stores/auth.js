import { defineStore } from 'pinia'
import api from '@/api'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: JSON.parse(localStorage.getItem('user')) || null,
    token: localStorage.getItem('token') || null
  }),
  actions: {
    async login(email, password) {
      const res = await api.post('/auth/login', { email, password })
      this.token = res.data.token
      this.user = { email: res.data.email, firstName: res.data.firstName, lastName: res.data.lastName }
      localStorage.setItem('token', res.data.token)
      localStorage.setItem('user', JSON.stringify(this.user))
    },
    async register(email, password, fullName) {
      const names = fullName.split(' ')
      const res = await api.post('/auth/register', {
        email, password,
        firstName: names[0] || '',
        lastName: names.slice(1).join(' ') || '',
        region: 'Владикавказ'
      })
      this.token = res.data.token
      this.user = { email: res.data.email, firstName: res.data.firstName, lastName: res.data.lastName }
      localStorage.setItem('token', res.data.token)
      localStorage.setItem('user', JSON.stringify(this.user))
    },
    logout() {
      this.token = null
      this.user = null
      localStorage.removeItem('token')
      localStorage.removeItem('user')
    }
  }
})
