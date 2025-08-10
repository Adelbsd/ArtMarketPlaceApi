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

        // GET : api/produits
        [HttpGet("api/produits")]
        public async Task<ActionResult<IEnumerable<Produit>>> GetAllProducts()
        {
            return await _context.Produits
                .Include(p => p.Artisan)
                .ToListAsync();
        }

        // GET : api/artisans/{artisanId}/products
        [HttpGet("api/artisans/{artisanId}/products")]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProductsByArtisan(int artisanId)
        {
            var artisan = await _context.Users.FindAsync(artisanId);
            if (artisan == null || artisan.Role != UserRole.Artisan)
                return NotFound("Artisan introuvable");

            var produits = await _context.Produits
                .Where(p => p.ArtisanId == artisanId)
                .ToListAsync();

            return produits;
        }

        // POST : api/artisans/{artisanId}/products
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

        // PUT : api/produits/{id}
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
