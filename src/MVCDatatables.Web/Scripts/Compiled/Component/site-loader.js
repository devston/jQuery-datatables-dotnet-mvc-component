/*----------------------------------------------------------------------------*\
    
    Site loader scripts
    --------------------

    All js used for the site loader can be found here.
    
\*----------------------------------------------------------------------------*/
/**
 *  This module contains all the logic for the OBM site loader.
 * */
var SiteLoader;
(function (SiteLoader) {
    /**
     * Show the loader in the given container.
     * @param {string} selector The selector for the container.
     */
    function show(selector) {
        if ($("#site-loader-" + selector.substr(1)).length === 0) {
            $(selector).append("<div class=\"loader-backdrop load-in-container\" id=\"site-loader-" + selector.substr(1) + "\">\n                                    <div class=\"loader-container\">\n                                        <div class=\"loader\"></div>\n                                    </div>\n                                </div>").addClass("overflow-hidden position-relative");
        }
    }
    SiteLoader.show = show;
    /**
     * Remove the loader.
     * @param {string} selector The selector of the container.
     */
    function remove(selector) {
        if ($("#site-loader-" + selector.substr(1)).length) {
            $("#site-loader-" + selector.substr(1)).remove();
        }
        // Remove .overflow-hidden regardless as the loader maybe lost by the parents html being replaced.
        $(selector).removeClass("overflow-hidden position-relative");
    }
    SiteLoader.remove = remove;
})(SiteLoader || (SiteLoader = {}));
;
//# sourceMappingURL=site-loader.js.map