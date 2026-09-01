using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherStation.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocationSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WeatherObservations_ObservedAtUtc",
                table: "WeatherObservations");

            migrationBuilder.DropIndex(
                name: "IX_ForecastObservations_IssuedAtUtc_ForecastForUtc",
                table: "ForecastObservations");

            migrationBuilder.AddColumn<string>(
                name: "LocationKey",
                table: "WeatherObservations",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "citra");

            migrationBuilder.AddColumn<string>(
                name: "LocationKey",
                table: "ForecastObservations",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "citra");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherObservations_LocationKey_ObservedAtUtc",
                table: "WeatherObservations",
                columns: new[] { "LocationKey", "ObservedAtUtc" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForecastObservations_LocationKey_IssuedAtUtc_ForecastForUtc",
                table: "ForecastObservations",
                columns: new[] { "LocationKey", "IssuedAtUtc", "ForecastForUtc" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WeatherObservations_LocationKey_ObservedAtUtc",
                table: "WeatherObservations");

            migrationBuilder.DropIndex(
                name: "IX_ForecastObservations_LocationKey_IssuedAtUtc_ForecastForUtc",
                table: "ForecastObservations");

            migrationBuilder.DropColumn(
                name: "LocationKey",
                table: "WeatherObservations");

            migrationBuilder.DropColumn(
                name: "LocationKey",
                table: "ForecastObservations");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherObservations_ObservedAtUtc",
                table: "WeatherObservations",
                column: "ObservedAtUtc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ForecastObservations_IssuedAtUtc_ForecastForUtc",
                table: "ForecastObservations",
                columns: new[] { "IssuedAtUtc", "ForecastForUtc" },
                unique: true);
        }
    }
}
