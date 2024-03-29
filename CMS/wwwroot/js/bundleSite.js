﻿
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