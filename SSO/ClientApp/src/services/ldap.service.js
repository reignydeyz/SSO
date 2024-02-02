import axios from 'axios';

const sync = async () => await axios.post(`/api/ldap/sync`);

export {
    sync
}