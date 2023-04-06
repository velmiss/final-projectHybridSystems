using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApp.Models;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDBContext _context;
        private string role;

        public ProductsController(ProductDBContext context)
        {
            _context = context;

            //check the current users AD role and store it in the string role
            if (User.IsInRole("admin"))
            {
                role = "admin";
            }
            else if (User.IsInRole("member"))
            {
                role = "member";
            }
            else if (User.IsInRole("contributor"))
            {
                role = "contributor";
            }
            else
                role = "guest";

        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts()
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductDTO(long id)
        {
          if (_context.Products == null)
          {
              return NotFound();
          }
            var productDTO = await _context.Products.FindAsync(id);

            if (productDTO == null)
            {
                return NotFound();
            }

            return productDTO;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductDTO(long id, ProductDTO productDTO)
        {
            if (id != productDTO.Id)
            {
                return BadRequest();
            }

            _context.Entry(productDTO).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductDTOExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> PostProductDTO(ProductDTO productDTO)
        {
          if (_context.Products == null)
          {
              return Problem("Entity set 'ProductDBContext.Products'  is null.");
          }
            _context.Products.Add(productDTO);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductDTO", new { id = productDTO.Id }, productDTO);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDTO(long id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var productDTO = await _context.Products.FindAsync(id);
            if (productDTO == null)
            {
                return NotFound();
            }

            _context.Products.Remove(productDTO);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductDTOExists(long id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
