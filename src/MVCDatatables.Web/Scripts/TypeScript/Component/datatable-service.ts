/*----------------------------------------------------------------------------*\
    
    Site table scripts
    ------------------

    All js used for the site tables can be found here.
    
\*----------------------------------------------------------------------------*/

/**
 *  This module contains all the logic required for the site table component.
 * */
namespace TableService {
    export class DataTable {
        private _dom: string;
        private _buttonSettings: any;
        private _allowExport: boolean = false;
        private _exportButtons: Array<any> = [];
        private _backendUrl: string;
        private _tableConfig: any;
        private _$div: JQuery<HTMLElement>;
        private _hoverTable: boolean;
        private _condensedTable: boolean;
        private _showLoader: boolean;
        private _hasCustomContents: boolean;
        private _customContents: string;
        private _renderedRowCallback: Function;
        private _hasChildRows: boolean;
        private _hideSearch: boolean;

        constructor(private _selector: string) {
            this._dom = "<'row align-items-center'<'col-12'f>>" +
                "<'row'<'col-12 mb-3'tr>>" +
                "<'row'<'col-12 col-md-6 mb-3 mb-md-0'<'d-flex align-items-center justify-content-center justify-content-md-start' li>><'col-12 col-md-6'p>>";
            this._$div = $(_selector);
        }

        /**
         * The table has excel exporting.
         * @param options The options (optional).
         */
        public WithExcelExport(options: any) {
            this._allowExport = true;
            const defaultOptions = {
                "extend": "excelHtml5",
                "text": "Export to Excel",
                "filename": "Export Excel",
                "exportOptions": {
                    "modifier": {
                        "page": "all"
                    }
                }
            };

            Object.assign(defaultOptions, options);

            this._exportButtons.push(defaultOptions);
            return this;
        }

        /**
         * The table has pdf exporting.
         * @param options The options (optional).
         */
        public WithPdfExport(options: any) {
            this._allowExport = true;

            const defaultOptions = {
                "extend": "pdfHtml5",
                "text": "Export to PDF",
                "filename": "Export PDF",
                "exportOptions": {
                    "modifier": {
                        "page": "all"
                    }
                }
            };

            Object.assign(defaultOptions, options);

            this._exportButtons.push(defaultOptions);
            return this;
        }

        /**
         * Set the datatable as a hover table.
         */
        public asHoverTable() {
            this._hoverTable = true;
            return this;
        }

        /**
         * Set the datatable as a condensed table.
         */
        public asCondensedTable() {
            this._condensedTable = true;
            return this;
        }

        /**
         * Show the loader when the table is processing data.
         */
        public showProcessingLoader() {
            this._showLoader = true;
            return this;
        }

        /**
         * Adds custom contents to the last column in the table.
         * @param contents The contents to display in the last column.
         */
        public withCustomContents(contents: string) {
            this._hasCustomContents = true;
            this._customContents = contents;
            return this;
        }

        /**
         * Added a rendered row callback.
         * @param callback The callback operation after a row is rendered
         */
        public withRenderedRowCallback(callback: (row: any, data: any, dataIndex: number, cells: Array<any>) => void) {
            this._renderedRowCallback = callback;
            return this;
        }

        /**
         * Enables child rows for the table.
         */
        public withChildRows() {
            this._hasChildRows = true;
            return this;
        }

        /**
         * Render the datatable with a backend service.
         * @param url The backend url.
         */
        public renderWithBackendUrl(url: string, params?: any) {
            const thisClass = this;
            thisClass._backendUrl = url;

            if (params == undefined) {
                params = {};
            }

            $.get(thisClass._backendUrl, $.param(params, true), function (Model: any) {
                thisClass._tableConfig = Model;
                thisClass.createTable();
                return thisClass;
            });
        }

        /*
         * Removes the search bar on the datatable.
         * https://datatables.net/reference/option/searching
         */
        public hideSearch() {
            this._hideSearch = true;

            return this;
        }

        /**
         * Render the datatable.
         */
        public Render() {
            const dataTableOptions = this.getBasicConfig();

            // Initialise the datatable with our generic config.
            this._$div.DataTable(dataTableOptions);
        }

        /**
         * Get the basic datatables config.
         */
        private getBasicConfig() {
            let dataTableOptions = {
                "dom": this._dom,
                "language": {
                    "info": "_START_ - _END_ of _TOTAL_",
                    "infoEmpty": "0 - 0 of 0",
                    "infoFiltered": "(filtered from _MAX_)",
                    "lengthMenu": "Show _MENU_",
                    "search": `<i aria-hidden="true" class="fal fa-search"></i>`,
                    "searchPlaceholder": "Search",
                    "paginate": {
                        "first": "<i aria-hidden=\"true\" class=\"fas fa-angle-double-left\"></i>",
                        "previous": "<i aria-hidden=\"true\" class=\"fas fa-angle-left\"></i>",
                        "next": "<i aria-hidden=\"true\" class=\"fas fa-angle-right\"></i>",
                        "last": "<i aria-hidden=\"true\" class=\"fas fa-angle-double-right\"></i>"
                    }
                },
                "columnDefs": [
                    {
                        targets: "no-sort",
                        orderable: false
                    }],

                // @ts-ignore: Datatables types being stupid so ignore the error.
                "buttons": {} as any,
                "pagingType": "full",
                "searching": true
            };

            if (this._allowExport) {
                dataTableOptions.dom = "<'row align-items-center'<'col-6'f><'col-6 text-right'B>>" +
                    "<'row'<'col-12 mb-3'tr>>" +
                    "<'row'<'col-12 col-md-6 mb-3 mb-md-0'<'d-flex align-items-center justify-content-center justify-content-md-start' li>><'col-12 col-md-6'p>>";

                dataTableOptions.buttons = {
                    "dom": {
                        "collection": {
                            "className": "dropdown-menu dropdown-menu-right"
                        }
                    },
                    "buttons": [
                        {
                            "extend": "collection",
                            "className": "btn btn-transparent",
                            "text": "<i aria-hidden=\"true\" class=\"fal fa-ellipsis-v\"></i>",
                            "titleAttr": "Export data",
                            "buttons": this._exportButtons
                        }
                    ]
                };
            }

            if (this._hideSearch) {
                dataTableOptions.searching = false;
            }

            return dataTableOptions;
        }

        /**
         * Create the table with a backend url.
         */
        private createTable() {
            const thisClass = this;
            const locale = "en-GB";
            let order: Array<any> = [];
            let columnsData = thisClass._tableConfig.columns;
            let colIndex: Array<number> = [];
            let childRowColumns: Array<any> = [];

            // Add the basic table mark up.
            thisClass._$div.append("<div class=\"datatable-container\"><table class=\"table dataTable w-100 datatable-basic\"><thead></thead><tbody></tbody></table></div>");

            for (let i = 0; i < columnsData.length; i++) {
                if (columnsData[i].isDefaultSortColumn === true) {
                    colIndex.push(i);
                }
            }

            if (colIndex.length > 0) {
                $.each(colIndex, function (_i: any, item: any) {
                    order.push([item, columnsData[item].sortDirection]);
                });
            }

            else {
                // Set the first visible column as the default sort algorithm.
                let firstVisibleColumnIndex: number | undefined = undefined;
                $.each(columnsData, function (index: number, item: any): boolean | void {
                    if (item.visible == true) {
                        firstVisibleColumnIndex = index;
                        return false;
                    }
                });

                if (firstVisibleColumnIndex != undefined) {
                    order.push([firstVisibleColumnIndex, "asc"]);
                }
            }

            let additionalAjaxParameters: any[] = [];

            // Set up any query string parameters.
            if (thisClass._tableConfig.QueryStringParameters != null) {
                thisClass._tableConfig.QueryStringParameters.forEach(function (parameter: any) {
                    additionalAjaxParameters[parameter.ParameterName] = parameter.Value;
                });
            }

            // Format any column data.
            columnsData.forEach(function (arrayItem: any) {
                arrayItem.render = function (data: any, _type: any, _full: any, meta: any) {
                    let colMetadata = meta.settings.oInit.columns[meta.col];
                    if (colMetadata != undefined) {
                        if (colMetadata.IsNumeric) {
                            let percSymbol = "";

                            if (colMetadata.IsPercent) {
                                percSymbol = " %";
                            }

                            return `<span>${Intl.NumberFormat(locale).format(data)}${percSymbol}<span/>`;
                        }

                        if (colMetadata.IsCurrency) {
                            return `<span>${Intl.NumberFormat(locale, { style: "currency", currency: colMetadata.CurrencyToShow }).format(data)}<span/>`;
                        }

                        if (colMetadata.IsLabelColumn === true) {
                            const badgeClass = colMetadata.LabelClassMapping[`${data}`] !== undefined
                                ? colMetadata.LabelClassMapping[`${data}`]
                                : "badge-primary";

                            return `<span class="badge badge-pill ${badgeClass}">${data}</span>`;
                        }

                        if (colMetadata.IsFormattedDecimal) {
                            if (colMetadata.Symbol) {
                                if (colMetadata.PrependSymbol) {
                                    return `<span>${colMetadata.Symbol}${parseFloat(data)}</span>`;
                                } else {
                                    return `<span>${parseFloat(data)}${colMetadata.Symbol}</span>`;
                                }
                            }

                            return `<span>${parseFloat(data)}</span>`;
                        }

                        if (colMetadata.type != undefined) {
                            if (colMetadata.type == "date") {
                                if (typeof data === "undefined" || !data) {
                                    return "";
                                }

                                let date = moment(data);

                                if (date.year() === 1) {
                                    return "";
                                }

                                if (colMetadata.IsDateHumanised == true) {
                                    return `<span title="${date.format("MMMM Do YYYY, h:mm:ss a")}">${date.fromNow()}<span/>`;
                                }

                                else {
                                    if (colMetadata.MomentDataFormat != "") {
                                        return `<span>${date.format(colMetadata.MomentDataFormat)}<span/>`;
                                    }
                                    else {
                                        moment.locale(locale);
                                        return `<span>${date.format("L")}<span/>`;
                                    }
                                }
                            }

                            if (colMetadata.type == "bool") {
                                if (typeof data === "undefined" || !data) {
                                    if (colMetadata.IsYesNo) {
                                        return "No";
                                    }

                                    return "";
                                }

                                if (colMetadata.IsYesNo) {
                                    return data == true ? "Yes" : "No";
                                }
                            }
                        }
                    }

                    return data;
                }

                // Check if the item should be in a child row.
                if (arrayItem.IsInChildRow) {
                    // Push it to the child row array.
                    childRowColumns.push(arrayItem);
                }
            });

            thisClass.checkMetaData();

            // Add any custom content to the last column.
            if (thisClass._hasCustomContents) {
                columnsData.push({
                    "className": "table-custom-options text-center",
                    "data": null,
                    "defaultContent": thisClass._customContents,
                    "orderable": false,
                    "searchable": false
                });
            }

            // Add an open / close button as the first column.
            if (thisClass._hasChildRows) {
                // Bit of a hack but set a data name so MVC will serialise the array as this is the first item.
                columnsData.unshift({
                    "className": "table-child-accordion",
                    "data": "ChildRowAccordion",
                    "defaultContent": '<button class="btn btn-transparent table-child-accordion-btn" type="button"><span class="fal fa-chevron-down table-child-accordion-icon"></span></button>',
                    "orderable": false,
                    "searchable": false
                });
            }

            let dtBasicSettings = {}

            // Add the extra parameters to the table config.
            $.extend(dtBasicSettings, thisClass.getBasicConfig(), {
                serverSide: true,
                order: order,
                ajax: {
                    url: thisClass._tableConfig.ServiceUrl,
                    type: thisClass._tableConfig.BackendUrlRequestType,
                    data: function (d: any) {
                        for (var item in additionalAjaxParameters) {
                            d[item] = additionalAjaxParameters[item];
                        }

                        return d;
                    },
                    success: function (response: any) {
                        console.log(response);
                    },
                    error: function (_xhr: any, _error: any, _thrown: any) {
                        SiteAlert.show("danger", "An error occurred when retrieving data", true);
                    }
                },
                "initComplete": function (_a: any, _b: any) {
                    $(".dataTables_filter input")
                        .off() // Unbind previous default bindings
                        .on("input", (delay(function (e) { // Bind our desired behavior
                            $(this).closest("div[id$='_wrapper']").find(".table").DataTable().search($(this).val().toString()).draw();
                            return;
                        }, 1000))); // Set delay in milliseconds

                    function delay(callback, ms) {
                        var timer = 0;
                        return function () {
                            var context = this, args = arguments;
                            clearTimeout(timer);
                            timer = setTimeout(function () {
                                callback.apply(context, args);
                            }, ms || 0);
                        };
                    }

                    console.log("complete")

                    thisClass._$div.trigger("dt-initialised");
                },
                columns: columnsData,
                "rowCallback": this._renderedRowCallback
            });

            const $table = thisClass._$div.find(".table").on("processing.dt", function (this: any, _e: any, settings: any, processing: boolean) {
                const tableId = `#${$(this).attr("id")}_wrapper`;

                // When data table is fetching data, show the site loader instead of the default datatable processing indicator.
                if (thisClass._showLoader) {
                    if (processing == true) {
                        SiteLoader.show(tableId);
                    }
                    else {
                        SiteLoader.remove(tableId);
                    }
                }
            }).DataTable(dtBasicSettings);

            // Bind custom refresh event.
            thisClass._$div.find(".table").on("dt-refresh", function () {
                $table.ajax.reload();
            });

            // Bind a custom event to any buttons.
            // @ts-ignore: Datatables types being stupid so ignore the error.
            $table.on("click", "tbody button", function (e) {
                e.preventDefault();
                const rowData = $table.row($(this).parents("tr")).data();
                $(this).trigger("dt-button-click", rowData);
            });

            // Bind open / close event if there are child rows.
            if (thisClass._hasChildRows) {
                // @ts-ignore: Datatables types being stupid so ignore the error.
                $table.on("click", "tbody .table-child-accordion-btn", function () {
                    // Get the row.
                    const $thisBtn = $(this);
                    const $tr = $thisBtn.closest("tr");
                    const row = $table.row($tr);

                    if (row.child.isShown()) {
                        $thisBtn.removeClass("table-child-accordion-icon-rotate");

                        // Bind event to slide the row.
                        $(".table-child-row-content", row.child()).slideUp(function () {
                            // This row is already open - close it.
                            row.child.hide();
                            $tr.removeClass("table-child-row-shown");
                        });
                    }
                    else {
                        // Open this row.
                        row.child(thisClass.formatChildRow(childRowColumns, row.data())).show();
                        row.child().addClass("table-child-row");
                        $tr.addClass("table-child-row-shown");
                        $thisBtn.addClass("table-child-accordion-icon-rotate");
                        $(".table-child-row-content", row.child()).slideDown();
                    }
                });
            }

            return thisClass;
        }

        private checkMetaData() {
            const thisClass = this;

            // Add the table-hover class if the user has set the table up as a 'hover table'.
            if (thisClass._hoverTable == true) {
                thisClass._$div.find(".table").addClass("table-hover");
            }

            // Add the table-sm class if the user has set the table up as a 'condensed table'.
            if (thisClass._condensedTable == true) {
                thisClass._$div.find(".table").addClass("table-sm");
            }
        }

        /**
         * Format data into a child row.
         * @param data The row data.
         */
        private formatChildRow(childRowColumns: Array<any>, data: any) {
            let rowContent = "<div class=\"table-child-row-content\">";

            // Add all the child row data.
            childRowColumns.forEach(function (arrayItem: any) {
                // Check if there is data.
                if (data[arrayItem.data]) {
                    rowContent += "<div class=\"table-child-row-column-container\">";
                    rowContent += `<h4 class="table-child-row-column-header">${arrayItem.title}</h4>`;
                    rowContent += `<p class="table-child-row-column-data">${data[arrayItem.data]}</p>`;
                    rowContent += "</div>";
                }
            });

            rowContent += "</div>"
            return rowContent;
        }
    }
}