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
const copyUser = async (id, param) => await axios.post(`/api/user/${id}`, param);
const updateUser = async (id, param) => await axios.put(`/api/user/${id}`, param);
const deleteUser = async (id) => await axios.delete(`/api/user/${id}`);

export {
    getUserById,
    addUser,
    copyUser,
    searchUser,
    updateUser,
    deleteUser
}