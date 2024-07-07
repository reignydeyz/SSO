<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-4">
            <h3 class="section-title">General</h3>
            <div class="section-intro">
                Basic information of the group.
            </div>
        </div>
        <div class="col-12 col-md-8">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <form class="settings-form" @submit.prevent="onSubmit">
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Name*</label>
                            <input v-model="group.name" type="text" class="form-control" id="setting-input-1"
                                maxlength="200" placeholder="Name" required="" autocomplete="off" />
                        </div>
                        <div class="mb-3">
                            <label for="setting-input-3" class="form-label">Description</label>
                            <input v-model="group.description" type="text" class="form-control" id="setting-input-3"
                                maxlength="500" placeholder="Description" autocomplete="off" />
                        </div>
                        <button type="submit" class="btn app-btn-primary">
                            Save Changes
                        </button>
                    </form>
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
    </div>
</template>

<script>
import { getAccount } from '@/services/account.service';
import { updateGroup } from "@/services/group.service";
import { emitter } from "@/services/emitter.service";
export default {
    props: ["group"],
    methods: {
        onSubmit() {
            emitter.emit("showLoader", true);

            updateGroup(this.group).then(
                (r) => {
                    this.$router.push("../../groups");
                },
                (err) => {
                    alert('Failed to update record.');
                    emitter.emit("showLoader", false);
                }
            );
        },

        isInRealm(realm) {
            return getAccount().authmethod === realm;
        }
    }
}
</script>