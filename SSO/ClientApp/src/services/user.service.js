import axios from 'axios';

const getUserById = async (id) => await axios.get("/api/user/" + id);

const searchUser = async (b, sort, sortDirection, page = 1, pageSize = 10) => {
    var filterStr = `filter=contains(firstName, '${b.firstName ?? ''}') `;    
    filterStr += `or contains(lastName, '${b.lastName ?? ''}') `;
    filterStr += `or contains(email, '${b.email ?? ''}') `;

    var url = `/odata/user?$${filterStr}     
    &$orderby=${sort} ${sortDirection}
    &$skip=${(page - 1) * pageSize}
    &$top=${pageSize}
    &$count=true`;

    return axios.get(url);
}

const addUser = async (param) => await axios.post("/api/user/", param);
const updateUser = async (id, param) => await axios.put(`/api/user/${id}`, param);

export {
    getUserById,
    addUser,
    searchUser,
    updateUser
}