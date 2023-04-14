using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

            //create a new empty list of products
            Products = new List<ProductDTO>();
            Products = await Api.GetProductsAsync();
        }
    }
}
