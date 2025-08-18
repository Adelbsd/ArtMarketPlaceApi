using Microsoft.AspNetCore.Mvc;
using ArtMarketPlaceAPI.Models;
using ArtMarketPlaceAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        //  GET : api/produits (avec recherche/filtre/tri)
        [HttpGet("api/produits")]
        public async Task<ActionResult<IEnumerable<Produit>>> GetAllProducts(
            string? search, string? sortBy, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Produits
                .Include(p => p.Artisan)
                .AsQueryable();

            // 🔍 Recherche par titre ou description
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Titre.Contains(search) || p.Description.Contains(search));

            // 💰 Filtrage par prix
            if (minPrice.HasValue) query = query.Where(p => p.Prix >= minPrice);
            if (maxPrice.HasValue) query = query.Where(p => p.Prix <= maxPrice);

            // ↕️ Tri
            query = sortBy switch
            {
                "price_asc" => query.OrderBy(p => p.Prix),
                "price_desc" => query.OrderByDescending(p => p.Prix),
                _ => query.OrderBy(p => p.Titre) // par défaut : tri alphabétique
            };

            return await query.ToListAsync();
        }

        //  GET : api/artisans/{artisanId}/products
        [HttpGet("api/artisans/{artisanId}/products")]
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

        //  POST : api/artisans/{artisanId}/products
        [HttpPost("api/artisans/{artisanId}/products")]
        public async Task<ActionResult<Produit>> CreateProductForArtisan(int artisanId, Produit produit)
        {
            var artisan = await _context.Users.FindAsync(artisanId);
            if (artisan == null || artisan.Role != UserRole.Artisan)
                return NotFound("Artisan introuvable");

            produit.ArtisanId = artisanId;
            _context.Produits.Add(produit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductsByArtisan),
                new { artisanId = artisanId }, produit);
        }

        //  PUT : api/produits/{id}
        [HttpPut("api/produits/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Produit produit)
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

        // DELETE : api/produits/{id}
        [HttpDelete("api/produits/{id}")]
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
