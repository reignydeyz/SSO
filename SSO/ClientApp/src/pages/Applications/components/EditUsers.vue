<template>
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
                        <input type="type" id="txtInput" class="form-control" placeholder="Name or email" required
                            autocomplete="off">
                            <small class="ms-1 text-muted">(typeahead)</small>
                    </div>
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
    </div>
    <hr class="mb-4" v-if="users.length > 0"/>
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
</template>

<script>
import autocomplete from 'autocompleter';
import { searchUser } from "@/services/user.service";
export default {
    props: ["app", "roles"],
    data: () => ({
        users: []
    }),
    mounted() {
        autocomplete({
            input: document.getElementById('txtInput'),
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
                this.users.push({ id: item.value, name: item.label });
            }
        });
    }
}
</script>