<template>
    <div class="modal fade" id="otpModal" tabindex="-1" aria-labelledby="otpModalLabel" aria-hidden="true"
        data-bs-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="otpModalLabel">Enter OTP</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form @submit.prevent="submit()">
                        <div class="mb-3">
                            <label for="otpInput" class="form-label">One-Time Password</label>
                            <input type="password" class="form-control" id="otpInput" maxlength="6" minlength="6" required
                                autocomplete="off" v-model="otp">
                        </div>
                        <button type="submit" class="btn app-btn-primary w-100">Verify OTP</button>
                    </form>
                    <p class="pt-3"><small>Open your two-factor authenticator (TOTP) app or browser extension to view
                            your OTP.</small></p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { login, loginToSystem } from "@/services/authentication.service";
import Cookies from 'js-cookie';
import { emitter } from '@/services/emitter.service';

export default {
    props: {
        param: {
            type: Object,
            required: true,
        },
        system: {
            type: Boolean,
            default: false, // Optional prop
        }
    },
    data: () => ({
        otp: '',
        urlParams: new URLSearchParams(window.location.search),
    }),
    methods: {
        submit() {
            emitter.emit('showLoader', true);

            if (this.system) {
                loginToSystem({ ...this.param, otp: this.otp }).then(r => {
                    Cookies.set('system', r.data.access_token, { expires: 1 });
                    this.$router.replace({ name: 'Init' });
                }, err => {
                    emitter.emit('showLoader', false);
                    alert(err.response.data);
                });
            }
            else {
                login({ ...this.param, otp: this.otp }).then(r => {
                    if (this.urlParams.get('callbackUrl')) {
                        window.location.href = `${this.urlParams.get('callbackUrl')}?token=${r.data.access_token}`;
                    }
                }, err => {
                    emitter.emit('showLoader', false);
                    alert(err.response.data);
                })
            }
        }
    }
}
</script>

<style scoped>
#otpInput {
    letter-spacing: 1rem;
    text-align: center;
}
</style>
