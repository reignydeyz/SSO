<template>
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl pt-5">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item">
                        <router-link to="../../main">Main</router-link>
                    </li>
                    <li class="breadcrumb-item">
                        <router-link to="../../applications">Applications</router-link>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">
                        <router-link :to="'../../applications/edit/' + this.$route.params.id">
                            {{ this.$route.params.id.slice(this.$route.params.id.length - 8) }}
                        </router-link>
                    </li>
                </ol>
            </nav>
            <div class="row g-3 mb-4 align-items-center justify-content-between">
                <div class="col-auto">
                    <h1 class="app-page-title mb-0">Copy</h1>
                </div>
            </div>
            <hr class="mb-4" />
            <div class="d-flex justify-content-center row g-4 settings-section">
                <div class="col-12 col-md-4">
                    <h3 class="section-title">Create a copy</h3>
                    <div class="section-intro">
                        Replicate <strong>General Info, Callbacks, Permissions, Roles,</strong> and
                        <strong>Users</strong> along with their dependencies from <router-link
                            :to="'../../applications/edit/' + this.$route.params.id">the source</router-link> for a
                        seamless transfer of comprehensive data.

                    </div>
                </div>
                <div class="col-12 col-md-8">
                    <div class="app-card app-card-settings shadow-sm p-4">
                        <div class="app-card-body">
                            <form class="settings-form" @submit.prevent="onSubmit">
                                <div class="mb-3">
                                    <label for="setting-input-2" class="form-label">Name*</label>
                                    <input v-model="application.name" type="text" class="form-control"
                                        id="setting-input-2" maxlength="200" placeholder="Name" required
                                        autocomplete="off" />
                                        <small class="text-muted">(Please note that if you key in the name of an existing
                                            record, that
                                            particular record will be overridden with the replicated data.)</small>
                                </div>

                                <button type="submit" class="btn app-btn-primary mt-3">
                                    Save Changes
                                </button>

                            </form>
                        </div>
                        <!--//app-card-body-->
                    </div>
                    <!--//app-card-->
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { copyApp } from "@/services/application.service";
import { emitter } from "@/services/emitter.service";
import * as navbar from "@/services/navbar.service";
export default {
    data: () => ({
        application: new Object(),
    }),
    mounted() {
        navbar.init(window.location.pathname);
    },
    methods: {
        onSubmit() {
            emitter.emit("showLoader", true);

            copyApp(this.$route.params.id, this.application).then(
                (r) => {
                    this.$router.push("../../applications");
                },
                (err) => {
                    alert(err.response.data);
                    emitter.emit("showLoader", false);
                }
            );
        },
    },
};
</script>