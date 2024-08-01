<template>
  <div class="app-content pt-3 p-md-3 p-lg-4">
    <div class="container-xl pt-5">
      <div class="row g-3 mb-4 align-items-center justify-content-between">
        <div class="col-auto">
          <h1 class="app-page-title mb-0">Settings</h1>
        </div>
      </div>
      <hr class="mb-4" />
      <Ldap />
    </div>
  </div>
</template>
<script>

import Ldap from "@/pages/Settings/components/Ldap.vue";
export default {
  components: {
    Ldap,
  },
};
</script>