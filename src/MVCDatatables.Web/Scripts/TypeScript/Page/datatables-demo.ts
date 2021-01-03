/*----------------------------------------------------------------------------*\
    
    Datatables demo
    ---------------

    All js related to the datatables demo page functionality is found here.
    
\*----------------------------------------------------------------------------*/


/**
 * A module containing all the logic for the datatables demo page.
 */
namespace DatatablesDemo {

    const basicTableSelector = "#basic-demo-table-container";

    /**
     * Initialise the module.
     */
    export function init() {
        initBasicDemoTable();
    }

    /**
     * Initialise the basic demo table.
     */
    function initBasicDemoTable() {
        const table = new TableService.DataTable(basicTableSelector);
        table.renderWithBackendUrl("/Home/DemoListTable/");
    }
}

// Initialise the datatables demo module on page load.
$(() => {
    DatatablesDemo.init();
});