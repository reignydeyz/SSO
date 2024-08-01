<template>
    <div class="row g-4 settings-section">
        <div class="col-12 col-md-3">
            <h3 class="section-title">LDAP</h3>
            <div class="section-intro">Use <i>Lightweight Directory Access Protocol (LDAP)</i> to authenticate and
                manage users.</div>
        </div>
        <div class="col-12 col-md-9">
            <div class="app-card app-card-settings shadow-sm p-4">
                <div class="app-card-body">
                    <form class="settings-form">
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Server*</label>
                            <input class="form-control" placeholder="Server" required autocomplete="off" />
                        </div>
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Port*</label>
                            <input class="form-control" type="number" required autocomplete="off" />
                        </div>
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Username*</label>
                            <input class="form-control" placeholder="Username" required autocomplete="off" />
                        </div>
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Password*</label>
                            <input class="form-control" type="password" required autocomplete="off" />
                        </div>
                        <div class="mb-3">
                            <label for="setting-input-2" class="form-label">Search base</label>
                            <input class="form-control" autocomplete="off" />
                        </div>
                        <div class="form-check mb-3">
                            <input class="form-check-input" type="checkbox" value=""
                                checked="">
                            <label class="form-check-label" for="settings-checkbox-1">
                                Use SSL
                            </label>
                        </div>
                        <button type="submit" class="btn app-btn-primary">
                            Save Changes
                        </button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>

</script>