import axios from 'axios';

const getCurrentRealm = async () => await axios.get("api/realm");

export {
    getCurrentRealm
}