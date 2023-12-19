import Cookies from 'js-cookie';
import { jwtDecode } from "jwt-decode";

const hasRootAccess = () => {
    const token = Cookies.get('system');

    if (!token) {
        return false;
    }

    const decoded = jwtDecode(token);

    return !!decoded.role;
}

const getAccount = () => {
    const token = Cookies.get('system');

    if (!token) {
        return false;
    }

    return jwtDecode(token);
}

export {
    hasRootAccess,
    getAccount
}