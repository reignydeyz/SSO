import Cookies from 'js-cookie';
import { jwtDecode } from "jwt-decode";

const canActivate = (to) => {
    if (Cookies.get('system')) {
        const token = Cookies.get('system');
        const claims = jwtDecode(token);

        if (to?.meta?.mustHaveRealmAccess && !(claims.role && claims.realm)) {
            return false;
        }
        
        if (to?.meta?.idp && to.meta.idp !== claims.authmethod) {
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