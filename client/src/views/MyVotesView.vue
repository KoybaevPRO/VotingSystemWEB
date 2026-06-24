<template>
  <div class="my-votes">
    <h1>Мои голоса</h1>

    <div v-if="loading" class="loading">Загрузка...</div>

    <div v-else-if="votes.length === 0" class="empty">
      <p>Вы ещё не голосовали</p>
      <div class="empty-actions">
        <RouterLink to="/candidates" class="btn">К кандидатам</RouterLink>
        <RouterLink to="/districts" class="btn">К районам</RouterLink>
      </div>
    </div>

    <div v-else class="list">
      <div v-for="v in votes" :key="v.id" class="vote-card">
        <div class="vote-icon">{{ v.type === 'candidate' ? '👤' : '🏘️' }}</div>
        <div class="vote-body">
          <template v-if="v.type === 'candidate'">
            <RouterLink :to="`/candidates/${v.candidateId}`" class="vote-title">
              {{ v.candidateName }}
            </RouterLink>
            <span class="vote-subtitle">{{ v.candidateCategory }}</span>
          </template>
          <template v-else>
            <span class="vote-title">{{ v.projectTitle }}</span>
            <span class="vote-subtitle">{{ v.districtName }} · {{ v.projectCategory }}</span>
          </template>
          <p v-if="v.comment" class="comment">💬 {{ v.comment }}</p>
          <span class="date">{{ new Date(v.voteDate).toLocaleDateString('ru-RU', { day: 'numeric', month: 'long', year: 'numeric' }) }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/api'

const votes = ref([])
const loading = ref(true)

onMounted(async () => {
  try {
    const [candRes, distRes] = await Promise.all([
      api.get('/candidates/my-votes'),
      api.get('/districts/my-votes')
    ])
    const all = [...(candRes.data || []), ...(distRes.data || [])]
    all.sort((a, b) => new Date(b.voteDate) - new Date(a.voteDate))
    votes.value = all
  } catch {}
  loading.value = false
})
</script>

<style scoped>
.list {
  display: flex;
  flex-direction: column;
  gap: 12px;
}
.vote-card {
  display: flex;
  gap: 16px;
  padding: 20px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
  align-items: flex-start;
}
.vote-icon {
  font-size: 1.5rem;
  width: 44px;
  height: 44px;
  background: #f0f2f5;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
}
.vote-body {
  flex: 1;
}
.vote-title {
  font-weight: 600;
  color: #1a1a2e;
  display: block;
  margin-bottom: 2px;
}
.vote-subtitle {
  font-size: 0.8rem;
  color: #888;
  display: block;
  margin-bottom: 4px;
}
.comment {
  font-size: 0.9rem;
  color: #555;
  margin-top: 4px;
}
.date {
  font-size: 0.8rem;
  color: #aaa;
  margin-top: 4px;
  display: block;
}
.loading, .empty {
  text-align: center;
  padding: 48px;
  color: #888;
}
.empty-actions {
  display: flex;
  gap: 12px;
  justify-content: center;
  margin-top: 16px;
}
.btn {
  display: inline-block;
  padding: 10px 24px;
  background: #4361ee;
  color: white;
  border-radius: 8px;
  font-weight: 500;
  font-size: 0.9rem;
  text-decoration: none;
}
.btn:hover {
  background: #3651d4;
  text-decoration: none;
}
</style>
