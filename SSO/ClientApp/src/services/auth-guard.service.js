import Cookies from 'js-cookie';
import { jwtDecode } from "jwt-decode";

const canActivate = (to) => {
    if (Cookies.get('system')) {
        const token = Cookies.get('system');
        const claims = jwtDecode(token);

        if (to?.meta?.mustBeRoot && !claims.role) {
            return false;
        }
        
        if (to?.meta?.realm && to.meta.realm !== claims.authmethod) {
            return false;
        }

        return true;
    }

    window.location.href = "/";
    return false;
}

export {
    canActivate
}