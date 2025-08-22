using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtMarketPlaceAPI.Data;
using ArtMarketPlaceAPI.Models;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    [Route("api/customers/{customerId}/cart")]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }


        public class AddToCartDto
        {
            public int ProduitId { get; set; }
            public int Quantite { get; set; } = 1;
        }

        public class CheckoutDto
        {
            public string ShippingAddress { get; set; } = "";
            public string PaymentMethod { get; set; } = "Carte";
        }

      
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int customerId, [FromBody] AddToCartDto dto)
        {
            if (dto.Quantite <= 0) dto.Quantite = 1;

            var clientExists = await _context.Users.AnyAsync(u => u.Id == customerId);
            if (!clientExists) return NotFound("Client introuvable.");

            var produit = await _context.Produits.FindAsync(dto.ProduitId);
            if (produit == null) return NotFound("Produit introuvable.");

            
            var existing = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.ClientId == customerId && ci.ProduitId == dto.ProduitId);

            if (existing != null)
            {
                existing.Quantite += dto.Quantite;
            }
            else
            {
                var item = new CartItem
                {
                    ClientId = customerId,
                    ProduitId = dto.ProduitId,
                    Quantite = dto.Quantite,
                 
                    Client = await _context.Users.FirstAsync(u => u.Id == customerId),
                    Produit = produit
                };
                _context.CartItems.Add(item);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Ajouté au panier." });
        }

     
        [HttpGet]
        public async Task<IActionResult> GetCart(int customerId)
        {
            var cart = await _context.CartItems
                .Where(c => c.ClientId == customerId)
                .Include(c => c.Produit)
                .ToListAsync();

            return Ok(cart.Select(c => new
            {
                c.Id,
                c.ProduitId,
                produit = new
                {
                    c.Produit.Id,
                    c.Produit.Titre,
                    c.Produit.ImageUrl,
                    c.Produit.Categorie,
                    c.Produit.Description,
                    c.Produit.Prix
                },
                c.Quantite
            }));
        }

       
        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int customerId, int cartItemId)
        {
            var item = await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.Id == cartItemId && ci.ClientId == customerId);

            if (item == null) return NotFound();

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

      
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart(int customerId)
        {
            var items = await _context.CartItems
                .Where(ci => ci.ClientId == customerId)
                .ToListAsync();

            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return NoContent();
        }

       
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(int customerId, [FromBody] CheckoutDto dto)
        {
            var cart = await _context.CartItems
                .Include(c => c.Produit)
                .Where(c => c.ClientId == customerId)
                .ToListAsync();

            if (!cart.Any()) return BadRequest("Panier vide.");

          
            var total = cart.Sum(c => (decimal)c.Produit.Prix * c.Quantite);

            var commande = new Commande
            {
                ClientId = customerId,
                DateCommande = DateTime.UtcNow,
                Statut = StatutCommande.EnAttente,
                StatutLivraison = StatutLivraison.EnAttenteRetrait,
                AdresseLivraison = dto.ShippingAddress,
                Total = total,
                Client = await _context.Users.FirstAsync(u => u.Id == customerId),
                Lignes = cart.Select(c => new LigneCommande
                {
                    ProduitId = c.ProduitId,
                    Produit = c.Produit,
                    Quantité = c.Quantite,
                    PrixUnitaire = (decimal)c.Produit.Prix
                }).ToList()
            };

            _context.Commandes.Add(commande);
            _context.CartItems.RemoveRange(cart);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Commande confirmée",
                commandeId = commande.Id,
                total = commande.Total,
                statut = commande.Statut.ToString()
            });
        }
    }
}
