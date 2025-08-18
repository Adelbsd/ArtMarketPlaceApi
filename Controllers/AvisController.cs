using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtMarketPlaceAPI.Data;
using ArtMarketPlaceAPI.Models;
using Microsoft.AspNetCore.Authorization;

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

        // GET 
        [HttpGet("produit/{produitId}")]
        public async Task<ActionResult<IEnumerable<Avis>>> GetAvisByProduit(int produitId)
        {
            return await _context.Avis
                .Include(a => a.Client)
                .Where(a => a.ProduitId == produitId)
                .ToListAsync();
        }

        // POST 
        [Authorize(Roles = "Client")]
        [HttpPost("produit/{produitId}")]
        public async Task<ActionResult<Avis>> CreateAvis(int produitId, Avis avis)
        {
            var produit = await _context.Produits.FindAsync(produitId);
            if (produit == null)
                return NotFound("Produit introuvable");

            // 
            var clientId = int.Parse(User.Claims.First(c => c.Type == "id").Value);

            avis.ProduitId = produitId;
            avis.ClientId = clientId;
            avis.DateAvis = DateTime.UtcNow;

            _context.Avis.Add(avis);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAvisByProduit), new { produitId = produitId }, avis);
        }
    }
}
