using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtMarketPlaceAPI.Data;
using ArtMarketPlaceAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    [Route("api/delivery-partners")]
    // [Authorize(Roles = "Livreur")]
    public class DeliveryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeliveryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{livreurId}/orders")]
        public async Task<ActionResult<IEnumerable<object>>> GetAssignedOrders(int livreurId)
        {
            var commandes = await _context.Commandes
                .Include(c => c.Client)
                .Include(c => c.Lignes!)
                    .ThenInclude(l => l.Produit)
                .Where(c => c.StatutLivraison != StatutLivraison.Distribuee)
                .Select(c => new
                {
                    orderId = c.Id,
                    customerName = c.Client != null ? c.Client.NomComplet : "Inconnu",
                    address = c.AdresseLivraison,
                    status = c.StatutLivraison.ToString(),
                    produits = c.Lignes!.Select(l => new
                    {
                        titre = l.Produit != null ? l.Produit.Titre : "Inconnu",
                        quantite = l.Quantité,
                        prixUnitaire = l.PrixUnitaire
                    }).ToList()
                })
                .ToListAsync();

            return Ok(commandes);
        }

[HttpPut("{livreurId}/orders/{orderId}/assign")]
public async Task<IActionResult> AssignOrder(int livreurId, int orderId)
{
    var commande = await _context.Commandes.FindAsync(orderId);
    if (commande == null) return NotFound();

    commande.LivreurId = livreurId;

    await _context.SaveChangesAsync();
    return NoContent();
}



        [HttpGet("{livreurId}/history")]
public async Task<ActionResult<IEnumerable<object>>> GetDeliveryHistory(int livreurId)
{
    var historique = await _context.Commandes
        .Include(c => c.Client)
        .Include(c => c.Lignes!)
            .ThenInclude(l => l.Produit)
        .Where(c => c.LivreurId == livreurId && c.StatutLivraison == StatutLivraison.Distribuee)
        .Select(c => new
        {
            orderId = c.Id,
            customerName = c.Client != null ? c.Client.NomComplet : "Inconnu",
            address = c.AdresseLivraison,
            date = c.DateLivraison ?? c.DateCommande,
            status = c.StatutLivraison.ToString(),
            produits = c.Lignes!.Select(l => new
            {
                titre = l.Produit != null ? l.Produit.Titre : "Inconnu",
                quantite = l.Quantité,
                prixUnitaire = l.PrixUnitaire
            }).ToList()
        })
        .ToListAsync();

    return Ok(historique);
}

        
        [HttpPut("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] StatusUpdateDto dto)
        {
            var commande = await _context.Commandes.FindAsync(orderId);
            if (commande == null)
                return NotFound();

            if (Enum.TryParse<StatutLivraison>(dto.Status, true, out var newStatus))
{
    commande.StatutLivraison = newStatus;

    if (newStatus == StatutLivraison.Distribuee)
        commande.DateLivraison = DateTime.UtcNow;

    await _context.SaveChangesAsync();
    return NoContent();
}

            return BadRequest("Statut invalide.");
        }
    }

    public class StatusUpdateDto
    {
        public required string Status { get; set; }
    }
}
