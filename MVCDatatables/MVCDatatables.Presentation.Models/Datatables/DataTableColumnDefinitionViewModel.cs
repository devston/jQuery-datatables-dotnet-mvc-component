using System;

namespace MVCDatatables.Presentation.Models.Datatables
{
    /// <summary>
    /// The datatable column definition model.
    /// </summary>
    public class DataTableColumnDefinitionViewModel
    {
        /*
         * Unfortunately some properties that link back to jquery datatables.net
         * must have their first character begin with a lowercase, otherwise the
         * plugin will not read them.
         */

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTableColumnDefinitionViewModel"/> class.
        /// </summary>
        public DataTableColumnDefinitionViewModel()
        {
            visible = true;
            orderable = false;
            searchable = false;
            isDefaultSortColumn = false;
            type = null;
            IsDateHumanised = false;
            MomentDataFormat = string.Empty;
            sortDirection = "asc";
            IsFirstSortColumn = false;
            IsSecondSortColum = false;
            IsThirdSortColumn = false;
            IsColourColumn = false;
            IsFormattedDecimal = false;
        }

        /// <summary>
        /// Sets the default sort column.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        public delegate void SetDefaultSortColumn(string columnName);

        /// <summary>
        /// Sets the default sort column.
        /// </summary>
        public event SetDefaultSortColumn DefaultSortSet;

        /// <summary>
        /// Gets or sets a value indicating whether this column is the default sort column.
        /// </summary>
        public bool isDefaultSortColumn { get; set; }

        /// <summary>
        /// Gets or sets the column title.
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// Gets or sets the column data.
        /// </summary>
        public string data { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is visible.
        /// </summary>
        public bool visible { get; set; }

        /// <summary>
        /// Gets or sets the custom classes for the column.
        /// </summary>
        public string className { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is orderable.
        /// </summary>
        public bool orderable { get; set; }

        /// <summary>
        /// Gets the sort direction for the column.
        /// </summary>
        public string sortDirection { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the column is searchable.
        /// </summary>
        public bool searchable { get; set; }

        /// <summary>
        /// Gets or sets the column type.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Gets a value indicating whether the date should be humanised.
        /// </summary>
        public bool IsDateHumanised { get; private set; }

        /// <summary>
        /// Gets the date format.
        /// </summary>
        public string MomentDataFormat { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the column is a currency field.
        /// </summary>
        public bool IsCurrency { get; private set; }

        /// <summary>
        /// Gets the currency to show.
        /// </summary>
        public string CurrencyToShow { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the data is numeric.
        /// </summary>
        public bool IsNumeric { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the data is a percentage.
        /// </summary>
        public bool IsPercent { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the boolean is formatted as yes or no.
        /// </summary>
        public bool IsYesNo { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the first sorted column.
        /// </summary>
        public bool IsFirstSortColumn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the second sorted column.
        /// </summary>
        public bool IsSecondSortColum { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is the third sorted column.
        /// </summary>
        public bool IsThirdSortColumn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this column is inside a child row.
        /// </summary>
        public bool IsInChildRow { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is colour column.
        /// </summary>
        /// <value><c>true</c> if this instance is colour column; otherwise, <c>false</c>.</value>
        public bool IsColourColumn { get; set; }

        /// <summary>
        /// Gets a value indicating whether this column is to be a formatted decimal.
        /// </summary>
        public bool IsFormattedDecimal { get; private set; }

        /// <summary>
        /// Gets the symbol for the column.
        /// </summary>
        public string Symbol { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this columns symbol should be prepended.
        /// </summary>
        public bool PrependSymbol { get; private set; }

        /// <summary>
        /// Adds a custom title to the column.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <returns>The view model.</returns>
        public DataTableColumnDefinitionViewModel WithTitle(string title)
        {
            this.title = title;
            return this;
        }

        /// <summary>
        /// Sets the column as hidden.
        /// </summary>
        /// <returns>The view model.</returns>
        public DataTableColumnDefinitionViewModel IsHidden()
        {
            visible = false;
            return this;
        }

        /// <summary>
        /// Tell the data table if this column is sortable.
        /// </summary>
        /// <param name="defaultSortColumn">Set to true if this is being used as the default sorting column.</param>
        /// <param name="sortDesc">Set to true if this column should be sorted descending as default.</param>
        /// <returns>The view model.</returns>
        public DataTableColumnDefinitionViewModel IsSortable(bool defaultSortColumn = false, bool sortDesc = false)
        {
            isDefaultSortColumn = defaultSortColumn;

            // If this column is set as the default sort column, raise an event to indicate it.
            if (defaultSortColumn)
            {
                DefaultSortSet(data);

                // Set the sort direction to descending.
                if (sortDesc)
                {
                    SortDirection(Datatables.SortDirection.desc);
                }
            }

            orderable = true;
            return this;
        }

        /// <summary>
        /// Add an additional class to the column.
        /// </summary>
        /// <param name="classToAppend">The css class.</param>
        /// <returns>The view model.</returns>
        public DataTableColumnDefinitionViewModel AppendClass(string classToAppend)
        {
            className = className + " " + classToAppend;
            return this;
        }

        /// <summary>
        /// Set the type of column.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The view model.</returns>
        public DataTableColumnDefinitionViewModel Type(Type type)
        {
            if (type == typeof(DateTime))
            {
                this.type = "date";
                if (string.IsNullOrEmpty(MomentDataFormat))
                {
                    DateFormat("LLL");
                }
            }

            return this;
        }

        /// <summary>
        /// Set the column as searchable.
        /// </summary>
        /// <returns>The view model.</returns>
        public DataTableColumnDefinitionViewModel AllowSearch()
        {
            searchable = true;
            return this;
        }

        public DataTableColumnDefinitionViewModel HumaniseDateAndTime()
        {
            IsDateHumanised = true;
            type = "date";

            if (string.IsNullOrEmpty(MomentDataFormat))
            {
                DateFormat("LLL");
            }

            return this;
        }

        public DataTableColumnDefinitionViewModel HumaniseDateOnly()
        {
            IsDateHumanised = true;
            type = "date";

            if (string.IsNullOrEmpty(MomentDataFormat))
            {
                DateFormat("LL");
            }

            return this;
        }

        /// <summary>
        /// Set the boolean to yes or no instead of true / false.
        /// </summary>
        /// <returns>The view model.</returns>
        public DataTableColumnDefinitionViewModel SetBooleanAsYesNo()
        {
            IsYesNo = true;
            type = "bool";
            return this;
        }

        /// <summary>
        /// Places the column inside of a child row.
        /// </summary>
        /// <returns>DataTableColumnDefinitionViewModel.</returns>
        public DataTableColumnDefinitionViewModel IsInsideChildRow()
        {
            visible = false;
            IsInChildRow = true;
            return this;
        }

        /// <summary>
        /// Determines whether [is colour value].
        /// </summary>
        /// <returns>DataTableColumnDefinitionViewModel.</returns>
        public DataTableColumnDefinitionViewModel IsColourValue()
        {
            IsColourColumn = true;
            return this;
        }

        /// <summary>
        /// Determines whether column should format the decimal.
        /// </summary>
        /// <param name="symbol">The symbol to display once formatted.</param>
        /// <param name="prependSymbol">Indicates if the symbol should be prepended or appended.</param>
        /// <returns>DataTableColumnDefinitionViewModel.</returns>
        public DataTableColumnDefinitionViewModel FormatDecimal(string symbol = "", bool prependSymbol = false)
        {
            IsFormattedDecimal = true;
            Symbol = symbol;
            PrependSymbol = prependSymbol;

            return this;
        }

        /// <summary>
        /// Sets the date format for the column.
        /// </summary>
        /// <param name="momentDateFormat">The date format used by moment js.</param>
        /// <returns>The view model.</returns>
        internal DataTableColumnDefinitionViewModel DateFormat(string momentDateFormat)
        {
            IsDateHumanised = false;
            MomentDataFormat = momentDateFormat;
            type = "date";
            return this;
        }

        /// <summary>
        /// Set the sort direction of the column.
        /// </summary>
        /// <param name="sort">The sort direction.</param>
        /// <returns>The view model.</returns>
        internal DataTableColumnDefinitionViewModel SortDirection(SortDirection sort = Datatables.SortDirection.asc)
        {
            sortDirection = Enum.GetName(sort.GetType(), sort);
            return this;
        }

        /// <summary>
        /// Set the column data type as currency.
        /// </summary>
        /// <param name="curr">The currency.</param>
        /// <returns>The view model.</returns>
        internal DataTableColumnDefinitionViewModel IsLocalisedCurrency(string curr)
        {
            if (curr != string.Empty)
            {
                IsCurrency = true;
                CurrencyToShow = curr;
            }

            return this;
        }

        /// <summary>
        /// Set the column as numeric.
        /// </summary>
        /// <returns>The view model.</returns>
        internal DataTableColumnDefinitionViewModel IsLocalisedNumeric()
        {
            IsNumeric = true;
            return this;
        }

        /// <summary>
        /// Set the column data type as percentage.
        /// </summary>
        /// <returns>The view model.</returns>
        internal DataTableColumnDefinitionViewModel IsLocalisedPercent()
        {
            IsNumeric = true;
            IsPercent = true;
            return this;
        }
    }
}
