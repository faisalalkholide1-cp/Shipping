using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingMvp.Migrations
{
    /// <inheritdoc />
    public partial class Added_ParcelStatusHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReturnReason",
                table: "ParcelsParcels",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ParcelsParcelStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParcelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ChangedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelsParcelStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParcelsParcelStatusHistories_ParcelsParcels_ParcelId",
                        column: x => x.ParcelId,
                        principalTable: "ParcelsParcels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParcelsParcels_AssignedCourierId",
                table: "ParcelsParcels",
                column: "AssignedCourierId");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelsParcelStatusHistories_ChangedAt",
                table: "ParcelsParcelStatusHistories",
                column: "ChangedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelsParcelStatusHistories_ParcelId",
                table: "ParcelsParcelStatusHistories",
                column: "ParcelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParcelsParcelStatusHistories");

            migrationBuilder.DropIndex(
                name: "IX_ParcelsParcels_AssignedCourierId",
                table: "ParcelsParcels");

            migrationBuilder.AlterColumn<string>(
                name: "ReturnReason",
                table: "ParcelsParcels",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);
        }
    }
}
