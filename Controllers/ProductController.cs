using Microsoft.AspNetCore.Mvc;
using ArtMarketPlaceAPI.Models;
using ArtMarketPlaceAPI.DTO;
using ArtMarketPlaceAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class ProduitsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProduitsController(AppDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produit>>> GetAllProducts(
            string? search, string? sortBy, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Produits
                .Include(p => p.Artisan)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Titre.Contains(search) || p.Description.Contains(search));

            if (minPrice.HasValue) query = query.Where(p => p.Prix >= minPrice);
            if (maxPrice.HasValue) query = query.Where(p => p.Prix <= maxPrice);

            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Prix),
                "price_desc" => query.OrderByDescending(p => p.Prix),
                _ => query.OrderBy(p => p.Titre)
            };

            return await query.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Produit>> GetProduitById(int id)
        {
            var produit = await _context.Produits
                .Include(p => p.Artisan)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (produit == null)
                return NotFound();

            return produit;
        }

      
        [HttpPost]
        public async Task<ActionResult<Produit>> CreateProduit([FromBody] CreateProduitDto dto)
        {
            if (dto == null)
                return BadRequest("Produit invalide");

            var produit = new Produit
            {
                Titre = dto.Titre,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,
                Prix = dto.Prix,
                Categorie = dto.Categorie,
                Stock = dto.Stock,
                DateAjout = DateTime.Now
            };

            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduitById), new { id = produit.Id }, produit);
        }

        
        [HttpGet("/api/artisans/{artisanId}/produits")]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProductsByArtisan(int artisanId)
        {
            var artisan = await _context.Users.FindAsync(artisanId);
            if (artisan == null || artisan.Role != UserRole.Artisan)
                return NotFound("Artisan introuvable");

            var produits = await _context.Produits
                .Where(p => p.ArtisanId == artisanId)
                .Include(p => p.Artisan)
                .ToListAsync();

            return Ok(produits);
        }

       
        [HttpPost("/api/artisans/{artisanId}/produits")]
        public async Task<ActionResult<Produit>> CreateProductForArtisan(int artisanId, [FromBody] CreateProduitDto dto)
        {
            var artisan = await _context.Users.FindAsync(artisanId);
            if (artisan == null || artisan.Role != UserRole.Artisan)
                return NotFound("Artisan introuvable");

            var produit = new Produit
            {
                Titre = dto.Titre,
                ImageUrl = dto.ImageUrl,
                Description = dto.Description,
                Prix = dto.Prix,
                Categorie = dto.Categorie,
                Stock = dto.Stock,
                DateAjout = DateTime.Now,
                ArtisanId = artisanId
            };

            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduitById), new { id = produit.Id }, produit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Produit produit)
        {
            if (id != produit.Id)
                return BadRequest();

            _context.Entry(produit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Produits.Any(p => p.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

  
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
                return NotFound();

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
