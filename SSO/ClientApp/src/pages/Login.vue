<template>
	<div class="col-12 col-md-12 col-lg-12 auth-main-col text-center p-5">
		<div class="d-flex flex-column align-content-end">
			<div class="app-auth-body mx-auto">
				<div class="app-auth-branding mb-4"><a class="app-logo" href="index.html"><img class="logo-icon me-2"
							:src="require('@/assets/logo.png')" alt="logo"></a></div>
				<h2 class="auth-heading text-center mb-3">Welcome</h2>
				<p class="mb-4">Login to SSO to continue to {{app}}.</p>
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
import { login } from "@/services/authentication.service";
import { getAppById } from "@/services/application.service";
import Cookies from 'js-cookie';
import { emitter } from '@/services/emitter.service';

export default {
	data: () => ({
		param: new Object(),
		urlParams: new URLSearchParams(window.location.search),
		app: '---',
	}),
	created() {
		if (!this.urlParams.get('appId')
			|| !this.urlParams.get('callbackUrl')
			|| !Cookies.get('appId')
			|| this.urlParams.get('appId') != Cookies.get('appId')) {
			window.location.href = "invalid";
		}

		getAppById(this.urlParams.get('appId')).then(r => {
			this.app = r.data.name;
		});
	},
	mounted() {
		this.param.appId = this.urlParams.get('appId');

		if (Cookies.get('token')) {
			var token = Cookies.get('token');

			window.location.href = `${this.urlParams.get('callbackUrl')}?token=${token}`;
		}
	},
	methods: {
		submit() {
			emitter.emit('showLoader', true);

			login(this.param).then(r => {
				emitter.emit('showLoader', false);

				if (this.urlParams.get('callbackUrl')) {
					window.location.href = `${this.urlParams.get('callbackUrl')}?token=${r.data.access_token}`;
				}
			}, err => {
				emitter.emit('showLoader', false);
				alert('Access denied.');
			});
		}
	}
}
</script>