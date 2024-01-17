import axios from 'axios';

const getAppRolePermissions = async (applicationId, roleId) => await axios.get(`/api/application/${applicationId}/role/${roleId}/permission`);

export {
    getAppRolePermissions
}