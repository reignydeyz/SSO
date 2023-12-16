import axios from 'axios';

const login = async (form) => await axios.post("api/authentication", form);
const loginToSystem = async (form) => await axios.post("api/authentication/system", form);

export {
    login,
    loginToSystem
}