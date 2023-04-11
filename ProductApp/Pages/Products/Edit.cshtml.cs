using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using ProductApp.Controllers;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    [Authorize(Policy = "RequireContributerRole")]

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

                //store all the creators in a list from Products.Creator to Creators
                Creators = Products.Select(x => new SelectListItem { Value = x.Creator, Text = x.Creator }).Distinct();
                //remove all the duplicates in the Creators list
                Creators = Creators.GroupBy(x => x.Value).Select(x => x.First());

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
                Product.Creator = User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
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
