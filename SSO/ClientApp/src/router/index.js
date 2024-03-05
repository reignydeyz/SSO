import { createWebHistory, createRouter } from "vue-router";
import { canActivate } from '@/services/auth-guard.service';

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
        path: "/changepassword",
        name: "ChangePassword",
        meta: { realm: 'Default' },
        component: () => import('@/pages/ChangePassword.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
    {
        path: "/main",
        name: "Main",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Main.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
    {
        path: "/applications",
        name: "Applications",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Applications/Manage.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
    {
        path: "/applications/new",
        name: "New Application",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Applications/New.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
    {
        path: "/applications/edit/:id",
        name: "Update Application",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Applications/Edit.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
    {
        path: "/applications/copy/:id",
        name: "Copy Application",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Applications/Copy.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
    {
        path: "/users",
        name: "Users",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Users/Manage.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
    {
        path: "/users/new",
        name: "New User",
        meta: { mustBeRoot: true, realm: 'Default' },
        component: () => import('@/pages/Users/New.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
    {
        path: "/users/edit/:id",
        name: "Update User",
        meta: { mustBeRoot: true },
        component: () => import('@/pages/Users/Edit.vue'),
        beforeEnter: (to, from, next) => {
            next(canActivate(to))
          }
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;