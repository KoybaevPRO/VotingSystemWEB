<template>
  <div class="districts">
    <h1>Районы Владикавказа</h1>

    <div v-if="loading" class="loading">Загрузка...</div>
    <div v-else class="grid">
      <div v-for="d in districts" :key="d.id" class="card">
        <img :src="d.imageUrl" alt="" class="photo" />
        <div class="card-body">
          <RouterLink :to="`/districts/${d.id}`" class="name">{{ d.name }}</RouterLink>
          <p class="desc">{{ d.description }}</p>
          <div class="meta">
            <span>👥 {{ d.population?.toLocaleString() }}</span>
            <span>📐 {{ d.area }} км²</span>
            <span>📋 {{ d.projectCount }} проектов</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/api'

const districts = ref([])
const loading = ref(true)

onMounted(async () => {
  const res = await api.get('/districts')
  districts.value = res.data
  loading.value = false
})
</script>

<style scoped>
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 16px;
}
.card {
  background: white;
  border-radius: 12px;
  overflow: hidden;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
  transition: transform 0.2s, box-shadow 0.2s;
}
.card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}
.photo {
  width: 100%;
  height: 160px;
  object-fit: cover;
}
.card-body {
  padding: 16px;
}
.name {
  font-size: 1.1rem;
  font-weight: 600;
  color: #1a1a2e;
  display: block;
  margin-bottom: 6px;
}
.name:hover {
  color: #4361ee;
}
.desc {
  font-size: 0.9rem;
  color: #666;
  line-height: 1.5;
  margin-bottom: 12px;
}
.meta {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
  font-size: 0.8rem;
  color: #888;
}
.loading {
  text-align: center;
  padding: 40px;
  color: #888;
}

@media (max-width: 600px) {
  .grid { grid-template-columns: 1fr; }
}
</style>
