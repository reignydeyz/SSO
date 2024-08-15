<template>
    <div class="alert alert-danger" role="alert" v-show="roles.length <= 0">
        It seems like roles haven't been set up yet. Unfortunately, adding a group is currently not possible.
    </div>
    <div v-show="roles.length > 0">
        <div class="row g-4 settings-section">
            <div class="col-12 col-md-2">
                <h3 class="section-title">Add group</h3>
                <div class="section-intro">
                    Assign group to the application.
                </div>
            </div>
            <div class="col-12 col-md-10">
                <div class="app-card app-card-settings shadow-sm p-4">
                    <div class="app-card-body">
                        <div class="col-md-8">
                            <input type="text" class="form-control" placeholder="Name" required autocomplete="off"
                                ref="group">
                            <small class="ms-1 text-muted">(typeahead)</small>
                        </div>
                    </div>
                    <!--//app-card-body-->
                </div>
                <!--//app-card-->
            </div>
        </div>

        <hr class="mb-4" v-show="groups.length > 0"/>
        <h5 class="section-title mb-3" v-show="groups.length > 0">Assigned groups</h5>
        <div v-for="i in groups" :key="i.id">
            <div class="row g-4 settings-section mb-4">
                <div class="col-6">
                    <div class="app-card app-card-settings shadow-sm p-4">
                        <div class="app-card-body">
                            <router-link :to="'../../groups/edit/' + i.id"><h3 class="section-title"><i>{{ i.name }}</i></h3></router-link>
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
                    <h5 class="modal-title">Add group/roles</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p><b>{{ group.name }}</b></p>
                    <div class="form-check form-check-inline mt-2" v-for="r in group.roles" :key="r.roleId">
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
import { searchGroup } from "@/services/group.service";
import { getAppGroupRoles, updateAppGroupRoles } from "@/services/application-group-role.service";
import { searchAppGroup, removeAppGroup } from "@/services/application-group.service";
import { emitter } from "@/services/emitter.service";
export default {
    props: ["app", "roles"],
    data: () => ({
        group: new Object(),
        groups: [],
        modal: null,
        query: "",
    }),
    watch: {
        roles(newVal, oldVal) {
            this.search();
        }
    },
    mounted() {
        // Listen for the hidden.bs.modal event and call onModalClose method
        this.modal = new bootstrap.Modal(this.$refs.modal);
        this.modal._element.addEventListener('hidden.bs.modal', this.onModalClose);

        autocomplete({
            input: this.$refs.group,
            minLength: 3,
            fetch: async function (text, update) {
                var res = await searchGroup(
                    { name: text },
                    'name',
                    'asc',
                    1,
                    5
                );

                var suggestions = res.data.value.map(obj => ({
                    label: `${obj.name}`,
                    value: obj.groupId
                }));

                update(suggestions);
            },
            onSelect: (item) => {
                this.group = {
                    id: item.value,
                    name: item.label,
                    roles: this.roles.map(role => ({ ...role, selected: false })),
                };

                this.modal.show();
            }
        });
    },
    methods: {
        onModalClose() {
            this.$refs.group.value = '';
        },

        onAdd() {
            var selectedIds = this.group.roles.filter(x => x.selected === true).map(x => x.roleId);

            if (selectedIds.length <= 0) {
                alert('Please select at least 1 entry.');
                return false;
            }

            emitter.emit("showLoader", true);
            updateAppGroupRoles(this.app.applicationId, this.group.id, selectedIds).then(r => {
                this.group = new Object();
                this.query = '';
                this.search();

                this.modal.hide();

                emitter.emit("showLoader", false);
            });
        },

        onUpdate(group) {
            var selectedIds = group.roles.filter(x => x.selected === true).map(x => x.roleId);

            if (selectedIds.length <= 0) {
                alert('Please select at least 1 entry.');
                return false;
            }

            emitter.emit("showLoader", true);
            updateAppGroupRoles(this.app.applicationId, group.id, selectedIds).then(r => {
                emitter.emit("showLoader", false);
            });
        },

        remove(groupId) {
            if (confirm('Are you sure you want to delete this record?')) {
                emitter.emit("showLoader", true);
                removeAppGroup(this.app.applicationId, groupId).then(r => {
                    this.search();
                    emitter.emit("showLoader", false);
                });
            }
        },

        search(p) {
            emitter.emit("showLoader", true);

            searchAppGroup(this.app.applicationId, { name: this.query },
                "name",
                "asc",
                1,
                15).then(async r => {
                    const groupPromises = r.data.value.map(async obj => {
                        const groupRoles = await Promise.all(
                            this.roles.map(async role => {
                                const appGroupRoles = await getAppGroupRoles(this.app.applicationId, obj.groupId);
                                const selected = appGroupRoles.data.some(x => x.roleId === role.roleId);
                                return { ...role, selected };
                            })
                        );

                        return {
                            name: `${obj.name}`,
                            id: obj.groupId,
                            roles: groupRoles
                        }
                    });

                    // Wait for all groupPromises to resolve
                    this.groups = await Promise.all(groupPromises);

                    emitter.emit("showLoader", false);
                }, err => {
                    console.log(err);
                    emitter.emit("showLoader", false);
                });
        }
    }
}
</script>