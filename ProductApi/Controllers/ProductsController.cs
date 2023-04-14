using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using ProductApi.Data;
using ProductApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Web;
using NuGet.Protocol;

namespace ProductApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductDBContext _context;
        private string role;

        public ProductsController(ProductDBContext context)
        {
            _context = context;



        }

        // GET: api/Products
        [HttpGet(Name = "GetProducts")]
        public async Task<List<ProductDTO>> GetProducts()
        {
            if(!User.IsInRole("admin"))
            {
                // get the username of the user and store it in id
                var id = User.GetDisplayName();
                //get all the products where the creator is the same as the user
                return await _context.Products.Where(x => x.Creator == User.GetDisplayName()).ToListAsync();
			}
			else
            {
                if (_context.Products == null)
                {
                    return null;
                }
                return await _context.Products.ToListAsync();
            }
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
        public async Task<ActionResult<ProductDTO>> PostProductDTO(ProductDTO product)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ProductDBContext.Products'  is null.");
            }


            //if the user logged in is not an admin
            if (!User.IsInRole("admin"))
            {
                //var id = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                var id = User.GetDisplayName();
                //set the product to be owned by the current user
                if (product.Creator != id) {
                    //return an error
                    return Problem("You are not authorized to create a product for another user.");
                }
                product.Creator = id;
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductDTO", new { id = product.Id }, product);
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
