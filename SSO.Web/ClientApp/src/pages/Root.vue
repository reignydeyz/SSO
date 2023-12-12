<template>
	<div class="col-12 col-md-12 col-lg-12 auth-main-col text-center p-5">
		<div class="d-flex flex-column align-content-end">
			<div class="app-auth-body mx-auto">
				<div class="app-auth-branding mb-4"><a class="app-logo" href="index.html"><img class="logo-icon me-2"
							:src="require('@/assets/logo.png')" alt="logo"></a></div>
				<h2 class="auth-heading text-center mb-5">Log in to SSO (root)</h2>
				<div class="auth-form-container text-start">
					<form class="auth-form login-form" @submit.prevent="submit()">
						<div class="email mb-3">
							<label class="sr-only" for="signin-email">Username</label>
							<input class="form-control signin-email" placeholder="Username" required="required"
								v-model="param.userName" autocomplete="off">
						</div><!--//form-group-->
						<div class="password mb-3">
							<label class="sr-only" for="signin-password">Password</label>
							<input type="password" class="form-control signin-password" placeholder="Password"
								required="required" v-model="param.password" autocomplete="off">

						</div><!--//form-group-->
						<div class="text-center">
							<button type="submit" class="btn app-btn-primary w-100 theme-btn mx-auto">Log In</button>
						</div>
					</form>
				</div><!--//auth-form-container-->

			</div><!--//auth-body-->
		</div><!--//flex-column-->
	</div><!--//auth-main-col-->
</template>

<script>
import { loginAsRoot } from "@/services/authentication.service";
import Cookies from 'js-cookie';
import { jwtDecode } from "jwt-decode";
import { emitter } from '@/services/emitter.service';

export default {
	data: () => ({
		param: new Object()
	}),
	mounted() {
		if (Cookies.get('root')) {
			var token = Cookies.get('root');

			let decoded = jwtDecode(token);

			// Check if the token is expired
			if (decoded.exp < Date.now() / 1000) {
				Cookies.remove('root');
			} else {
				window.location.href = 'main';
			}
		}
	},
	methods: {
		submit() {
			emitter.emit('showLoader', true);

			loginAsRoot(this.param).then(r => {
				emitter.emit('showLoader', false);
				Cookies.set('root', r.data, { expires: 1 });

				window.location.href = 'main';
			}, err => {
				emitter.emit('showLoader', false);
				alert('Access denied.');
			});
		}
	}
}
</script>