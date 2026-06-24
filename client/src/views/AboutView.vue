<template>
  <div class="about">
    <div class="header">
      <h1>О проекте</h1>
      <p>Система электронного голосования для города Владикавказ</p>
    </div>

    <section class="section">
      <h2>Технологии</h2>
      <div class="tech-tags">
        <span>Vue.js</span>
        <span>ASP.NET Core</span>
        <span>Entity Framework</span>
        <span>SQLite</span>
        <span>JWT</span>
      </div>
    </section>

    <section class="section">
      <h2>Разработчики</h2>
      <div class="developers">
        <div v-for="dev in developers" :key="dev.fullName" class="dev-card">
          <div class="dev-avatar">{{ dev.fullName.charAt(0) }}</div>
          <div>
            <h3>{{ dev.fullName }}</h3>
            <span class="role">{{ dev.role }}</span>
            <p>{{ dev.description }}</p>
          </div>
        </div>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/api'

const developers = ref([])

onMounted(async () => {
  const res = await api.get('/home/about')
  developers.value = res.data.developers
})
</script>

<style scoped>
.header {
  text-align: center;
  padding: 40px 24px;
  background: white;
  border-radius: 12px;
  margin-bottom: 24px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.header p {
  color: #666;
  font-size: 1rem;
}
.section {
  background: white;
  padding: 24px;
  border-radius: 12px;
  margin-bottom: 16px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.tech-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 8px;
}
.tech-tags span {
  background: #eef0ff;
  color: #4361ee;
  padding: 6px 16px;
  border-radius: 20px;
  font-size: 0.85rem;
  font-weight: 500;
}
.developers {
  display: flex;
  flex-direction: column;
  gap: 12px;
}
.dev-card {
  display: flex;
  gap: 16px;
  align-items: flex-start;
  padding: 16px;
  background: #f8f9fa;
  border-radius: 10px;
}
.dev-avatar {
  width: 48px;
  height: 48px;
  border-radius: 50%;
  background: #4361ee;
  color: white;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 1.2rem;
  font-weight: 600;
  flex-shrink: 0;
}
.dev-card h3 {
  font-size: 1rem;
  margin-bottom: 2px;
}
.role {
  font-size: 0.8rem;
  color: #4361ee;
  font-weight: 500;
  display: block;
  margin-bottom: 4px;
}
.dev-card p {
  font-size: 0.9rem;
  color: #555;
  line-height: 1.4;
}
</style>
