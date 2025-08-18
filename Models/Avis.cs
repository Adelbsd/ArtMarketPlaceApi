using System;

namespace ArtMarketPlaceAPI.Models
{
    public class Avis
    {
        public int Id { get; set; }
        public int Note { get; set; }
        public string Commentaire { get; set; } = string.Empty;
        public DateTime DateAvis { get; set; } = DateTime.UtcNow;

        public int ProduitId { get; set; }
        public Produit? Produit { get; set; }   

        public int ClientId { get; set; }
        public User? Client { get; set; }       
    }
}
