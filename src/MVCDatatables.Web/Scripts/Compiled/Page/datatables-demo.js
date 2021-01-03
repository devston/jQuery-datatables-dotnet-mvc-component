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
    var childRowTableSelector = "#child-row-demo-table-container";
    var exportTableSelector = "#export-demo-table-container";
    /**
     * Initialise the module.
     */
    function init() {
        initBasicDemoTable();
        initChildRowDemoTable();
        initExportDemoTable();
    }
    DatatablesDemo.init = init;
    /**
     * Initialise the basic demo table.
     */
    function initBasicDemoTable() {
        var table = new TableService.DataTable(basicTableSelector);
        table.showProcessingLoader().renderWithBackendUrl("/Home/DemoListTable/");
    }
    /**
     * Initialise the child row demo table.
     */
    function initChildRowDemoTable() {
        var table = new TableService.DataTable(childRowTableSelector);
        table.showProcessingLoader().withChildRows().renderWithBackendUrl("/Home/ChildDemoListTable/");
    }
    /**
     * Initialise the export demo table.
     */
    function initExportDemoTable() {
        var table = new TableService.DataTable(exportTableSelector);
        table.showProcessingLoader().WithExcelExport({ title: "Test", messageBottom: "Test" }).WithPdfExport({ title: "Test", messageBottom: "Test" }).renderWithBackendUrl("/Home/DemoListTable/");
    }
})(DatatablesDemo || (DatatablesDemo = {}));
// Initialise the datatables demo module on page load.
$(function () {
    DatatablesDemo.init();
});
//# sourceMappingURL=datatables-demo.js.map