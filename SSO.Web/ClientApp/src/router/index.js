import { createWebHistory, createRouter } from "vue-router";

const routes = [
    {
        path: "/",
        name: "Login",
        component: () => import('@/pages/Login.vue'),
    },
    {
        path: "/main",
        name: "Main",
        component: () => import('@/pages/Main.vue'),
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;