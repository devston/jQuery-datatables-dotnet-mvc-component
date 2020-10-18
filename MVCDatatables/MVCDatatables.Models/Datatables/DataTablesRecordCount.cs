namespace MVCDatatables.Models.Datatables
{
    /// <summary>
    /// The datatables record count domain model.
    /// </summary>
    public class DataTablesRecordCount
    {
        /// <summary>
        /// Gets or sets the filtered record count.
        /// </summary>
        public int FilteredCount { get; set; }

        /// <summary>
        /// Gets or sets the total record count.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
