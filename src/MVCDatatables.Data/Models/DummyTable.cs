using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCDatatables.Data.Models
{
    /// <summary>
    /// A dummy data table to demo the api.
    /// </summary>
    public class DummyTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DummyKey { get; set; }

        public string RandomData { get; set; }
    }
}
