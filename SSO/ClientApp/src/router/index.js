import { createWebHistory, createRouter } from "vue-router";
import { canActivate } from '@/services/auth-guard.service'
import * as navbar from "@/services/navbar.service";

const routes = [
    {
        path: "/",
        name: "System",
        component: () => import('@/pages/System.vue'),
    },
    {
        path: "/login",
        name: "Login",
        component: () => import('@/pages/Login.vue'),
    },
    ,
    {
        path: "/init",
        name: "Init",
        component: () => import('@/pages/Init.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate())
          }
    },
    {
        path: "/main",
        name: "Main",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Main.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate())
          }
    },
    {
        path: "/applications",
        name: "Applications",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Applications/Manage.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate())
          }
    },
    {
        path: "/applications/new",
        name: "New Application",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Applications/New.vue'),
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