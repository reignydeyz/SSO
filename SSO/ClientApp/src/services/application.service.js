import axios from 'axios';

const getAppById = async (id) => await axios.get("/api/application/" + id);

const searchApp = async (b, sort, sortDirection, page = 1, pageSize = 10) => {
    var filterStr = `filter=contains(name, '${b.name ?? ''}') `;

    var url = `/odata/application?$${filterStr}     
    &$orderby=${sort} ${sortDirection}
    &$skip=${(page - 1) * pageSize}
    &$top=${pageSize}
    &$count=true`;
}

const addApp = async (param) => await axios.post("/api/application/", param);

export {
    getAppById,
    addApp,
    searchApp
}