using System.Collections.Generic;
using System.Linq;
using Mvc.JQuery.DataTables;
using MVCDatatables.Services.Datatables;
using NUnit.Framework;
using Shouldly;

namespace MVCDatatables.Services.UnitTests.DataTables
{
    [TestFixture]
    public class DataTableServiceTests
    {
        [Test]
        public void GetFilteredResultOrDefault_GivenRecordsFilteredResultLessThanOrEqualToZero_ShouldReturnNull()
        {
            //Arrange
            var dataTableService = new DataTableService();
            var list = new List<string>();

            //Act
            var result = dataTableService.GetFilteredResultOrDefault(new DataTablesParam(), list.AsQueryable());

            //Assert
            result.ShouldBeNull();
        }

        [Test]
        public void GetFilteredResultOrDefault_GivenRecordsFilteredWithNoFilterResultGreaterThanZero_ShouldReturnThreeRecords()
        {
            //Arrange
            var dataTableService = new DataTableService();
            var list = new List<string>() { "one", "two", "three" };
            var param = new DataTablesParam();

            //Act
            var result = dataTableService.GetFilteredResultOrDefault(param, list.AsQueryable());

            //Assert
            result.recordsFiltered.ShouldBe(3);
        }

        [Test]
        public void GetFilteredResultOrDefault_GivenRecordsFilteredWithFilterClauseResultGreaterThanZero_ShouldReturnOneRecords()
        {
            //Arrange
            var dataTableService = new DataTableService();
            var list = new List<string>() { "one", "two", "three" };
            var param = new DataTablesParam();

            //Act
            var result = dataTableService.GetFilteredResultOrDefault(param, list.AsQueryable(), x => x == "one");

            //Assert
            result.recordsFiltered.ShouldBe(1);
        }

        [Test]
        public void GetFilteredResultOrDefault_GivenRecordsFilteredWithInvalidFilterClauseResultLessThanOrEqualToZero_ShouldReturnNull()
        {
            //Arrange
            var dataTableService = new DataTableService();
            var list = new List<string>() { "one", "two", "three" };
            var param = new DataTablesParam();

            //Act
            var result = dataTableService.GetFilteredResultOrDefault(param, list.AsQueryable(), x => x == "four");

            //Assert
            result.ShouldBeNull();
        }

        [Test]
        public void GetFilteredResultOrDefault_GivenRecordsFilteredWithNoFilterClauseResultGreaterThanZero_ShouldReturnThreeRecords()
        {
            //Arrange
            var dataTableService = new DataTableService();
            var list = new List<string>() { "one", "two", "three" };
            var param = new DataTablesParam();

            //Act
            var result = dataTableService.GetFilteredResultOrDefault(param, list.AsQueryable(), null);

            //Assert
            result.recordsFiltered.ShouldBe(3);
        }

        [Test]
        public void GetFilteredResultOrDefault_GivenRecordsFilteredWithParamsAndFilterClauseResultGreaterThanZero_ShouldReturnOneRecords()
        {
            //Arrange
            var dataTableService = new DataTableService();
            var list = new List<string>() { "one", "two", "three" };
            var param = new DataTablesParam()
            {
                sSearch = "one"
            };

            //Act
            var result = dataTableService.GetFilteredResultOrDefault(param, list.AsQueryable(), x => x == "one");

            //Assert
            result.recordsFiltered.ShouldBe(1);
        }
    }

}
