<template>
    <header class="app-header fixed-top">
        <div class="app-header-inner">
            <div class="container-fluid py-2">
                <div class="app-header-content">
                    <div class="row justify-content-between align-items-center">
                        <div class="col-auto">
                            <a id="sidepanel-toggler" class="sidepanel-toggler d-inline-block d-xl-none" href="#"
                                @click="onMenuClick()">
                                <i class="fas fa-bars"></i>
                            </a>
                        </div>

                        <div class="app-utilities col-auto">
                            <div class="app-utility-item">
                                <span class="text-muted">SSO</span>
                            </div>

                            <div class="app-utility-item app-user-dropdown dropdown">
                                <a class="dropdown-toggle" id="user-dropdown-toggle" data-bs-toggle="dropdown" href="#"
                                    role="button" aria-expanded="false">
                                    <i class="far fa-user-circle fs-4"></i>
                                </a>
                                <ul class="dropdown-menu" aria-labelledby="user-dropdown-toggle">
                                    <li>
                                        <button class="dropdown-item" href="#" @click="signout">
                                            Log Out
                                        </button>
                                    </li>
                                </ul>
                            </div>
                            <!--//app-user-dropdown-->
                        </div>
                        <!--//app-utilities-->
                    </div>
                    <!--//row-->
                </div>
                <!--//app-header-content-->
            </div>
            <!--//container-fluid-->
        </div>
        <!--//app-header-inner-->
        <div id="app-sidepanel" class="app-sidepanel">
            <div id="sidepanel-drop" class="sidepanel-drop"></div>
            <div class="sidepanel-inner d-flex flex-column">
                <a href="#" id="sidepanel-close" class="sidepanel-close d-xl-none" @click="onMenuClose()">&times;</a>
                <div class="app-branding">
                    <router-link to="/main" class="app-logo">
                        <img class="logo-icon me-2" :src="require('@/assets/logo.png')" alt="logo" /><span
                            class="logo-text">SSO</span>
                    </router-link>
                </div>
                <!--//app-branding-->

                <nav id="app-nav-main" class="app-nav app-nav-main flex-grow-1">
                    <ul class="app-menu list-unstyled accordion" id="menu-accordion">
                        <li class="nav-item">
                            <!--//Bootstrap Icons: https://icons.getbootstrap.com/ -->

                            <router-link to="/main" class="nav-link">
                                <span class="nav-icon">
                                    <i class="fas fa-home"></i>
                                </span>
                                <span class="nav-link-text">Main</span>
                            </router-link>
                            <!--//nav-link-->
                        </li>
                    </ul>
                    <!--//app-menu-->
                </nav>
                <!--//app-nav-->
                <div class="app-sidepanel-footer">

                </div>
                <!--//app-sidepanel-footer-->
            </div>
            <!--//sidepanel-inner-->
        </div>
        <!--//app-sidepanel-->
    </header>
</template>

<script>
import { logout } from "@/services/authentication.service";
export default {
    mounted() {

    },

    methods: {
        onMenuClick() {
            document
                .getElementById("app-sidepanel")
                .classList.add("sidepanel-visible");
        },

        onMenuClose() {
            document
                .getElementById("app-sidepanel")
                .classList.remove("sidepanel-visible");
        },

        signout() {
            logout().then(r => {
                window.location.href = '/';
            })
        },
    },
};
</script>

<style scope>
.app-nav .nav-icon {
    position: absolute;
    left: 1rem;
    top: initial;
}
</style>