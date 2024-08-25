using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoomReservationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class Added_Reservations_AppUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Reservations_ReservationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_HostId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ReservationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserReservations",
                columns: table => new
                {
                    ParticipantsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReservationsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReservations", x => new { x.ParticipantsId, x.ReservationsId });
                    table.ForeignKey(
                        name: "FK_UserReservations_AspNetUsers_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReservations_Reservations_ReservationsId",
                        column: x => x.ReservationsId,
                        principalTable: "Reservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_ReservationsId",
                table: "UserReservations",
                column: "ReservationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_HostId",
                table: "Reservations",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_HostId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "UserReservations");

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ReservationId",
                table: "AspNetUsers",
                column: "ReservationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Reservations_ReservationId",
                table: "AspNetUsers",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_HostId",
                table: "Reservations",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
