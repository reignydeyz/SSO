import Cookies from 'js-cookie';

const canActivate = () => {
    if (Cookies.get('root'))
    {
        return true;
    }

    window.location.href = "/root";
    return false;
}

export {
    canActivate
}