import axios from 'axios';

const searchAppUser = async (appId, b, sort, sortDirection, page = 1, pageSize = 10) => {
    var filterStr = `filter=contains(firstName, '${b.firstName ?? ''}') `;    
    filterStr += `or contains(lastName, '${b.lastName ?? ''}') `;
    filterStr += `or contains(email, '${b.email ?? ''}') `;

    var url = `/odata/applicationuser?appId=${appId}&$${filterStr}     
    &$orderby=${sort} ${sortDirection}
    &$skip=${(page - 1) * pageSize}
    &$top=${pageSize}
    &$count=true`;

    return axios.get(url);
}

export {
    searchAppUser
}