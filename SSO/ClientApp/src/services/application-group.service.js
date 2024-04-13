import axios from 'axios';

const searchAppGroup = async (appId, b, sort, sortDirection, page = 1, pageSize = 10) => {
    var filterStr = `filter=contains(name, '${b.firstName ?? ''}') `;

    var url = `/odata/applicationgroup?appId=${appId}&$${filterStr}     
    &$orderby=${sort} ${sortDirection}
    &$skip=${(page - 1) * pageSize}
    &$top=${pageSize}
    &$count=true`;

    return axios.get(url);
}

const removeAppGroup = async (appId, groupId) => await axios.delete(`/api/application/${appId}/group/${groupId}`);

export {
    searchAppGroup,
    removeAppGroup
}