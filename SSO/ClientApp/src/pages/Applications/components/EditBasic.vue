<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-4">
            <h3 class="section-title">General</h3>
            <div class="section-intro">
                Basic information of your application.
            </div>
        </div>
        <div class="col-12 col-md-8">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <form class="settings-form" @submit.prevent="onSubmit">
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Name*</label>
                            <input v-model="app.name" type="text" class="form-control" id="setting-input-1"
                                placeholder="Name" required="" autocomplete="off" />
                        </div>
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Token expiration (minutes)*</label>
                            <input v-model="app.tokenExpiration" type="number" class="form-control" id="setting-input-2"
                                placeholder="Number" required="" autocomplete="off" />
                        </div>
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Refresh token expiration (minutes)*</label>
                            <input v-model="app.refreshTokenExpiration"  type="number" class="form-control" id="setting-input-3"
                                placeholder="Number" required="" autocomplete="off" />
                        </div>
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Max access failed count*</label>
                            <input v-model="app.maxAccessFailedCount"  type="number" class="form-control" id="setting-input-4"
                                placeholder="Number" required="" autocomplete="off" />
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
export default {
    props: ["app"],
}
</script>