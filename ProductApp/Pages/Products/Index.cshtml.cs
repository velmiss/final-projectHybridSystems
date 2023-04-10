using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class IndexModel : PageModel
    {
        

        public IList<ProductDTO> Products { get;set; } = default!;

        public async Task OnGetAsync()
        {
        

            //create a new instance of the ProductApi class
            ProductApi Api = new ProductApi();
            //create a new empty list of products
            Products = new List<ProductDTO>();
            Products = await Api.GetProductsAsync();
        }
    }
}
