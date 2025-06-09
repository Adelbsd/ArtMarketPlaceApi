using ArtMarketPlaceAPI.Models;

public class Produit
{
    public int Id { get; set; }
    public required string Titre { get; set; }
    public required string Description { get; set; }
    public float Prix { get; set; }
    public required string Categorie { get; set; }
    public int Stock { get; set; } = 1;
    public DateTime DateAjout { get; set; } = DateTime.UtcNow;

    public int ArtisanId { get; set; }
    public required User Artisan { get; set; }

    public ICollection<LigneCommande>? LigneCommandes { get; set; }
    public ICollection<Avis>? Avis { get; set; }
}
