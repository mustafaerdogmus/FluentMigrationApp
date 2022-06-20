using FluentMigrator;
using static FluentMigrationApp.MigrationTypeStruct;

namespace FluentMigrationApp.Migrations
{
    [Migration(4, "v4.0.0")]
    [Tags(MigrationTypes.Send_To_Production)]
    public class MigrationV4 : FluentMigrator.Migration
    {
        public override void Down()
        {
            #region Tables

            Delete.Table("TestV4");

            #endregion Tables
        }

        public override void Up()
        {
            #region Tables

            Create.Table("TestV4")
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