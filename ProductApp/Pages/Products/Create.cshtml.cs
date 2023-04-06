using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    public class CreateModel : PageModel
    {
       
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

           
            return RedirectToPage("./Index");
        }
    }
}
