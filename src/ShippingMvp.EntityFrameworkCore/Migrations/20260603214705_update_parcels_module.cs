using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShippingMvp.Migrations
{
    /// <inheritdoc />
    public partial class update_parcels_module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedTime",
                table: "ParcelsParcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledTime",
                table: "ParcelsParcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliveredTime",
                table: "ParcelsParcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OutForDeliveryTime",
                table: "ParcelsParcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickedUpTime",
                table: "ParcelsParcels",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReturnReason",
                table: "ParcelsParcels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnedTime",
                table: "ParcelsParcels",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedTime",
                table: "ParcelsParcels");

            migrationBuilder.DropColumn(
                name: "CancelledTime",
                table: "ParcelsParcels");

            migrationBuilder.DropColumn(
                name: "DeliveredTime",
                table: "ParcelsParcels");

            migrationBuilder.DropColumn(
                name: "OutForDeliveryTime",
                table: "ParcelsParcels");

            migrationBuilder.DropColumn(
                name: "PickedUpTime",
                table: "ParcelsParcels");

            migrationBuilder.DropColumn(
                name: "ReturnReason",
                table: "ParcelsParcels");

            migrationBuilder.DropColumn(
                name: "ReturnedTime",
                table: "ParcelsParcels");
        }
    }
}
