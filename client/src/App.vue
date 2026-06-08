<template>
  <div id="app">
    <NavBar />
    <main class="container py-3">
      <router-view />
    </main>
    <footer class="border-top footer text-muted py-3 mt-auto">
      <div class="container">
        <div class="row">
          <div class="col-md-6">
            &copy; 2026 — Система голосования г. Владикавказ
          </div>
          <div class="col-md-6 text-md-end">
            <small>Разработано: Неграш Н., Дзагоев М., Тотоев А.</small>
          </div>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import NavBar from './components/NavBar.vue'
</script>

<style>
#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}
main {
  flex: 1;
}
</style>
