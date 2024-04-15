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
</template>

<script>
import autocomplete from 'autocompleter';
import { searchApp } from "@/services/application.service";
import { getAppRoles } from "@/services/application-role.service";
import { getGroupApps } from "@/services/group-application.service";
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
        autocomplete({
            input: this.$refs.app,
            minLength: 3,
            fetch: async function (text, update) {
                text = text.toLowerCase();
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
            }
        });
    },
    methods: {
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