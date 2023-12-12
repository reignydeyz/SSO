import axios from 'axios';

const login = async (form) => await axios.post("api/authentication", form);
const loginAsRoot = async (form) => await axios.post("api/authentication/root", form);

export {
    login,
    loginAsRoot
}