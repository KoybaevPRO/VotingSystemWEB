import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import CandidatesView from '@/views/CandidatesView.vue'
import CandidateDetailsView from '@/views/CandidateDetailsView.vue'
import DistrictsView from '@/views/DistrictsView.vue'
import DistrictDetailsView from '@/views/DistrictDetailsView.vue'
import AboutView from '@/views/AboutView.vue'
import LoginView from '@/views/LoginView.vue'
import RegisterView from '@/views/RegisterView.vue'
import MyVotesView from '@/views/MyVotesView.vue'

const routes = [
  { path: '/', name: 'home', component: HomeView },
  { path: '/candidates', name: 'candidates', component: CandidatesView },
  { path: '/candidates/:id', name: 'candidate-details', component: CandidateDetailsView },
  { path: '/districts', name: 'districts', component: DistrictsView },
  { path: '/districts/:id', name: 'district-details', component: DistrictDetailsView },
  { path: '/about', name: 'about', component: AboutView },
  { path: '/login', name: 'login', component: LoginView },
  { path: '/register', name: 'register', component: RegisterView },
  { path: '/my-votes', name: 'my-votes', component: MyVotesView, meta: { requiresAuth: true } }
]

const router = createRouter({
  history: createWebHistory(),
  routes
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  if (to.meta.requiresAuth && !token) {
    next('/login')
  } else {
    next()
  }
})

export default router
