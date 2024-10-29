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

    <hr class="mb-4"/>
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

    <div class="row g-4 settings-section mb-4">
        <div class="col-md-4" v-for="i in users" :key="i.id">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <router-link :to="'../../users/edit/' + i.id">
                        <h3 class="section-title"><i>{{ i.name }}</i></h3>
                    </router-link>
                    <div class="mt-3">
                        <button type="button" class="btn app-btn-outline-danger bg-white"
                            @click="remove(i.id)">Remove</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import autocomplete from 'autocompleter';
import { getTokenInfo } from '@/services/account.service';
import { searchUser } from "@/services/user.service";
import { searchGroupUser, addGroupUser, removeGroupUser } from "@/services/group-user.service";
import { emitter } from "@/services/emitter.service";
import { pagination } from "@/services/pagination.service";
export default {
    props: ["group"],
    data: () => ({
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
                emitter.emit("showLoader", true);
                addGroupUser(this.group.groupId, item.value).then(r => {
                    this.$refs.user.value = '';
                    this.query = '';
                    this.search();
                    emitter.emit("showLoader", false);
                });
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
                this.pagination.pageSize).then(async r => {
                    const userPromises = r.data.value.map(async obj => {
                        return {
                            name: `${obj.firstName} ${obj.lastName} (${obj.email})`,
                            id: obj.userId
                        }
                    });

                    this.users = await Promise.all(userPromises);

                    this.pagination = pagination(
                        Object.values(r.data)[1], // Gets the @odata.count which is the 2nd property
                        this.pagination.currentPage,
                        this.pagination.pageSize
                    );

                    emitter.emit("showLoader", false);
                });
        },

        remove(userId) {
            if (confirm('Are you sure you want to delete this record?')) {
                emitter.emit("showLoader", true);
                removeGroupUser(this.group.groupId, userId).then(r => {
                    this.search();
                    emitter.emit("showLoader", false);
                });
            }
        },

        isInIdp(idp) {
            return getTokenInfo().authmethod === idp;
        }
    }
}
</script>