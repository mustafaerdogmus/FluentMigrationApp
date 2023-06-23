using FluentMigrator;
using static FluentMigrationApp.MigrationTypeStruct;

namespace FluentMigrationApp.Migrations
{
    [Migration(3, "v3.0.0")]
    [Tags(MigrationTypes.Apply_Migration
        , MigrationTypes.Send_To_Development
        , MigrationTypes.Send_To_Test
        , MigrationTypes.Send_To_Production)]
    public class MigrationV3 : FluentMigrator.Migration
    {
        public override void Down()
        {
            #region Tables

            Delete.Table("TestV3");

            #endregion Tables
        }

        public override void Up()
        {
            #region Tables

            Create.Table("TestV3")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("NameSurname").AsString(50).NotNullable();

            #endregion Tables

            #region Initial Data

            //Insert.IntoTable("TestV3")
            //    .Row(new { Id = 1, NameSurname = "TEST1 TEST1" })
            //    .Row(new { Id = 2, NameSurname = "TEST2 TEST2" });

            #endregion Initial Data
        }
    }
}