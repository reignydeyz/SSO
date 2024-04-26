import axios from 'axios';

const healthCheck = async () => await axios.get(`/hc`);

export {
    healthCheck
}