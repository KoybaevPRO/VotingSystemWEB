<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-primary" role="status"></div>
  </div>

  <template v-else>
    <h1 class="mb-4">Мои голоса</h1>

    <ul class="nav nav-tabs mb-4">
      <li class="nav-item">
        <a class="nav-link" :class="{ active: tab === 'all' }" href="#" @click.prevent="tab = 'all'">Все</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" :class="{ active: tab === 'candidate' }" href="#" @click.prevent="tab = 'candidate'">За кандидатов</a>
      </li>
      <li class="nav-item">
        <a class="nav-link" :class="{ active: tab === 'improvement' }" href="#" @click.prevent="tab = 'improvement'">За благоустройство</a>
      </li>
    </ul>

    <div v-if="!filteredVotes.length" class="alert alert-info d-flex align-items-center gap-3">
      <i class="bi bi-info-circle fs-4"></i>
      <span>У вас пока нет голосов в этой категории.</span>
      <router-link v-if="tab !== 'improvement'" to="/candidates" class="btn btn-outline-primary btn-sm ms-auto">
        <i class="bi bi-people"></i> Список кандидатов
      </router-link>
      <router-link v-else to="/districts" class="btn btn-outline-success btn-sm ms-auto">
        <i class="bi bi-buildings"></i> Районы города
      </router-link>
    </div>

    <div v-else class="table-responsive">
      <table class="table table-hover align-middle">
        <thead :class="tab === 'improvement' ? 'table-success' : 'table-dark'">
          <tr>
            <th>Тип</th>
            <th>Объект</th>
            <th>Категория</th>
            <th>Район</th>
            <th>Дата</th>
            <th>Комментарий</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="vote in filteredVotes" :key="vote.id">
            <td>
              <span class="badge" :class="vote.type === 'candidate' ? 'bg-primary' : 'bg-success'">
                {{ vote.type === 'candidate' ? 'Кандидат' : 'Проект' }}
              </span>
            </td>
            <td>
              <template v-if="vote.type === 'candidate'">
                <router-link :to="`/candidates/${vote.candidateId}`" class="fw-semibold text-decoration-none">
                  {{ vote.candidateName }}
                </router-link>
              </template>
              <template v-else>
                <span class="fw-semibold">{{ vote.projectTitle }}</span>
              </template>
            </td>
            <td><span class="badge bg-secondary">{{ vote.candidateCategory || vote.projectCategory }}</span></td>
            <td class="text-muted small">{{ vote.districtName || '—' }}</td>
            <td>{{ new Date(vote.voteDate).toLocaleString('ru-RU') }}</td>
            <td class="text-muted small">{{ vote.comment || '—' }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </template>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import api from '../api'

const loading = ref(true)
const tab = ref('all')
const candidateVotes = ref([])
const improvementVotes = ref([])

const filteredVotes = computed(() => {
  if (tab.value === 'all') {
    return [...candidateVotes.value, ...improvementVotes.value].sort((a, b) => new Date(b.voteDate) - new Date(a.voteDate))
  }
  if (tab.value === 'candidate') return candidateVotes.value
  return improvementVotes.value
})

onMounted(async () => {
  try {
    const [candidateRes, improvementRes] = await Promise.all([
      api.get('/candidates/my-votes'),
      api.get('/districts/my-votes')
    ])
    candidateVotes.value = candidateRes.data
    improvementVotes.value = improvementRes.data
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
})
</script>
