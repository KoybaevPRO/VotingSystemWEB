<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-success" role="status"></div>
  </div>

  <div v-else-if="error" class="alert alert-danger">{{ error }}</div>

  <template v-else-if="data">
    <nav aria-label="breadcrumb">
      <ol class="breadcrumb">
        <li class="breadcrumb-item"><router-link to="/districts">Районы</router-link></li>
        <li class="breadcrumb-item active">{{ data.district.name }}</li>
      </ol>
    </nav>

    <div class="row mb-4">
      <div class="col-md-4 text-center">
        <img :src="data.district.imageUrl" :alt="data.district.name" class="img-fluid mb-3" style="max-height: 150px;" @error="e => e.target.style.display='none'" />
      </div>
      <div class="col-md-8">
        <h2>{{ data.district.name }}</h2>
        <p>{{ data.district.description }}</p>
        <div class="row text-muted small">
          <div class="col-auto"><i class="bi bi-people"></i> Население: {{ data.district.population.toLocaleString() }} чел.</div>
          <div class="col-auto"><i class="bi bi-geo-alt"></i> Площадь: {{ data.district.area }} км²</div>
        </div>
      </div>
    </div>

    <h3 class="mb-3">Проекты благоустройства</h3>
    <p class="text-muted mb-4">Проголосуйте за проекты, которые хотите видеть в первую очередь</p>

    <div v-if="message" :class="'alert ' + (messageType === 'success' ? 'alert-success' : 'alert-danger')">
      {{ message }}
    </div>

    <div class="row">
      <div v-for="project in data.projects" :key="project.id" class="col-md-6 mb-3">
        <div class="card h-100" :class="project.hasVoted ? 'border-success' : ''">
          <div class="card-body">
            <div class="d-flex justify-content-between align-items-start">
              <div>
                <h5 class="card-title">{{ project.title }}</h5>
                <span class="badge bg-secondary mb-2">{{ project.category }}</span>
              </div>
              <span class="badge bg-warning text-dark">{{ project.budgetEstimate }}</span>
            </div>
            <p class="card-text small">{{ project.description }}</p>
            <div class="d-flex justify-content-between align-items-center">
              <small class="text-muted">
                <i class="bi bi-hand-thumbs-up"></i> Голосов: <strong>{{ project.voteCount }}</strong>
              </small>
              <template v-if="auth.isAuthenticated">
                <span v-if="project.hasVoted" class="badge bg-success">
                  <i class="bi bi-check-circle"></i> Вы проголосовали
                </span>
                <button v-else class="btn btn-outline-success btn-sm" @click="voteForProject(project.id)" :disabled="votingId === project.id">
                  <i v-if="votingId === project.id" class="spinner-border spinner-border-sm"></i>
                  <i v-else class="bi bi-hand-thumbs-up"></i> Голосовать
                </button>
              </template>
              <router-link v-else to="/login" class="btn btn-outline-primary btn-sm">
                Войдите, чтобы голосовать
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div v-if="!data.projects.length" class="alert alert-info">
      Проекты благоустройства пока не добавлены
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
const data = ref(null)
const votingId = ref(null)
const message = ref('')
const messageType = ref('success')

async function fetchDistrict() {
  loading.value = true
  try {
    const { data: result } = await api.get(`/districts/${route.params.id}`)
    data.value = result
  } catch (e) {
    error.value = 'Район не найден'
  } finally {
    loading.value = false
  }
}

async function voteForProject(projectId) {
  votingId.value = projectId
  message.value = ''
  try {
    await api.post(`/districts/projects/${projectId}/vote`)
    message.value = 'Ваш голос за проект учтён'
    messageType.value = 'success'
    const project = data.value.projects.find(p => p.id === projectId)
    if (project) {
      project.hasVoted = true
      project.voteCount++
    }
  } catch (e) {
    message.value = e.response?.data?.message || 'Ошибка голосования'
    messageType.value = 'error'
  } finally {
    votingId.value = null
  }
}

onMounted(fetchDistrict)
</script>
