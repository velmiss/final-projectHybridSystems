using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using ProductApp.Controllers;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    public class DetailsModel : PageModel
    {

        ProductApi Api;
        public ProductDTO Product { get; set; } = default!;
        public DetailsModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            Api = new ProductApi(logger, tokenAcquisition, configuration);
        }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null )
            {
                return NotFound();
            }
            else
            {
                Product = await Api.GetProductAsync(id.Value);
            }


            return Page();
        }
    }
}
