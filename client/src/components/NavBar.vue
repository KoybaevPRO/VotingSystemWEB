<template>
  <nav class="navbar navbar-expand-sm navbar-light bg-white border-bottom box-shadow mb-3">
    <div class="container-fluid">
      <router-link class="navbar-brand fw-bold" to="/">
        <i class="bi bi-check2-square"></i> Система голосования
      </router-link>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarContent">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarContent">
        <ul class="navbar-nav flex-grow-1">
          <li class="nav-item">
            <router-link class="nav-link text-dark" to="/">
              <i class="bi bi-house"></i> Главная
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link text-dark" to="/candidates">
              <i class="bi bi-people"></i> Кандидаты
            </router-link>
          </li>
          <li class="nav-item">
            <router-link class="nav-link text-dark" to="/districts">
              <i class="bi bi-buildings"></i> Благоустройство
            </router-link>
          </li>
          <li v-if="auth.isAuthenticated" class="nav-item dropdown">
            <a class="nav-link dropdown-toggle text-dark" href="#" role="button" data-bs-toggle="dropdown">
              <i class="bi bi-hand-thumbs-up"></i> Мои голоса
            </a>
            <ul class="dropdown-menu">
              <li><router-link class="dropdown-item" to="/my-votes">Все голоса</router-link></li>
            </ul>
          </li>
          <li class="nav-item">
            <router-link class="nav-link text-dark" to="/about">
              <i class="bi bi-info-circle"></i> О проекте
            </router-link>
          </li>
        </ul>
        <ul class="navbar-nav">
          <template v-if="auth.isAuthenticated">
            <li class="nav-item dropdown">
              <a class="nav-link dropdown-toggle text-dark" href="#" role="button" data-bs-toggle="dropdown">
                <i class="bi bi-person-circle"></i> {{ auth.fullName }}
              </a>
              <ul class="dropdown-menu dropdown-menu-end">
                <li><a class="dropdown-item" href="#" @click.prevent="logout"><i class="bi bi-box-arrow-right"></i> Выйти</a></li>
              </ul>
            </li>
          </template>
          <template v-else>
            <li class="nav-item">
              <router-link class="nav-link text-dark" to="/register">
                <i class="bi bi-person-plus"></i> Регистрация
              </router-link>
            </li>
            <li class="nav-item">
              <router-link class="nav-link text-dark" to="/login">
                <i class="bi bi-box-arrow-in-right"></i> Войти
              </router-link>
            </li>
          </template>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script setup>
import { useAuthStore } from '../stores/auth'
import { useRouter } from 'vue-router'

const auth = useAuthStore()
const router = useRouter()

function logout() {
  auth.logout()
  router.push('/')
}
</script>
