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
                            <input type="text" class="form-control" id="otpInput" maxlength="6" minlength="6" required
                                autocomplete="off">
                        </div>
                        <button type="submit" class="btn app-btn-primary w-100">Verify OTP</button>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: ["user"],
    methods: {
        submit() {
            console.log('Current User Info:', this.user)
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
