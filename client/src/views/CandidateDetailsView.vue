<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-primary" role="status"></div>
  </div>

  <div v-else-if="error" class="alert alert-danger">{{ error }}</div>

  <template v-else-if="candidate">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item"><router-link to="/candidates">Кандидаты</router-link></li>
        <li class="breadcrumb-item active">{{ candidate.fullName }}</li>
      </ol>
    </nav>

    <div class="row">
      <div class="col-md-8">
        <div class="card mb-4 shadow-sm">
          <div class="card-body">
            <div class="d-flex align-items-center mb-3">
              <img :src="candidate.photoUrl" :alt="candidate.fullName" width="64" height="64" class="me-3" @error="e => e.target.style.display='none'" />
              <div>
                <h2 class="card-title mb-1">{{ candidate.fullName }}</h2>
                <span class="badge bg-secondary">{{ candidate.category }}</span>
              </div>
            </div>
            <h5 class="mt-4">Описание</h5>
            <p class="card-text">{{ candidate.description }}</p>
            <h5>Биография</h5>
            <p class="card-text">{{ candidate.biography }}</p>
            <p class="text-muted small">Добавлен: {{ new Date(candidate.createdAt).toLocaleDateString('ru-RU') }}</p>
          </div>
        </div>
      </div>

      <div class="col-md-4">
        <div class="card mb-4 shadow-sm text-center">
          <div class="card-body">
            <i class="bi bi-hand-thumbs-up text-primary" style="font-size: 3rem;"></i>
            <h3 class="display-6 mt-2">{{ candidate.voteCount }}</h3>
            <p class="text-muted">всего голосов</p>
          </div>
        </div>

        <div v-if="auth.isAuthenticated" class="card shadow-sm">
          <div class="card-body">
            <h5 class="card-title">Голосование</h5>
            <div v-if="candidate.hasVoted" class="alert alert-success">
              <i class="bi bi-check-circle"></i> Вы уже проголосовали за этого кандидата
            </div>
            <form v-else @submit.prevent="submitVote">
              <div class="mb-3">
                <label class="form-label">Комментарий (необязательно)</label>
                <textarea v-model="comment" class="form-control" rows="2" maxlength="200" placeholder="Ваше мнение о кандидате"></textarea>
              </div>
              <button type="submit" class="btn btn-primary w-100" :disabled="voting">
                <i v-if="voting" class="spinner-border spinner-border-sm"></i>
                <i v-else class="bi bi-check-lg"></i> Голосовать
              </button>
            </form>
          </div>
        </div>
        <div v-else class="card shadow-sm">
          <div class="card-body text-center">
            <i class="bi bi-person-lock" style="font-size: 2rem;"></i>
            <p class="mt-2">Чтобы голосовать:</p>
            <router-link to="/login" class="btn btn-outline-primary btn-sm w-100 mb-1"><i class="bi bi-box-arrow-in-right"></i> Войти</router-link>
            <router-link to="/register" class="btn btn-outline-success btn-sm w-100"><i class="bi bi-person-plus"></i> Регистрация</router-link>
          </div>
        </div>

        <div v-if="message" :class="'alert mt-3 ' + (messageType === 'success' ? 'alert-success' : 'alert-danger')">
          {{ message }}
        </div>
      </div>
    </div>
  </template>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import api from '../api'
import { useAuthStore } from '../stores/auth'

const route = useRoute()
const auth = useAuthStore()
const loading = ref(true)
const error = ref('')
const candidate = ref(null)
const comment = ref('')
const voting = ref(false)
const message = ref('')
const messageType = ref('success')

async function fetchCandidate() {
  loading.value = true
  try {
    const { data } = await api.get(`/candidates/${route.params.id}`)
    candidate.value = data
  } catch (e) {
    error.value = 'Кандидат не найден'
  } finally {
    loading.value = false
  }
}

async function submitVote() {
  voting.value = true
  message.value = ''
  try {
    await api.post(`/candidates/${candidate.value.id}/vote`, { comment: comment.value })
    message.value = 'Ваш голос учтён'
    messageType.value = 'success'
    candidate.value.hasVoted = true
    candidate.value.voteCount++
  } catch (e) {
    message.value = e.response?.data?.message || 'Ошибка голосования'
    messageType.value = 'error'
  } finally {
    voting.value = false
  }
}

onMounted(fetchCandidate)
</script>
