using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Web;
using ProductApp.Controllers;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    public class CreateModel : PageModel
    {
        ProductApi Api;
        public CreateModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration) 
        {
            Api = new ProductApi(logger, tokenAcquisition, configuration);
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductDTO Product { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
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
