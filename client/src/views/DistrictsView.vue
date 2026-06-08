<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-success" role="status"></div>
  </div>

  <template v-else>
    <div class="text-center mb-5">
      <h1 class="display-5">Благоустройство районов Владикавказа</h1>
      <p class="lead text-muted">Выберите район и проголосуйте за проекты по его благоустройству</p>
    </div>

    <div class="row row-cols-1 row-cols-md-2 row-cols-lg-4 g-4">
      <div v-for="district in districts" :key="district.id" class="col d-flex">
        <div class="card h-100 shadow-sm w-100">
          <div class="card-body text-center d-flex flex-column">
            <div class="mb-2">
              <img :src="district.imageUrl" :alt="district.name" width="80" height="80" @error="e => e.target.style.display='none'" />
            </div>
            <h5 class="card-title">{{ district.name }}</h5>
            <p class="card-text small text-muted flex-grow-1">{{ district.description }}</p>
            <div class="d-flex justify-content-around mb-2 small text-muted">
              <span><i class="bi bi-people"></i> {{ district.population.toLocaleString() }}</span>
              <span><i class="bi bi-geo-alt"></i> {{ district.area }} км²</span>
            </div>
            <div class="mt-auto">
              <p class="mb-2"><span class="badge bg-info">{{ district.projectCount }} проектов</span></p>
              <router-link :to="`/districts/${district.id}`" class="btn btn-outline-success w-100">
                <i class="bi bi-eye"></i> Подробнее и голосование
              </router-link>
            </div>
          </div>
        </div>
      </div>
    </div>
  </template>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../api'

const loading = ref(true)
const districts = ref([])

onMounted(async () => {
  try {
    const { data } = await api.get('/districts')
    districts.value = data
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
})
</script>
