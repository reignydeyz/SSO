import axios from 'axios';

const getAppById = async (id) => await axios.get("/api/application/" + id);

const searchApp = async (b, sort, sortDirection, page = 1, pageSize = 10) => {
    var filterStr = `filter=contains(name, '${b.name ?? ''}') `;

    var url = `/odata/application?$${filterStr}     
    &$orderby=${sort} ${sortDirection}
    &$skip=${(page - 1) * pageSize}
    &$top=${pageSize}
    &$count=true`;

    return axios.get(url);
}

const addApp = async (param) => await axios.post("/api/application/", param);

const copyApp = async (id, param) => await axios.post(`/api/application/${id}`, param);

const updateApp = async (param) => await axios.put(`/api/application/${param.applicationId}`, param);

const deleteApp = async (id) => await axios.delete(`/api/application/${id}`);

export {
    getAppById,
    addApp,
    copyApp,
    searchApp,
    updateApp,
    deleteApp
}