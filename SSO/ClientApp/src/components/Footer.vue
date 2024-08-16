<template>
    <footer class="app-footer">
        <div class="container text-center py-3"><small class="copyright">© {{ getYear() }} by <a class="app-link"
                    href="http://periapsys.com" target="_blank">Periapsys.com</a>. All Rights Reserved
                <br /> v {{ version }}</small></div>
    </footer>
</template>

<script>
import { healthCheck } from "@/services/health-check.service";
export default {
    data() {
        return {
            version: "",
        };
    },
    created() {
        healthCheck().then(r => {
            this.version = r.data.entries.HealthCheckHandler.data.version;
        });
    },
    methods: {
        getYear() {
            return new Date().getFullYear();
        }
    }
}
</script>