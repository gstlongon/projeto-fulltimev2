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
    },
    {
      path: '/lojas',
      name: 'lojas',
      component: () => import('../views/LojasView.vue')
    },
    {
      path: '/encomendas',
      name: 'encomendas',
      component: () => import('../views/EncomendasView.vue')
    }
  ]
})

export default router
