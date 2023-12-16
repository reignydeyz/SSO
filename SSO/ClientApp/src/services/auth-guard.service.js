import Cookies from 'js-cookie';

const canActivate = () => {
    if (Cookies.get('root')) {
        return true;
    }

    window.location.href = "/";
    return false;
}

export {
    canActivate
}