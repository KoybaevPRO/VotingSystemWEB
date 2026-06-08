<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-primary" role="status"></div>
  </div>

  <template v-else>
    <h1 class="mb-4">Список кандидатов</h1>

    <div class="mb-3">
      <div class="row g-3 align-items-end">
        <div class="col-auto">
          <label class="form-label fw-semibold">Категория:</label>
          <select v-model="selectedCategory" class="form-select" @change="fetchCandidates">
            <option value="">Все категории</option>
            <option v-for="cat in categories" :key="cat" :value="cat">{{ cat }}</option>
          </select>
        </div>
        <div v-if="selectedCategory" class="col-auto">
          <button class="btn btn-outline-secondary" @click="selectedCategory = ''; fetchCandidates()">Сбросить фильтр</button>
        </div>
      </div>
    </div>

    <div class="row">
      <div v-for="candidate in candidates" :key="candidate.id" class="col-md-6 col-lg-4 mb-4">
        <div class="card h-100 shadow-sm">
          <div class="text-center pt-3">
            <img :src="candidate.photoUrl" :alt="candidate.fullName" width="80" height="80" @error="e => e.target.style.display='none'" />
          </div>
          <div class="card-body">
            <h5 class="card-title text-center">{{ candidate.fullName }}</h5>
            <p class="text-center mb-2">
              <span class="badge bg-secondary">{{ candidate.category }}</span>
            </p>
            <p class="card-text small">{{ candidate.description }}</p>
          </div>
          <div class="card-footer bg-white d-flex justify-content-between align-items-center">
            <small class="text-muted">
              <i class="bi bi-hand-thumbs-up"></i> Голосов:
              <span class="fw-bold">{{ candidate.voteCount }}</span>
            </small>
            <router-link :to="`/candidates/${candidate.id}`" class="btn btn-outline-primary btn-sm">Подробнее</router-link>
          </div>
        </div>
      </div>
    </div>

    <div v-if="!candidates.length" class="alert alert-info">Кандидаты не найдены</div>
  </template>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../api'

const loading = ref(true)
const candidates = ref([])
const categories = ref([])
const selectedCategory = ref('')

async function fetchCandidates() {
  loading.value = true
  try {
    const params = selectedCategory.value ? { category: selectedCategory.value } : {}
    const { data } = await api.get('/candidates', { params })
    candidates.value = data.candidates
    categories.value = data.categories
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
}

onMounted(fetchCandidates)
</script>
