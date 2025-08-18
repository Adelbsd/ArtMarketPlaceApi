namespace ArtMarketPlaceAPI.Dtos
{
    public class AvisDto
    {
        public int Note { get; set; }
        public string Commentaire { get; set; } = string.Empty;
        public int ProduitId { get; set; }
        public int ClientId { get; set; }
    }
}
