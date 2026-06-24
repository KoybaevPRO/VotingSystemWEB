<template>
  <div class="auth-page">
    <div class="auth-card">
      <h1>Вход</h1>
      <form @submit.prevent="login">
        <div class="field">
          <input v-model="email" type="email" placeholder="Email" required />
        </div>
        <div class="field">
          <input v-model="password" type="password" placeholder="Пароль" required />
        </div>
        <p v-if="error" class="error">{{ error }}</p>
        <button type="submit" :disabled="loading" class="btn">
          {{ loading ? 'Вход...' : 'Войти' }}
        </button>
      </form>
      <p class="switch">Нет аккаунта? <RouterLink to="/register">Зарегистрируйтесь</RouterLink></p>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth'

const router = useRouter()
const auth = useAuthStore()
const email = ref('')
const password = ref('')
const error = ref('')
const loading = ref(false)

async function login() {
  loading.value = true
  error.value = ''
  try {
    await auth.login(email.value, password.value)
    router.push('/')
  } catch (e) {
    error.value = e.response?.data?.message || 'Ошибка входа'
  }
  loading.value = false
}
</script>

<style scoped>
.auth-page {
  display: flex;
  justify-content: center;
  padding-top: 40px;
}
.auth-card {
  width: 100%;
  max-width: 400px;
  background: white;
  padding: 32px;
  border-radius: 12px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.auth-card h1 {
  text-align: center;
  margin-bottom: 24px;
}
.field {
  margin-bottom: 14px;
}
.field input {
  width: 100%;
  padding: 12px 16px;
  border: 2px solid #e0e0e0;
  border-radius: 10px;
  font-size: 0.95rem;
  outline: none;
  transition: border-color 0.2s;
  font-family: inherit;
}
.field input:focus {
  border-color: #4361ee;
}
.btn {
  width: 100%;
  padding: 12px;
  background: #4361ee;
  color: white;
  border: none;
  border-radius: 10px;
  font-size: 1rem;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.2s;
  font-family: inherit;
}
.btn:hover {
  background: #3651d4;
}
.btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
.error {
  color: #e74c3c;
  font-size: 0.9rem;
  margin-bottom: 12px;
  text-align: center;
}
.switch {
  text-align: center;
  margin-top: 16px;
  color: #666;
  font-size: 0.9rem;
}
</style>
