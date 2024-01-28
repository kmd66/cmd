
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