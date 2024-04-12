<template>
    <div class="alert alert-danger" role="alert" v-show="roles.length <= 0">
        It seems like roles haven't been set up yet. Unfortunately, adding a user is currently not possible.
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
    </div>
</template>

<script>
import autocomplete from 'autocompleter';
import { searchGroup } from "@/services/group.service";
import { emitter } from "@/services/emitter.service";
export default {
    props: ["app", "roles"],
    data: () => ({
        group: new Object(),
        groups: [],
        modal: null,
        query: "",
        pagination: new Object(),
    }),
    mounted() {
        autocomplete({
            input: this.$refs.group,
            minLength: 3,
            fetch: async function (text, update) {
                text = text.toLowerCase();
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
                this.user = {
                    id: item.value,
                    name: item.label
                };
            }
        });
    }
}
</script>