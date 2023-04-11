using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using ProductApp.Controllers;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    //authorize the user for the role admin
    [Authorize(Policy = "RequireAdminRole")]

	[AuthorizeForScopes(ScopeKeySection = "NoviaHybrid:ApiScopes")]
	public class DeleteModel : PageModel
    {
        ProductApi Api;
        public DeleteModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            Api = new ProductApi(logger, tokenAcquisition, configuration);
        }
        [BindProperty]
      public ProductDTO Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                Product = await Api.GetProductAsync(id.Value);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                await Api.DeleteProduct(id.Value);
            }

    
            return RedirectToPage("./Index");
        }
    }
}
