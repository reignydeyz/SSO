import Cookies from 'js-cookie';
import { jwtDecode } from "jwt-decode";
import axios from 'axios';

const hasRealmAccess = () => {
    const token = Cookies.get('system');

    if (!token) {
        return false;
    }

    const decoded = jwtDecode(token);

    return !!(decoded.role && decoded.realm);
}

const getTokenInfo = () => {
    const token = Cookies.get('system');

    if (!token) {
        return false;
    }

    return jwtDecode(token);
}

const changePassword = async (param) => await axios.post("/api/account/changepassword", param);

async function generate2faQrcode() {
    try {
        const response = await axios.post('/api/account/2fa', null, {
            responseType: 'blob' // Specify that you're expecting a Blob (PNG image) here
        });
        return response.data; // Return only the Blob data
    } catch (error) {
        console.error('Error fetching 2FA QR code:', error);
        throw error; // Rethrow the error for handling in the component
    }
};

const disable2fa = async () => await axios.delete("/api/account/2fa");
const getAccount = async () => { 
    if (!Cookies.get('system')) {
        return false;
    }

    return axios.get("api/account"); 
}

export {
    hasRealmAccess,
    getTokenInfo,
    changePassword,
    generate2faQrcode,
    disable2fa,
    getAccount
}