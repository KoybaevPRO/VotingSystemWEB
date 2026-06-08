<template>
  <div class="row justify-content-center mt-5">
    <div class="col-md-6 col-lg-4">
      <div class="card shadow">
        <div class="card-body p-4">
          <h3 class="text-center mb-4"><i class="bi bi-person-plus"></i> Регистрация</h3>

          <div v-if="error" class="alert alert-danger">
            <div v-for="(msg, i) in (Array.isArray(error) ? error : [error])" :key="i">{{ msg }}</div>
          </div>

          <form @submit.prevent="handleRegister">
            <div class="mb-3">
              <label class="form-label">Имя</label>
              <input v-model="firstName" class="form-control" required />
            </div>
            <div class="mb-3">
              <label class="form-label">Фамилия</label>
              <input v-model="lastName" class="form-control" required />
            </div>
            <div class="mb-3">
              <label class="form-label">Email</label>
              <input v-model="email" type="email" class="form-control" required />
            </div>
            <div class="mb-3">
              <label class="form-label">Пароль</label>
              <input v-model="password" type="password" class="form-control" required minlength="6" />
            </div>
            <button type="submit" class="btn btn-success w-100" :disabled="loading">
              <i v-if="loading" class="spinner-border spinner-border-sm"></i>
              <i v-else class="bi bi-check-lg"></i> Зарегистрироваться
            </button>
          </form>

          <p class="text-center mt-3 mb-0">
            Уже есть аккаунт? <router-link to="/login">Войти</router-link>
          </p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = useRouter()
const auth = useAuthStore()
const firstName = ref('')
const lastName = ref('')
const email = ref('')
const password = ref('')
const loading = ref(false)
const error = ref('')

async function handleRegister() {
  loading.value = true
  error.value = ''
  try {
    await auth.register(email.value, password.value, firstName.value, lastName.value)
    router.push('/')
  } catch (e) {
    error.value = e.response?.data || 'Ошибка регистрации'
  } finally {
    loading.value = false
  }
}
</script>
