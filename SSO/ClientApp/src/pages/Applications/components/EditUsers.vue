<template>
    <div class="alert alert-danger" role="alert" v-show="roles.length <= 0">
        It seems like roles haven't been set up yet. Unfortunately, adding a user is currently not possible.
    </div>
    <div v-show="roles.length > 0">
        <div class="row g-4 settings-section">
            <div class="col-12 col-md-2">
                <h3 class="section-title">Add user</h3>
                <div class="section-intro">
                    Assign user to the application.
                </div>
            </div>
            <div class="col-12 col-md-10">
                <div class="app-card app-card-settings shadow-sm p-4">
                    <div class="app-card-body">
                        <div class="col-md-8">
                            <input type="text" class="form-control" placeholder="Name or email" required
                                autocomplete="off" ref="user">
                            <small class="ms-1 text-muted">(typeahead)</small>
                        </div>
                    </div>
                    <!--//app-card-body-->
                </div>
                <!--//app-card-->
            </div>
        </div>

        <hr class="mb-4" />
        <h5 class="section-title mb-3">Assigned users</h5>
        <form @submit.prevent="search(1)">
            <div class="row g-2 align-items-start mb-4">
                <div class="col-8 col-md-6">
                    <input v-model="query" type="text" class="form-control" placeholder="Name or email" autocomplete="off">
                    <small class="ms-1">Found: {{ pagination.totalRecords ?? 0 }}</small>
                </div>
                <div class="col-auto d-flex align-top">
                    <button class="btn app-btn-primary" type="submit">Search</button>
                </div>
            </div>
        </form>

        <div v-for="i in users" :key="i.id">
            <div class="row g-4 settings-section mb-4">
                <div class="col-12">
                    <div class="app-card app-card-settings shadow-sm p-4">
                        <div class="app-card-body">
                            <router-link :to="'../../users/edit/' + i.id"><h3 class="section-title"><i>{{ i.name }}</i></h3></router-link>
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

        <nav aria-label="..." v-if="pagination.endPage > 1" class="app-pagination">
            <ul class="pagination flex-wrap">
                <li class="page-item" v-if="pagination.currentPage > 1">
                    <a class="page-link" tabindex="-1" aria-disabled="true" style="cursor: pointer"
                        @click="paginate(1)">First</a>
                </li>
                <li class="page-item" v-if="pagination.currentPage > 1">
                    <a class="page-link" style="cursor: pointer" @click="paginate(pagination.currentPage - 1)">Previous</a>
                </li>
                <li class="page-item" v-bind:class="{ active: pagination.currentPage == p }"
                    v-for="p in pagination.pageIndices" :key="p">
                    <a class="page-link" style="cursor: pointer" @click="paginate(p)">{{ p }}</a>
                </li>
                <li class="page-item" v-if="pagination.currentPage < pagination.totalPages">
                    <a class="page-link" style="cursor: pointer" @click="paginate(pagination.currentPage + 1)">Next</a>
                </li>
                <li class="page-item" v-if="pagination.currentPage < pagination.totalPages">
                    <a class="page-link" style="cursor: pointer" @click="paginate(pagination.totalPages)">Last</a>
                </li>
            </ul>
        </nav>
    </div>

    <div class="modal" id="myModal" tabindex="-1" data-bs-backdrop="static" ref="modal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add user/roles</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p><b>{{ user.name }}</b></p>
                    <div class="form-check form-check-inline mt-2" v-for="r in user.roles" :key="r.roleId">
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
import { searchUser } from "@/services/user.service";
import { getAppUserRoles, updateAppUserRoles } from "@/services/application-user-role.service";
import { searchAppUser, removeAppUser } from "@/services/application-user.service";
import { emitter } from "@/services/emitter.service";
import { pagination } from "@/services/pagination.service";
export default {
    props: ["app", "roles"],
    data: () => ({
        user: new Object(),
        users: [],
        modal: null,
        query: "",
        sort: "firstName",
        sortDirection: "asc",
        pagination: new Object(),
    }),
    watch: {
        roles(newVal, oldVal) {
            this.search();
        }
    },
    mounted() {
        this.pagination.currentPage = 1;
        this.pagination.pageSize = 15;

        // Listen for the hidden.bs.modal event and call onModalClose method
        this.modal = new bootstrap.Modal(this.$refs.modal);
        this.modal._element.addEventListener('hidden.bs.modal', this.onModalClose);

        autocomplete({
            input: this.$refs.user,
            minLength: 3,
            fetch: async function (text, update) {
                text = text.toLowerCase();
                var res = await searchUser(
                    { firstName: text, lastName: text, email: text },
                    'firstName',
                    'asc',
                    1,
                    5
                );

                var suggestions = res.data.value.map(obj => ({
                    label: `${obj.firstName} ${obj.lastName} (${obj.email})`,
                    value: obj.userId
                }));

                update(suggestions);
            },
            onSelect: (item) => {
                this.user = {
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
            this.$refs.user.value = '';
        },

        onAdd() {
            var selectedIds = this.user.roles.filter(x => x.selected === true).map(x => x.roleId);

            if (selectedIds.length <= 0) {
                alert('Please select at least 1 entry.');
                return false;
            }

            emitter.emit("showLoader", true);
            updateAppUserRoles(this.app.applicationId, this.user.id, selectedIds).then(r => {
                this.user = new Object();
                this.query = '';
                this.search();

                this.modal.hide();

                emitter.emit("showLoader", false);
            });
        },

        onUpdate(user) {
            var selectedIds = user.roles.filter(x => x.selected === true).map(x => x.roleId);

            if (selectedIds.length <= 0) {
                alert('Please select at least 1 entry.');
                return false;
            }

            emitter.emit("showLoader", true);
            updateAppUserRoles(this.app.applicationId, user.id, selectedIds).then(r => {
                emitter.emit("showLoader", false);
            });
        },

        search(p) {
            emitter.emit("showLoader", true);
            this.pagination.currentPage = p ?? this.pagination.currentPage;

            searchAppUser(this.app.applicationId, { firstName: this.query, lastName: this.query, email: this.query },
                this.sort,
                this.sortDirection,
                this.pagination.currentPage,
                this.pagination.pageSize).then(async r => {

                    const userPromises = r.data.value.map(async obj => {
                        const userRoles = await Promise.all(
                            this.roles.map(async role => {
                                const appUserRoles = await getAppUserRoles(this.app.applicationId, obj.userId);
                                const selected = appUserRoles.data.some(x => x.roleId === role.roleId);
                                return { ...role, selected };
                            })
                        );

                        return {
                            name: `${obj.firstName} ${obj.lastName} (${obj.email})`,
                            id: obj.userId,
                            roles: userRoles
                        }
                    });

                    // Wait for all userPromises to resolve
                    this.users = await Promise.all(userPromises);

                    this.pagination = pagination(
                        Object.values(r.data)[1], // Gets the @odata.count which is the 2nd property
                        this.pagination.currentPage,
                        this.pagination.pageSize
                    );

                    emitter.emit("showLoader", false);
                }, err => {
                    console.log(err);
                    emitter.emit("showLoader", false);
                });
        },

        remove(userId) {
            if (confirm('Are you sure you want to delete this record?')) {
                emitter.emit("showLoader", true);
                removeAppUser(this.app.applicationId, userId).then(r => {
                    this.search();
                    emitter.emit("showLoader", false);
                });
            }
        },

        paginate(page) {
            this.pagination.currentPage = page;
            this.search();
        },
    }
}
</script>