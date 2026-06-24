<template>
  <div class="home">
    <section class="hero">
      <h1>Электронное голосование</h1>
      <p>Система для голосования за кандидатов и проекты развития города Владикавказ</p>
      <div class="hero-actions">
        <RouterLink to="/candidates" class="btn btn-primary">Кандидаты</RouterLink>
        <RouterLink to="/districts" class="btn btn-secondary">Районы</RouterLink>
      </div>
    </section>

    <section class="stats">
      <div class="stat-card">
        <span class="stat-icon">👤</span>
        <span class="stat-value">{{ stats.candidates }}</span>
        <span class="stat-label">Кандидатов</span>
      </div>
      <div class="stat-card">
        <span class="stat-icon">🏘️</span>
        <span class="stat-value">{{ stats.districts }}</span>
        <span class="stat-label">Районов</span>
      </div>
      <div class="stat-card">
        <span class="stat-icon">🗳️</span>
        <span class="stat-value">{{ stats.votes }}</span>
        <span class="stat-label">Голосов</span>
      </div>
    </section>

    <section class="info">
      <div class="info-card">
        <h3>Как голосовать</h3>
        <ol>
          <li>Зарегистрируйтесь в системе</li>
          <li>Выберите кандидата или проект</li>
          <li>Отдайте свой голос</li>
        </ol>
      </div>
      <div class="info-card">
        <h3>О системе</h3>
        <p>Прозрачное электронное голосование с защитой от повторных голосов. Каждый пользователь может голосовать только один раз за каждого кандидата.</p>
      </div>
    </section>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '@/api'

const stats = ref({ candidates: 0, districts: 0, votes: 0 })

onMounted(async () => {
  try {
    const [candidatesRes, districtsRes, votesRes] = await Promise.all([
      api.get('/candidates'),
      api.get('/districts'),
      api.get('/candidates/my-votes').catch(() => ({ data: { length: 0 } }))
    ])
    stats.value = {
      candidates: candidatesRes.data.candidates?.length || 0,
      districts: districtsRes.data?.length || 0,
      votes: Array.isArray(votesRes.data) ? votesRes.data.length : 0
    }
  } catch {}
})
</script>

<style scoped>
.hero {
  text-align: center;
  padding: 48px 24px;
  background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
  border-radius: 16px;
  color: white;
  margin-bottom: 32px;
}
.hero h1 {
  font-size: 2rem;
  color: white;
  margin-bottom: 12px;
}
.hero p {
  font-size: 1.1rem;
  opacity: 0.85;
  margin-bottom: 24px;
  line-height: 1.5;
}
.hero-actions {
  display: flex;
  gap: 12px;
  justify-content: center;
}
.btn {
  padding: 12px 32px;
  border-radius: 10px;
  font-size: 1rem;
  font-weight: 600;
  text-decoration: none;
  transition: all 0.2s;
}
.btn:hover {
  text-decoration: none;
  transform: translateY(-1px);
}
.btn-primary {
  background: #4361ee;
  color: white;
}
.btn-primary:hover {
  background: #3651d4;
  color: white;
}
.btn-secondary {
  background: rgba(255,255,255,0.15);
  color: white;
  border: 1px solid rgba(255,255,255,0.3);
}
.btn-secondary:hover {
  background: rgba(255,255,255,0.25);
  color: white;
}

.stats {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
  margin-bottom: 32px;
}
.stat-card {
  background: white;
  padding: 24px;
  border-radius: 12px;
  text-align: center;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.stat-icon {
  font-size: 2rem;
  display: block;
  margin-bottom: 8px;
}
.stat-value {
  font-size: 1.8rem;
  font-weight: 700;
  color: #1a1a2e;
  display: block;
}
.stat-label {
  font-size: 0.85rem;
  color: #666;
  margin-top: 4px;
}

.info {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
}
.info-card {
  background: white;
  padding: 24px;
  border-radius: 12px;
  box-shadow: 0 1px 4px rgba(0,0,0,0.06);
}
.info-card h3 {
  margin-bottom: 12px;
  font-size: 1.1rem;
}
.info-card ol {
  padding-left: 20px;
}
.info-card li {
  margin-bottom: 8px;
  line-height: 1.4;
}
.info-card p {
  line-height: 1.6;
  color: #444;
}

@media (max-width: 600px) {
  .hero { padding: 32px 16px; }
  .hero h1 { font-size: 1.5rem; }
  .stats { grid-template-columns: 1fr; }
  .info { grid-template-columns: 1fr; }
}
</style>
