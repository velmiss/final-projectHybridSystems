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
    public class DeleteModel : PageModel
    {

        [BindProperty]
      public ProductDTO Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null )
            {
                return NotFound();
            }

    
            return RedirectToPage("./Index");
        }
    }
}
