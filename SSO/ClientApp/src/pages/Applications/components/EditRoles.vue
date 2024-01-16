<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-2">
            <h3 class="section-title">Create role</h3>
        </div>
        <div class="col-12 col-md-10">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <form class="row g-2" @submit.prevent="onSubmit">
                        <div class="col-md-8">
                            <label class="visually-hidden">Name</label>
                            <input v-model="appRole.name" type="type" class="form-control" placeholder="Name" required>
                        </div>
                        <div class="col-auto">
                            <button type="submit" class="btn app-btn-primary mb-3">Add</button>
                        </div>
                    </form>
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
    </div>
    <div v-for="i in appRoles" :key="i.roleId">
        <hr class="mb-4" />
        <div class="row g-4 settings-section">
            <div class="col-12 col-md-2">
                <h3 class="section-title">{{ i.name }}</h3>
                <div class="section-intro">
                    Choose permission for this role.
                </div>
            </div>
            <div class="col-12 col-md-10">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    // TBD
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
        </div>
    </div>
</template>

<script>
import { getAppRoles, addAppRole, removeAppRole } from "@/services/application-role.service";
import { emitter } from "@/services/emitter.service";
export default {
    props: ["app"],
    data: () => ({
        appRole: new Object(),
        appRoles: [],
        updated: false
    }),
    async updated() {
        if (!this.updated) {
            this.appRoles = (await getAppRoles(this.app.applicationId)).data;
            this.updated = true;
        }
    },
    methods: {
        onSubmit() {
            emitter.emit("showLoader", true);

            addAppRole(this.app.applicationId, this.appRole).then(
                (r) => {
                    getAppRoles(this.app.applicationId).then(res => {
                        this.appRoles = res.data;
                        this.appRole = new Object();
                        emitter.emit("showLoader", false);
                    });
                },
                (err) => {
                    alert('Failed to add record.');
                    emitter.emit("showLoader", false);
                }
            );
        },

        onRemove(id) {
            if (confirm('Are you sure you want to delete this record?')) {
                emitter.emit("showLoader", true);
                // TODO: Delete role
            }
        }
    }
}
</script>