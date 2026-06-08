import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  { path: '/', name: 'Home', component: () => import('../views/HomeView.vue') },
  { path: '/about', name: 'About', component: () => import('../views/AboutView.vue') },
  { path: '/candidates', name: 'Candidates', component: () => import('../views/CandidatesView.vue') },
  { path: '/candidates/:id', name: 'CandidateDetails', component: () => import('../views/CandidateDetailsView.vue') },
  { path: '/districts', name: 'Districts', component: () => import('../views/DistrictsView.vue') },
  { path: '/districts/:id', name: 'DistrictDetails', component: () => import('../views/DistrictDetailsView.vue') },
  { path: '/my-votes', name: 'MyVotes', component: () => import('../views/MyVotesView.vue'), meta: { requiresAuth: true } },
  { path: '/login', name: 'Login', component: () => import('../views/LoginView.vue') },
  { path: '/register', name: 'Register', component: () => import('../views/RegisterView.vue') },
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
