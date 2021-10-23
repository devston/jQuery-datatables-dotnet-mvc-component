/*----------------------------------------------------------------------------*\
    
    Datatables demo
    ---------------

    All js related to the datatables demo page functionality is found here.
    
\*----------------------------------------------------------------------------*/

import $ from "jquery";
import { TableService } from "../Component/datatable-service";

/**
 * A module containing all the logic for the datatables demo page.
 */
namespace DatatablesDemo {

    const basicTableSelector = "#basic-demo-table-container";
    const childRowTableSelector = "#child-row-demo-table-container";
    const exportTableSelector = "#export-demo-table-container";

    /**
     * Initialise the module.
     */
    export function init() {
        initBasicDemoTable();
        initChildRowDemoTable();
        initExportDemoTable();
    }

    /**
     * Initialise the basic demo table.
     */
    function initBasicDemoTable() {
        const table = new TableService.DataTable(basicTableSelector);
        table.showProcessingLoader().renderWithBackendUrl("/Home/DemoListTable/");
    }

    /**
     * Initialise the child row demo table.
     */
    function initChildRowDemoTable() {
        const table = new TableService.DataTable(childRowTableSelector);
        table.showProcessingLoader().withChildRows().renderWithBackendUrl("/Home/ChildDemoListTable/");
    }

    /**
     * Initialise the export demo table.
     */
    function initExportDemoTable() {
        const table = new TableService.DataTable(exportTableSelector);
        table.showProcessingLoader().WithExcelExport({ title: "Test", messageBottom: "Test" }).WithPdfExport({ title: "Test", messageBottom: "Test" }).renderWithBackendUrl("/Home/DemoListTable/");
    }
}

// Initialise the datatables demo module on page load.
$(() => {
    DatatablesDemo.init();
});