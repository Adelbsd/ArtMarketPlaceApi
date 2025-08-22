using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtMarketPlaceAPI.Data;
using ArtMarketPlaceAPI.Models;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }
      
[HttpGet("{customerId}/dashboard")]
public async Task<ActionResult<object>> GetCustomerDashboard(int customerId)
{
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    var commandes = await _context.Commandes
        .Include(c => c.Lignes)
            .ThenInclude(l => l.Produit)
        .Where(c => c.ClientId == customerId)
        .OrderByDescending(c => c.DateCommande)
        .ToListAsync();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.

    if (!commandes.Any())
    {
        return Ok(new
        {
            recentPurchases = new List<object>(),
            favorites = new List<object>(),
            ordersStatus = new List<object>()
        });
    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
    var dashboard = new
    {
       
        recentPurchases = commandes.Take(5).Select(c => new
        {
            id = c.Id,
            dateCommande = c.DateCommande,
            produits = c.Lignes.Select(l => new
            {
                titre = l.Produit.Titre,
                quantite = l.Quantité,
                prixUnitaire = l.PrixUnitaire
            })
        }),

        
        ordersStatus = commandes.Select(c => new
        {
            id = c.Id,
            statut = c.Statut.ToString(),
            
        })
    };
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

    return Ok(dashboard);
}


       
        [HttpGet("{customerId}/orders")]
        public async Task<ActionResult<IEnumerable<object>>> GetCustomerOrders(int customerId)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
            var commandes = await _context.Commandes
                .Include(c => c.Lignes)
                    .ThenInclude(l => l.Produit)
                .Where(c => c.ClientId == customerId)
                .Select(c => new
                {
                    c.Id,
                    c.DateCommande,
                    c.Statut,
                    c.StatutLivraison,
                    c.Total,
                    Produits = c.Lignes.Select(l => new {
                        l.Produit.Titre,
                        l.Quantité,
                        l.PrixUnitaire
                    })
                })
                .ToListAsync();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            return Ok(commandes);
        }

        [HttpPost("{customerId}/orders")]
        public async Task<ActionResult> CreateOrder(int customerId, [FromBody] CreateOrderDto dto)
        {
            if (dto == null || dto.Produits == null || !dto.Produits.Any())
                return BadRequest("La commande doit contenir au moins un produit.");

            var commande = new Commande
            {
                DateCommande = DateTime.UtcNow,
                Statut = StatutCommande.EnAttente,
                StatutLivraison = StatutLivraison.EnAttenteRetrait,
                ClientId = customerId,
                Total = 0,
                Lignes = new List<LigneCommande>()
            };

            foreach (var item in dto.Produits)
            {
                var produit = await _context.Produits.FindAsync(item.ProduitId);
                if (produit == null)
                    return BadRequest($"Produit {item.ProduitId} introuvable.");

                commande.Lignes.Add(new LigneCommande
                {
                    ProduitId = produit.Id,
                    Quantité = item.Quantité,
                    PrixUnitaire = produit.Prix
                });

                commande.Total += produit.Prix * item.Quantité;
            }

            _context.Commandes.Add(commande);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerOrders), new { customerId = customerId }, commande);
        }
    }

   
    public class CreateOrderDto
    {
        public List<CreateOrderLineDto> Produits { get; set; } = new();
    }

    public class CreateOrderLineDto
    {
        public int ProduitId { get; set; }
        public int Quantité { get; set; }
    }
}
