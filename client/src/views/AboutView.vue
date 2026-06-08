<template>
  <div v-if="loading" class="text-center py-5">
    <div class="spinner-border text-primary" role="status"></div>
  </div>

  <template v-else-if="data">
    <h1 class="mb-4">О проекте</h1>

    <div class="row mb-4">
      <div class="col">
        <div class="card shadow-sm">
          <div class="card-body">
            <h5 class="card-title">
              <i class="bi bi-check2-square text-primary"></i> Система голосования г. Владикавказ
            </h5>
            <p class="card-text">
              Данная система разработана для проведения электронного голосования
              на территории города Владикавказ. Мы стремимся сделать процесс голосования
              максимально прозрачным, удобным и доступным для каждого жителя.
            </p>
            <div class="row mt-3">
              <div class="col-md-4 mb-2">
                <div class="d-flex align-items-center">
                  <i class="bi bi-gear me-2 text-primary"></i>
                  <span><strong>Технологии:</strong> {{ data.technologies }}</span>
                </div>
              </div>
              <div class="col-md-4 mb-2">
                <div class="d-flex align-items-center">
                  <i class="bi bi-calendar me-2 text-primary"></i>
                  <span><strong>Дата:</strong> Май 2026 года</span>
                </div>
              </div>
              <div class="col-md-4 mb-2">
                <div class="d-flex align-items-center">
                  <i class="bi bi-geo-alt me-2 text-primary"></i>
                  <span><strong>Место:</strong> г. Владикавказ, РСО-Алания</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <h2 class="mb-3">Команда разработчиков</h2>
    <div class="row mb-4">
      <div v-for="dev in data.developers" :key="dev.fullName" class="col-md-4 mb-4">
        <div class="card h-100 shadow-sm text-center">
          <div class="card-body">
            <i class="bi bi-person-circle text-secondary" style="font-size: 4rem;"></i>
            <h5 class="card-title mt-3">{{ dev.fullName }}</h5>
            <h6 class="card-subtitle mb-2 text-muted">{{ dev.role }}</h6>
            <p class="card-text">{{ dev.description }}</p>
          </div>
        </div>
      </div>
    </div>

    <h2 class="mb-3">О городе</h2>
    <div class="card shadow-sm mb-4">
      <div class="card-body">
        <div class="d-flex align-items-center mb-3">
          <i class="bi bi-heart text-danger me-2" style="font-size: 2rem;"></i>
          <h4 class="mb-0">Владикавказ — город, в котором хочется жить!</h4>
        </div>
        <p>
          <strong>Владикавказ</strong> (осет. Дзæуджыхъæу) — столица Республики Северная Осетия-Алания,
          один из красивейших городов Северного Кавказа. Основанный в 1784 году, он раскинулся
          в живописной долине у подножия Кавказских гор, где бурные воды Терека придают особый шарм
          этому удивительному месту.
        </p>
        <p>
          Сегодня Владикавказ — это современный, динамично развивающийся город, где бережно хранят
          богатейшее культурное наследие осетинского народа.
        </p>
        <div class="alert alert-success mt-3">
          <i class="bi bi-megaphone"></i>
          <strong>Не оставайтесь в стороне!</strong> Ваш голос важен для будущего нашего города.
        </div>
      </div>
    </div>
  </template>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../api'

const loading = ref(true)
const data = ref(null)

onMounted(async () => {
  try {
    const { data: result } = await api.get('/home/about')
    data.value = result
  } catch (e) {
    console.error(e)
  } finally {
    loading.value = false
  }
})
</script>
