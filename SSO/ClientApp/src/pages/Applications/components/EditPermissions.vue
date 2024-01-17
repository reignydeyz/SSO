<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-2">
            <h3 class="section-title">Claims or permissions</h3>
            <div class="section-intro">
                Manage the claims for role assignment.
            </div>
        </div>
        <div class="col-12 col-md-10">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <form class="row g-3" @submit.prevent="onSubmit">
                        <div class="col-md-4">
                            <label class="visually-hidden">Name</label>
                            <input v-model="permission.name" type="text" class="form-control" placeholder="Name" required>
                        </div>
                        <div class="col-md-4">
                            <label class="visually-hidden">Description</label>
                            <input v-model="permission.description" type="text" class="form-control"
                                placeholder="Description" required>
                        </div>
                        <div class="col-auto">
                            <button type="submit" class="btn app-btn-primary mb-3">Add</button>
                        </div>
                    </form>
                    <div class="table-responsive" v-if="permissions && permissions.length > 0">
                        <table class="table app-table-hover mb-0 text-left">
                            <thead>
                                <tr>
                                    <th class="cell"></th>
                                    <th class="cell">Name</th>
                                    <th class="cell">Description</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="i in permissions" :key="i.permissionId">
                                    <td class="cell">
                                        <button class="app-btn-sm app-btn-outline-danger bg-white" type="button"
                                            @click="onRemove(i.permissionId)">
                                            Remove
                                        </button>
                                    </td>
                                    <td class="cell">{{ i.name }}</td>
                                    <td class="cell">{{ i.description }}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
    </div>
</template>

<script>
import { addAppPermission, removeAppPermission } from "@/services/application-permission.service";
import { emitter } from "@/services/emitter.service";
export default {
    props: ["app", "permissions"],
    emits: ["loadPermissions"],
    data: () => ({
        permission: new Object()
    }),
    methods: {
        onSubmit() {
            emitter.emit("showLoader", true);

            addAppPermission(this.app.applicationId, this.permission).then(r => {                
                this.permission = new Object();
                this.$emit("loadPermissions");
                emitter.emit("showLoader", false);
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
                removeAppPermission(this.app.applicationId, id).then(r => {
                    this.$emit("loadPermissions");
                    emitter.emit("showLoader", false);
                });
            }
        }
    }
}
</script>