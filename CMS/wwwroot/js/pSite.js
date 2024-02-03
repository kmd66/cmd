var mainSlideRightLastChildOffset

document.addEventListener("scroll", (event) => {
    setMainSlideRightLastChildOffset(this);
    showGetToTop(this);
    showFixNavbar(this);
});

function PageInit() {
    getGallery();
    //setNavbar();
    setMainSlideRightLastChild();
    setCommentEl();
    getScores();
    callXzoom();
    getMany();
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
    document.getElementById('fixNavbar').classList.remove("disable");
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

function setCommentEl() {
    $('.commentUserAvatar').each(function (i, obj) {
        $(obj).css("background-color", getRandomColor());
    });
    $('.commentScore').each(function (i, obj) {
        var t = $(obj).data("score") - 1;
        $(obj).empty();
        for (let ei = 0; ei < 5; ei++) {
            if (ei <= t)
                $(obj).append('<span class= "star2 active"></span >');
            else
                $(obj).append('<span class= "star2"></span >');
        }
    });
}
function getRandomColor() {
    var letters = '0123456789ABCDEF';
    var color = '#';
    for (var i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

function getScores() {
    var star = '.star',
        selected = '.selected';
    $(star).unbind('click');
    $(star).on('click', function () {
        $(selected).each(function () {
            $(this).removeClass('selected');
        });
        $(this).addClass('selected');
    });
}

function getMany() {
    $('.many').each(function (i, obj) {
        var t = formatMoney($(obj).text().trim());
        $(obj).text(t);
    });
}
function formatMoney(amount, decimalCount = 0, decimal = ".", thousands = ",") {
    try {
        decimalCount = Math.abs(decimalCount);
        decimalCount = isNaN(decimalCount) ? 0 : decimalCount;

        const negativeSign = amount < 0 ? "-" : "";

        let i = parseInt(amount = Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
        let j = (i.length > 3) ? i.length % 3 : 0;

        return negativeSign +
            (j ? i.substr(0, j) + thousands : '') +
            i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) +
            (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
    } catch (e) {
        return "0";
    }
}
function getGallery() {
    var list = [];
    $('img.g').each(function (i, obj) {
        list.push({ at: $(obj).attr('slide'), obj: obj });
    });
    const grouped = groupBy(list, pet => pet.at);
    var insertEl;
    grouped.forEach((x) => {
        x.forEach((num, idx, arr) => {
            $(num.obj).addClass('cGallery');
            $(num.obj).attr('galleryId', idx);
            var p = $(num.obj).parent();
            if (idx == 0) {
                insertEl = p;
                p.empty();
            }
            else {
                p.remove();
            }
            insertEl.append(num.obj);
        });
    });
    eventGallery();
}
function eventGallery() {
    $('img.g').unbind('click');
    $('#galleryClose').unbind('click');
    $('#galleryNext').unbind('click');
    $('#galleryPrev').unbind('click');
    //var t = $._data($('#galleryClose')[0], "events");
    //if ($("#galleryModalDialog").hasClass("galleryModalDialog"))
    //    return;
    //$("#galleryModalDialog").addClass("galleryModalDialog");
    //
    $('img.g').click(function () {

        const groupId = $(this).attr('slide');
        const src = $(this).attr('src').replace("-thumbnail", '');
        const group = $(`[slide=${groupId}]`);
        if (group.length > 1) {
            $('#galleryNext').css("visibility", "visible");
            $('#galleryPrev').css("visibility", "visible");
        }
        else {
            $('#galleryNext').css("visibility", "hidden");
            $('#galleryPrev').css("visibility", "hidden");
        }
        const dialog = $('#galleryModalDialog');
        console.log(`${$(this).attr('galleryId')} | ${$(this).attr('slide')} | ${group.length}`);
        dialog.empty();
        dialog.append(`<img class="c" src="${src}" style="max-height: ${$(window).height() - 40}px" galleryId="${$(this).attr('galleryId')}" slideD="${$(this).attr('slide')}">`);
        $('#galleryModal').modal('show');

    });
    $('#galleryClose').click(function () {
        $('#galleryModal').modal('hide');
    });
    $('#galleryNext').click(function () {
        moveGallery(true);
    });
    $('#galleryPrev').click(function () {
        moveGallery( true);
    });
}
function moveGallery(next) {
    const el = $("#galleryModalDialog img:first-child");
    var t = el.attr('slideD');
    const group = $(`[slide=${el.attr('slideD')}]`);
    var myArr = Array.from(group);
    const dialog = $('#galleryModalDialog');

    var idx = 0;
    myArr.forEach((x, i, a) => {
        if ($(x).attr('galleryId') == el.attr('galleryId'))
            idx = i;
    });
    if (next)
        idx++;
    else
        idx--;
    if (idx < 0)
        idx = group.length - 1;
    if (idx > group.length - 1)
        idx = 0;
    dialog.empty();
    const img = group[idx];
    const src = $(img).attr('src').replace("-thumbnail", '');
    dialog.append(`<img class="c" src="${src}" style="max-height: ${$(window).height() - 40}px" galleryId="${$(img).attr('galleryId')}" slideD="${$(img).attr('slide')}">`);
};

