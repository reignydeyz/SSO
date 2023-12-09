import axios from 'axios';

const login = async (form) => await axios.post("api/authentication", form);

export {
    login
}