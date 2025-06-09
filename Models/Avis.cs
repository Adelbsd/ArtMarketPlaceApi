using ArtMarketPlaceAPI.Models;

public class Avis
{
    public int Id { get; set; }
    public int Note { get; set; }
    public required string Commentaire { get; set; }
    public DateTime DateAvis { get; set; } = DateTime.UtcNow;

    public int ProduitId { get; set; }
    public required Produit Produit { get; set; }

    public int ClientId { get; set; }
    public required User Client { get; set; }
}
