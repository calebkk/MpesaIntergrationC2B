﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MpesaIntergration.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MpesaC2Bs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransAmount = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessShortCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillRefNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrgAccountBalance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdPartyTransID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MSISDN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MpesaC2Bs", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MpesaC2Bs");
        }
    }
}
