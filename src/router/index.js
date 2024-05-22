import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/Dashboard.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView
    },
    {
      path: '/clientes',
      name: 'clientes',
      component: () => import('../views/ClientesView.vue')
    },
    {
      path: '/motoristas',
      name: 'motoristas',
      component: () => import('../views/MotoristasView.vue')
    },
    {
      path: '/rotas',
      name: 'rotas',
      component: () => import('../views/RotasView.vue')
    }
  ]
})

export default router
