using ArtMarketPlaceAPI.Models;

public class Commande
{
    public int Id { get; set; }
    public DateTime DateCommande { get; set; } = DateTime.UtcNow;
    public StatutCommande Statut { get; set; } = StatutCommande.EnAttente;
    public decimal Total { get; set; }

    public int ClientId { get; set; }
    public required User Client { get; set; }

    public int? LivreurId { get; set; }
    public User? Livreur { get; set; }

    public StatutLivraison? StatutLivraison { get; set; }

    public ICollection<LigneCommande>? Lignes { get; set; }
}
namespace ArtMarketPlaceAPI.Models
{
    public class LigneCommande
    {
        public int Id { get; set; }

        public int CommandeId { get; set; }
        public required Commande Commande { get; set; }

        public int ProduitId { get; set; }
        public required Produit Produit { get; set; }

        public int Quantité { get; set; }
        public decimal PrixUnitaire { get; set; }
    }
}

