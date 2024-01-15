<template>
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl pt-5">
            <nav aria-label="breadcrumb">
                <ol class="breadcrumb">
                    <li class="breadcrumb-item">
                        <router-link to="../../">Main</router-link>
                    </li>
                    <li class="breadcrumb-item">
                        <router-link to="../../applications">Applications</router-link>
                    </li>
                    <li class="breadcrumb-item active" aria-current="page">
                        {{ this.$route.params.id.slice(this.$route.params.id.length - 8) }}
                    </li>
                </ol>
            </nav>
            <div class="row g-3 mb-4 align-items-center justify-content-between">
                <div class="col-auto">
                    <h1 class="app-page-title mb-0">Update</h1>
                </div>
            </div>
            <ul class="nav nav-pills mb-3" id="pills-tab" role="tablist">
                <li class="nav-item" role="presentation">
                    <button class="nav-link active" id="pills-home-tab" data-bs-toggle="pill" data-bs-target="#pills-home"
                        type="button" role="tab" aria-controls="pills-home" aria-selected="true">
                        General Info
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="pills-permissions-tab" data-bs-toggle="pill" data-bs-target="#pills-permissions"
                        type="button" role="tab" aria-controls="pills-permissions">
                        Permissions
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="pills-roles-tab" data-bs-toggle="pill" data-bs-target="#pills-roles"
                        type="button" role="tab" aria-controls="pills-roles">
                        Roles
                    </button>
                </li>
                <li class="nav-item" role="presentation">
                    <button class="nav-link" id="pills-users-tab" data-bs-toggle="pill" data-bs-target="#pills-users"
                        type="button" role="tab" aria-controls="pills-users">
                        Users
                    </button>
                </li>
            </ul>
            <div class="tab-content" id="pills-tabContent">
                <div class="tab-pane fade show active" id="pills-home" role="tabpanel" aria-labelledby="pills-home-tab">
                    <hr class="mb-4" />
                    <EditBasic :app="app" />

                    <hr class="mb-4" />
                    <EditCallbacks :app="app" />
                </div>
                <div class="tab-pane fade" id="pills-permissions" role="tabpanel" aria-labelledby="pills-permissions-tab">
                    <hr class="mb-4" />
                    <EditPermissions :app="app" />
                </div>
                <div class="tab-pane fade" id="pills-roles" role="tabpanel" aria-labelledby="pills-roles-tab">
                    // Roles
                </div>
                <div class="tab-pane fade" id="pills-users" role="tabpanel" aria-labelledby="pills-users-tab">
                    // Users
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import * as navbar from "@/services/navbar.service";
import { getAppById } from "@/services/application.service";
import { emitter } from "@/services/emitter.service";
import EditBasic from "@/pages/Applications/components/EditBasic.vue";
import EditCallbacks from "@/pages/Applications/components/EditCallbacks.vue";
import EditPermissions from "@/pages/Applications/components/EditPermissions.vue";
export default {
    components: {
        EditBasic,
        EditCallbacks,
        EditPermissions
    },
    data: () => ({
        app: new Object()
    }),
    mounted() {
        navbar.init(this.$route);

        emitter.emit("showLoader", true);

        getAppById(this.$route.params.id).then(r => {
            this.app = r.data;
            emitter.emit("showLoader", false);
        });
    }
}
</script>