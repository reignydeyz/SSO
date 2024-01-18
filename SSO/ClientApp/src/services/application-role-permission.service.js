import axios from 'axios';

const getAppRolePermissions = async (applicationId, roleId) => await axios.get(`/api/application/${applicationId}/role/${roleId}/permission`);
const updateAppRolePermissions = async (applicationId, roleId, permissionIds) => await axios.put(`/api/application/${applicationId}/role/${roleId}/permission`, permissionIds);

export {
    getAppRolePermissions,
    updateAppRolePermissions
}