import axios from 'axios';

const getAppGroupRoles = async (applicationId, groupId) => await axios.get(`/api/application/${applicationId}/group/${groupId}/role`);
const updateAppGroupRoles = async (applicationId, groupId, roleIds) => await axios.put(`/api/application/${applicationId}/group/${groupId}/role`, roleIds);

export {
    getAppGroupRoles,
    updateAppGroupRoles
}