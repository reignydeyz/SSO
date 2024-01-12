<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-4">
            <h3 class="section-title">Callbacks</h3>
            <div class="section-intro">
                Manage your redirect URLs.
            </div>
        </div>
        <div class="col-12 col-md-8">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <form class="row g-2">
                        <div class="col-md-8">
                            <label for="inputUrl" class="visually-hidden">Url</label>
                            <input type="text" class="form-control" id="inputUrl" placeholder="Url">
                        </div>
                        <div class="col-auto">
                            <button type="submit" class="btn app-btn-primary mb-3">Add</button>
                        </div>
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