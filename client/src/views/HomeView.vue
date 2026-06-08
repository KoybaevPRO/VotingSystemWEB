<template>
  <div class="text-center mb-4">
    <img src="/images/vladikavkaz.svg" alt="Владикавказ" class="img-fluid rounded shadow-sm mb-4" style="max-height: 300px; width: 100%; object-fit: cover;" />
    <h1 class="display-4">Система голосования</h1>
    <p class="lead text-muted">Электронное голосование города Владикавказ</p>
    <hr class="my-4" />
  </div>

  <div class="row g-4">
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 shadow-sm border-primary">
        <div class="card-body text-center">
          <i class="bi bi-people-fill text-primary" style="font-size: 3rem;"></i>
          <h5 class="card-title mt-3">Голосование за кандидатов</h5>
          <p class="card-text text-muted">Просмотрите список кандидатов и отдайте свой голос за достойного представителя</p>
          <router-link to="/candidates" class="btn btn-primary">Перейти к кандидатам</router-link>
        </div>
      </div>
    </div>
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 shadow-sm border-success">
        <div class="card-body text-center">
          <i class="bi bi-buildings-fill text-success" style="font-size: 3rem;"></i>
          <h5 class="card-title mt-3">Благоустройство районов</h5>
          <p class="card-text text-muted">Выберите проекты по улучшению инфраструктуры районов Владикавказа</p>
          <router-link to="/districts" class="btn btn-success">К проектам</router-link>
        </div>
      </div>
    </div>
    <div class="col-md-6 col-lg-4">
      <div class="card h-100 shadow-sm border-info">
        <div class="card-body text-center">
          <i class="bi bi-info-circle-fill text-info" style="font-size: 3rem;"></i>
          <h5 class="card-title mt-3">О проекте</h5>
          <p class="card-text text-muted">Узнайте о разработчиках системы и используемых технологиях</p>
          <router-link to="/about" class="btn btn-info text-white">Подробнее</router-link>
        </div>
      </div>
    </div>
  </div>

  <div v-if="!auth.isAuthenticated" class="row mt-5">
    <div class="col text-center">
      <div class="p-4 bg-light rounded">
        <h4>Примите участие в голосовании</h4>
        <p class="text-muted">Зарегистрируйтесь или войдите, чтобы голосовать</p>
        <router-link to="/register" class="btn btn-primary me-2">
          <i class="bi bi-person-plus"></i> Регистрация
        </router-link>
        <router-link to="/login" class="btn btn-outline-primary">
          <i class="bi bi-box-arrow-in-right"></i> Войти
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup>
import { useAuthStore } from '../stores/auth'
const auth = useAuthStore()
</script>
