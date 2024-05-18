using FluentMigrator;
using FluentMigrator.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    [Migration(160520241844)]
    public class Migration_160520241844 : Migration
    {
        public override void Down()
        {
            Delete.Table("Orders");
        }

        public override void Up()
        {
            Create.Table("Orders")
                .WithColumn("Id").AsInt32().NotNullable().Identity(1, 1).PrimaryKey()
                .WithColumn("guid").AsGuid().NotNullable() //.WithDefault(SystemMethods.NewGuid)
                .WithColumn("amount").AsDecimal().NotNullable()
                .WithColumn("currency").AsString().NotNullable()
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
