<template>
  <div class="candidates">
    <h1>Кандидаты</h1>

    <div class="filters">
      <button
        v-for="cat in categories"
        :key="cat"
        :class="{ active: selectedCategory === cat }"
        @click="selectedCategory = cat"
      >{{ cat }}</button>
      <button
        :class="{ active: selectedCategory === null }"
        @click="selectedCategory = null"
      >Все</button>
    </div>

    <div v-if="loading" class="loading">Загрузка...</div>
    <div v-else-if="filteredCandidates.length === 0" class="empty">
      Нет кандидатов в этой категории
    </div>
    <div v-else class="grid">
      <div v-for="c in filteredCandidates" :key="c.id" class="card">
        <img :src="c.photoUrl" alt="" class="avatar" />
        <div class="card-body">
          <RouterLink :to="`/candidates/${c.id}`" class="name">{{ c.fullName }}</RouterLink>
          <span class="category-tag">{{ c.category }}</span>
          <p class="desc">{{ c.description }}</p>
          <div class="card-footer">
            <span class="votes">🗳️ {{ c.voteCount }}</span>
            <span v-if="c.hasVoted" class="voted-badge">Вы голосовали</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import api from '@/api'

const candidates = ref([])
const categories = ref([])
const selectedCategory = ref(null)
const loading = ref(true)

onMounted(async () => {
  const res = await api.get('/candidates')
  candidates.value = res.data.candidates
  categories.value = res.data.categories
  loading.value = false
})

const filteredCandidates = computed(() => {
  if (!selectedCategory.value) return candidates.value
  return candidates.value.filter(c => c.category === selectedCategory.value)
})
</script>

<style scoped>
.filters {
  display: flex;
  gap: 8px;
  flex-wrap: wrap;
  margin-bottom: 24px;
}
.filters button {
  padding: 8px 18px;
  border: 2px solid #e0e0e0;
  background: white;
  border-radius: 20px;
  cursor: pointer;
  font-size: 0.85rem;
  font-weight: 500;
  transition: all 0.2s;
  color: #555;
}
.filters button:hover {
  border-color: #4361ee;
  color: #4361ee;
}
.filters button.active {
  background: #4361ee;
  color: white;
  border-color: #4361ee;
}
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 16px;
}
.card {
  background: white;
  border-radius: 12px;
  padding: 20px;
  display: flex;
  gap: 16px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
  transition: transform 0.2s, box-shadow 0.2s;
}
.card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}
.avatar {
  width: 64px;
  height: 64px;
  border-radius: 50%;
  object-fit: cover;
  flex-shrink: 0;
}
.card-body {
  flex: 1;
  min-width: 0;
}
.name {
  font-size: 1.05rem;
  font-weight: 600;
  color: #1a1a2e;
  display: block;
  margin-bottom: 4px;
}
.name:hover {
  color: #4361ee;
}
.category-tag {
  display: inline-block;
  font-size: 0.75rem;
  background: #eef0ff;
  color: #4361ee;
  padding: 2px 10px;
  border-radius: 10px;
  margin-bottom: 8px;
  font-weight: 500;
}
.desc {
  font-size: 0.9rem;
  color: #666;
  line-height: 1.4;
  margin-bottom: 10px;
}
.card-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
}
.votes {
  font-size: 0.85rem;
  color: #888;
}
.voted-badge {
  font-size: 0.75rem;
  background: #e8f5e9;
  color: #2e7d32;
  padding: 2px 10px;
  border-radius: 10px;
  font-weight: 500;
}
.loading, .empty {
  text-align: center;
  padding: 40px;
  color: #888;
}

@media (max-width: 600px) {
  .grid { grid-template-columns: 1fr; }
}
</style>
