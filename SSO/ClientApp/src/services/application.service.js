import axios from 'axios';

const getAppById = async (id) => await axios.get("/api/application/" + id);
const addApp = async (param) => await axios.post("/api/application/", param);

export {
    getAppById,
    addApp
}