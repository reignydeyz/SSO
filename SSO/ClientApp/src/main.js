import '@popperjs/core/lib/popper.js'
import "bootstrap-icons/font/bootstrap-icons.css"
import '@fortawesome/fontawesome-free/css/all.css'
import '@fortawesome/fontawesome-free/js/all.js'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import axios from 'axios'
import Cookies from 'js-cookie';

window.bootstrap = require('bootstrap/dist/js/bootstrap.bundle.js');

axios.defaults.baseURL = process.env.VUE_APP_API_URL;

axios.interceptors.request.use(
    config => {
        if (Cookies.get('system')) {
            config.headers.authorization = `Bearer ${Cookies.get('system')}`;
        }
        return config;
    }, error => {
        return Promise.reject(error);
    });

axios.interceptors.response.use(response => {
    return response;
}, error => {
    // Go back to login page when not authenticated
    if (error.response.status === 401) {
        if (window.location.pathname != '/login') {
            Cookies.remove("system");
            window.location.href = "/";
        }
    }
    else if (error.response.status === 403) {
        alert("You don`t have permission to perform this action.");
    }
    return Promise.reject(error);
});

createApp(App).use(router).mount('#app')
