<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-4">
            <h3 class="section-title">Callbacks</h3>
            <div class="section-intro">
                Manage the redirect URLs.
            </div>
        </div>
        <div class="col-12 col-md-8">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <form class="row g-2" @submit.prevent="onSubmit">
                        <div class="col-md-8">
                            <label class="visually-hidden">URL</label>
                            <input v-model="appCallback.url" type="url" class="form-control" placeholder="URL" required>
                        </div>
                        <div class="col-auto">
                            <button type="submit" class="btn app-btn-primary mb-3">Add</button>
                        </div>
                    </form>
                    <div class="table-responsive" v-if="appCallbacks.length > 0">
                        <table class="table app-table-hover mb-0 text-left">
                            <thead>
                                <tr>
                                    <th class="cell"></th>
                                    <th class="cell">URL</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="i in appCallbacks" :key="i">
                                    <td class="cell">
                                        <button class="btn-sm app-btn-secondary" type="button" @click="onRemove(i)">
                                            Remove
                                        </button>
                                    </td>
                                    <td class="cell">{{ i }}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!--//app-card-body-->
            </div>
            <!--//app-card-->
        </div>
    </div>
</template>

<script>
import { getAppCallbacks, addAppCallback, removeAppCallback } from "@/services/application-callback.service";
import { emitter } from "@/services/emitter.service";
export default {
    props: ["app"],
    data: () => ({
        appCallback: new Object(),
        appCallbacks: [],
        updated: false
    }),
    async updated() {
        if (!this.updated) {
            this.appCallbacks = (await getAppCallbacks(this.app.applicationId)).data;
            this.updated = true;
        }
    },
    methods: {
        onSubmit() {
            emitter.emit("showLoader", true);

            addAppCallback(this.app.applicationId, this.appCallback).then(
                (r) => {
                    getAppCallbacks(this.app.applicationId).then(res => {
                        this.appCallbacks = res.data;
                        this.appCallback = new Object();
                        emitter.emit("showLoader", false);
                    });
                },
                (err) => {
                    alert('Failed to add record.');
                    emitter.emit("showLoader", false);
                }
            );
        },

        onRemove(url) {
            if (confirm('Are you sure you want to delete this record?')) {
                emitter.emit("showLoader", true);
                removeAppCallback(this.app.applicationId, url).then(r => {
                    getAppCallbacks(this.app.applicationId).then(res => {
                        this.appCallbacks = res.data;
                        emitter.emit("showLoader", false);
                    });
                });
            }
        }
    }
}
</script>