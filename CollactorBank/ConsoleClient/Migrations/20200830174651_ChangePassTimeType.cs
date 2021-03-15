using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleClient.Migrations
{
    public partial class ChangePassTimeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<string>(
            //    name: "PassTime",
            //    table: "PassTime",
            //    nullable: true,
            //    oldClrType: typeof(DateTime),
            //    oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AlterColumn<DateTime>(
            //    name: "PassTime",
            //    table: "PassTime",
            //    type: "TEXT",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldNullable: true);
        }
    }
}
