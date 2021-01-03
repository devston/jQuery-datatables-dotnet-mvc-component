/*----------------------------------------------------------------------------*\
    
    Site table scripts
    ------------------

    All js used for the site tables can be found here.
    
\*----------------------------------------------------------------------------*/
/**
 *  This module contains all the logic required for the site table component.
 * */
var TableService;
(function (TableService) {
    var DataTable = /** @class */ (function () {
        function DataTable(_selector) {
            this._selector = _selector;
            this._allowExport = false;
            this._exportButtons = [];
            this._dom = "<'row align-items-center'<'col-12'f>>" +
                "<'row'<'col-12 mb-3'tr>>" +
                "<'row'<'col-12 col-md-6 mb-3 mb-md-0'<'d-flex align-items-center justify-content-center justify-content-md-start' li>><'col-12 col-md-6'p>>";
            this._$div = $(_selector);
        }
        /**
         * The table has excel exporting.
         * @param options The options (optional).
         */
        DataTable.prototype.WithExcelExport = function (options) {
            this._allowExport = true;
            var defaultOptions = {
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
        };
        /**
         * The table has pdf exporting.
         * @param options The options (optional).
         */
        DataTable.prototype.WithPdfExport = function (options) {
            this._allowExport = true;
            var defaultOptions = {
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
        };
        /**
         * Set the datatable as a hover table.
         */
        DataTable.prototype.asHoverTable = function () {
            this._hoverTable = true;
            return this;
        };
        /**
         * Set the datatable as a condensed table.
         */
        DataTable.prototype.asCondensedTable = function () {
            this._condensedTable = true;
            return this;
        };
        /**
         * Show the loader when the table is processing data.
         */
        DataTable.prototype.showProcessingLoader = function () {
            this._showLoader = true;
            return this;
        };
        /**
         * Adds custom contents to the last column in the table.
         * @param contents The contents to display in the last column.
         */
        DataTable.prototype.withCustomContents = function (contents) {
            this._hasCustomContents = true;
            this._customContents = contents;
            return this;
        };
        /**
         * Added a rendered row callback.
         * @param callback The callback operation after a row is rendered
         */
        DataTable.prototype.withRenderedRowCallback = function (callback) {
            this._renderedRowCallback = callback;
            return this;
        };
        /**
         * Enables child rows for the table.
         */
        DataTable.prototype.withChildRows = function () {
            this._hasChildRows = true;
            return this;
        };
        /**
         * Render the datatable with a backend service.
         * @param url The backend url.
         */
        DataTable.prototype.renderWithBackendUrl = function (url, params) {
            var thisClass = this;
            thisClass._backendUrl = url;
            if (params == undefined) {
                params = {};
            }
            $.get(thisClass._backendUrl, $.param(params, true), function (Model) {
                thisClass._tableConfig = Model;
                thisClass.createTable();
                return thisClass;
            });
        };
        /*
         * Removes the search bar on the datatable.
         * https://datatables.net/reference/option/searching
         */
        DataTable.prototype.hideSearch = function () {
            this._hideSearch = true;
            return this;
        };
        /**
         * Render the datatable.
         */
        DataTable.prototype.Render = function () {
            var dataTableOptions = this.getBasicConfig();
            // Initialise the datatable with our generic config.
            this._$div.DataTable(dataTableOptions);
        };
        /**
         * Get the basic datatables config.
         */
        DataTable.prototype.getBasicConfig = function () {
            var dataTableOptions = {
                "dom": this._dom,
                "language": {
                    "info": "_START_ - _END_ of _TOTAL_",
                    "infoEmpty": "0 - 0 of 0",
                    "infoFiltered": "(filtered from _MAX_)",
                    "lengthMenu": "Show _MENU_",
                    "search": "<i aria-hidden=\"true\" class=\"fal fa-search\"></i>",
                    "searchPlaceholder": "Search"
                },
                "columnDefs": [
                    {
                        targets: "no-sort",
                        orderable: false
                    }
                ],
                // @ts-ignore: Datatables types being stupid so ignore the error.
                "buttons": {},
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
                            "text": "Open",
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
        };
        /**
         * Create the table with a backend url.
         */
        DataTable.prototype.createTable = function () {
            var thisClass = this;
            var locale = "en-GB";
            var order = [];
            var columnsData = thisClass._tableConfig.columns;
            var colIndex = [];
            var childRowColumns = [];
            // Add the basic table mark up.
            thisClass._$div.append("<div class=\"datatable-container\"><table class=\"table dataTable w-100 datatable-basic\"><thead></thead><tbody></tbody></table></div>");
            for (var i = 0; i < columnsData.length; i++) {
                if (columnsData[i].isDefaultSortColumn === true) {
                    colIndex.push(i);
                }
            }
            if (colIndex.length > 0) {
                $.each(colIndex, function (_i, item) {
                    order.push([item, columnsData[item].sortDirection]);
                });
            }
            else {
                // Set the first visible column as the default sort algorithm.
                var firstVisibleColumnIndex_1 = undefined;
                $.each(columnsData, function (index, item) {
                    if (item.visible == true) {
                        firstVisibleColumnIndex_1 = index;
                        return false;
                    }
                });
                if (firstVisibleColumnIndex_1 != undefined) {
                    order.push([firstVisibleColumnIndex_1, "asc"]);
                }
            }
            var additionalAjaxParameters = [];
            // Set up any query string parameters.
            if (thisClass._tableConfig.QueryStringParameters != null) {
                thisClass._tableConfig.QueryStringParameters.forEach(function (parameter) {
                    additionalAjaxParameters[parameter.ParameterName] = parameter.Value;
                });
            }
            // Format any column data.
            columnsData.forEach(function (arrayItem) {
                arrayItem.render = function (data, _type, _full, meta) {
                    var colMetadata = meta.settings.oInit.columns[meta.col];
                    if (colMetadata != undefined) {
                        if (colMetadata.IsNumeric) {
                            var percSymbol = "";
                            if (colMetadata.IsPercent) {
                                percSymbol = " %";
                            }
                            return "<span>" + Intl.NumberFormat(locale).format(data) + percSymbol + "<span/>";
                        }
                        if (colMetadata.IsCurrency) {
                            return "<span>" + Intl.NumberFormat(locale, { style: "currency", currency: colMetadata.CurrencyToShow }).format(data) + "<span/>";
                        }
                        if (colMetadata.IsLabelColumn === true) {
                            var badgeClass = colMetadata.LabelClassMapping["" + data] !== undefined
                                ? colMetadata.LabelClassMapping["" + data]
                                : "badge-primary";
                            return "<span class=\"badge badge-pill " + badgeClass + "\">" + data + "</span>";
                        }
                        if (colMetadata.IsFormattedDecimal) {
                            if (colMetadata.Symbol) {
                                if (colMetadata.PrependSymbol) {
                                    return "<span>" + colMetadata.Symbol + parseFloat(data) + "</span>";
                                }
                                else {
                                    return "<span>" + parseFloat(data) + colMetadata.Symbol + "</span>";
                                }
                            }
                            return "<span>" + parseFloat(data) + "</span>";
                        }
                        if (colMetadata.type != undefined) {
                            if (colMetadata.type == "date") {
                                if (typeof data === "undefined" || !data) {
                                    return "";
                                }
                                var date = moment(data);
                                if (date.year() === 1) {
                                    return "";
                                }
                                if (colMetadata.IsDateHumanised == true) {
                                    return "<span title=\"" + date.format("MMMM Do YYYY, h:mm:ss a") + "\">" + date.fromNow() + "<span/>";
                                }
                                else {
                                    if (colMetadata.MomentDataFormat != "") {
                                        return "<span>" + date.format(colMetadata.MomentDataFormat) + "<span/>";
                                    }
                                    else {
                                        moment.locale(locale);
                                        return "<span>" + date.format("L") + "<span/>";
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
                };
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
                    "defaultContent": '<button class="btn btn-transparent table-child-accordion-btn" type="button">Open</button>',
                    "orderable": false,
                    "searchable": false
                });
            }
            var dtBasicSettings = {};
            // Add the extra parameters to the table config.
            $.extend(dtBasicSettings, thisClass.getBasicConfig(), {
                serverSide: true,
                order: order,
                ajax: {
                    url: thisClass._tableConfig.ServiceUrl,
                    type: thisClass._tableConfig.BackendUrlRequestType,
                    data: function (d) {
                        for (var item in additionalAjaxParameters) {
                            d[item] = additionalAjaxParameters[item];
                        }
                        return d;
                    },
                    error: function (_xhr, _error, _thrown) {
                        SiteAlert.show("danger", "An error occurred when retrieving data", true);
                    }
                },
                "initComplete": function (_a, _b) {
                    $(".dataTables_filter input")
                        .off() // Unbind previous default bindings
                        .on("input", (delay(function (e) {
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
                    thisClass._$div.trigger("dt-initialised");
                },
                columns: columnsData,
                "rowCallback": this._renderedRowCallback
            });
            var $table = thisClass._$div.find(".table").on("processing.dt", function (_e, settings, processing) {
                var tableId = "#" + $(this).attr("id") + "_wrapper";
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
                var rowData = $table.row($(this).parents("tr")).data();
                $(this).trigger("dt-button-click", rowData);
            });
            // Bind open / close event if there are child rows.
            if (thisClass._hasChildRows) {
                // @ts-ignore: Datatables types being stupid so ignore the error.
                $table.on("click", "tbody .table-child-accordion-btn", function () {
                    // Get the row.
                    var $thisBtn = $(this);
                    var $tr = $thisBtn.closest("tr");
                    var row = $table.row($tr);
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
        };
        DataTable.prototype.checkMetaData = function () {
            var thisClass = this;
            // Add the table-hover class if the user has set the table up as a 'hover table'.
            if (thisClass._hoverTable == true) {
                thisClass._$div.find(".table").addClass("table-hover");
            }
            // Add the table-sm class if the user has set the table up as a 'condensed table'.
            if (thisClass._condensedTable == true) {
                thisClass._$div.find(".table").addClass("table-sm");
            }
        };
        /**
         * Format data into a child row.
         * @param data The row data.
         */
        DataTable.prototype.formatChildRow = function (childRowColumns, data) {
            var rowContent = "<div class=\"table-child-row-content\">";
            // Add all the child row data.
            childRowColumns.forEach(function (arrayItem) {
                // Check if there is data.
                if (data[arrayItem.data]) {
                    rowContent += "<div class=\"table-child-row-column-container\">";
                    rowContent += "<h4 class=\"table-child-row-column-header\">" + arrayItem.title + "</h4>";
                    rowContent += "<p class=\"table-child-row-column-data\">" + data[arrayItem.data] + "</p>";
                    rowContent += "</div>";
                }
            });
            rowContent += "</div>";
            return rowContent;
        };
        return DataTable;
    }());
    TableService.DataTable = DataTable;
})(TableService || (TableService = {}));
//# sourceMappingURL=datatable-service.js.map