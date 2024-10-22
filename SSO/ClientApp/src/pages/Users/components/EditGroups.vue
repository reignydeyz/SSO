<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-2">
            <h3 class="section-title">Add to group</h3>
            <div class="section-intro">
                Make user member of the group.
            </div>
        </div>
        <div class="col-12 col-md-10">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <div class="col-md-8">
                        <input type="text" class="form-control" placeholder="Group" required autocomplete="off"
                            ref="group">
                        <small class="ms-1 text-muted">(typeahead)</small>
                    </div>
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
    </div>

    <div v-show="groups.length > 0">
        <hr class="mb-4" v-if="isInIdp('Default')" />
        <h5 class="section-title mb-3">Groups</h5>
        <div class="row g-4 settings-section mb-4">
            <div class="col-md-4" v-for="i in groups" :key="i.groupId">
                <div class="app-card app-card-settings shadow-sm p-4">
                    <div class="app-card-body">
                        <router-link :to="'../../groups/edit/' + i.groupId">
                            <h3 class="section-title"><i>{{ i.name }}</i></h3>
                        </router-link>

                        <div class="mt-3">
                            <button type="button" class="btn app-btn-outline-danger bg-white"
                                @click="remove(i.groupId)">Remove</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import autocomplete from 'autocompleter';
import { getTokenInfo } from '@/services/account.service';
import { searchGroup } from "@/services/group.service";
import { getUserGroups } from "@/services/user-group.service";
import { addGroupUser, removeGroupUser } from '@/services/group-user.service';
import { emitter } from "@/services/emitter.service";
export default {
    props: ["user"],
    data: () => ({
        groups: [],
    }),
    watch: {
        user(newVal, oldVal) {
            this.getGroups();
        }
    },
    mounted() {
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
                    label: obj.name,
                    value: obj.groupId
                }));

                update(suggestions);
            },
            onSelect: async (item) => {
                this.$refs.group.value = '';

                await addGroupUser(item.value, this.user.userId);

                await this.getGroups();
            }
        });
    },
    methods: {
        getGroups() {
            emitter.emit("showLoader", true);

            getUserGroups(this.user.userId).then(r => {
                this.groups = r.data;
                emitter.emit("showLoader", false);
            });
        },

        remove(groupId) {
            if (confirm('Are you sure you want to delete this record?')) {
                emitter.emit("showLoader", true);
                removeGroupUser(groupId, this.user.userId).then(r => {
                    this.getGroups();
                    emitter.emit("showLoader", false);
                });
            }
        },

        isInIdp(idp) {
            return getTokenInfo().authmethod === idp;
        },
    }
}
</script>