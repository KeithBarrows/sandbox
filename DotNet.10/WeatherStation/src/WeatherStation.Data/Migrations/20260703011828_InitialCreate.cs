using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WeatherStation.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherObservations",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ObservedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    TempF = table.Column<double>(type: "double precision", nullable: true),
                    FeelsLikeF = table.Column<double>(type: "double precision", nullable: true),
                    DewpointF = table.Column<double>(type: "double precision", nullable: true),
                    HumidityPct = table.Column<int>(type: "integer", nullable: true),
                    WindSpeedMph = table.Column<double>(type: "double precision", nullable: true),
                    WindGustMph = table.Column<double>(type: "double precision", nullable: true),
                    WindDirDeg = table.Column<int>(type: "integer", nullable: true),
                    UvIndex = table.Column<double>(type: "double precision", nullable: true),
                    PressureInHg = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    PrecipInPerHr = table.Column<decimal>(type: "numeric(6,2)", precision: 6, scale: 2, nullable: true),
                    Source = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherObservations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherObservations_ObservedAtUtc",
                table: "WeatherObservations",
                column: "ObservedAtUtc",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherObservations");
        }
    }
}
