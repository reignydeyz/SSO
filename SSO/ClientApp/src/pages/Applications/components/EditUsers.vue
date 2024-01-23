<template>
    <div class="alert alert-danger" role="alert" v-show="roles.length <= 0">
        It seems like roles haven't been set up yet. Unfortunately, adding a user is currently not possible.
    </div>

    <div class="row g-4 settings-section" v-show="roles.length > 0">
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
                        <input v-model="user.name" type="text" class="form-control" placeholder="Name or email" required
                            autocomplete="off" ref="user">
                        <small class="ms-1 text-muted">(typeahead)</small>
                    </div>
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
    </div>
    <hr class="mb-4" v-if="users.length > 0" />
    <div v-for="i in users" :key="i.userId">
        <div class="row g-4 settings-section mb-4">
            <div class="col-12">
                <div class="app-card app-card-settings shadow-sm p-4">
                    <div class="app-card-body">
                        <h3 class="section-title"><i>{{ i.name }}</i></h3>
                        // TBD
                    </div>
                </div>
            </div>
        </div>
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
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn app-btn-primary">Add</button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import autocomplete from 'autocompleter';
import { searchUser } from "@/services/user.service";
export default {
    props: ["app", "roles"],
    data: () => ({
        user: new Object(),
        users: [],
    }),
    mounted() {
        // Listen for the hidden.bs.modal event and call onModalClose method
        new bootstrap.Modal(this.$refs.modal)._element.addEventListener('hidden.bs.modal', this.onModalClose);

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
                
                new bootstrap.Modal(this.$refs.modal).show();
            }
        });
    },
    methods: {
        onModalClose() {
            this.user = new Object();
        }
    }
}
</script>