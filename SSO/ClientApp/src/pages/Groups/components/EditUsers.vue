<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-2">
            <h3 class="section-title">Add user</h3>
            <div class="section-intro">
                Add user to the group.
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
</template>

<script>
import autocomplete from 'autocompleter';
import { searchUser } from "@/services/user.service";
import { searchGroupUser } from "@/services/group-user.service";
import { emitter } from "@/services/emitter.service";
import { pagination } from "@/services/pagination.service";
export default {
    props: ["group"],
    data: () => ({
        user: new Object(),
        users: [],
        query: "",
        sort: "firstName",
        sortDirection: "asc",
        pagination: new Object(),
    }),
    watch: {
        group(newVal, oldVal) {
            this.search();
        }
    },
    mounted() {
        this.pagination.currentPage = 1;
        this.pagination.pageSize = 15;

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
                    name: item.label
                };
            }
        });
    },
    methods: {
        search(p) {
            emitter.emit("showLoader", true);
            this.pagination.currentPage = p ?? this.pagination.currentPage;

            searchGroupUser(this.group.groupId, { firstName: this.query, lastName: this.query, email: this.query },
                this.sort,
                this.sortDirection,
                this.pagination.currentPage,
                this.pagination.pageSize).then(r => {
                    this.users = r.data.value;

                    this.pagination = pagination(
                        Object.values(r.data)[1], // Gets the @odata.count which is the 2nd property
                        this.pagination.currentPage,
                        this.pagination.pageSize
                    );

                    emitter.emit("showLoader", false);
                });
        }
    }
}
</script>