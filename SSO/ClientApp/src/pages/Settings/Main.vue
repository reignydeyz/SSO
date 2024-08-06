<template>
  <div class="app-content pt-3 p-md-3 p-lg-4">
    <div class="container-xl pt-5">
      <div class="row g-3 mb-4 align-items-center justify-content-between">
        <div class="col-auto">
          <h1 class="app-page-title mb-0">Settings</h1>
        </div>
      </div>
      <hr class="mb-4" />
      <Ldap :realm="realm"/>
    </div>
  </div>
</template>
<script>

import * as navbar from "@/services/navbar.service";
import { emitter } from "@/services/emitter.service";
import { getCurrentRealm } from "@/services/realm.service";
import Ldap from "@/pages/Settings/components/Ldap.vue";
export default {
  components: {
    Ldap,
  },
  data: () => ({
    realm: new Object(),
  }),
  async mounted() {
    navbar.init(this.$route);

    emitter.emit("showLoader", true);

    this.realm = (await getCurrentRealm()).data;

    emitter.emit("showLoader", false);
  }
};
</script>