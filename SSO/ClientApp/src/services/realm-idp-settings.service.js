import axios from 'axios';

const modifyLdapSetting = async (param) => await axios.post("api/realm/idp/ldap", param);

export {
    modifyLdapSetting
}