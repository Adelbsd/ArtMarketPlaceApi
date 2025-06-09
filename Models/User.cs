namespace ArtMarketPlaceAPI.Models
{
    public enum UserRole
    {
        Artisan,
        Client,
        Livreur,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public required string NomComplet { get; set; }
        public required string Email { get; set; }
        public required byte[] MdpHash { get; set; }
        public required byte[] MdpSalt { get; set; }
        public UserRole Role { get; set; }

        
      public ICollection<Commande> CommandesClient { get; set; } = new List<Commande>();
public ICollection<Commande> CommandesLivreur { get; set; } = new List<Commande>();
public ICollection<Produit> Produits { get; set; } = new List<Produit>();

    }
}
