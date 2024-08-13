<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-2">
            <h3 class="section-title">Add application</h3>
            <div class="section-intro">
                Assign group to the application.
            </div>
        </div>
        <div class="col-12 col-md-10">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <div class="col-md-8">
                        <input v-model="app.name" type="text" class="form-control" placeholder="App" required
                            autocomplete="off" ref="app">
                        <small class="ms-1 text-muted">(typeahead)</small>
                    </div>
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
    </div>

    <div v-show="apps.length > 0">
        <hr class="mb-4" />
        <h5 class="section-title mb-3">Assigned apps</h5>

        <div v-for="i in apps" :key="i.id">
            <div class="row g-4 settings-section mb-4">
                <div class="col-12">
                    <div class="app-card app-card-settings shadow-sm p-4">
                        <div class="app-card-body">
                            <router-link :to="'../../applications/edit/' + i.id"><h3 class="section-title"><i>{{ i.name }}</i></h3></router-link>
                            
                            <div class="form-check form-check-inline mt-3" v-for="r in i.roles" :key="r.roleId">
                                <label>
                                    <input class="form-check-input" type="checkbox" value="" v-model="r.selected" />
                                    <span>{{ r.name }}</span>
                                </label>
                            </div>
                            <div class="mt-3">
                                <button type="button" class="btn app-btn-primary me-2" @click="onUpdate(i)">
                                    Save changes
                                </button>
                                <button type="button" class="btn app-btn-outline-danger bg-white"
                                    @click="remove(i.id)">Remove</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal" id="myModal" tabindex="-1" data-bs-backdrop="static" ref="modal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add app/roles</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p><b>{{ app.name }}</b></p>
                    <div class="form-check form-check-inline mt-2" v-for="r in app.roles" :key="r.roleId">
                        <label>
                            <input class="form-check-input" type="checkbox" value="" v-model="r.selected" />
                            <span>{{ r.name }}</span>
                        </label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn app-btn-primary" @click="onAdd">Add</button>
                    <button type="button" class="btn app-btn-outline-primary" data-bs-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import autocomplete from 'autocompleter';
import { searchApp } from "@/services/application.service";
import { getAppRoles } from "@/services/application-role.service";
import { getGroupApps } from "@/services/group-application.service";
import { removeAppGroup } from "@/services/application-group.service";
import { getAppGroupRoles, updateAppGroupRoles } from "@/services/application-group-role.service";
import { emitter } from "@/services/emitter.service";
export default {
    props: ["group"],
    data: () => ({
        app: new Object(),
        apps: [],
        modal: null,
    }),
    watch: {
        group(newVal, oldVal) {
            this.getApps();
        }
    },
    mounted() {
        // Listen for the hidden.bs.modal event and call onModalClose method
        this.modal = new bootstrap.Modal(this.$refs.modal);
        this.modal._element.addEventListener('hidden.bs.modal', this.onModalClose);

        autocomplete({
            input: this.$refs.app,
            minLength: 3,
            fetch: async function (text, update) {
                var res = await searchApp(
                    { name: text },
                    'name',
                    'asc',
                    1,
                    5
                );

                var suggestions = res.data.value.map(obj => ({
                    label: obj.name,
                    value: obj.applicationId
                }));

                update(suggestions);
            },
            onSelect: async (item) => {
                const appRoles = (await getAppRoles(item.value)).data;

                this.app = {
                    id: item.value,
                    name: item.label,
                    roles: appRoles.map(role => ({ ...role, selected: false })),
                };

                this.modal.show();
            }
        });
    },
    methods: {
        onAdd() {
            var selectedIds = this.app.roles.filter(x => x.selected === true).map(x => x.roleId);

            if (selectedIds.length <= 0) {
                alert('Please select at least 1 entry.');
                return false;
            }

            emitter.emit("showLoader", true);
            updateAppGroupRoles(this.app.id, this.group.groupId, selectedIds).then(r => {
                this.app = new Object();
                this.getApps();

                this.modal.hide();

                emitter.emit("showLoader", false);
            });
        },
        onUpdate(app) {
            var selectedIds = app.roles.filter(x => x.selected === true).map(x => x.roleId);

            if (selectedIds.length <= 0) {
                alert('Please select at least 1 entry.');
                return false;
            }

            emitter.emit("showLoader", true);
            updateAppGroupRoles(app.id, this.group.groupId, selectedIds).then(r => {
                emitter.emit("showLoader", false);
            });
        },
        remove(appId) {
            if (confirm('Are you sure you want to delete this record?')) {
                emitter.emit("showLoader", true);
                removeAppGroup(appId, this.group.groupId).then(r => {
                    this.getApps();
                    emitter.emit("showLoader", false);
                });
            }
        },
        getApps() {
            getGroupApps(this.group.groupId).then(async r => {
                const appPromises = r.data.map(async obj => {
                    const appRoles = (await getAppRoles(obj.applicationId)).data;
                    const groupRoles = await Promise.all(
                        appRoles.map(async role => {
                            const appGroupRoles = await getAppGroupRoles(obj.applicationId, this.group.groupId);
                            const selected = appGroupRoles.data.some(x => x.roleId === role.roleId);
                            return { ...role, selected };
                        })
                    );

                    return {
                        name: obj.name,
                        id: obj.applicationId,
                        roles: groupRoles
                    }
                });

                this.apps = await Promise.all(appPromises);

                emitter.emit("showLoader", false);
            });
        }
    }
}
</script>