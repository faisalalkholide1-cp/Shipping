using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingMvp.Migrations
{
    /// <inheritdoc />
    public partial class Added_Parcels_Module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParcelsParcels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TrackingNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SenderPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ReceiverPhone = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PickupAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedCourierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParcelsParcels", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParcelsParcels_CreationTime",
                table: "ParcelsParcels",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelsParcels_Status",
                table: "ParcelsParcels",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ParcelsParcels_TenantId_TrackingNumber",
                table: "ParcelsParcels",
                columns: new[] { "TenantId", "TrackingNumber" },
                unique: true,
                filter: "[TenantId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParcelsParcels");
        }
    }
}
