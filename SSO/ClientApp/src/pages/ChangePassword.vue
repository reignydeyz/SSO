<template>
	<div class="col-12 col-md-12 col-lg-12 auth-main-col text-center p-5">
		<div class="d-flex flex-column align-content-end">
			<div class="app-auth-body mx-auto">
				<div class="app-auth-branding mb-4"><a class="app-logo" href="index.html"><img class="logo-icon"
							:src="require('@/assets/logo.png')" alt="logo"></a></div>
				<h2 class="auth-heading text-center mb-3">Change password</h2>
				<div class="auth-form-container text-start">
					<form class="auth-form login-form" @submit.prevent="submit()">
						<div class="email mb-3">
							<label>Current password</label>
							<input type="password" class="form-control signin-password" placeholder="Password"
								required="required" v-model="param.currentPassword" autocomplete="off">
						</div><!--//form-group-->
						<div class="password mb-3">
							<label>New password</label>
							<input type="password" class="form-control signin-password" placeholder="New password"
								required="required" v-model="param.newPassword" autocomplete="off">

						</div><!--//form-group-->
						<div class="password mb-3">
							<label>Repeat password</label>
							<input type="password" class="form-control signin-password" placeholder="New password"
								required="required" v-model="param.repeatPassword" autocomplete="off">

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

export default {
	data: () => ({
		param: new Object(),
	}),
	methods: {
		submit() {
		},
		back() {
			history.back();
		}
	}
}
</script>