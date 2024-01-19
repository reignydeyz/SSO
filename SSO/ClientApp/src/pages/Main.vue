<template>
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl pt-5">
            <h1 class="app-page-title">Overview</h1>
            <div class="row g-4 mb-4">
                <div class="col-12 col-lg-4">
                    <div class="
              app-card app-card-basic
              d-flex
              flex-column
              align-items-start
              shadow-sm
            ">
                        <div class="app-card-header p-3 border-bottom-0">
                            <div class="row align-items-center gx-3">
                                <div class="col-auto">
                                    <div class="app-icon-holder">
                                        <i class="fas fa-cubes"></i>
                                    </div>
                                    <!--//icon-holder-->
                                </div>
                                <!--//col-->
                                <div class="col-auto">
                                    <h4 class="app-card-title">Applications</h4>
                                </div>
                                <!--//col-->
                            </div>
                            <!--//row-->
                        </div>
                        <!--//app-card-header-->
                        <div class="app-card-body px-4">
                            <div class="intro">
                                Manage apps; assign permissons, roles and users.
                            </div>
                        </div>
                        <!--//app-card-body-->
                        <div class="app-card-footer p-4 mt-auto">
                            <div class="btn-group">
                                <router-link to="/applications/new" class="btn app-btn-secondary">Create New</router-link>
                                <router-link to="/applications" class="btn app-btn-secondary">Manage</router-link>
                            </div>
                        </div>
                        <!--//app-card-footer-->
                    </div>
                    <!--//app-card-->
                </div>

                <div class="col-12 col-lg-4">
                    <div class="
              app-card app-card-basic
              d-flex
              flex-column
              align-items-start
              shadow-sm
            ">
                        <div class="app-card-header p-3 border-bottom-0">
                            <div class="row align-items-center gx-3">
                                <div class="col-auto">
                                    <div class="app-icon-holder">
                                        <i class="fas fa-users"></i>
                                    </div>
                                    <!--//icon-holder-->
                                </div>
                                <!--//col-->
                                <div class="col-auto">
                                    <h4 class="app-card-title">Users</h4>
                                </div>
                                <!--//col-->
                            </div>
                            <!--//row-->
                        </div>
                        <!--//app-card-header-->
                        <div class="app-card-body px-4">
                            <div class="intro">
                                Manage users; assign apps.
                            </div>
                        </div>
                        <!--//app-card-body-->
                        <div class="app-card-footer p-4 mt-auto">
                            <div class="btn-group">
                                <router-link to="/users/new" class="btn app-btn-secondary">Create New</router-link>
                                <router-link to="/users" class="btn app-btn-secondary">Manage</router-link>
                            </div>
                        </div>
                        <!--//app-card-footer-->
                    </div>
                    <!--//app-card-->
                </div>
            </div>
        </div>
        <!--//row-->
    </div>
</template>

<script>
import * as navbar from "@/services/navbar.service";

export default {
    mounted() {
        navbar.init(this.$route);
    }
}
</script>