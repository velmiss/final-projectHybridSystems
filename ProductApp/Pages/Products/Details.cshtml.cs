using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    public class DetailsModel : PageModel
    {
        

      public ProductDTO Product { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null )
            {
                return NotFound();
            }


            return Page();
        }
    }
}
