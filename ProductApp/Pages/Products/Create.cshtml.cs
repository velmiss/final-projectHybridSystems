using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Identity.Web;
using ProductApp.Controllers;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    [Authorize(Policy = "RequireContributorRole")]

	[AuthorizeForScopes(ScopeKeySection = "NoviaHybrid:ApiScopes")]

	public class CreateModel : PageModel
    {
        ProductApi Api;
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Creators;
 
        public IList<ProductDTO> Products { get;set; } = default!;

        public CreateModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration) 
        {
            Api = new ProductApi(logger, tokenAcquisition, configuration);

        }
        public async Task OnGet()
        {
            if (User.IsInRole("admin"))
            {
                //convert from a list from Api.GetCreators() to a IEnumerable<SelectListItem> that is stored in Creators
                Creators = (await Api.GetCreators(User)).Select(x => new SelectListItem(x, x));
            }

            //return Page();
        }

        [BindProperty]
        public ProductDTO Product { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!User.IsInRole("admin"))
            {
                //Save the current username as the creator of the product
                Product.Creator = User.GetDisplayName();


                //Product.Creator = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            }
            if (!ModelState.IsValid || Product == null)
            {
                return Page();
            }
            else
            {

                await Api.PostProduct(Product);
            }

           
            return RedirectToPage("./Index");
        }
    }
}
