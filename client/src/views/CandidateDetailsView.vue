<template>
  <div v-if="candidate" class="details">
    <RouterLink to="/candidates" class="back">← Назад к списку</RouterLink>

    <div class="profile">
      <img :src="candidate.photoUrl" alt="" class="photo" />
      <div class="profile-info">
        <h1>{{ candidate.fullName }}</h1>
        <span class="category-tag">{{ candidate.category }}</span>
        <div class="vote-count">🗳️ {{ candidate.voteCount }} голосов</div>
      </div>
    </div>

    <div class="section">
      <h2>О кандидате</h2>
      <p class="bio">{{ candidate.description }}</p>
      <p class="bio">{{ candidate.biography }}</p>
    </div>

    <div class="vote-section">
      <button
        v-if="!auth.user"
        disabled
        class="btn vote-btn"
      >Авторизуйтесь, чтобы голосовать</button>
      <button
        v-else-if="candidate.hasVoted"
        class="btn voted-btn"
        disabled
      >✓ Вы уже проголосовали</button>
      <button
        v-else
        @click="vote"
        :disabled="voting"
        class="btn vote-btn active"
      >{{ voting ? 'Обработка...' : 'Проголосовать' }}</button>
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
const candidate = ref(null)
const voting = ref(false)

onMounted(async () => {
  const res = await api.get(`/candidates/${route.params.id}`)
  candidate.value = res.data
})

async function vote() {
  voting.value = true
  await api.post(`/candidates/${route.params.id}/vote`)
  candidate.value.hasVoted = true
  candidate.value.voteCount++
  voting.value = false
}
</script>

<style scoped>
.back {
  display: inline-block;
  margin-bottom: 20px;
  color: #666;
  font-size: 0.9rem;
}
.back:hover {
  color: #4361ee;
}
.profile {
  display: flex;
  gap: 24px;
  align-items: center;
  margin-bottom: 24px;
  padding: 24px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.photo {
  width: 120px;
  height: 120px;
  border-radius: 50%;
  object-fit: cover;
  flex-shrink: 0;
}
.profile-info h1 {
  margin-bottom: 8px;
}
.category-tag {
  display: inline-block;
  font-size: 0.8rem;
  background: #eef0ff;
  color: #4361ee;
  padding: 4px 14px;
  border-radius: 12px;
  font-weight: 500;
  margin-bottom: 8px;
}
.vote-count {
  font-size: 1rem;
  color: #666;
  margin-top: 4px;
}
.section {
  background: white;
  padding: 24px;
  border-radius: 12px;
  margin-bottom: 24px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.bio {
  line-height: 1.7;
  color: #444;
  margin-bottom: 8px;
}
.vote-section {
  text-align: center;
  padding: 24px;
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.btn {
  padding: 14px 40px;
  border-radius: 10px;
  font-size: 1rem;
  font-weight: 600;
  border: none;
  cursor: pointer;
  transition: all 0.2s;
}
.vote-btn {
  background: #e0e0e0;
  color: #999;
}
.vote-btn.active {
  background: #4361ee;
  color: white;
}
.vote-btn.active:hover {
  background: #3651d4;
  transform: translateY(-1px);
}
.voted-btn {
  background: #e8f5e9;
  color: #2e7d32;
}
.loading {
  text-align: center;
  padding: 40px;
  color: #888;
}

@media (max-width: 600px) {
  .profile { flex-direction: column; text-align: center; }
}
</style>
