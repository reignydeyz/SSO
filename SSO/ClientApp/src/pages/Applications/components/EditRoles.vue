<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-2">
            <h3 class="section-title">Create role</h3>
            <div class="section-intro">
                Add new role for the application.
            </div>
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
    <div v-for="i in roles" :key="i.roleId">
        <hr class="mb-4" />
        <div class="row g-4 settings-section">
            <div class="col-12 col-md-2">
                <h3 class="section-title"><i>{{ i.name }}</i></h3>
                <button type="button" class="app-btn-sm app-btn-outline-danger" @click="onRemove(i.roleId)">Remove</button>
            </div>
            <div class="col-12 col-md-10" v-if="getPermissions(i.roleId).length > 0">
                <div class="app-card app-card-settings shadow-sm p-4">
                    <div class="app-card-body">
                        <p>Select the permissions for this role</p>
                        <div class="form-check form-check-inline" v-for="p in getPermissions(i.roleId)"
                            :key="p.permissionId">
                            <label>
                                <input class="form-check-input" type="checkbox" value="" v-model="p.selected" />
                                <span>{{ p.description }}</span>
                            </label>
                        </div>
                        <br />
                        <button type="button" class="btn app-btn-primary mt-3">
                            Save changes
                        </button>
                    </div>
                    <!--//app-card-body-->
                </div>
                <!--//app-card-->
            </div>
        </div>
    </div>
</template>

<script>
import { addAppRole, removeAppRole } from "@/services/application-role.service";
import { getAppRolePermissions } from "@/services/application-role-permission.service";
import { emitter } from "@/services/emitter.service";
export default {
    props: ["app", "roles", "permissions"],
    emits: ["loadRoles"],
    data: () => ({
        appRole: new Object(),
        rolePermissions: []
    }),
    watch: {
        permissions(newVal, oldVal) {
            emitter.emit("showLoader", true);

            this.rolePermissions = [];

            this.roles.forEach(async (role) => {

                const selectedPermissions = (await getAppRolePermissions(this.app.applicationId, role.roleId)).data;

                newVal.forEach(item => {
                    this.rolePermissions.push({
                        roleId: role.roleId,
                        permissionId: item.permissionId,
                        description: item.description,
                        selected: selectedPermissions.some(x => x.permissionId === item.permissionId),
                    });
                });
            });

            emitter.emit("showLoader", false);
        },

        roles(newVal, oldVal) {
            emitter.emit("showLoader", true);

            this.rolePermissions = [];

            newVal.forEach(async (item) => {
                const selectedPermissions = (await getAppRolePermissions(this.app.applicationId, item.roleId)).data;

                this.permissions.forEach(permission => {
                    this.rolePermissions.push({
                        roleId: item.roleId,
                        permissionId: permission.permissionId,
                        description: permission.description,
                        selected: selectedPermissions.some(x => x.permissionId === permission.permissionId),
                    });
                });
            });

            emitter.emit("showLoader", false);
        }
    },
    methods: {
        onSubmit() {
            emitter.emit("showLoader", true);

            addAppRole(this.app.applicationId, this.appRole).then(
                (r) => {
                    this.role = new Object();
                    this.$emit("loadRoles");
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
                removeAppRole(this.app.applicationId, id).then(r => {
                    this.$emit("loadRoles");
                    emitter.emit("showLoader", false);
                });
            }
        },

        getPermissions(roleId) {
            return this.rolePermissions.filter(x => x.roleId === roleId);
        }
    }
}
</script>