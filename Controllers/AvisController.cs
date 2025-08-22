using ArtMarketPlaceAPI.Data;
using ArtMarketPlaceAPI.Dtos;
using ArtMarketPlaceAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvisController(AppDbContext context)
        {
            _context = context;
        }

        
        [Authorize(Roles = "Client")]
        [HttpPost("produit/{produitId}")]
        public async Task<IActionResult> AjouterAvis(int produitId, [FromBody] AvisDto dto)
        {
           
            var clientIdClaim = User.FindFirst("id")?.Value;
            if (clientIdClaim == null) return Unauthorized("Client non reconnu.");

            int clientId = int.Parse(clientIdClaim);

           
            var produit = await _context.Produits.FindAsync(produitId);
            if (produit == null) return NotFound("Produit introuvable.");

           
            var avis = new Avis
            {
                Note = dto.Note,
                Commentaire = dto.Commentaire,
                ProduitId = produitId,
                ClientId = clientId,
                DateAvis = DateTime.UtcNow
            };

            _context.Avis.Add(avis);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Avis ajouté avec succès ✅", avis });
        }

        
        [HttpGet("produit/{produitId}")]
        public async Task<IActionResult> GetAvisProduit(int produitId)
        {
            var avisList = await _context.Avis
                .Where(a => a.ProduitId == produitId)
                .Include(a => a.Client)
                .Select(a => new
                {
                    a.Id,
                    a.Note,
                    a.Commentaire,
                    a.DateAvis,
                    ClientNom = a.Client.NomComplet
                })
                .ToListAsync();

            return Ok(avisList);
        }
    }
}
