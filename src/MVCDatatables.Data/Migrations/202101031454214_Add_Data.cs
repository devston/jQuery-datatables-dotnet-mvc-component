﻿namespace MVCDatatables.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Data : DbMigration
    {
        public override void Up()
        {
            Sql(@"
INSERT INTO [dbo].[DummyTables]([RandomData])
VALUES('Value 1'),
      ('Value 2'),
      ('Value 3'),
      ('Value 4'),
      ('Value 5'),
      ('Value 6'),
      ('Value 7'),
      ('Value 8'),
      ('Value 9'),
      ('Value 10'),
      ('Value 11'),
      ('Value 12'),
      ('Value 13'),
      ('Value 14'),
      ('Value 15'),
      ('Value 16'),
      ('Value 17'),
      ('Value 18'),
      ('Value 19'),
      ('Value 20'),
      ('Value 21'),
      ('Value 22'),
      ('Value 23'),
      ('Value 24'),
      ('Value 25'),
      ('Value 26'),
      ('Value 27'),
      ('Value 28'),
      ('Value 29'),
      ('Value 30'),
      ('Value 31'),
      ('Value 32'),
      ('Value 33'),
      ('Value 34'),
      ('Value 35'),
      ('Value 36'),
      ('Value 37'),
      ('Value 38'),
      ('Value 39'),
      ('Value 40'),
      ('Value 41'),
      ('Value 42'),
      ('Value 43'),
      ('Value 44'),
      ('Value 45'),
      ('Value 46'),
      ('Value 47'),
      ('Value 48'),
      ('Value 49'),
      ('Value 50')"
                );
        }
        
        public override void Down()
        {
            Sql("DELETE FROM [dbo].[DummyTables]");
        }
    }
}
