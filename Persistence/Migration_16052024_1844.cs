using FluentMigrator;
using FluentMigrator.SqlServer;

namespace Infrastructure
{
    [Migration(160520241844)]
    public class Migration_160520241844 : Migration
    {
        public override void Down()
        {
            Delete.Table("Transactions");
        }

        public override void Up()
        {
            Create.Table("Transactions")
                .WithColumn("Id").AsInt32().NotNullable().Identity(1, 1).PrimaryKey()
                .WithColumn("guid").AsGuid().NotNullable().WithDefault(SystemMethods.NewGuid)
                .WithColumn("amount").AsDecimal().NotNullable()
                .WithColumn("currency").AsCustom("NVARCHAR(3)").NotNullable()
                .WithColumn("card_holder_number").AsString().NotNullable()
                .WithColumn("holder_name").AsString().NotNullable()
                .WithColumn("expiration_month").AsInt16().NotNullable()
                .WithColumn("expiration_year").AsInt16().NotNullable()
                .WithColumn("cvv").AsInt16().NotNullable()
                .WithColumn("order_reference").AsCustom("NVARCHAR(50)").NotNullable()
                .WithColumn("status").AsString().NotNullable();
        }
    }
}
