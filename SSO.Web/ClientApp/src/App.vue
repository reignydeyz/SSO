<template>
    <router-view />
    <Loader v-show="loading" />
</template>

<script>
import Loader from "@/components/Loader.vue";
import { emitter } from '@/services/emitter.service';

export default {
    components: { Loader },
    data() {
        return { loading: false };
    },
    created() {
        emitter.on("showLoader", (e) => {
            this.loading = e;
        });
    },
}
</script>

<style>
:root {
    --primary-color: #F80;
    --highlight-color: #f4e6fb;
}

.btn-danger,
.btn-danger:hover {
    color: white;
    background-color: crimson;
}

.app-btn-outline-primary {
    background: #fff;
    color: var(--primary-color);
    border: 1px solid var(--primary-color);
}

.app-btn-outline-primary:hover {
    background: var(--primary-color);
    color: white;
    border: 1px solid var(--primary-color);
}

.app-btn-outline-danger {
    background: #fff;
    color: crimson;
    border: 1px solid crimson
}

.app-btn-outline-danger:hover {
    background: crimson;
    color: white;
    border: 1px solid crimson
}
</style>