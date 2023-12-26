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

            <div class="tab-content" id="orders-table-tab-content">
                <div class="tab-pane fade active show" id="orders-all" role="tabpanel" aria-labelledby="orders-all-tab">
                    <div class="app-card app-card-orders-table shadow-sm mb-5">
                        <div class="table-responsive">
                            <table class="table app-table-hover mb-0 text-left">
                                <thead>
                                    <tr>
                                        <th class="cell">ID</th>
                                        <th class="cell">Name</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="i in applications" :key="i.applicationId">
                                        <td class="cell">{{ i.applicationId }}</td>
                                        <td class="cell">{{ i.name }}</td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import * as navbar from "@/services/navbar.service";
import { searchApp } from "@/services/application.service";
import { emitter } from "@/services/emitter.service";
import { pagination } from "@/services/pagination.service";
export default {
    data: () => ({
        application: new Object(),
        applications: [],
        sort: "name",
        sortDirection: "asc",
        pagination: new Object(),
    }),
    mounted() {
        navbar.init(window.location.pathname);

        this.pagination.currentPage = 1;
        this.pagination.pageSize = 15;

        this.search();
    },
    methods: {
        search(p) {
            emitter.emit("showLoader", true);
            this.pagination.currentPage = p ?? this.pagination.currentPage;
            searchApp(
                this.application,
                this.sort,
                this.sortDirection,
                this.pagination.currentPage,
                this.pagination.pageSize
            ).then(
                (r) => {
                    this.applications = r.data.value;

                    this.pagination = pagination(
                        Object.values(r.data)[1], // Gets the @odata.count which is the 2nd property
                        this.pagination.currentPage,
                        this.pagination.pageSize
                    );

                    emitter.emit("showLoader", false);
                },
                (err) => {
                    console.log(err);
                }
            );
        }
    }
}
</script>