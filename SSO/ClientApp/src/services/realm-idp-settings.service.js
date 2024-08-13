import axios from 'axios';

const modifyLdapSetting = async (param) => await axios.post("api/realm/idp/ldap", param);
const deleteLdapSetting = async () => await axios.delete(`/api/realm/idp/ldap`)

export {
    modifyLdapSetting,
    deleteLdapSetting
}