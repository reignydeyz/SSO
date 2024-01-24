import axios from 'axios';

const getAppUserRoles = async (applicationId, userId) => await axios.get(`/api/application/${applicationId}/user/${userId}/role`);
const updateAppUserRoles = async (applicationId, userId, roleIds) => await axios.put(`/api/application/${applicationId}/user/${userId}/role`, roleIds);

export {
    getAppUserRoles,
    updateAppUserRoles
}