import Cookies from 'js-cookie';

const canActivate = (to) => {
    if (Cookies.get('token')) {
        return true;
    }

    window.location.href = "/";
    return false;
}

export {
    canActivate
}