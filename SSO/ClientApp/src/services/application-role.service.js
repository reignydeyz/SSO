import axios from 'axios';

const getAppRoles = async (applicationId) => await axios.get(`/api/application/${applicationId}/role`);
const addAppRole = async (applicationId, param) => await axios.post(`/api/application/${applicationId}/role`, param);
const removeAppRole = async(applicationId, id) => await axios.delete(`/api/application/${applicationId}/role/${id}`);

export {
    getAppRoles,
    addAppRole,
    removeAppRole
}