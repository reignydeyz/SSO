import axios from 'axios';

const getUserGroups = async (userid) => await axios.get(`/api/user/${userid}/group`);

export {
    getUserGroups
}