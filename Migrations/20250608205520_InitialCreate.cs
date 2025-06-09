using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArtMarketPlaceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomComplet = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MdpHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    MdpSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Commandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCommande = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Statut = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    LivreurId = table.Column<int>(type: "int", nullable: true),
                    StatutLivraison = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commandes_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commandes_Users_LivreurId",
                        column: x => x.LivreurId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Produits",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prix = table.Column<float>(type: "real", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    DateAjout = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArtisanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produits", x => x.id);
                    table.ForeignKey(
                        name: "FK_Produits_Users_ArtisanId",
                        column: x => x.ArtisanId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<int>(type: "int", nullable: false),
                    Commentaire = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateAvis = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProduitId = table.Column<int>(type: "int", nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Avis_Produits_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "Produits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Avis_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Avis_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LignesCommandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandeId = table.Column<int>(type: "int", nullable: false),
                    ProduitId = table.Column<int>(type: "int", nullable: false),
                    Quantité = table.Column<int>(type: "int", nullable: false),
                    PrixUnitaire = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LignesCommandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LignesCommandes_Commandes_CommandeId",
                        column: x => x.CommandeId,
                        principalTable: "Commandes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LignesCommandes_Produits_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "Produits",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "MdpHash", "MdpSalt", "NomComplet", "Role" },
                values: new object[,]
                {
                    { 1, "admin@example.com", new byte[] { 15, 53, 158, 165, 178, 103, 100, 253, 253, 195, 69, 132, 27, 58, 182, 123, 7, 172, 187, 31, 157, 72, 71, 133, 4, 202, 232, 141, 171, 9, 10, 37, 163, 112, 36, 250, 162, 232, 140, 199, 22, 141, 172, 6, 174, 190, 135, 20, 48, 196, 243, 182, 237, 15, 45, 91, 155, 133, 129, 108, 42, 50, 92, 101 }, new byte[] { 35, 97, 140, 40, 143, 43, 76, 207, 202, 229, 41, 77, 0, 232, 21, 9, 87, 174, 51, 32, 14, 187, 13, 40, 242, 245, 37, 162, 165, 240, 186, 85, 106, 45, 72, 232, 48, 23, 47, 249, 71, 15, 128, 45, 145, 73, 121, 39, 149, 137, 28, 11, 98, 130, 230, 190, 10, 32, 246, 82, 69, 34, 99, 132, 76, 174, 185, 126, 54, 27, 136, 197, 90, 68, 218, 25, 98, 58, 47, 150, 245, 15, 196, 112, 217, 180, 91, 57, 22, 189, 23, 143, 158, 37, 158, 213, 130, 22, 70, 17, 97, 91, 185, 80, 231, 79, 4, 252, 204, 96, 107, 137, 41, 187, 197, 207, 90, 111, 98, 33, 216, 64, 143, 93, 191, 154, 47, 120 }, "Admin Test", 3 },
                    { 2, "artisan@example.com", new byte[] { 15, 53, 158, 165, 178, 103, 100, 253, 253, 195, 69, 132, 27, 58, 182, 123, 7, 172, 187, 31, 157, 72, 71, 133, 4, 202, 232, 141, 171, 9, 10, 37, 163, 112, 36, 250, 162, 232, 140, 199, 22, 141, 172, 6, 174, 190, 135, 20, 48, 196, 243, 182, 237, 15, 45, 91, 155, 133, 129, 108, 42, 50, 92, 101 }, new byte[] { 35, 97, 140, 40, 143, 43, 76, 207, 202, 229, 41, 77, 0, 232, 21, 9, 87, 174, 51, 32, 14, 187, 13, 40, 242, 245, 37, 162, 165, 240, 186, 85, 106, 45, 72, 232, 48, 23, 47, 249, 71, 15, 128, 45, 145, 73, 121, 39, 149, 137, 28, 11, 98, 130, 230, 190, 10, 32, 246, 82, 69, 34, 99, 132, 76, 174, 185, 126, 54, 27, 136, 197, 90, 68, 218, 25, 98, 58, 47, 150, 245, 15, 196, 112, 217, 180, 91, 57, 22, 189, 23, 143, 158, 37, 158, 213, 130, 22, 70, 17, 97, 91, 185, 80, 231, 79, 4, 252, 204, 96, 107, 137, 41, 187, 197, 207, 90, 111, 98, 33, 216, 64, 143, 93, 191, 154, 47, 120 }, "Artisan Test", 0 },
                    { 3, "client@example.com", new byte[] { 15, 53, 158, 165, 178, 103, 100, 253, 253, 195, 69, 132, 27, 58, 182, 123, 7, 172, 187, 31, 157, 72, 71, 133, 4, 202, 232, 141, 171, 9, 10, 37, 163, 112, 36, 250, 162, 232, 140, 199, 22, 141, 172, 6, 174, 190, 135, 20, 48, 196, 243, 182, 237, 15, 45, 91, 155, 133, 129, 108, 42, 50, 92, 101 }, new byte[] { 35, 97, 140, 40, 143, 43, 76, 207, 202, 229, 41, 77, 0, 232, 21, 9, 87, 174, 51, 32, 14, 187, 13, 40, 242, 245, 37, 162, 165, 240, 186, 85, 106, 45, 72, 232, 48, 23, 47, 249, 71, 15, 128, 45, 145, 73, 121, 39, 149, 137, 28, 11, 98, 130, 230, 190, 10, 32, 246, 82, 69, 34, 99, 132, 76, 174, 185, 126, 54, 27, 136, 197, 90, 68, 218, 25, 98, 58, 47, 150, 245, 15, 196, 112, 217, 180, 91, 57, 22, 189, 23, 143, 158, 37, 158, 213, 130, 22, 70, 17, 97, 91, 185, 80, 231, 79, 4, 252, 204, 96, 107, 137, 41, 187, 197, 207, 90, 111, 98, 33, 216, 64, 143, 93, 191, 154, 47, 120 }, "Client Test", 1 },
                    { 4, "livreur@example.com", new byte[] { 15, 53, 158, 165, 178, 103, 100, 253, 253, 195, 69, 132, 27, 58, 182, 123, 7, 172, 187, 31, 157, 72, 71, 133, 4, 202, 232, 141, 171, 9, 10, 37, 163, 112, 36, 250, 162, 232, 140, 199, 22, 141, 172, 6, 174, 190, 135, 20, 48, 196, 243, 182, 237, 15, 45, 91, 155, 133, 129, 108, 42, 50, 92, 101 }, new byte[] { 35, 97, 140, 40, 143, 43, 76, 207, 202, 229, 41, 77, 0, 232, 21, 9, 87, 174, 51, 32, 14, 187, 13, 40, 242, 245, 37, 162, 165, 240, 186, 85, 106, 45, 72, 232, 48, 23, 47, 249, 71, 15, 128, 45, 145, 73, 121, 39, 149, 137, 28, 11, 98, 130, 230, 190, 10, 32, 246, 82, 69, 34, 99, 132, 76, 174, 185, 126, 54, 27, 136, 197, 90, 68, 218, 25, 98, 58, 47, 150, 245, 15, 196, 112, 217, 180, 91, 57, 22, 189, 23, 143, 158, 37, 158, 213, 130, 22, 70, 17, 97, 91, 185, 80, 231, 79, 4, 252, 204, 96, 107, 137, 41, 187, 197, 207, 90, 111, 98, 33, 216, 64, 143, 93, 191, 154, 47, 120 }, "Livreur Test", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avis_ClientId",
                table: "Avis",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Avis_ProduitId",
                table: "Avis",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_Avis_UserId",
                table: "Avis",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_ClientId",
                table: "Commandes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_LivreurId",
                table: "Commandes",
                column: "LivreurId");

            migrationBuilder.CreateIndex(
                name: "IX_LignesCommandes_CommandeId",
                table: "LignesCommandes",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_LignesCommandes_ProduitId",
                table: "LignesCommandes",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_Produits_ArtisanId",
                table: "Produits",
                column: "ArtisanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Avis");

            migrationBuilder.DropTable(
                name: "LignesCommandes");

            migrationBuilder.DropTable(
                name: "Commandes");

            migrationBuilder.DropTable(
                name: "Produits");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
