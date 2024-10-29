<template>
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl pt-5">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item">
                        <router-link to="../../main">Main</router-link>
                    </li>
                    <li class="breadcrumb-item">
                        <router-link to="../../users">Users</router-link>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">
                        <router-link :to="'../../users/edit/' + this.$route.params.id">
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
                        Replicate <strong>General Info, Applications,</strong> along with their dependencies from
                        <router-link :to="'../../users/edit/' + this.$route.params.id">the source</router-link> for a
                        seamless transfer of comprehensive data.

                    </div>
                </div>
                <div class="col-12 col-md-8">
                    <div class="app-card app-card-settings shadow-sm p-4">
                        <div class="app-card-body">
                            <form class="settings-form" @submit.prevent="onSubmit">
                                <div class="mb-3">
                                    <label class="form-label">First name*</label>
                                    <input v-model="user.firstName" type="text" class="form-control" minlength="3"
                                        maxlength="200" placeholder="First name" required autocomplete="off"
                                        ref="FirstName" />
                                    <div class="invalid-feedback">
                                        {{ errorMessage }}
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Last name*</label>
                                    <input v-model="user.lastName" type="text" class="form-control" minlength="2"
                                        maxlength="200" placeholder="Last name" required autocomplete="off"
                                        ref="LastName" />
                                    <div class="invalid-feedback">
                                        {{ errorMessage }}
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Email</label>
                                    <input v-model="user.email" type="email" class="form-control" placeholder="Email"
                                        autocomplete="off" ref="Email" />
                                    <div class="invalid-feedback">
                                        {{ errorMessage }}
                                    </div>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Username*</label>
                                    <input v-model="user.username" class="form-control" minlength="1" maxlength="20"
                                        placeholder="Username" required autocomplete="off" ref="Username" />
                                    <div class="invalid-feedback">
                                        {{ errorMessage }}
                                    </div>
                                </div>
                                <div class="mb-3" v-if="isInIdp('Default')">
                                    <label class="form-label">Password</label>
                                    <div class="input-group">
                                        <input v-model="user.password" id="password" type="password"
                                            class="form-control" minlength="7" placeholder="Password" autocomplete="off"
                                            ref="Password" />
                                        <button class="btn app-btn-outline-secondary" type="button"
                                            @click="toggleViewKey('password')">
                                            <i class="bi bi-eye"></i>
                                        </button>
                                        <div class="invalid-feedback">
                                            {{ errorMessage }}
                                        </div>
                                    </div>
                                </div>
                                <div class="mb-3" v-if="isInIdp('Default')">
                                    <label class="form-label">Repeat password</label>
                                    <div class="input-group">
                                        <input type="password" id="re-password" class="form-control signin-password"
                                            placeholder="Password" v-model="user.repeatPassword" autocomplete="off"
                                            ref="RepeatPassword">
                                        <button class="btn app-btn-outline-secondary" type="button"
                                            @click="toggleViewKey('re-password')">
                                            <i class="bi bi-eye"></i>
                                        </button>
                                        <div class="invalid-feedback">
                                            {{ errorMessage }}
                                        </div>
                                    </div>
                                </div><!--//form-group-->

                                <p><small class="text-muted">(Please note that if you key in the first name, last name, username of an existing
                                        record, that
                                        particular record will be overridden with the replicated data.)</small></p>

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
import { getTokenInfo } from '@/services/account.service';
import { copyUser } from "@/services/user.service";
import { emitter } from "@/services/emitter.service";
import * as navbar from "@/services/navbar.service";
export default {
    data: () => ({
        user: new Object(),
        errorMessage: '---'
    }),
    mounted() {
        navbar.init(window.location.pathname);
    },
    methods: {
        toggleViewKey(param) {
            var e = document.getElementById(param);
            if (e.type === "password") {
                e.type = "text";
            } else {
                e.type = "password";
            }
        },

        onSubmit() {
            // Clear error messages
            const allInputs = document.querySelectorAll('.is-invalid');

            allInputs.forEach((input) => {
                input.classList.remove('is-invalid');
            });

            this.user.email = this.user.email === '' ? null : this.user.email;

            emitter.emit("showLoader", true);
            copyUser(this.$route.params.id, this.user).then(r => {
                this.$router.push("../../users");
                emitter.emit("showLoader", false);
            }, error => {
                // Check if the error is related to a specific input field
                if (error && error.response && error.response.data && error.response.data.errors) {
                    const errorField = Object.keys(error.response.data.errors)[0];

                    // Add 'is-invalid' class to the corresponding input field
                    const inputElement = this.$refs[errorField];
                    if (inputElement) {
                        inputElement.classList.add('is-invalid');

                        const errorMessage = Object.values(error.response.data.errors)[0][0];
                        this.errorMessage = errorMessage;
                    }
                }
                else {
                    alert(error.response.data);
                }

                emitter.emit("showLoader", false);
                return false;
            });
        },

        isInIdp(idp) {
            return getTokenInfo().authmethod === idp;
        }
    }
}
</script>