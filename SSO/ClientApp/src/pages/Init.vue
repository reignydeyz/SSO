<template>
	<div class="col-12 col-md-12 col-lg-12 auth-main-col text-center p-5">
		<div class="d-flex flex-column align-content-end">
			<div class="app-auth-body mx-auto">
				<div class="app-auth-branding mb-4"><a class="app-logo" href="index.html"><img class="logo-icon"
							:src="require('@/assets/logo.png')" alt="logo"></a></div>
				<h2 class="auth-heading text-center mb-3">Hi!</h2>
				<p class="mb-4">{{account.given_name}}</p>
				<div class="auth-form-container text-start">
                    <div class="auth-option text-center pt-3"><router-link to="/changepassword" class="text-link">Change password</router-link></div>
					<div class="auth-option text-center pt-3"><a class="text-link" href="#" @click="signout">Logout</a></div>
				</div><!--//auth-form-container-->

			</div><!--//auth-body-->
		</div><!--//flex-column-->
	</div><!--//auth-main-col-->
</template>

<script>
import { hasRootAccess, getAccount } from '@/services/account.service';
import { logout } from "@/services/authentication.service";
import Cookies from 'js-cookie';
export default {
    data() {
        return { account: new Object() };
    },
    created() {
		if (hasRootAccess()) {
			window.location.href = "main";
		}
        this.account = getAccount();
    },
	methods: {
		signout() {
            logout().then(r => {
				Cookies.remove('system');
                location.reload();
            })
        },
	}
}
</script>
