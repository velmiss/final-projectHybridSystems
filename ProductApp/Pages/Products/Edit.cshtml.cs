using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Web;
using ProductApp.Controllers;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    [Authorize(Policy = "RequireContributorRole")]

	[AuthorizeForScopes(ScopeKeySection = "NoviaHybrid:ApiScopes")]

	public class EditModel : PageModel
    {

        [BindProperty]
        public ProductDTO Product { get; set; } = default!;
        ProductApi Api;
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Creators;
        public IList<ProductDTO> Products { get; set; } = default!;
        public EditModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            Api = new ProductApi(logger, tokenAcquisition, configuration);
        }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (User.IsInRole("admin"))
            {
                Products = new List<ProductDTO>();
                Products = await Api.GetProductsAsync();

                //convert from a list from Api.GetCreators() to a IEnumerable<SelectListItem> that is stored in Creators
                Creators = (await Api.GetCreators(User)).Select(x => new SelectListItem(x, x));

            }
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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (!User.IsInRole("admin"))
            {
                Product.Creator = User.Identity.Name;
                //Product.Creator = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            }
            if (!ModelState.IsValid|| id == null)
            {
                return Page();
            }
            else
            {
                await Api.UpdateProduct(id.Value,Product);
            }



            return RedirectToPage("./Index");
        }


    }
}
