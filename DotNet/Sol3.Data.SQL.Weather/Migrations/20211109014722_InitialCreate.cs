using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sol3.Data.SQL.Weather.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherResponse",
                columns: table => new
                {
                    WeatherResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Base = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Visibility = table.Column<int>(type: "int", nullable: true),
                    Dt = table.Column<int>(type: "int", nullable: true),
                    Timezone = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherResponse", x => x.WeatherResponseId);
                });

            migrationBuilder.CreateTable(
                name: "Cloud",
                columns: table => new
                {
                    WeatherResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    All = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cloud", x => x.WeatherResponseId);
                    table.ForeignKey(
                        name: "FK_Cloud_WeatherResponse",
                        column: x => x.WeatherResponseId,
                        principalTable: "WeatherResponse",
                        principalColumn: "WeatherResponseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Coords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeatherResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Lon = table.Column<double>(type: "float", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coords_WeatherResponse_WeatherResponseId",
                        column: x => x.WeatherResponseId,
                        principalTable: "WeatherResponse",
                        principalColumn: "WeatherResponseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mains",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeatherResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Temp = table.Column<double>(type: "float", nullable: false),
                    FeelsLike = table.Column<double>(type: "float", nullable: false),
                    TempMin = table.Column<double>(type: "float", nullable: false),
                    TempMax = table.Column<double>(type: "float", nullable: false),
                    Pressure = table.Column<int>(type: "int", nullable: false),
                    Humidity = table.Column<int>(type: "int", nullable: false),
                    SeaLevel = table.Column<int>(type: "int", nullable: false),
                    GrndLevel = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mains", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mains_WeatherResponse_WeatherResponseId",
                        column: x => x.WeatherResponseId,
                        principalTable: "WeatherResponse",
                        principalColumn: "WeatherResponseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeatherResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sunrise = table.Column<int>(type: "int", nullable: false),
                    Sunset = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sys_WeatherResponse_WeatherResponseId",
                        column: x => x.WeatherResponseId,
                        principalTable: "WeatherResponse",
                        principalColumn: "WeatherResponseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Weather",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WeatherResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Main = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Icon = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weather", x => new { x.Id, x.WeatherResponseId });
                    table.ForeignKey(
                        name: "FK_Weather_WeatherResponse",
                        column: x => x.WeatherResponseId,
                        principalTable: "WeatherResponse",
                        principalColumn: "WeatherResponseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Winds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeatherResponseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Speed = table.Column<double>(type: "float", nullable: false),
                    Deg = table.Column<int>(type: "int", nullable: false),
                    Gust = table.Column<double>(type: "float", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Winds_WeatherResponse_WeatherResponseId",
                        column: x => x.WeatherResponseId,
                        principalTable: "WeatherResponse",
                        principalColumn: "WeatherResponseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coords_WeatherResponseId",
                table: "Coords",
                column: "WeatherResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mains_WeatherResponseId",
                table: "Mains",
                column: "WeatherResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sys_WeatherResponseId",
                table: "Sys",
                column: "WeatherResponseId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weather_WeatherResponseId",
                table: "Weather",
                column: "WeatherResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_Winds_WeatherResponseId",
                table: "Winds",
                column: "WeatherResponseId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cloud");

            migrationBuilder.DropTable(
                name: "Coords");

            migrationBuilder.DropTable(
                name: "Mains");

            migrationBuilder.DropTable(
                name: "Sys");

            migrationBuilder.DropTable(
                name: "Weather");

            migrationBuilder.DropTable(
                name: "Winds");

            migrationBuilder.DropTable(
                name: "WeatherResponse");
        }
    }
}
