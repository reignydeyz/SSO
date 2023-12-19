import Cookies from 'js-cookie';

const canActivate = (to) => {
    if (Cookies.get('system')) {
        return true;
    }

    window.location.href = "/";
    return false;
}

export {
    canActivate
}