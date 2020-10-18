using Mvc.JQuery.DataTables;
using System.Web.Mvc;

namespace MVCDatatables.Web.Common.DataTables
{
    /// <summary>
    /// The datatables model binder.
    /// </summary>
    public class DataTablesModelBinder : IModelBinder
    {
        /// <inheritdoc/>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            IGenericValueProvider valueProvider = new MvcValueProvider(bindingContext.ValueProvider);
            return Bind(valueProvider);
        }

        /// <summary>
        /// Bind the datatables model.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        /// <returns>The bound model.</returns>
        private object Bind(IGenericValueProvider valueProvider)
        {
            int columns = valueProvider.GetValue<int>("iColumns");

            if (columns == 0)
            {
                return BindV10Model(valueProvider);
            }
            else
            {
                return BindLegacyModel(valueProvider, columns);
            }
        }

        /// <summary>
        /// Bind a datatables +v10 model.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        /// <returns>The datatables param.</returns>
        private object BindV10Model(IGenericValueProvider valueProvider)
        {
            var obj = new DataTablesParam();
            obj.iDisplayStart = valueProvider.GetValue<int>("start");
            obj.iDisplayLength = valueProvider.GetValue<int>("length");
            obj.sEcho = valueProvider.GetValue<int>("draw");

            if (valueProvider.GetValue<string>("search[value]") == null)
            {
                obj.sSearch = valueProvider.GetValue<string>("search.value");
                obj.bEscapeRegex = valueProvider.GetValue<bool>("search.regex");
                int colIdx = 0;

                while (true)
                {
                    string colPrefix = string.Format("columns[{0}]", colIdx);
                    string colName = valueProvider.GetValue<string>(colPrefix + ".data");

                    if (string.IsNullOrWhiteSpace(colName))
                    {
                        break;
                    }

                    obj.sColumnNames.Add(colName);
                    obj.bSortable.Add(valueProvider.GetValue<bool>(colPrefix + ".orderable"));
                    obj.bSearchable.Add(valueProvider.GetValue<bool>(colPrefix + ".searchable"));
                    obj.sSearchValues.Add(valueProvider.GetValue<string>(colPrefix + ".search.value"));
                    obj.bEscapeRegexColumns.Add(valueProvider.GetValue<bool>(colPrefix + ".searchable.regex"));
                    colIdx++;
                }

                obj.iColumns = colIdx;
                colIdx = 0;

                while (true)
                {
                    string colPrefix = string.Format("order[{0}]", colIdx);
                    int? orderColumn = valueProvider.GetValue<int?>(colPrefix + ".column");

                    if (orderColumn.HasValue)
                    {
                        obj.iSortCol.Add(orderColumn.Value);
                        obj.sSortDir.Add(valueProvider.GetValue<string>(colPrefix + ".dir"));
                        colIdx++;
                    }
                    else
                    {
                        break;
                    }
                }

                obj.iSortingCols = colIdx;
            }
            else
            {
                obj.sSearch = valueProvider.GetValue<string>("search[value]");
                obj.bEscapeRegex = valueProvider.GetValue<bool>("search[regex]");
                int colIdx = 0;

                while (true)
                {
                    string colPrefix = string.Format("columns[{0}]", colIdx);
                    string colName = valueProvider.GetValue<string>(colPrefix + "[data]");

                    if (string.IsNullOrWhiteSpace(colName))
                    {
                        break;
                    }

                    obj.sColumnNames.Add(colName);
                    obj.bSortable.Add(valueProvider.GetValue<bool>(colPrefix + "[orderable]"));
                    obj.bSearchable.Add(valueProvider.GetValue<bool>(colPrefix + "[searchable]"));
                    obj.sSearchValues.Add(valueProvider.GetValue<string>(colPrefix + "[search][value]"));
                    obj.bEscapeRegexColumns.Add(valueProvider.GetValue<bool>(colPrefix + "[searchable][regex]"));
                    colIdx++;
                }

                obj.iColumns = colIdx;
                colIdx = 0;

                while (true)
                {
                    string colPrefix = string.Format("order[{0}]", colIdx);
                    int? orderColumn = valueProvider.GetValue<int?>(colPrefix + "[column]");

                    if (orderColumn.HasValue)
                    {
                        obj.iSortCol.Add(orderColumn.Value);
                        obj.sSortDir.Add(valueProvider.GetValue<string>(colPrefix + "[dir]"));
                        colIdx++;
                    }
                    else
                    {
                        break;
                    }
                }

                obj.iSortingCols = colIdx;
            }

            return obj;
        }

        /// <summary>
        /// Bind a datatables model from datatables lower than v10.
        /// </summary>
        /// <param name="valueProvider">The value provider.</param>
        /// <param name="columns">The datatable columns.</param>
        /// <returns>The datatables param.</returns>
        private DataTablesParam BindLegacyModel(IGenericValueProvider valueProvider, int columns)
        {
            var obj = new DataTablesParam(columns);

            obj.iDisplayStart = valueProvider.GetValue<int>("iDisplayStart");
            obj.iDisplayLength = valueProvider.GetValue<int>("iDisplayLength");
            obj.sSearch = valueProvider.GetValue<string>("sSearch");
            obj.bEscapeRegex = valueProvider.GetValue<bool>("bEscapeRegex");
            obj.iSortingCols = valueProvider.GetValue<int>("iSortingCols");
            obj.sEcho = valueProvider.GetValue<int>("sEcho");

            for (int i = 0; i < obj.iColumns; i++)
            {
                obj.bSortable.Add(valueProvider.GetValue<bool>("bSortable_" + i));
                obj.bSearchable.Add(valueProvider.GetValue<bool>("bSearchable_" + i));
                obj.sSearchValues.Add(valueProvider.GetValue<string>("sSearch_" + i));
                obj.bEscapeRegexColumns.Add(valueProvider.GetValue<bool>("bEscapeRegex_" + i));
                obj.iSortCol.Add(valueProvider.GetValue<int>("iSortCol_" + i));
                obj.sSortDir.Add(valueProvider.GetValue<string>("sSortDir_" + i));
            }

            return obj;
        }
    }
}