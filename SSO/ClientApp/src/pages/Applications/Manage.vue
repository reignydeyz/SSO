<template>
    <div class="app-content pt-3 p-md-3 p-lg-4">
        <div class="container-xl pt-5">
            <div class="row g-3 mb-4 align-itemss-center justify-content-between">
                <div class="col-auto">
                    <h1 class="app-page-title mb-0">Applications</h1>
                </div>
                <div class="col-auto">
                    <div class="page-utilities">
                        <div class="row g-2 justify-content-start justify-content-md-end align-items-center">
                            <div class="col-auto">
                                <form class="docs-search-form row gx-1 align-items-center">
                                    <div class="col-auto">
                                        <input v-model="application.name" type="text" class="form-control search-docs"
                                            placeholder="Name" />
                                    </div>
                                    <div class="col-auto">
                                        <button type="submit" class="btn app-btn-secondary">
                                            Search
                                        </button>
                                    </div>
                                </form>
                            </div>
                            <!--//col-->

                            <div class="col-auto">
                                <router-link to="/applications/new" class="btn app-btn-primary"><i
                                        class="bi bi-plus-lg"></i>&nbsp;Create
                                    New</router-link>
                            </div>
                        </div>
                        <!--//row-->
                    </div>
                    <!--//table-utilities-->
                </div>
                <!--//col-auto-->
            </div>
        </div>
    </div>
</template>

<script>
import * as navbar from "@/services/navbar.service";
export default {
    data: () => ({
        application: new Object(),
    }),
    mounted() {
        navbar.init(window.location.pathname);
    },
}
</script>