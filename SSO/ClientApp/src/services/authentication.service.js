import axios from 'axios';

const login = async (form) => await axios.post("/api/authentication", form);
const loginToSystem = async (form) => await axios.post("/api/authentication/system", form);
const logout = async () => await axios.get("/api/authentication/logout");

export {
    login,
    loginToSystem,
    logout
}