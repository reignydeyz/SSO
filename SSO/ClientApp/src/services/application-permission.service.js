import axios from 'axios';

const getAppPermissions = async (applicationId) => await axios.get(`/api/application/${applicationId}/permission`);
const addAppPermission = async (applicationId, param) => await axios.post(`/api/application/${applicationId}/permission`, param);
const removeAppPermission = async(applicationId, id) => await axios.delete(`/api/application/${applicationId}/permission/${id}`);

export {
    getAppPermissions,
    addAppPermission,
    removeAppPermission
}