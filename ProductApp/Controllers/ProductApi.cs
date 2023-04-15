using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using ProductApp.Models;
using ProductApp.Pages.Products;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.Graph;
using Azure.Identity;

namespace ProductApp.Controllers
{
    [AuthorizeForScopes(ScopeKeySection = "NoviaHybrid:ApiScopes")]
    [Authorize]
    public class ProductApi
    {
        private HttpClient mClient;

        public List<ProductDTO> products { get; set; } = default!;
        
        private readonly string mApiScopes = string.Empty;
        private readonly string mApiAccessAsUserScope = string.Empty;
        private readonly string mApiBaseAddress = string.Empty;
        private ITokenAcquisition mTokenAcquisition;
        private IConfiguration mConfiguration;
        private readonly ILogger<IndexModel> _logger;
        private string token;

        public ProductApi(ILogger<IndexModel> logger, ITokenAcquisition tokenAcquisition, IConfiguration configuration)
        {

            _logger = logger;
            mTokenAcquisition = tokenAcquisition;

            
            mClient = new HttpClient();
            mClient.DefaultRequestHeaders.Add("Accept", "application/json");
            mClient.DefaultRequestHeaders.Add("User-Agent", "ProductApp");
            
            mClient = new HttpClient();
            mApiScopes = configuration["NoviaHybrid:ApiScopes"];
            mApiAccessAsUserScope = mApiScopes.Split(' ').Where(instring => instring.Contains("access_as_user")).First();
            mApiBaseAddress = configuration["NoviaHybrid:ApiBaseAddress"];
            mConfiguration = configuration;
            
        }

        public async Task<List<ProductDTO>> GetProductsAsync()
        {
            await PrepareAuthenticatedClient(mApiAccessAsUserScope);
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

        public async Task<ProductDTO> GetProductAsync(long id)
        {
            await PrepareAuthenticatedClient(mApiAccessAsUserScope);
            ProductDTO product = new ProductDTO();

            var responseTask = await mClient.GetAsync($"{mApiBaseAddress}/api/Products/{id}");
            if (responseTask.StatusCode == HttpStatusCode.OK)
            {
                var readTask = await responseTask.Content.ReadFromJsonAsync<ProductDTO>();
                if (readTask == null)
                {
                    product = new ProductDTO();
                }
                product = readTask;
            }
            else //web api sent error response 
            {
                //log response status here..

                product = new ProductDTO();

            }
            return product;
        }

        public async Task<bool> PostProduct(ProductDTO product)
        {

            await PrepareAuthenticatedClient(mApiAccessAsUserScope);
            var responseTask = await mClient.PostAsJsonAsync($"{mApiBaseAddress}/api/Products/", product);
            if (responseTask.StatusCode == HttpStatusCode.Created)
            {
                return true;
                //log response status here..
            }
            else //web api sent error response 
            {
                return false;
                //log response status here..
            }
        }
       
        

        public async Task<bool> UpdateProduct(long id, ProductDTO product)
        {
            await PrepareAuthenticatedClient(mApiAccessAsUserScope);
            var responseTask = await mClient.PutAsJsonAsync($"{mApiBaseAddress}/api/Products/"+ id , product);
            if(responseTask.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteProduct(long id)
        {
            await PrepareAuthenticatedClient(mApiAccessAsUserScope);
            var responseTask = await mClient.DeleteAsync($"{mApiBaseAddress}/api/Products/"+id);
            if (responseTask.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //get Creators from Azure AD group Product method should recive the User that is currently logged in
        public async Task<List<string>> GetCreators(System.Security.Claims.ClaimsPrincipal user)
        {
            List<string> creators = new List<string>();
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var tenantId = mConfiguration["AzureAd:TenantId"];
            var clientId = mConfiguration["AzureAd:ClientId"];
            var clientSecret = mConfiguration["AzureAd:ClientSecret"];
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };
            
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);

            var members = await graphClient.Groups["7e3728b6-e968-4695-a517-9e7d070848a7"].Members.GraphUser.GetAsync();

            foreach (var member in members.Value)
            {

                creators.Add(member.UserPrincipalName);
            }
            return creators;
        }
            
        private async Task PrepareAuthenticatedClient(string ApiScopeToUse)
        {
            //this returns System.NullReferenceException: 'Object reference not set to an instance of an object.'
            
            var accessToken = await mTokenAcquisition.GetAccessTokenForUserAsync(new[] { ApiScopeToUse });
            Debug.WriteLine($"access token-{accessToken}");
            token = accessToken;
            mClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            mClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
