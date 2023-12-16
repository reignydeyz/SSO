import axios from 'axios';

const getAppById = async (id) => await axios.get("api/application/" + id);

export {
    getAppById
}