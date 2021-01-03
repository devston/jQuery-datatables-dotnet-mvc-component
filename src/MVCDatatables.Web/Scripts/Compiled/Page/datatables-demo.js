/*----------------------------------------------------------------------------*\
    
    Datatables demo
    ---------------

    All js related to the datatables demo page functionality is found here.
    
\*----------------------------------------------------------------------------*/
/**
 * A module containing all the logic for the datatables demo page.
 */
var DatatablesDemo;
(function (DatatablesDemo) {
    var basicTableSelector = "#basic-demo-table-container";
    /**
     * Initialise the module.
     */
    function init() {
        initBasicDemoTable();
    }
    DatatablesDemo.init = init;
    /**
     * Initialise the basic demo table.
     */
    function initBasicDemoTable() {
        var table = new TableService.DataTable(basicTableSelector);
        table.renderWithBackendUrl("/Home/DemoListTable/");
    }
})(DatatablesDemo || (DatatablesDemo = {}));
// Initialise the datatables demo module on page load.
$(function () {
    DatatablesDemo.init();
});
//# sourceMappingURL=datatables-demo.js.map