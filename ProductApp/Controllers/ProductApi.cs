using Azure;
using Microsoft.Identity.Web;
using ProductApp.Models;
using ProductApp.Pages.Products;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ProductApp.Controllers
{
    public class ProductApi
    {
        private string httpConnect = "https://localhost:7045/api";
        private HttpClient mClient;

        public List<ProductDTO> products { get; set; } = default!;
        
        private readonly string mApiScopes = string.Empty;
        private readonly string mApiAccessAsUserScope = string.Empty;
        private readonly string mApiBaseAddress = string.Empty;
        private readonly ITokenAcquisition mTokenAcquisition;

        private readonly ILogger<IndexModel> _logger;

        public ProductApi()
        {
            /*
            ILogger<IndexModel> logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<IndexModel>>();

            ITokenAcquisition tokenAcquisition = builder.Services.BuildServiceProvider()
                .GetRequiredService<ITokenAcquisition>();


            */    
            IConfiguration configuration;
            //gett the configuration from appsettings.json and store it in the configuration variable
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            
            mClient = new HttpClient();
            mClient.DefaultRequestHeaders.Add("Accept", "application/json");
            mClient.DefaultRequestHeaders.Add("User-Agent", "ProductApp");

            //_logger = logger;
            mClient = new HttpClient();
            //mTokenAcquisition = tokenAcquisition;
            mApiScopes = configuration["NoviaHybrid:ApiScopes"];
            mApiAccessAsUserScope = mApiScopes.Split(' ').Where(instring => instring.Contains("access_as_user")).First();
            mApiBaseAddress = configuration["NoviaHybrid:ApiBaseAddress"];
            //
            //Add Novia hybrid in appsettings.json!
            //
            
            // We need to initialize every time, stateless calls
            //products = new List<ProductDTO>();
        }

        //api that connects to https://localhost:7045
        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            //await PrepareAuthenticatedClient(mApiAccessAsUserScope);
            products = new List<ProductDTO>();

            var responseTask = await mClient.GetAsync($"{mApiBaseAddress}/api/Products");
            if (responseTask.StatusCode == HttpStatusCode.OK)
            {
                
                var readTask = await responseTask.Content.ReadFromJsonAsync<List<ProductDTO>>();
                
                if (readTask == null)
                {
                    products = new List<ProductDTO>();
                }
                products = readTask;
            }
            else //web api sent error response 
            {
                //log response status here..

                products = Enumerable.Empty<ProductDTO>().ToList();

            }
            return products;
        }




        private async Task PrepareAuthenticatedClient(string ApiScopeToUse)
        {
            var accessToken = await mTokenAcquisition.GetAccessTokenForUserAsync(new[] { ApiScopeToUse });
            Debug.WriteLine($"access token-{accessToken}");
            mClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            mClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
