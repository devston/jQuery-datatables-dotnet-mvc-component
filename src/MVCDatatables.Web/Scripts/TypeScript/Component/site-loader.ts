/*----------------------------------------------------------------------------*\
    
    Site loader scripts
    --------------------

    All js used for the site loader can be found here.
    
\*----------------------------------------------------------------------------*/

/**
 *  This module contains all the logic for the OBM site loader.
 * */
namespace SiteLoader {

    /**
     * Show the loader in the given container.
     * @param {string} selector The selector for the container.
     */
    export function show(selector: string) {
        if ($(`#site-loader-${selector.substr(1)}`).length === 0) {
            $(selector).append(`<div class="loader-backdrop load-in-container" id="site-loader-${selector.substr(1)}">
                                    <div class="loader-container">
                                        <div class="loader"></div>
                                    </div>
                                </div>`).addClass("overflow-hidden position-relative");
        }
    }

    /**
     * Remove the loader.
     * @param {string} selector The selector of the container.
     */
    export function remove(selector: string) {
        if ($(`#site-loader-${selector.substr(1)}`).length) {
            $(`#site-loader-${selector.substr(1)}`).remove();
        }

        // Remove .overflow-hidden regardless as the loader maybe lost by the parents html being replaced.
        $(selector).removeClass("overflow-hidden position-relative");
    }
};