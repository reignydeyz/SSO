import axios from 'axios';

const getUserApps = async (userid) => await axios.get(`/api/user/${userid}/application`);

export {
    getUserApps
}