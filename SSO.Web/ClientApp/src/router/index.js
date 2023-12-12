import { createWebHistory, createRouter } from "vue-router";
import { canActivate } from '@/services/auth-guard.service'

const routes = [
    {
        path: "/",
        name: "Login",
        component: () => import('@/pages/Login.vue'),
    },
    {
        path: "/root",
        name: "Root",
        component: () => import('@/pages/Root.vue'),
    },
    {
        path: "/main",
        name: "Main",
        component: () => import('@/pages/Main.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate())
          }
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;