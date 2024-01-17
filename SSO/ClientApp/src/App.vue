<template>
    <Navbar v-if="showNav" />
    <div v-bind:class="showNav ? 'app-wrapper' : 'row g-0 app-auth-wrapper'">
        <router-view />
    </div>
    <Loader v-show="loading" />
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import Loader from "@/components/Loader.vue";
import { emitter } from '@/services/emitter.service';

export default {
    components: {
        Navbar, Loader,
    },
    watch: {
        '$route'(to, from) {
            // Check if the current route is in the allowedNavs array
            this.showNav = !this.allowedNavs.includes(to.path);
            document.body.style.backgroundColor = this.showNav ? '#F8F8FF' : '';
        },
    },
    data() {
        return { loading: false, showNav: true, allowedNavs: ['/', '/init', '/login', '/changepassword'] };
    },
    created() {
        emitter.on("showLoader", (e) => {
            this.loading = e;
        });
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
    /*background: #fff;*/
    color: crimson;
    border: 1px solid crimson
}

.app-btn-outline-danger:hover {
    background: crimson !important;
    color: white;
    border: 1px solid crimson
}

.app-btn-sm {
    padding: 0.2rem 0.4rem; /* Adjust padding to make it smaller */
    font-size: 0.75rem;    /* Adjust font size to make text smaller */
    line-height: 1.3;      /* Set line height for better text alignment */
    border-radius: 0.15rem; /* Add border-radius for rounded corners */
}

a:hover {
    color: var(--primary-color);
}

.sortable {
    cursor: pointer;
}

button.nav-link.active {
  background-color: var(--primary-color) !important;
  color: white;
}

button.nav-link,
button.nav-link:hover {
  color: var(--primary-color);
}

@media (max-width: 767px) {
  .table-responsive .dropdown-menu {
    transform: translate3d(0px, 3px, 0px) !important;
    position: relative !important;
  }
}
@media (min-width: 768px) {
  .table-responsive {
    overflow: inherit;
  }
}

::placeholder {
  /* Chrome, Firefox, Opera, Safari 10.1+ */
  color: silver !important;
  opacity: 1; /* Firefox */
}
</style>