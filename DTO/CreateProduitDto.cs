namespace ArtMarketPlaceAPI.Models
{
    public class CreateProduitDto
    {
        public string Titre { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Prix { get; set; }
        public string Categorie { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
