using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AquaparkWebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class DeleteBehaviourChangeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hall",
                columns: table => new
                {
                    HallID = table.Column<byte>(type: "tinyint", nullable: false),
                    PoolsMaxDepth = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    PoolsMinDepth = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    HallMaxPeople = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hall", x => x.HallID);
                });

            migrationBuilder.CreateTable(
                name: "Slide",
                columns: table => new
                {
                    SlideID = table.Column<byte>(type: "tinyint", nullable: false),
                    SlideMinHeight = table.Column<byte>(type: "tinyint", nullable: true),
                    SlideMaxHeight = table.Column<byte>(type: "tinyint", nullable: true),
                    SlideMaxWeight = table.Column<byte>(type: "tinyint", nullable: true),
                    SlideMaxPeople = table.Column<byte>(type: "tinyint", nullable: false),
                    SlideMinAge = table.Column<byte>(type: "tinyint", nullable: true),
                    SlideHighestPoint = table.Column<byte>(type: "tinyint", nullable: false),
                    SlideName = table.Column<string>(type: "varchar(40)", unicode: false, maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slide", x => x.SlideID);
                });

            migrationBuilder.CreateTable(
                name: "Visitor",
                columns: table => new
                {
                    VisitorID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: false),
                    Height = table.Column<byte>(type: "tinyint", nullable: false),
                    Weight = table.Column<byte>(type: "tinyint", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitor", x => x.VisitorID);
                });

            migrationBuilder.CreateTable(
                name: "Pool",
                columns: table => new
                {
                    PoolID = table.Column<byte>(type: "tinyint", nullable: false),
                    PoolDepth = table.Column<decimal>(type: "decimal(2,1)", nullable: false),
                    PoolMinHeight = table.Column<byte>(type: "tinyint", nullable: true),
                    WaterType = table.Column<byte>(type: "tinyint", nullable: false),
                    Hall = table.Column<byte>(type: "tinyint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pool", x => x.PoolID);
                    table.ForeignKey(
                        name: "FK_Pool_Hall",
                        column: x => x.Hall,
                        principalTable: "Hall",
                        principalColumn: "HallID");
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    LocationHall = table.Column<byte>(type: "tinyint", nullable: true),
                    LocationSlide = table.Column<byte>(type: "tinyint", nullable: true),
                    LocationID = table.Column<byte>(type: "tinyint", nullable: true),
                    TicketOwner = table.Column<int>(type: "int", nullable: true),
                    TicketStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    Price = table.Column<decimal>(type: "smallmoney", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketID);
                    table.ForeignKey(
                        name: "FK_Ticket_Hall",
                        column: x => x.LocationID,
                        principalTable: "Hall",
                        principalColumn: "HallID");
                    table.ForeignKey(
                        name: "FK_Ticket_Slide",
                        column: x => x.LocationID,
                        principalTable: "Slide",
                        principalColumn: "SlideID");
                    table.ForeignKey(
                        name: "FK_Ticket_Visitor",
                        column: x => x.TicketOwner,
                        principalTable: "Visitor",
                        principalColumn: "VisitorID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pool_Hall",
                table: "Pool",
                column: "Hall");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_LocationID",
                table: "Ticket",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_TicketOwner",
                table: "Ticket",
                column: "TicketOwner");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pool");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Hall");

            migrationBuilder.DropTable(
                name: "Slide");

            migrationBuilder.DropTable(
                name: "Visitor");
        }
    }
}
