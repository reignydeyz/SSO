import axios from 'axios';

const getGroupById = async (id) => await axios.get("/api/group/" + id);

const searchGroup = async (b, sort, sortDirection, page = 1, pageSize = 10) => {
    var filterStr = `filter=contains(name, '${b.name ?? ''}') `;

    var url = `/odata/group?$${filterStr}     
    &$orderby=${sort} ${sortDirection}
    &$skip=${(page - 1) * pageSize}
    &$top=${pageSize}
    &$count=true`;

    return axios.get(url);
}

const addGroup = async (param) => await axios.post("/api/group/", param);

const updateGroup = async (param) => await axios.put(`/api/group/${param.groupId}`, param);

const deleteGroup = async (id) => await axios.delete(`/api/group/${id}`);

export {
    getGroupById,
    addGroup,
    searchGroup,
    updateGroup,
    deleteGroup
}