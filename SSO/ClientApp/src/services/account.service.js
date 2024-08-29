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

const getAccount = () => {
    const token = Cookies.get('system');

    if (!token) {
        return false;
    }

    return jwtDecode(token);
}

const changePassword = async (param) => await axios.post("/api/account/changepassword", param);

export {
    hasRealmAccess,
    getAccount,
    changePassword
}