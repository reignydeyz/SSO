<template>
    <footer class="app-footer">
        <div class="container text-center py-3">
            <small class="copyright">© {{ getYear() }} by <a class="app-link" href="http://periapsys.com"
                    target="_blank">Periapsys.com</a>. All Rights Reserved
                <br /> v {{ version }}</small>

            <div class="alert alert-info mt-5 d-flex justify-content-between align-items-center p-2 small">
                <div class="flex-grow-1 text-center">
                    New release available! Check out <a href="https://github.com/reignydeyz/sso/releases"
                        target="_blank">version {{ latestVersion }}</a> for the latest features and improvements.
                </div>
                <button type="button" class="btn-close btn-sm" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>

        </div>
    </footer>
</template>

<script>
import { healthCheck } from "@/services/health-check.service";
export default {
    data() {
        return {
            version: "",
            latestVersion: ""
        };
    },
    created() {
        healthCheck().then(r => {
            this.version = r.data.entries.HealthCheckHandler.data.version;
            this.latestVersion = r.data.entries.HealthCheckHandler.data.latestVersion;
        });
    },
    methods: {
        getYear() {
            return new Date().getFullYear();
        }
    }
}
</script>