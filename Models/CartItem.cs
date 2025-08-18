using System;
using ArtMarketPlaceAPI.Models;

namespace ArtMarketPlaceAPI.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ProduitId { get; set; }
        public required Produit Produit { get; set; }

        public int ClientId { get; set; }
        public required User Client { get; set; }

        public int Quantite { get; set; } = 1;
        public DateTime DateAjout { get; set; } = DateTime.UtcNow;
    }
}
