<template>
  <div v-if="district" class="details">
    <RouterLink to="/districts" class="back">← Назад к районам</RouterLink>

    <div class="header">
      <img :src="district.district.imageUrl" alt="" class="photo" />
      <div class="header-info">
        <h1>{{ district.district.name }}</h1>
        <div class="meta">
          <span>👥 {{ district.district.population?.toLocaleString() }}</span>
          <span>📐 {{ district.district.area }} км²</span>
        </div>
        <p>{{ district.district.description }}</p>
      </div>
    </div>

    <h2>Проекты развития ({{ district.projects.length }})</h2>
    <div class="projects">
      <div v-for="p in district.projects" :key="p.id" class="project">
        <div class="project-header">
          <h3>{{ p.title }}</h3>
          <span class="category-tag">{{ p.category }}</span>
        </div>
        <p class="project-desc">{{ p.description }}</p>
        <div class="project-footer">
          <span class="budget">💰 {{ p.budgetEstimate }}</span>
          <span class="project-votes">🗳️ {{ p.voteCount }}</span>
          <button
            v-if="auth.user && !p.hasVoted"
            @click="voteForProject(p.id)"
            :disabled="voting"
            class="btn vote-btn"
          >{{ voting ? '...' : 'Поддержать' }}</button>
          <span v-else-if="p.hasVoted" class="voted-badge">✓ Поддержано</span>
        </div>
      </div>
    </div>
  </div>
  <div v-else class="loading">Загрузка...</div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { useAuthStore } from '@/stores/auth'
import api from '@/api'

const route = useRoute()
const auth = useAuthStore()
const district = ref(null)
const voting = ref(false)

onMounted(async () => {
  const res = await api.get(`/districts/${route.params.id}`)
  district.value = res.data
})

async function voteForProject(projectId) {
  voting.value = true
  await api.post(`/districts/projects/${projectId}/vote`)
  const project = district.value.projects.find(p => p.id === projectId)
  if (project) {
    project.hasVoted = true
    project.voteCount++
  }
  voting.value = false
}
</script>

<style scoped>
.back {
  display: inline-block;
  margin-bottom: 16px;
  color: #666;
  font-size: 0.9rem;
}
.back:hover {
  color: #4361ee;
}
.header {
  display: flex;
  gap: 24px;
  margin-bottom: 24px;
  padding: 24px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.photo {
  width: 200px;
  height: 150px;
  object-fit: cover;
  border-radius: 10px;
  flex-shrink: 0;
}
.header-info h1 {
  margin-bottom: 8px;
}
.header-info p {
  color: #444;
  line-height: 1.5;
  margin-top: 8px;
}
.meta {
  display: flex;
  gap: 16px;
  font-size: 0.9rem;
  color: #666;
}
.projects {
  display: flex;
  flex-direction: column;
  gap: 12px;
}
.project {
  background: white;
  padding: 20px;
  border-radius: 12px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.project-header {
  display: flex;
  align-items: center;
  gap: 10px;
  margin-bottom: 8px;
}
.project-header h3 {
  font-size: 1.1rem;
}
.category-tag {
  font-size: 0.75rem;
  background: #eef0ff;
  color: #4361ee;
  padding: 2px 10px;
  border-radius: 10px;
  font-weight: 500;
}
.project-desc {
  color: #555;
  line-height: 1.5;
  margin-bottom: 12px;
}
.project-footer {
  display: flex;
  align-items: center;
  gap: 16px;
}
.budget {
  font-size: 0.9rem;
  color: #2e7d32;
  font-weight: 500;
}
.project-votes {
  font-size: 0.85rem;
  color: #888;
}
.btn {
  padding: 8px 20px;
  border-radius: 8px;
  font-size: 0.85rem;
  font-weight: 600;
  border: none;
  cursor: pointer;
  transition: all 0.2s;
}
.vote-btn {
  background: #4361ee;
  color: white;
}
.vote-btn:hover {
  background: #3651d4;
}
.voted-badge {
  font-size: 0.8rem;
  background: #e8f5e9;
  color: #2e7d32;
  padding: 4px 12px;
  border-radius: 8px;
  font-weight: 500;
}
.loading {
  text-align: center;
  padding: 40px;
  color: #888;
}

@media (max-width: 600px) {
  .header { flex-direction: column; }
  .photo { width: 100%; height: 180px; }
}
</style>
