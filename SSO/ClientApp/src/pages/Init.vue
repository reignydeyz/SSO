<template>
	<div class="col-12 col-md-12 col-lg-12 auth-main-col text-center p-5">
		<div class="d-flex flex-column align-content-end">
			<div class="app-auth-body mx-auto">
				<div class="app-auth-branding mb-4"><a class="app-logo" href="index.html"><img class="logo-icon"
							:src="require('@/assets/logo.png')" alt="logo"></a></div>
				<h2 class="auth-heading text-center mb-3">Hi!</h2>
				<p class="mb-4">{{ tokenInfo.given_name }}</p>
				<div class="auth-form-container text-start">
					<div class="auth-option text-center pt-3"><router-link to="/changepassword" class="text-link">Change
							password</router-link></div>
					<div class="auth-option text-center pt-3"><router-link to="#" class="text-link" @click="on2faClick">2FA</router-link>
					</div>
					<div class="auth-option text-center pt-3"><a class="text-link" href="#" @click="signout">Logout</a>
					</div>
				</div><!--//auth-form-container-->

			</div><!--//auth-body-->
		</div><!--//flex-column-->
	</div><!--//auth-main-col-->
	<TwoFactor :binaryData="modalBinaryData" />
</template>

<script>
import TwoFactor from "@/components/TwoFactor.vue";
import { hasRealmAccess, getTokenInfo, getAccount } from '@/services/account.service';
import { generate2faQrcode } from "@/services/account.service";

export default {
	components: { TwoFactor },
	data() {
		return {
			tokenInfo: new Object(),
			modalBinaryData: null, // Store the fetched binary data
			modal: null,
			account: new Object(),
		};
	},
	created() {
		if (hasRealmAccess()) {
			window.location.replace("main");
		}
		this.tokenInfo = getTokenInfo();
	},
	mounted() {
		// Listen for the hidden.bs.modal event and call onModalClose method
		this.modal = new bootstrap.Modal(document.getElementById('2faModal'));

		getAccount().then(r => {
            this.account = r.data;
        });
	},
	methods: {
		signout() {
			this.$router.push('/logout');
		},
		isInIdp(idp) {
			return this.tokenInfo.authmethod === idp;
		},

		async on2faClick() {
			if (!this.account.twoFactorEnabled) {
                if (!confirm("Enable Two-Factor Authentication (2FA)?")) {
                    return;
                }
            }

			const blobData = await generate2faQrcode(); // Fetch the QR code as a Blob
			this.modalBinaryData = blobData; // Store the fetched Blob data
			this.modal.show();
		}
	}
}
</script>
