using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtMarketPlaceAPI.Data;
using ArtMarketPlaceAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    [Route("api/delivery-partners")]
    [Authorize(Roles = "Livreur")]
    public class DeliveryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DeliveryController(AppDbContext context)
        {
            _context = context;
        }

        //  Livraisons en cours
        [HttpGet("{livreurId}/orders")]
        public async Task<ActionResult<IEnumerable<object>>> GetAssignedOrders(int livreurId)
        {
            var commandes = await _context.Commandes
                .Include(c => c.Client)
                .Where(c => c.LivreurId == livreurId && c.StatutLivraison != StatutLivraison.Distribuée)
                .Select(c => new
                {
                    orderId = c.Id,
                    customerName = c.Client.NomComplet,
                    address = c.AdresseLivraison,
                    status = c.StatutLivraison.ToString()
                })
                .ToListAsync();

            return Ok(commandes);
        }

        //  Historique
        [HttpGet("{livreurId}/history")]
        public async Task<ActionResult<IEnumerable<object>>> GetDeliveryHistory(int livreurId)
        {
            var historique = await _context.Commandes
                .Include(c => c.Client)
                .Where(c => c.LivreurId == livreurId && c.StatutLivraison == StatutLivraison.Distribuée)
                .Select(c => new
                {
                    orderId = c.Id,
                    customerName = c.Client.NomComplet,
                    address = c.AdresseLivraison,
                    date = c.DateLivraison ?? c.DateCommande, // si pas de date livraison, fallback sur date commande
                    status = c.StatutLivraison.ToString()
                })
                .ToListAsync();

            return Ok(historique);
        }

        //  Mise à jour du statut
        [HttpPut("orders/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] StatusUpdateDto dto)
        {
            var commande = await _context.Commandes.FindAsync(orderId);
            if (commande == null)
                return NotFound();

            if (Enum.TryParse<StatutLivraison>(dto.Status, out var newStatus))
            {
                commande.StatutLivraison = newStatus;

                //  si la commande est livrée, on ajoute une date de livraison
                if (newStatus == StatutLivraison.Distribuée)
                {
                    commande.DateLivraison = DateTime.UtcNow;
                }

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
