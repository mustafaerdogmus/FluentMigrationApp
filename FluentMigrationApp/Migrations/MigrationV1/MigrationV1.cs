using FluentMigrator;
using static FluentMigrationApp.MigrationTypeStruct;

namespace FluentMigrationApp.Migrations
{
    [Migration(1, "v1.0.0")]
    [Tags(MigrationTypes.Send_To_Production)]
    public class MigrationV1 : Migration
    {
        public override void Down()
        {
            #region Foreign Keys

            Delete.ForeignKey("FK_Journey_CaptainId").OnTable("Journey");
            Delete.ForeignKey("FK_JourneyDocuments_JourneyId").OnTable("JourneyDocuments");
            Delete.ForeignKey("FK_JourneyDocuments_CaptainId").OnTable("JourneyDocuments");

            #endregion Foreign Keys

            #region Tables

            Delete.Table("Captain");
            Delete.Table("Journey");
            Delete.Table("JourneyDocuments");

            #endregion Tables
        }

        public override void Up()
        {
            #region Tables

            Create.Table("Captain")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("NameSurname").AsString(50).NotNullable();

            Create.Table("Journey")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("JourneyDescription").AsString(100).NotNullable()
                .WithColumn("CaptainId").AsInt32().Nullable();

            Create.Table("JourneyDocuments")
                .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("JourneyDocumentsDescription").AsDecimal(9, 2).NotNullable()
                .WithColumn("JourneyId").AsInt32().NotNullable()
                .WithColumn("CaptainId").AsInt32().NotNullable();

            #endregion Tables

            #region Foreign Keys

            //Captain ve journey arasinda ki iliski
            Create.ForeignKey("FK_Journey_CaptainId")
                .FromTable("Journey").ForeignColumn("CaptainId")
                .ToTable("Captain").PrimaryColumn("Id");
            //journey ve journeydocuments arasina ki iliski
            Create.ForeignKey("FK_JourneyDocuments_JourneyId")
                .FromTable("JourneyDocuments").ForeignColumn("JourneyId")
                .ToTable("Journey").PrimaryColumn("Id");
            //captain ve journeyDocuments arasinda ki iliski
            Create.ForeignKey("FK_JourneyDocuments_CaptainId")
                .FromTable("JourneyDocuments").ForeignColumn("CaptainId")
                .ToTable("Captain").PrimaryColumn("Id");

            #endregion Foreign Keys

            #region Initial Data

            Insert.IntoTable("Captain")
                .Row(new { NameSurname = "MustafaErdogmus" })
                .Row(new { NameSurname = "MustafaErdogmus" });

            #endregion Initial Data
        }
    }
}