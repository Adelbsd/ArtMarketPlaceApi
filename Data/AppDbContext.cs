using Microsoft.EntityFrameworkCore;
using ArtMarketPlaceAPI.Models;

namespace ArtMarketPlaceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<LigneCommande> LignesCommandes { get; set; }
        public DbSet<Avis> Avis { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Relations ----

            // Commande ↔ Client
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Client)
                .WithMany(u => u.CommandesClient)
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Commande ↔ Livreur
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Livreur)
                .WithMany(u => u.CommandesLivreur)
                .HasForeignKey(c => c.LivreurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Produit ↔ Artisan
            modelBuilder.Entity<Produit>()
                .HasOne(p => p.Artisan)
                .WithMany(u => u.Produits)
                .HasForeignKey(p => p.ArtisanId)
                .OnDelete(DeleteBehavior.Cascade);

            // LigneCommande ↔ Commande
            modelBuilder.Entity<LigneCommande>()
                .HasOne(l => l.Commande)
                .WithMany(c => c.Lignes)
                .HasForeignKey(l => l.CommandeId)
                .OnDelete(DeleteBehavior.Restrict);

            // LigneCommande ↔ Produit
            modelBuilder.Entity<LigneCommande>()
                .HasOne(l => l.Produit)
                .WithMany(p => p.LigneCommandes)
                .HasForeignKey(l => l.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);

            // Avis ↔ Client
            modelBuilder.Entity<Avis>()
                .HasOne(a => a.Client)
                .WithMany()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Avis ↔ Produit
            modelBuilder.Entity<Avis>()
                .HasOne(a => a.Produit)
                .WithMany(p => p.Avis)
                .HasForeignKey(a => a.ProduitId)
                .OnDelete(DeleteBehavior.Cascade);

            // CartItem ↔ Produit
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Produit)
                .WithMany()
                .HasForeignKey(ci => ci.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);

            // CartItem ↔ Client
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Client)
                .WithMany()
                .HasForeignKey(ci => ci.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---- Décimaux ----
            modelBuilder.Entity<Commande>()
                .Property(c => c.Total)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LigneCommande>()
                .Property(l => l.PrixUnitaire)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Produit>()
                .Property(p => p.Prix)
                .HasPrecision(18, 2);

            // ---- Seed Users ----
            CreatePasswordHash("MotDePasse123!", out byte[] hash, out byte[] salt);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    NomComplet = "Admin Test",
                    Email = "admin@example.com",
                    MdpHash = hash,
                    MdpSalt = salt,
                    Role = UserRole.Admin
                },
                new User
                {
                    Id = 2,
                    NomComplet = "Artisan Test",
                    Email = "artisan@example.com",
                    MdpHash = hash,
                    MdpSalt = salt,
                    Role = UserRole.Artisan
                },
                new User
                {
                    Id = 3,
                    NomComplet = "Client Test",
                    Email = "client@example.com",
                    MdpHash = hash,
                    MdpSalt = salt,
                    Role = UserRole.Client
                },
                new User
                {
                    Id = 4,
                    NomComplet = "Livreur Test",
                    Email = "livreur@example.com",
                    MdpHash = hash,
                    MdpSalt = salt,
                    Role = UserRole.Livreur
                }
            );
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
