const pagination = (totalRecords, page, pageSize = 100) => {
    var p = new Object();
    p.totalPages = Math.ceil(totalRecords / pageSize);

    p.currentPage = page != null ? page : 1;
    p.startPage = p.currentPage - 5;
    p.endPage = p.currentPage + 4;

    if (p.startPage <= 0) {
        p.endPage -= (p.startPage - 1);
        p.startPage = 1;
    }
    if (p.endPage > p.totalPages) {
        p.endPage = p.totalPages;
        if (p.endPage > 10) {
            p.startPage = p.endPage - 9;
        }
    }

    p.totalRecords = totalRecords;
    p.pageSize = pageSize;
    p.pageIndices = [];

    for (var x = p.startPage; x <= p.endPage; x++) {
        p.pageIndices.push(x);
    }

    return p;
}

export {
    pagination
}