<template>
    <Navbar v-if="isRoot()" />
    <div v-bind:class="isRoot() ? 'app-wrapper' : 'row g-0 app-auth-wrapper'">
        <router-view />
    </div>
    <Loader v-show="loading" />
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Loader from "@/components/Loader.vue";
import { emitter } from '@/services/emitter.service';
import { hasRootAccess } from '@/services/account.service';

export default {
    components: { Navbar, Loader },
    data() {
        return { loading: false };
    },
    created() {
        emitter.on("showLoader", (e) => {
            this.loading = e;
        });
        
        if (this.isRoot()) {
            document.body.style.backgroundColor = "#F5F6FE";
        }
    },
    methods: {
        isRoot() {
            return hasRootAccess();
        }
    }
}
</script>

<style>
:root {
    --primary-color: #F80;
    --highlight-color: #fff3e5;
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