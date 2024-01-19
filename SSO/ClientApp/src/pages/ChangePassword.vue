<template>
	<div class="col-12 col-md-12 col-lg-12 auth-main-col text-center p-5">
		<div class="d-flex flex-column align-content-end">
			<div class="app-auth-body mx-auto">
				<div class="app-auth-branding mb-4"><a class="app-logo" href="index.html"><img class="logo-icon"
							:src="require('@/assets/logo.png')" alt="logo"></a></div>
				<h2 class="auth-heading text-center mb-3">Change password</h2>
				<div class="auth-form-container text-start">
					<form class="auth-form login-form" @submit.prevent="submit()">
						<div class="mb-3">
							<label>Current password</label>
							<div class="input-group">
								<input type="password" id="password" class="form-control signin-password"
									placeholder="Password" required="required" v-model="param.currentPassword"
									autocomplete="off" ref="CurrentPassword">
								<button class="btn app-btn-outline-primary" type="button" id="button-addon2"
									@click="toggleViewKey('password')">
									<i class="bi bi-eye"></i>
								</button>
								<div class="invalid-feedback">
									{{ errorMessage }}
								</div>
							</div>

						</div><!--//form-group-->
						<div class="mb-3">
							<label>New password</label>
							<div class="input-group">
								<input type="password" id="new-password" class="form-control signin-password"
									placeholder="New password" required="required" v-model="param.newPassword"
									autocomplete="off" ref="NewPassword">
								<button class="btn app-btn-outline-primary" type="button" id="button-addon2"
									@click="toggleViewKey('new-password')">
									<i class="bi bi-eye"></i>
								</button>
								<div class="invalid-feedback">
									{{ errorMessage }}
								</div>
							</div>

						</div><!--//form-group-->
						<div class="mb-3">
							<label>Repeat password</label>
							<div class="input-group">
								<input type="password" id="re-new-password" class="form-control signin-password"
									placeholder="New password" required="required" v-model="param.repeatPassword"
									autocomplete="off" ref="RepeatPassword">
								<button class="btn app-btn-outline-primary" type="button" id="button-addon2"
									@click="toggleViewKey('re-new-password')">
									<i class="bi bi-eye"></i>
								</button>
								<div class="invalid-feedback">
									{{ errorMessage }}
								</div>
							</div>

						</div><!--//form-group-->
						<div class="text-center">
							<button type="submit" class="btn app-btn-primary w-100 theme-btn mx-auto">Submit</button>
							<div class="auth-option text-center pt-5"><a class="text-link" href="#" @click="back">Back</a>
							</div>
						</div>
					</form>
				</div><!--//auth-form-container-->

			</div><!--//auth-body-->
		</div><!--//flex-column-->
	</div><!--//auth-main-col-->
</template>

<script>
import { changePassword } from "@/services/account.service";
export default {
	data: () => ({
		param: new Object(),
		errorMessage: '---'
	}),
	methods: {
		submit() {
			// Clear error messages
			const allInputs = document.querySelectorAll('.is-invalid');

			allInputs.forEach((input) => {
				input.classList.remove('is-invalid');
			});

			changePassword(this.param).then(r => {
				this.$router.push("/");
			}, error => {

				// Check if the error is related to a specific input field
				if (error && error.response && error.response.data && error.response.data.errors) {
					const errorField = Object.keys(error.response.data.errors)[0];

					// Add 'is-invalid' class to the corresponding input field
					const inputElement = this.$refs[errorField];
					if (inputElement) {
						inputElement.classList.add('is-invalid');

						const errorMessage = Object.values(error.response.data.errors)[0][0];
						this.errorMessage = errorMessage;
					}
				}
				else {
					alert('Change password failed.');
				}

				return false;
			});
		},
		back() {
			history.back();
		},
		toggleViewKey(param) {
			var e = document.getElementById(param);
			if (e.type === "password") {
				e.type = "text";
			} else {
				e.type = "password";
			}
		},
	}
}
</script>