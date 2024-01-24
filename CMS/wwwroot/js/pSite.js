var mainSlideRightLastChildOffset

document.addEventListener("scroll", (event) => {
    setMainSlideRightLastChildOffset(this);
    showGetToTop(this);
    showFixNavbar(this);
});

function PageInit() {
    setNavbar();
    setMainSlideRightLastChild();
}

function setNavbar() {

    var parent = document.getElementById('navbar');
    var fixNavbar = document.getElementById('fixNavbar');
    if (!parent || !fixNavbar)
        return;

    fixNavbar.innerHTML = '';
    NodeList.prototype.forEach = Array.prototype.forEach;
    var children = parent.childNodes;
    children.forEach(function (item) {
        var cln = item.cloneNode(true);
        fixNavbar.appendChild(cln);
    });
}

function setMainSlideRightLastChild() {

    resetMainSlideRightLastChild();

    var r = $(".mainSlideRight").first();
    var ra = $(".mainSlideRight div:last-child").first();
    if (r.length == 0 || ra.length == 0)
        return;

    var b1 = (ra.offset().top + ra.width() + 300);
    var b2 = $(".mainSlideLeft").height() + $(".mainSlideLeft").offset().top;
    mainSlideRightLastChildOffset = {
        b: b1 < b2,
        y: ra.offset().top,
        x: ra.offset().left,
        w: ra.width(),
        h: ra.height(),
        p: 0,
        p_y: r.offset().top,
        p_h: r.height()
    };
}

function resetMainSlideRightLastChild() {
    mainSlideRightLastChildOffset = {};
    var r = $(".mainSlideRight").first();
    var ra = $(".mainSlideRight div:last-child").first();

    if (r.length == 0 || ra.length == 0)
        return;
    ra.first().removeClass('mainSlideLeftLastAbsolute');
    ra.first().removeClass('mainSlideLeftLastFix');

}

function setMainSlideRightLastChildOffset(t) {
    if (!mainSlideRightLastChildOffset || !mainSlideRightLastChildOffset.b)
        return;
    if (screen.width <= 768) {
        resetMainSlideRightLastChild();
        return;
    }

    if (mainSlideRightLastChildOffset.p == 1
        && t.scrollY < mainSlideRightLastChildOffset.y - 80) {
        $(".mainSlideRight div:last-child").first().removeClass('mainSlideLeftLastFix');
        mainSlideRightLastChildOffset.p = 0;
    }

    if (mainSlideRightLastChildOffset.p == 1
        && t.scrollY + mainSlideRightLastChildOffset.h + 80 > mainSlideRightLastChildOffset.p_y + mainSlideRightLastChildOffset.p_h
    ) {
        $(".mainSlideRight div:last-child").first().removeClass('mainSlideLeftLastFix');
        $(".mainSlideRight div:last-child").first().addClass('mainSlideLeftLastAbsolute');
        mainSlideRightLastChildOffset.p = 2;
    }
    if (mainSlideRightLastChildOffset.p == 2
        && t.scrollY + mainSlideRightLastChildOffset.h + 80 < mainSlideRightLastChildOffset.p_y + mainSlideRightLastChildOffset.p_h
    ) {
        $(".mainSlideRight div:last-child").first().removeClass('mainSlideLeftLastAbsolute');
        $(".mainSlideRight div:last-child").first().addClass('mainSlideLeftLastFix');
        mainSlideRightLastChildOffset.p = 1;
    }
    if (!mainSlideRightLastChildOffset.p
        && t.scrollY > mainSlideRightLastChildOffset.y - 80) {
        $(".mainSlideRight div:last-child").first().addClass('mainSlideLeftLastFix');
        mainSlideRightLastChildOffset.p = 1;
    }
}


function showGetToTop(t) {
    var el = document.getElementById('getToTop');
    if (t.scrollY > 450 && el?.className.indexOf('d-hide') > -1) {
        document.getElementById('getToTop').classList.remove("d-hide");
    }
    if (t.scrollY < 450 && el?.className.indexOf('d-hide') == -1) {
        document.getElementById('getToTop').classList.add("d-hide");
    }
}

function showFixNavbar(t) {
    var el = document.getElementById('fixNavbar');
    if (t.scrollY > 450 && el?.className.indexOf('d-hide') > -1) {
        document.getElementById('fixNavbar').classList.remove("d-hide");
    }
    if (t.scrollY < 450 && el?.className.indexOf('d-hide') == -1) {
        document.getElementById('fixNavbar').classList.add("d-hide");
    }
}
function getToTop() {
    document.documentElement.scrollIntoView({ behavior: "smooth" })
}
