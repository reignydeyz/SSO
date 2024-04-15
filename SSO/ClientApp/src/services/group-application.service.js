import axios from 'axios';

const getGroupApps = async (groupId) => await axios.get(`/api/group/${groupId}/application`);

export {
    getGroupApps
}