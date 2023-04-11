using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using ProductApp.Controllers;
using ProductApp.Models;

namespace ProductApp.Pages.Products
{
    [Authorize(Policy = "RequireMemberRole")]
    [AuthorizeForScopes(ScopeKeySection = "NoviaHybrid:ApiScopes")]
    public class IndexModel : PageModel
    {
        

        public IList<ProductDTO> Products { get;set; } = default!;
        ProductApi Api;
        public IndexModel(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {
            Api = new ProductApi(logger,tokenAcquisition,configuration);
        }

        public async Task OnGetAsync()
        {

            if (User.IsInRole("contributor"))
            {
            }
            //create a new instance of the ProductApi class

            //create a new empty list of products
            Products = new List<ProductDTO>();
            Products = await Api.GetProductsAsync();
        }
    }
}
