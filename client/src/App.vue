<template>
  <div class="app">
    <nav class="nav">
      <RouterLink to="/" class="logo">Голосование Владикавказ</RouterLink>
      <div class="nav-links">
        <RouterLink to="/candidates">Кандидаты</RouterLink>
        <RouterLink to="/districts">Районы</RouterLink>
        <RouterLink to="/about">О проекте</RouterLink>
        <template v-if="auth.user">
          <RouterLink to="/my-votes">Мои голоса</RouterLink>
          <a href="#" @click.prevent="auth.logout">Выход</a>
        </template>
        <template v-else>
          <RouterLink to="/login">Вход</RouterLink>
          <RouterLink to="/register">Регистрация</RouterLink>
        </template>
      </div>
    </nav>
    <main class="main">
      <RouterView />
    </main>
  </div>
</template>

<script setup>
import { useAuthStore } from '@/stores/auth'
const auth = useAuthStore()
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
body {
  font-family: 'Segoe UI', system-ui, -apple-system, sans-serif;
  background: #f0f2f5;
  color: #1a1a2e;
  min-height: 100vh;
}
a {
  color: #4361ee;
  text-decoration: none;
}
a:hover {
  text-decoration: underline;
}
.app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}
.nav {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 24px;
  height: 60px;
  background: #1a1a2e;
  color: white;
  position: sticky;
  top: 0;
  z-index: 100;
  box-shadow: 0 2px 8px rgba(0,0,0,0.15);
}
.logo {
  font-size: 1.1rem;
  font-weight: 700;
  color: white !important;
  text-decoration: none !important;
  white-space: nowrap;
}

.nav-links {
  display: flex;
  gap: 6px;
  align-items: center;
}
.nav-links a {
  color: rgba(255,255,255,0.8);
  text-decoration: none;
  padding: 8px 14px;
  border-radius: 8px;
  font-size: 0.9rem;
  transition: all 0.2s;
}
.nav-links a:hover {
  background: rgba(255,255,255,0.1);
  color: white;
}
.nav-links a.router-link-exact-active {
  background: rgba(255,255,255,0.15);
  color: white;
  font-weight: 600;
}
.main {
  flex: 1;
  max-width: 900px;
  width: 100%;
  margin: 0 auto;
  padding: 32px 24px;
}

h1 {
  font-size: 1.8rem;
  margin-bottom: 16px;
  color: #1a1a2e;
}
h2 {
  font-size: 1.3rem;
  margin-bottom: 12px;
  color: #1a1a2e;
}

button {
  font-family: inherit;
}
</style>
