import axios from 'axios';

const getAppCallbacks = async (applicationId) => await axios.get(`/api/application/${applicationId}/callback`);
const addAppCallback = async (applicationId, param) => await axios.post(`/api/application/${applicationId}/callback`, param);
const removeAppCallback = async(applicationId, param) => await axios.delete(`/api/application/${applicationId}/callback?url=${param}`);

export {
    getAppCallbacks,
    addAppCallback,
    removeAppCallback
}