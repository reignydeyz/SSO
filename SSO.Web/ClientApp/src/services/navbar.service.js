function init(route) {
    // Iterate main menus
    document.querySelectorAll("a.nav-link").forEach(r => {
        r.classList.remove("active");

        if (!r.href.includes("#")) {
            if (r.pathname == "/" && window.location.href === (window.location.origin + "/")
                || (window.location.href.includes(r.pathname) && r.pathname != "/")) {
                r.classList.add("active");
            }
        }
    });

    // Iterate main sub-menus
    document.querySelectorAll("a.submenu-link").forEach(r => {
        r.classList.remove("active");

        if (!r.href.includes("#")) {
            if (window.location.pathname === r.pathname) {
                r.classList.add("active");
            }
        }
    });

    responsiveSidePanel();

    window.addEventListener('load', function () {
        responsiveSidePanel();
    });

    window.addEventListener('resize', function () {
        responsiveSidePanel();
    });

    function responsiveSidePanel() {
        const sidePanel = document.getElementById('app-sidepanel');

        let w = window.innerWidth;
        if (w >= 1200) {
            // If larger 
            sidePanel.classList.remove('sidepanel-hidden');
            sidePanel.classList.add('sidepanel-visible');

        } else {
            // If smaller
            sidePanel.classList.remove('sidepanel-visible');
            sidePanel.classList.add('sidepanel-hidden');
        }
    }
}

export {
    init
}