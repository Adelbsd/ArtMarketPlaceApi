using Microsoft.AspNetCore.Mvc;
using ArtMarketPlaceAPI.Models;
using ArtMarketPlaceAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ArtMarketPlaceAPI.Controllers
{
    [ApiController]
    [Route("api/Produits")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProducts()
        {
            return await _context.Produits.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Produit>> CreateProduit(Produit Produit)
        {
            _context.Produits.Add(Produit);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduits), new { id = Produit.Id }, Produit);
        }

        private object GetProduits()
        {
            throw new NotImplementedException();
        }
    }
}
