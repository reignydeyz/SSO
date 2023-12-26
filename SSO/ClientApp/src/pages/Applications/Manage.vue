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
                                <form class="docs-search-form row gx-1 align-items-center" @submit.prevent="search(1)">
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
                                <caption class="p-2">
                                    <div class="d-flex justify-content-between">
                                        <div class="p-2"><small>Found: {{ this.pagination.totalRecords }}</small></div>
                                        <div class="p-2">
                                            <small>Items per page:&nbsp;&nbsp;</small>
                                            <select class="form-select form-select-sm ms-auto d-inline-flex w-auto"
                                                v-model="pagination.pageSize" @change="search(1)">
                                                <option value="10">10</option>
                                                <option value="15">15</option>
                                                <option value="20">20</option>
                                                <option value="35">35</option>
                                                <option value="50">50</option>
                                                <option value="75">75</option>
                                                <option value="100">100</option>
                                            </select>
                                        </div>
                                    </div>
                                </caption>
                                <thead>
                                    <tr>
                                        <th class="cell">ID</th>
                                        <th class="cell" @click="sortData('name')">
                                            Name
                                            <i v-if="sort === 'name'" v-bind:class="{
                                                'bi-arrow-down': sortDirection === 'asc',
                                                'bi-arrow-up': sortDirection === 'desc',
                                            }"></i>
                                        </th>
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

                    <nav aria-label="..." v-if="pagination.endPage > 1" class="app-pagination">
                        <ul class="pagination justify-content-center flex-wrap">
                            <li class="page-item" v-if="pagination.currentPage > 1">
                                <a class="page-link" tabindex="-1" aria-disabled="true" style="cursor: pointer"
                                    @click="paginate(1)">First</a>
                            </li>
                            <li class="page-item" v-if="pagination.currentPage > 1">
                                <a class="page-link" style="cursor: pointer"
                                    @click="paginate(pagination.currentPage - 1)">Previous</a>
                            </li>
                            <li class="page-item" v-bind:class="{ active: pagination.currentPage == p }"
                                v-for="p in pagination.pageIndices" :key="p">
                                <a class="page-link" style="cursor: pointer" @click="paginate(p)">{{ p }}</a>
                            </li>
                            <li class="page-item" v-if="pagination.currentPage < pagination.totalPages">
                                <a class="page-link" style="cursor: pointer"
                                    @click="paginate(pagination.currentPage + 1)">Next</a>
                            </li>
                            <li class="page-item" v-if="pagination.currentPage < pagination.totalPages">
                                <a class="page-link" style="cursor: pointer"
                                    @click="paginate(pagination.totalPages)">Last</a>
                            </li>
                        </ul>
                    </nav>
                    <!--//app-pagination-->
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
        },
        paginate(page) {
            this.pagination.currentPage = page;
            this.search();
        },

        sortData(field) {
            if (field === this.sort) {
                this.sortDirection = this.sortDirection === "asc" ? "desc" : "asc";
            } else {
                this.sortDirection = "asc";
            }

            this.sort = field;
            this.search(1);
        },
    }
}
</script>