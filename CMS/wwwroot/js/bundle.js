/**
 * Alert class responsible for creating app Alerts.
 */
function JsShowAlert(obj) {
    new Alert({
        type: obj.type,
        message: obj.message,
        expires: obj.expires,
        withProgress: obj.withProgress,
        container: obj.container,
    })
}
class $Alert {

    /**
     * Create a new Alert Class instance.
     *
     * @param { Object } config Alert config.
     */
    constructor(config = {}) {
        // A DOM element in which an Alert should appear.
        this.container = document.querySelector(config.container ?? 'body')

        // An Alert type.
        this.type = config.type;

        // An Alert message.
        this.message = config.message;

        // An Alert duration.
        this.duration = (config.duration ?? 5) * 1000;

        // Determine whether an Alert should be removed from the DOM.
        this.destroy = config.destroy ?? true;

        // Determine whether an Alert should expire/auto-close.
        this.expires = config.expires ?? true;

        // Determine whether an Alert should have a progress bar.
        this.withProgress = config.withProgress ?? false;

        // Determine whether an Alert should have an info icon.
        this.info = config.info ?? true;

        // Determine whether an Alert should have a close icon.
        this.close = config.close ?? true;

        // An Alert intervals for 'close' and 'progress'.
        this.intervals = {
            close   : this.expires ? setTimeout(this.#closeAlert.bind(this), this.duration) : null,
            progress: setInterval(this.#progress.bind(this), this.duration / 100),
        }

        this.#build();
        this.#addSelectors();
        this.#addListeners();
    }

    /**
     * Build an Alert.
     *
     * @returns { HTMLElement }
     */
    #build() {
        let alert = this.#buildAlertContainer()
        let message = this.#buildAlertMessage()
        let info = this.#buildInfoIcon();
        let close = this.#buildCloseIcon();
        let progress = this.#buildProgressBar()

        alert.append(info, message, close, progress);

        this.alert = alert;

        this.container.append(alert);
    }

    /**
     * Move Alert progress bar.
     */
    #progress() {
        if (this.progressBar && this.expires) {
            if (this.progressBar.value >= 100) {
                this.#closeAlert.bind(this);
                clearInterval(this.intervals.progress);
            } else {
                this.progressBar.value++;
            }
        }
    }

    /**
     * Close Alert message.
     */
    #closeAlert() {
        // If progress bar exists, set its value to 100, effectively completing the progress.
        if (this.progressBar && this.expires) {
            this.progressBar.value = 100;
        }

        // Clear Alert Close interval.
        clearInterval(this.intervals.close);

        // Close Alert message.
        setTimeout(() => this.alert.classList.replace('alert-is-open', 'alert-is-closed'), this.expires ? 500 : 0);

        // Remove an Alert message from the DOM.
        if (this.destroy) {
            setTimeout(() => this.alert.parentElement?.removeChild(this.alert), this.expires ? 1000 : 500);
        }
    }

    /**
     * Add Alert Query Selectors.
     */
    #addSelectors() {
        this.button = this.alert.querySelector('.alert__close') ?? this.button;
        this.progressBar = this.alert.querySelector('.alert__progress');
    }

    /**
     * Add Alert Event Listeners.
     */
    #addListeners() {
        this.button?.addEventListener('click', this.#closeAlert.bind(this));
    }

    /**
     * Build an Alert container.
     *
     * @return { HTMLElement }
     */
    #buildAlertContainer() {
        return this.#createElement('div', {
            class: 'alert alert-' + this.type + ' alert-is-open',
            role : 'alert',
        });
    }

    /**
     * Build an Alert message.
     *
     * @return { HTMLElement }
     */
    #buildAlertMessage() {
        return this.#createElement('p', {
            class: 'alert__message',
            text : this.message,
        });
    }

    /**
     * Build an Alert Progress Bar.
     *
     * @return { HTMLElement|String }
     */
    #buildProgressBar() {
        if (!this.withProgress) {
            return ''
        }

        return this.#createElement('progress', {
            class: 'alert__progress',
            value: 0,
            max  : 100,
        });
    }

    /**
     * Build an Alert Info icon.
     *
     * @return { HTMLElement|String }
     */
    #buildInfoIcon() {
        switch (typeof this.info) {
            case 'boolean':
                if (!this.info) {
                    return '';
                }

                return this.#toHTML('<svg aria-hidden="true" class="alert__info" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" d="M18 10a8 8 0 11-16 0 8 8 0 0116 0zm-7-4a1 1 0 11-2 0 1 1 0 012 0zM9 9a1 1 0 000 2v3a1 1 0 001 1h1a1 1 0 100-2v-3a1 1 0 00-1-1H9z" clip-rule="evenodd"></path></svg>');
            case 'string':
                return this.#toHTML(`<span class="alert__info">${this.info}</span>`)
        }
    }

    /**
     * Build an Alert Close icon.
     *
     * @return { HTMLElement|String }
     */
    #buildCloseIcon() {
        switch (typeof this.close) {
            case 'boolean':
                if (!this.close) {
                    return '';
                }

                return this.#toHTML('<button class="alert__close" role="button"><svg aria-hidden="true" fill="currentColor" viewBox="0 0 20 20" xmlns="http://www.w3.org/2000/svg"><path fill-rule="evenodd" d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z" clip-rule="evenodd"></path></svg></button>')

            case 'string':
                this.button = this.#toHTML(`('<button class="alert__close" role="button">${this.close}</button>`)

                return this.button;
        }
    }

    /**
     * Create an HTML element.
     *
     * @param { String } tag The tag of an element.
     * @param { Object } attributes Element attributes.
     *
     * @returns { HTMLElement }
     */
    #createElement(tag, attributes = {}) {
        let element = document.createElement(tag);

        for (let attribute in attributes) {
            if (attribute === 'text') {
                element.innerHTML = attributes[attribute];

                continue;
            }

            element.setAttribute(attribute, attributes[attribute]);
        }

        return element;
    }

    /**
     * Convert a String to HTML element.
     *
     * @param { Object } string HTML string.
     *
     * @returns { HTMLElement }
     */
    #toHTML(string) {
        let pattern = /(?!<!DOCTYPE)<([^\s>]+)(\s|>)+/;
        let tag = string.match(pattern)[1];
        let HTML = document.createElement('html');
        let DOM = document.createElement(tag);
        HTML.append(DOM);
        DOM.outerHTML = string;

        return tag === 'html' ? HTML : HTML.querySelector(tag);
    }
}

if (typeof exports != 'undefined') {
    module.exports = Alert = $Alert
} else {
    window.Alert = $Alert;
}
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
        var t = formatMoney($(obj).text().trim().replaceAll(',', ''));
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
                p.addClass("pGallery");
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



function CopyToClipboard(data) {
    navigator.clipboard.writeText(data);
}
function ShowLoad() {
    $("#loding").addClass("d-block");
}
function HideLoad() {
    $("#loding").removeClass("d-block");
}
function OpenNewTab(url) {
    window.open(url, '_blank');
}
function ShowModal(id) {
    $(`#${id}`).modal("show");
}
function HideModal(id) {
    $(`#${id}`).modal("hide");
}
async function saveFile(obj) {
    var input = document.getElementById(obj.fileId);
    var file = input.files[0];
    if (!file)
        return { Success: false, Message: 'فایل انتخاب نشده' };

    let formData = new FormData();
    formData.append("file", file);
    formData.append("dir", obj.dir);
    return $.ajax({
        type: "POST",
        mimeTypes: "multipart/form-data",
        url: '/api/Attachment/Upload',
        data: formData,
        headers: {
            "Authorization": obj.token
        },
        contentType: false,
        cache: false,
        processData: false,
        timeout: 800000,
    }).then((res) => {
        $(`#${obj.fileId}`).val("");
        return res;

    }).catch((e) => {
        return { Success: false, Message: e.statusText, Code: e.status };
    });
}

function ScrollIntoView(id) {
    const y = document.getElementById(id).getBoundingClientRect().top + window.scrollY;
    window.scroll({
        top: y -50,
        behavior: 'smooth'
    });
}
function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}
function groupBy(list, keyGetter) {
    const map = new Map();
    list.forEach((item) => {
        const key = keyGetter(item);
        const collection = map.get(key);
        if (!collection) {
            map.set(key, [item]);
        } else {
            collection.push(item);
        }
    });
    return map;
}

function getToTop() {
    document.documentElement.scrollIntoView({ behavior: "smooth" })
}

function showNavSearch() {
    getToTop();
    var el = document.getElementById('navSearch');
    if (el?.className.indexOf('d-none') > -1) {
        el.classList.remove("d-none");
    }
}

function hideNavSearch() {
    getToTop();
    var el = document.getElementById('navSearch');
    if (el?.className.indexOf('d-none') == -1) {
        el.classList.add("d-none");
    }
}

function showOffcanvas(id) {
    var el = document.getElementById(id);
    if (el?.className.indexOf('show') == -1) {
        el.classList.add("show");
        document.body.classList.add("bodyOverflowHide");
    }
}

function hideOffcanvas(id) {
    var el = document.getElementById(id);
    if (el?.className.indexOf('show') > -1) {
        el.classList.remove("show");
        document.body.classList.remove("bodyOverflowHide");
    }
}

var HomeSlideObj;
var HomeSlideMove = false;

function InitHomeSlide() {
    if (HomeSlideObj)
        clearInterval(HomeSlideObj);
    HomeSlideObj = setInterval(function () {
        moveRight();
    }, 3000);

    var slideCount = $('#homeSlider ul li').length;
    var slideWidth = $('#homeSlider ul li').width();
    var slideHeight = $('#homeSlider ul li').height();
    var sliderUlWidth = slideCount * slideWidth;

    //$('#homeSlider').css({ width: slideWidth, height: slideHeight });

    //$('#homeSlider ul').css({ width: sliderUlWidth, marginLeft: - slideWidth });

    //$('#homeSlider ul li:last-child').prependTo('#homeSlider ul');

    function move(_slideWidth) {
        HomeSlideMove = true;
        var el = $('#homeSlider ul li:last-child');
        var bHomeSlider = document.getElementById('bHomeSlider');
        if (!el || !bHomeSlider)
            return;
        copySlideElement(el);
        $('#homeSlider ul').animate({
            left: + _slideWidth
        }, 1000, function () {
            HomeSlideMove = false;
            el.prependTo('#homeSlider ul');
            $('#homeSlider ul').css('left', '');

            bHomeSlider.innerHTML = '';
        });
    };
    function moveLeft() {
        move(slideWidth *-1);
    };

    function moveRight() {
        move(slideWidth);
    };

    function copySlideElement(el) {
        var bEl = document.getElementById('bHomeSlider');
        if (!bEl) {
            clearInterval(HomeSlideObj);
            return;
        }
        bEl.innerHTML = '';
        NodeList.prototype.forEach = Array.prototype.forEach;
        var children = el[0].childNodes;
        children.forEach(function (item) {
            var cln = item.cloneNode(true);
            bEl.appendChild(cln);
        });
    };

    $('span.control_prev').click(function () {
        if (HomeSlideMove)
            return;
        if (HomeSlideObj)
            clearInterval(HomeSlideObj);
        moveRight();
    });

    $('span.control_next').click(function () {
        if (HomeSlideMove)
            return;
        if (HomeSlideObj)
            clearInterval(HomeSlideObj);
        moveLeft();
    });

}
function callXzoom() {
    $('.xzoom, .xzoom-gallery').xzoom({ zoomWidth: 400, title: true, tint: '#333', Xoffset: 15 });
    $('.xzoom2, .xzoom-gallery2').xzoom({ zoomWidth: 300, position: '#xzoom2-id', tint: '#ffa200' });
    $('.xzoom3, .xzoom-gallery3').xzoom({ zoomWidth: 300, position: 'lens', lensShape: 'circle', sourceClass: 'xzoom-hidden' });
    $('.xzoom4, .xzoom-gallery4').xzoom({ zoomWidth: 300, tint: '#006699', Xoffset: 15 });
    $('.xzoom5, .xzoom-gallery5').xzoom({ zoomWidth: 300,  int: '#006699', Xoffset: 15 });

    //Integration with hammer.js
    var isTouchSupported = 'ontouchstart' in window;

    if (isTouchSupported) {
        //If touch device
        $('.xzoom, .xzoom2, .xzoom3, .xzoom4, .xzoom5').each(function () {
            var xzoom = $(this).data('xzoom');
            xzoom.eventunbind();
        });



    } else {
        //If not touch device

        //Integration with fancybox plugin
        $('#xzoom-fancy').bind('click', function (event) {
            var xzoom = $(this).data('xzoom');
            xzoom.closezoom();
            $.fancybox.open(xzoom.gallery().cgallery, { padding: 0, helpers: { overlay: { locked: false } } });
            event.preventDefault();
        });

        //Integration with magnific popup plugin
        $('#xzoom-magnific').bind('click', function (event) {
            var xzoom = $(this).data('xzoom');
            xzoom.closezoom();
            var gallery = xzoom.gallery().cgallery;
            var i, images = new Array();
            for (i in gallery) {
                images[i] = { src: gallery[i] };
            }
            $.magnificPopup.open({ items: images, type: 'image', gallery: { enabled: true } });
            event.preventDefault();
        });
    }
}
/*!-----------------------------------------------------
 * xZoom v1.0.15
 * (c) 2013 by Azat Ahmedov & Elman Guseynov
 * https://github.com/payalord
 * https://dribbble.com/elmanvebs
 * Apache License 2.0
 *------------------------------------------------------*/
function detect_old_ie(){if(/MSIE (\d+\.\d+);/.test(navigator.userAgent)){var o=new Number(RegExp.$1);return!(9<=o)&&(8<=o||(7<=o||(6<=o||(5<=o||void 0))))}return!1}window.requestAnimFrame=window.requestAnimationFrame||window.webkitRequestAnimationFrame||window.mozRequestAnimationFrame||window.oRequestAnimationFrame||window.msRequestAnimationFrame||function(o){window.setTimeout(o,20)},function(Ao){function n(s,o){this.xzoom=!0;var t,a,p,l,r,e,n,d,i,c,h,f,u,v,m,g,w,x,b,z,y,C,k,O,M,A,S,H,W,F,I,T,X,Y,R,q,E,L,D,Z,_,j,N,Q,$,B,G,J,K,P,U,V=this,oo={},to=(new Array,new Array),eo=0,io=0,so=0,no=0,ao=0,po=0,lo=0,ro=0,co=0,ho=0,fo=0,uo=0,vo=0,mo=detect_old_ie(),go=/MSIE (\d+\.\d+);/.test(navigator.userAgent),wo="";function xo(){var o=document.documentElement;return{left:(window.pageXOffset||o.scrollLeft)-(o.clientLeft||0),top:(window.pageYOffset||o.scrollTop)-(o.clientTop||0)}}function bo(){var o;"circle"==V.options.lensShape&&"lens"==V.options.position&&(o=((M=A=Math.max(M,A))+2*Math.max(F,W))/2,k.css({"-moz-border-radius":o,"-webkit-border-radius":o,"border-radius":o}))}function zo(o,t,e,i){"lens"==V.options.position?(C.css({top:-(t-n)*T+A/2,left:-(o-d)*I+M/2}),V.options.bg&&(k.css({"background-image":"url("+C.attr("src")+")","background-repeat":"no-repeat","background-position":-(o-d)*I+M/2+"px "+(-(t-n)*T+A/2)+"px"}),e&&i&&k.css({"background-size":e+"px "+i+"px"}))):C.css({top:-H*T,left:-S*I})}function yo(o,t){var e,i;1<(so=so<-1?-1:so)&&(so=1),X<Y?i=(e=l*(X-(X-1)*so))/R:e=(i=r*(Y-(Y-1)*so))*R,L?(no=o,ao=t,po=e,lo=i):(L||(ro=po=e,co=lo=i),M=l/(I=e/a),A=r/(T=i/p),bo(),ko(o,t),C.width(e),C.height(i),k.width(M),k.height(A),k.css({top:H-F,left:S-W}),O.css({top:-H,left:-S}),zo(o,t,e,i))}function Co(){var o=ho,t=fo,e=uo,i=vo,s=ro,n=co;o+=(no-o)/V.options.smoothLensMove,t+=(ao-t)/V.options.smoothLensMove,e+=(no-e)/V.options.smoothZoomMove,i+=(ao-i)/V.options.smoothZoomMove,s+=(po-s)/V.options.smoothScale,n+=(lo-n)/V.options.smoothScale,M=l/(I=s/a),A=r/(T=n/p),bo(),ko(o,t),C.width(s),C.height(n),k.width(M),k.height(A),k.css({top:H-F,left:S-W}),O.css({top:-H,left:-S}),ko(e,i),zo(o,t,s,n),ho=o,fo=t,uo=e,vo=i,ro=s,co=n,L&&requestAnimFrame(Co)}function ko(o,t){S=(o-=d)-M/2,H=(t-=n)-A/2,"lens"!=V.options.position&&V.options.lensCollision&&(S<0&&(S=0),M<=a&&a-M<S&&(S=a-M),a<M&&(S=a/2-M/2),H<0&&(H=0),A<=p&&p-A<H&&(H=p-A),p<A&&(H=p/2-A/2))}function Oo(){void 0!==m&&m.remove(),void 0!==w&&w.remove(),void 0!==N&&N.remove()}function Mo(o){var t=o.attr("title"),o=o.attr("xtitle");return o||t||""}this.adaptive=function(){0!=B&&0!=G||(s.css("width",""),s.css("height",""),B=s.width(),G=s.height()),Oo(),Q=Ao(window).width(),$=Ao(window).height(),J=s.width(),K=s.height(),B<J&&(J=B),G<K&&(K=G),(Q<B||$<G?!0:!1)?s.width("100%"):0!=B&&s.width(B),"fullscreen"!=P&&(!function(){var o=s.offset();l="auto"==V.options.zoomWidth?J:V.options.zoomWidth;r="auto"==V.options.zoomHeight?K:V.options.zoomHeight;"#"==V.options.position.substr(0,1)?oo=Ao(V.options.position):oo.length=0;if(0!=oo.length)return!0;switch(P){case"lens":case"inside":return!0;case"top":n=o.top,d=o.left,i=n-r,c=d;break;case"left":n=o.top,d=o.left,i=n,c=d-l;break;case"bottom":n=o.top,d=o.left,i=n+K,c=d;break;case"right":default:n=o.top,d=o.left,i=n,c=d+J}return!(Q<c+l||c<0)}()?V.options.position=V.options.mposition:V.options.position=P),V.options.lensReverse||(U=V.options.adaptiveReverse&&V.options.position==V.options.mposition)},this.xscroll=function(o){var t,e;u=o.pageX||o.originalEvent.pageX,v=o.pageY||o.originalEvent.pageY,o.preventDefault(),o.xscale?(so=o.xscale,yo(u,v)):(t=-o.originalEvent.detail||o.originalEvent.wheelDelta||o.xdelta,e=u,o=v,mo&&(e=D,o=Z),so+=t=0<t?-.05:.05,yo(e,o))},this.openzoom=function(o){switch(u=o.pageX,v=o.pageY,V.options.adaptive&&V.adaptive(),so=V.options.defaultScale,L=!1,m=Ao("<div></div>"),""!=V.options.sourceClass&&m.addClass(V.options.sourceClass),m.css("position","absolute"),x=Ao("<div></div>"),""!=V.options.loadingClass&&x.addClass(V.options.loadingClass),x.css("position","absolute"),g=Ao('<div style="position: absolute; top: 0; left: 0;"></div>'),m.append(x),w=Ao("<div></div>"),""!=V.options.zoomClass&&"fullscreen"!=V.options.position&&w.addClass(V.options.zoomClass),w.css({position:"absolute",overflow:"hidden",opacity:1}),V.options.title&&""!=wo&&(N=Ao("<div></div>"),j=Ao("<div></div>"),N.css({position:"absolute",opacity:1}),V.options.titleClass&&j.addClass(V.options.titleClass),j.html("<span>"+wo+"</span>"),N.append(j),V.options.fadeIn&&N.css({opacity:0})),k=Ao("<div></div>"),""!=V.options.lensClass&&k.addClass(V.options.lensClass),k.css({position:"absolute",overflow:"hidden"}),V.options.lens&&(lenstint=Ao("<div></div>"),lenstint.css({position:"absolute",background:V.options.lens,opacity:V.options.lensOpacity,width:"100%",height:"100%",top:0,left:0,"z-index":9999}),k.append(lenstint)),function(){switch(p="fullscreen"==V.options.position?(a=Ao(window).width(),Ao(window).height()):(a=s.width(),s.height()),x.css({top:p/2-x.height()/2,left:a/2-x.width()/2}),(e=V.options.rootOutput||"fullscreen"==V.options.position?s.offset():s.position()).top=Math.round(e.top),e.left=Math.round(e.left),V.options.position){case"fullscreen":n=xo().top,d=xo().left,c=i=0;break;case"inside":n=e.top,d=e.left,c=i=0;break;case"top":n=e.top,d=e.left,i=n-r,c=d;break;case"left":n=e.top,d=e.left,i=n,c=d-l;break;case"bottom":n=e.top,d=e.left,i=n+p,c=d;break;case"right":default:n=e.top,d=e.left,i=n,c=d+a}n-=m.outerHeight()/2,d-=m.outerWidth()/2,"#"==V.options.position.substr(0,1)?oo=Ao(V.options.position):oo.length=0,0==oo.length&&"inside"!=V.options.position&&"fullscreen"!=V.options.position?(V.options.adaptive&&B&&G||(B=a,G=p),l="auto"==V.options.zoomWidth?a:V.options.zoomWidth,r="auto"==V.options.zoomHeight?p:V.options.zoomHeight,i+=V.options.Yoffset,c+=V.options.Xoffset,w.css({width:l+"px",height:r+"px",top:i,left:c}),"lens"!=V.options.position&&t.append(w)):"inside"==V.options.position||"fullscreen"==V.options.position?(l=a,r=p,w.css({width:l+"px",height:r+"px"}),m.append(w)):(l=oo.width(),r=oo.height(),V.options.rootOutput?(i=oo.offset().top,c=oo.offset().left,t.append(w)):(i=oo.position().top,c=oo.position().left,oo.parent().append(w)),i+=(oo.outerHeight()-r-w.outerHeight())/2,c+=(oo.outerWidth()-l-w.outerWidth())/2,w.css({width:l+"px",height:r+"px",top:i,left:c})),V.options.title&&""!=wo&&("inside"==V.options.position||"lens"==V.options.position||"fullscreen"==V.options.position?(h=i,f=c,m.append(N)):(h=i+(w.outerHeight()-r)/2,f=c+(w.outerWidth()-l)/2,t.append(N)),N.css({width:l+"px",height:r+"px",top:h,left:f})),m.css({width:a+"px",height:p+"px",top:n,left:d}),g.css({width:a+"px",height:p+"px"}),V.options.tint&&"inside"!=V.options.position&&"fullscreen"!=V.options.position?g.css("background-color",V.options.tint):mo&&g.css({"background-image":"url("+s.attr("src")+")","background-color":"#fff"}),y=new Image;var o="";switch(go&&(o="?r="+(new Date).getTime()),y.src=s.attr("xoriginal")+o,(C=Ao(y)).css("position","absolute"),(y=new Image).src=s.attr("src"),(O=Ao(y)).css("position","absolute"),O.width(a),V.options.position){case"fullscreen":case"inside":w.append(C);break;case"lens":k.append(C),V.options.bg&&C.css({display:"none"});break;default:w.append(C),k.append(O)}}(),"inside"!=V.options.position&&"fullscreen"!=V.options.position?((V.options.tint||mo)&&m.append(g),V.options.fadeIn&&(g.css({opacity:0}),k.css({opacity:0}),w.css({opacity:0}))):V.options.fadeIn&&w.css({opacity:0}),t.append(m),V.eventmove(m),V.eventleave(m),V.options.position){case"inside":i-=(w.outerHeight()-w.height())/2,c-=(w.outerWidth()-w.width())/2;break;case"top":i-=w.outerHeight()-w.height(),c-=(w.outerWidth()-w.width())/2;break;case"left":i-=(w.outerHeight()-w.height())/2,c-=w.outerWidth()-w.width();break;case"bottom":c-=(w.outerWidth()-w.width())/2;break;case"right":i-=(w.outerHeight()-w.height())/2}w.css({top:i,left:c}),C.xon("load",function(o){if(x.remove(),!V.options.openOnSmall&&(C.width()<l||C.height()<r))return V.closezoom(),o.preventDefault(),!1;V.options.scroll&&V.eventscroll(m),"inside"!=V.options.position&&"fullscreen"!=V.options.position?(m.append(k),V.options.fadeIn?(g.fadeTo(300,V.options.tintOpacity),k.fadeTo(300,1),w.fadeTo(300,1)):(g.css({opacity:V.options.tintOpacity}),k.css({opacity:1}),w.css({opacity:1}))):V.options.fadeIn?w.fadeTo(300,1):w.css({opacity:1}),V.options.title&&""!=wo&&(V.options.fadeIn?N.fadeTo(300,1):N.css({opacity:1})),q=C.width(),E=C.height(),V.options.adaptive&&(a<B||p<G)&&(O.width(a),O.height(p),q*=a/B,E*=p/G,C.width(q),C.height(E)),ro=po=q,co=lo=E,R=q/E,X=q/l,Y=E/r;for(var t,e=["padding-","border-"],i=F=W=0;i<e.length;i++)t=parseFloat(k.css(e[i]+"top-width")),F+=t!=t?0:t,t=parseFloat(k.css(e[i]+"bottom-width")),F+=t!=t?0:t,t=parseFloat(k.css(e[i]+"left-width")),W+=t!=t?0:t,t=parseFloat(k.css(e[i]+"right-width")),W+=t!=t?0:t;F/=2,W/=2,uo=ho=no=u,vo=fo=ao=v,yo(u,v),V.options.smooth&&(L=!0,requestAnimFrame(Co)),V.eventclick(m)})},this.movezoom=function(o){u=o.pageX,v=o.pageY,mo&&(D=u,Z=v);var t=u-d,e=v-n;U&&(o.pageX-=2*(t-a/2),o.pageY-=2*(e-p/2)),(t<0||a<t||e<0||p<e)&&m.trigger("mouseleave"),V.options.smooth?(no=o.pageX,ao=o.pageY):(bo(),ko(o.pageX,o.pageY),k.css({top:H-F,left:S-W}),O.css({top:-H,left:-S}),zo(o.pageX,o.pageY,0,0))},this.eventdefault=function(){V.eventopen=function(o){o.xon("mouseenter",V.openzoom)},V.eventleave=function(o){o.xon("mouseleave",V.closezoom)},V.eventmove=function(o){o.xon("mousemove",V.movezoom)},V.eventscroll=function(o){o.xon("mousewheel DOMMouseScroll",V.xscroll)},V.eventclick=function(o){o.xon("click",function(o){s.trigger("click")})}},this.eventunbind=function(){s.xoff("mouseenter"),V.eventopen=function(o){},V.eventleave=function(o){},V.eventmove=function(o){},V.eventscroll=function(o){},V.eventclick=function(o){}},this.init=function(o){V.options=Ao.extend({},Ao.fn.xzoom.defaults,o),t=V.options.rootOutput?Ao("body"):s.parent(),P=V.options.position,U=V.options.lensReverse&&"inside"==V.options.position,V.options.smoothZoomMove<1&&(V.options.smoothZoomMove=1),V.options.smoothLensMove<1&&(V.options.smoothLensMove=1),V.options.smoothScale<1&&(V.options.smoothScale=1),V.options.adaptive&&Ao(window).xon("load",function(){B=s.width(),G=s.height(),V.adaptive(),Ao(window).resize(V.adaptive)}),V.eventdefault(),V.eventopen(s)},this.destroy=function(){V.eventunbind()},this.closezoom=function(){L=!1,V.options.fadeOut?(V.options.title&&""!=wo&&N.fadeOut(299),"inside"==V.options.position&&"fullscreen"==V.options.position||w.fadeOut(299),m.fadeOut(300,function(){Oo()})):Oo()},this.gallery=function(){for(var o=new Array,t=0,e=io;e<to.length;e++)o[t]=to[e],t++;for(e=0;e<io;e++)o[t]=to[e],t++;return{index:io,ogallery:to,cgallery:o}},this.xappend=function(e){var i=e.parent();function o(o){Oo(),o.preventDefault(),V.options.activeClass&&(_.removeClass(V.options.activeClass),(_=e).addClass(V.options.activeClass)),io=Ao(this).data("xindex"),V.options.fadeTrans&&((z=new Image).src=s.attr("src"),(b=Ao(z)).css({position:"absolute",top:s.offset().top,left:s.offset().left,width:s.width(),height:s.height()}),Ao(document.body).append(b),b.fadeOut(200,function(){b.remove()}));var t=i.attr("href"),o=e.attr("xpreview")||e.attr("src");wo=Mo(e),e.attr("title")&&s.attr("title",e.attr("title")),s.attr("xoriginal",t),s.removeAttr("style"),s.attr("src",o),V.options.adaptive&&(B=s.width(),G=s.height())}to[eo]=i.attr("href"),i.data("xindex",eo),0==eo&&V.options.activeClass&&(_=e).addClass(V.options.activeClass),0==eo&&V.options.title&&(wo=Mo(e)),eo++,V.options.hover&&i.xon("mouseenter",i,o),i.xon("click",i,o)},this.init(o)}Ao.fn.xon=Ao.fn.on||Ao.fn.bind,Ao.fn.xoff=Ao.fn.off||Ao.fn.bind,Ao.fn.xzoom=function(t){var e,i;if(this.selector){var o,s=this.selector.split(",");for(o in s)s[o]=Ao.trim(s[o]);this.each(function(o){if(1==s.length)if(0==o){if(void 0!==(e=Ao(this)).data("xzoom"))return e.data("xzoom");e.x=new n(e,t)}else void 0!==e.x&&(i=Ao(this),e.x.xappend(i));else if(Ao(this).is(s[0])&&0==o){if(void 0!==(e=Ao(this)).data("xzoom"))return e.data("xzoom");e.x=new n(e,t)}else void 0===e.x||Ao(this).is(s[0])||(i=Ao(this),e.x.xappend(i))})}else this.each(function(o){if(0==o){if(void 0!==(e=Ao(this)).data("xzoom"))return e.data("xzoom");e.x=new n(e,t)}else void 0!==e.x&&(i=Ao(this),e.x.xappend(i))});return void 0!==e&&(e.data("xzoom",e.x),Ao(e).trigger("xzoom_ready"),e.x)},Ao.fn.xzoom.defaults={position:"right",mposition:"inside",rootOutput:!0,Xoffset:0,Yoffset:0,fadeIn:!0,fadeTrans:!0,fadeOut:!1,smooth:!0,smoothZoomMove:3,smoothLensMove:1,smoothScale:6,defaultScale:0,scroll:!0,tint:!1,tintOpacity:.5,lens:!1,lensOpacity:.5,lensShape:"box",lensCollision:!0,lensReverse:!1,openOnSmall:!0,zoomWidth:"auto",zoomHeight:"auto",sourceClass:"xzoom-source",loadingClass:"xzoom-loading",lensClass:"xzoom-lens",zoomClass:"xzoom-preview",activeClass:"xactive",hover:!1,adaptive:!0,adaptiveReverse:!1,title:!1,titleClass:"xzoom-caption",bg:!1}}(jQuery);