<template>
    <div class="app-content pt-3 p-md-3 p-lg-4">
    <div class="container-xl pt-5">
      <nav aria-label="breadcrumb">
        <ol class="breadcrumb">
          <li class="breadcrumb-item">
            <router-link to="../main">Main</router-link>
          </li>
          <li class="breadcrumb-item">
            <router-link to="../groups">Groups</router-link>
          </li>
        </ol>
      </nav>
      <div class="row g-3 mb-4 align-items-center justify-content-between">
        <div class="col-auto">
          <h1 class="app-page-title mb-0">Create New Group</h1>
        </div>

        <!--//col-auto-->
      </div>
      <hr class="mb-4" />
      <div class="d-flex justify-content-center row g-4 settings-section">
        <div class="col-12 col-md-6">
          <div class="app-card app-card-settings shadow-sm p-4">
            <div class="app-card-body">
              <form class="settings-form" @submit.prevent="onSubmit">
                <div class="mb-3">
                  <label for="setting-input-2" class="form-label">Name*</label>
                  <input v-model="group.name" type="text" class="form-control" id="setting-input-2" maxlength="200"
                    placeholder="Name" required autocomplete="off" />
                </div>
                <div class="mb-3">
                  <label for="setting-input-3" class="form-label">Description</label>
                  <input v-model="group.description" type="text" class="form-control" id="setting-input-3" maxlength="500"
                    placeholder="Description" autocomplete="off" />
                </div>

                <button type="submit" class="btn app-btn-primary mt-3">
                  Save Changes
                </button>
              </form>
            </div>
            <!--//app-card-body-->
          </div>
          <!--//app-card-->
        </div>
      </div>
      <!--//row-->
    </div>
    <!--//container-fluid-->
  </div>
</template>

<script>
import { addGroup } from "@/services/group.service";
import { emitter } from "@/services/emitter.service";
import * as navbar from "@/services/navbar.service";
export default {
  data: () => ({
    group: new Object(),
  }),
  mounted() {
    navbar.init(window.location.pathname);
  },
  methods: {
    onSubmit() {
      emitter.emit("showLoader", true);

      addGroup(this.group).then(
        (r) => {
          this.$router.push("../groups");
        },
        (err) => {
          alert('Failed to add new record.');
          emitter.emit("showLoader", false);
        }
      );
    },
  },
};
</script>