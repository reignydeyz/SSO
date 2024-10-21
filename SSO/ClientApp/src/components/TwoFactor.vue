<template>
    <div class="modal fade" id="2faModal" tabindex="-1" aria-labelledby="2faModalLabel" aria-hidden="true"
        data-bs-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="2faModalLabel">2FA QR Code</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" @click="onModalClose"></button>
                </div>
                <div class="modal-body">
                    <img v-if="binaryData" :src="imageSrc" alt="QR Code" class="img-fluid" />
                    <button class="btn app-btn-outline-danger w-100" data-bs-dismiss="modal" @click="onDisableClick">Disable 2FA</button>
                    <p class="pt-3"><small>For better security, use two-factor authentication (2FA) with an
                            authenticator app (e.g., Google Authenticator, Authy, Microsoft Authenticator, or FreeOTP).
                            Please scan this QR code to set it up.</small></p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { disable2fa } from "@/services/account.service";

export default {
    props: {
        binaryData: {
            type: Blob,
            default: null,
        },
    },
    computed: {
        imageSrc() {
            return this.binaryData ? URL.createObjectURL(this.binaryData) : '';
        },
    },
    methods: {
        async onDisableClick() {
            if (confirm("Are you sure you want to disable 2FA?")) {
                await disable2fa();
            }
        },

        async onModalClose() {
            if (!confirm("Keep 2FA enabled?")) {
                await disable2fa();
            }
        }
    }
}
</script>
