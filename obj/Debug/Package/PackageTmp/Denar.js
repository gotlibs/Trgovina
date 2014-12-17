var menuActive = false;

function bodyClick(event) {
    var srcElement = event.srcElement ? event.srcElement : event.target; //ie7, ie8 nimata event.target
    if (jQuery(".submenu:visible").length) {
        if (!jQuery(srcElement).is(".menuItem")) {
            jQuery(".submenu").hide();
            jQuery(".menuItem").removeClass("menuItemSelected");
            menuActive = false;
        }
    }

    if (jQuery(".dropContainer:visible").length) {
        if (jQuery(srcElement).parents(".multiselectDropdown").length == 0) {
            jQuery(".dropContainer").hide();
        }
    }
}

function minHeadClick(el) {
    var head = jQuery(el);
    var body = head.next(".formBody");

    if (jQuery(el).is(".formHeadOpened")) {
        //console.log("this is opened");
        head.removeClass("formHeadOpened");
        body.addClass("formClosed");
    } else {
        //console.log("this is closed");
        head.addClass("formHeadOpened");
        body.removeClass("formClosed");
    }
}

jQuery(document).ready(function () {
    jQuery(".menuItem").click(function (event) {//event je tu samo za debug
        if (menuActive) {
            jQuery(".menuItem").removeClass("menuItemSelected");
            jQuery(this).next().hide();
            jQuery(this).removeClass("menuItemSelected");
            menuActive = false;
        } else {
            jQuery(this).next().show();
            jQuery(this).addClass("menuItemSelected");
            menuActive = true;
        }
    });

    jQuery(".menuItem").mouseenter(function () {
        if (menuActive) {
            jQuery(".submenu").hide();
            jQuery(".menuItem").removeClass("menuItemSelected");
            jQuery(this).addClass("menuItemSelected");
            jQuery(this).next().show();

        }
    });

    jQuery(document).keyup(function (event) {
        //ce zelimo prepreciti esc na input elementih lahko dodamo preverjanje za srcElement
        //var srcElement = event.srcElement ? event.srcElement : event.target;
        //if (!jQuery(srcElement).is("input, select, textarea")) {...

        if (event.which == 27) {//ESC
            var el = jQuery(".popupMain").parent();
            if (el.length) {
                if (el.is(":visible")) {
                    el.hide();
                    jQuery(".modalBckg").hide();
                }
            }
        }
    });
});