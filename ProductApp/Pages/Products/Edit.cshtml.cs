using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using ProductApp.Controllers;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    public class EditModel : PageModel
    {

        [BindProperty]
        public ProductDTO Product { get; set; } = default!;
        ProductApi Api;
        public EditModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            Api = new ProductApi(logger, tokenAcquisition, configuration);
        }

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(long? id)
        {
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
