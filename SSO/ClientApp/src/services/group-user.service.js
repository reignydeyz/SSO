import axios from 'axios';

const searchGroupUser = async (groupId, b, sort, sortDirection, page = 1, pageSize = 10) => {
    var filterStr = `filter=contains(firstName, '${b.firstName ?? ''}') `;    
    filterStr += `or contains(lastName, '${b.lastName ?? ''}') `;
    filterStr += `or contains(email, '${b.email ?? ''}') `;

    var url = `/odata/groupuser?group=${groupId}&$${filterStr}     
    &$orderby=${sort} ${sortDirection}
    &$skip=${(page - 1) * pageSize}
    &$top=${pageSize}
    &$count=true`;

    return axios.get(url);
}

const removeGroupUser = async (groupId, userId) => await axios.delete(`/api/group/${groupId}/user/${userId}`);

export {
    searchGroupUser,
    removeGroupUser
}