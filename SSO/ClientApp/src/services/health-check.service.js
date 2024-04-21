import axios from 'axios';

const healthCheck = async () => await axios.post(`/hc`);

export {
    healthCheck
}