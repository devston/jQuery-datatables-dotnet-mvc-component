using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCDatatables.Presentation.Models.Datatables
{
    /// <summary>
    /// The datatable view model.
    /// </summary>
    public class DataTableViewModel
    {
        /// <summary>
        /// The sort data list.
        /// </summary>
        private List<string> _sortDataList;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableViewModel"/> class.
        /// </summary>
        /// <param name="entityDescription">The entity description, this will appear in the footer of the table when filtering data.</param>
        public DataTableViewModel(string entityDescription)
        {
            columns = new List<DataTableColumnDefinitionViewModel>();
            EntityDescription = entityDescription;
            BackendUrlRequestType = "GET";
            AdditionalData = new Dictionary<string, string>();
            QueryStringParameters = new List<QueryStringParameter>();
            _sortDataList = new List<string>();
        }

        /// <summary>
        /// Gets or sets the entity description.
        /// </summary>
        public string EntityDescription { get; set; }

        /// <summary>
        /// Gets or sets the backend service url.
        /// </summary>
        public HtmlString BackendServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the service url.
        /// </summary>
        public string ServiceUrl { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the columns.
        /// </summary>
        public List<DataTableColumnDefinitionViewModel> columns { get; set; }

        /// <summary>
        /// Gets a value indicating whether to show the search box.
        /// </summary>
        public bool ShowSearchBox
        {
            get
            {
                return columns.Where(a => a.searchable == true).Any();
            }
        }

        /// <summary>
        /// Gets or sets the parameters needed for the request.
        /// </summary>
        public List<QueryStringParameter> QueryStringParameters { get; set; }

        /// <summary>
        /// Gets or sets any additional data.
        /// </summary>
        public Dictionary<string, string> AdditionalData { get; set; }

        /// <summary>
        /// Gets the backend url request type.
        /// </summary>
        public string BackendUrlRequestType { get; private set; }

        /// <summary>
        /// Gets the sort data.
        /// </summary>
        public string[] SortData
        {
            get
            {
                return _sortDataList.ToArray();
            }
        }

        /// <summary>
        /// Add a column to the table.
        /// </summary>
        /// <param name="fieldName">The data field name.</param>
        /// <returns>The datatable column view model.</returns>
        public DataTableColumnDefinitionViewModel AddColumn(string fieldName)
        {
            return AddColumn(fieldName, fieldName);
        }

        /// <summary>
        /// This method will set multiple column as the order by clause. The column which appear first will be primary, second will be secondary and so on.
        /// NOTE : This will reset any sorting set by passing the defaultSortColumn parameter of IsSortable method.
        /// </summary>
        /// <param name="columnsNames">The column names.</param>
        public void SetMultipleColumnSortAsDefault(List<string> columnsNames)
        {
            if (columns.Where(a => columnsNames.Contains(a.data)).Count() == columnsNames.Count())
            {
                _sortDataList = columnsNames;
            }
            else
            {
                throw new ArgumentException($"Incorrect column in {nameof(columnsNames)}");
            }
        }

        /// <summary>
        /// Add a column to the table.
        /// </summary>
        /// <param name="fieldName">The field name.</param>
        /// <param name="fieldDescription">The field description.</param>
        /// <returns>The datatable column view model.</returns>
        public DataTableColumnDefinitionViewModel AddColumn(string fieldName, string fieldDescription)
        {
            var col = new DataTableColumnDefinitionViewModel()
            {
                className = "wrap-column",
                title = fieldDescription,
                data = fieldName
            };

            // Subscribe to the sort order event
            col.DefaultSortSet += Col_DefaultSortSet;
            columns.Add(col);

            return col;
        }

        /// <summary>
        /// Add a query string parameter.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        public void AddQueryStringParameters(NameValueCollection queryString)
        {
            var ret = new List<QueryStringParameter>();

            foreach (var key in queryString.AllKeys)
            {
                ret.Add(new QueryStringParameter(key, queryString[key]));
            }

            QueryStringParameters.AddRange(ret.ToArray());
        }

        /// <summary>
        /// Add an array of query string parameters.
        /// </summary>
        /// <typeparam name="T">The datatype of the values.</typeparam>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="values">The values for the parameter.</param>
        public void AddQueryStringArrayParameters<T>(string parameterName, List<T> values)
        {
            var ret = new List<QueryStringParameter>();

            if (!parameterName.Contains("[]"))
            {
                parameterName = parameterName + "[]";
            }

            if (values == null)
            {
                values = new List<T>();
            }

            QueryStringParameters.AddRange(CreateArrayQueryStringParameter(parameterName, string.Join(",", values)));
        }

        /// <summary>
        /// Add additional data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddAdditionalData(string key, string value)
        {
            AdditionalData.Add(key, value);
        }

        /// <summary>
        /// Get the sort details.
        /// </summary>
        /// <returns>The sorted details.</returns>
        public string[] GetSortDetails()
        {
            var sortList = new List<string>();

            if (columns.Where(x => x.IsFirstSortColumn == true).Any())
            {
                // Check if multi level sort is specified
                // multi level sort has been specified
                var primnaryColumn = columns.Where(x => x.IsFirstSortColumn == true).First();
                sortList.Add(columns.IndexOf(primnaryColumn).ToString());

                if (columns.Where(x => x.IsSecondSortColum == true).Any())
                {
                    var sec = columns.Where(x => x.IsSecondSortColum == true).First();
                    sortList.Add(columns.IndexOf(sec).ToString());
                }

                if (columns.Where(x => x.IsThirdSortColumn == true).Any())
                {
                    var t = columns.Where(x => x.IsThirdSortColumn == true).First();
                    sortList.Add(columns.IndexOf(t).ToString());
                }
            }
            else
            {
                foreach (var column in columns.Where(x => x.isDefaultSortColumn == true).ToList())
                {
                    sortList.Add(columns.IndexOf(column).ToString());
                    sortList.Add(column.sortDirection);
                }
            }

            return sortList.ToArray();
        }

        /// <summary>
        /// Map the query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        public void MapQueryString(NameValueCollection queryString)
        {
            var ret = new List<QueryStringParameter>();

            foreach (string key in queryString.AllKeys)
            {
                if (key.Contains("[]"))
                {
                    ret.Add(CreateQueryStringParameter(key, queryString[key]));
                }
                else
                {
                    ret.AddRange(CreateArrayQueryStringParameter(key, queryString[key]));
                }
            }

            QueryStringParameters.AddRange(ret);
        }

        /// <summary>
        /// Add a title to the table.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The datatable view model.</returns>
        public DataTableViewModel WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Add a backend url to the table.
        /// </summary>
        /// <param name="backendUrl">The backend url.</param>
        /// <returns>The datatable view model.</returns>
        public DataTableViewModel WithBackendURL(string backendUrl)
        {
            BackendServiceUrl = new MvcHtmlString(backendUrl);
            ServiceUrl = backendUrl;
            return this;
        }

        /// <summary>
        /// Set the request type to POST.
        /// </summary>
        /// <returns>The datatable view model.</returns>
        public DataTableViewModel UsePOSTForBackendAjaxCalls()
        {
            BackendUrlRequestType = "POST";
            return this;
        }

        /// <summary>
        /// Set the default sorted column.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        private void Col_DefaultSortSet(string columnName)
        {
            _sortDataList.Add(columnName);
        }

        /// <summary>
        /// Create the query string parameter array.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The collection of query string parameters.</returns>
        private IEnumerable<QueryStringParameter> CreateArrayQueryStringParameter(string key, string value)
        {
            var parameters = new List<QueryStringParameter>();
            string[] multValues = value.Split(',');

            if (key.Contains("[]"))
            {
                var keywithBracketsRemoved = key.Replace("]", string.Empty).Replace("[", string.Empty);

                if (value != null)
                {
                    for (int i = 0; i < multValues.Count(); i++)
                    {
                        parameters.Add(new QueryStringParameter(keywithBracketsRemoved + "[" + i + "]", multValues[i]));
                    }
                }
            }

            return parameters;
        }

        /// <summary>
        /// Create the query string parameter.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>The query string parameter.</returns>
        private QueryStringParameter CreateQueryStringParameter(string key, string value)
        {
            return new QueryStringParameter(key, value);
        }
    }

}
