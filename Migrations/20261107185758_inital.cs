using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ConfidentVotesAPI.Migrations
{
    /// <inheritdoc />
    public partial class inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    person_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.person_id);
                });

            migrationBuilder.CreateTable(
                name: "elections",
                columns: table => new
                {
                    election_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    person_id = table.Column<int>(type: "integer", nullable: false),
                    election_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    election_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_elections", x => x.election_id);
                    table.ForeignKey(
                        name: "fk_elections_users_person_id",
                        column: x => x.person_id,
                        principalTable: "users",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "choices",
                columns: table => new
                {
                    choice_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "text", nullable: false),
                    election_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_choices", x => x.choice_id);
                    table.ForeignKey(
                        name: "fk_choices_elections_election_id",
                        column: x => x.election_id,
                        principalTable: "elections",
                        principalColumn: "election_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "election_person",
                columns: table => new
                {
                    allowed_elections_election_id = table.Column<int>(type: "integer", nullable: false),
                    voters_person_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_election_person", x => new { x.allowed_elections_election_id, x.voters_person_id });
                    table.ForeignKey(
                        name: "fk_election_person_elections_allowed_elections_election_id",
                        column: x => x.allowed_elections_election_id,
                        principalTable: "elections",
                        principalColumn: "election_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_election_person_users_voters_person_id",
                        column: x => x.voters_person_id,
                        principalTable: "users",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "votes",
                columns: table => new
                {
                    vote_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    election_id = table.Column<int>(type: "integer", nullable: false),
                    choice_id = table.Column<int>(type: "integer", nullable: false),
                    person_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_votes", x => x.vote_id);
                    table.ForeignKey(
                        name: "fk_votes_choices_choice_id",
                        column: x => x.choice_id,
                        principalTable: "choices",
                        principalColumn: "choice_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_votes_elections_election_id",
                        column: x => x.election_id,
                        principalTable: "elections",
                        principalColumn: "election_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_votes_users_person_id",
                        column: x => x.person_id,
                        principalTable: "users",
                        principalColumn: "person_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_choices_election_id",
                table: "choices",
                column: "election_id");

            migrationBuilder.CreateIndex(
                name: "ix_election_person_voters_person_id",
                table: "election_person",
                column: "voters_person_id");

            migrationBuilder.CreateIndex(
                name: "ix_elections_person_id",
                table: "elections",
                column: "person_id");

            migrationBuilder.CreateIndex(
                name: "ix_votes_choice_id",
                table: "votes",
                column: "choice_id");

            migrationBuilder.CreateIndex(
                name: "ix_votes_election_id_person_id",
                table: "votes",
                columns: new[] { "election_id", "person_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_votes_person_id",
                table: "votes",
                column: "person_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "election_person");

            migrationBuilder.DropTable(
                name: "votes");

            migrationBuilder.DropTable(
                name: "choices");

            migrationBuilder.DropTable(
                name: "elections");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
