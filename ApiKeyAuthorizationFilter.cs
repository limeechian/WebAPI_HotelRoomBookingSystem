using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HotelRoomManagementApp
{
    public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
    {

        private readonly IConfiguration _configuration;
    

        public ApiKeyAuthorizationFilter(IConfiguration configuration)
        {
            _configuration = configuration;
           
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // Check if the [ApiKey] attribute is applied to the action or controller
            var hasApiKeyAttribute = context.ActionDescriptor.EndpointMetadata.OfType<ApiKeyAttribute>().FirstOrDefault();

            if (hasApiKeyAttribute != null)
            {
                // Retrieve the expected API key from configuration
                var apiKeyFromConfig = _configuration["ApiKeySetting:ApiKey"];
                //var expirationHours = int.Parse(_configuration["ApiKeySetting:ExpirationHours"]);

                // Calculate the expiration date
                //var apiKeyExpiration = DateTime.Now.AddHours(expirationHours);


                var apiKeyFromRequest = context.HttpContext.Request.Headers["API-Key"];

                var apiKeyFromRequestString = DecryptApiKey(apiKeyFromRequest);

                // Perform your API key validation logic here
                //var apiKey = context.HttpContext.Request.Headers["API-Key"];

                if (!IsValidApiKey(apiKeyFromConfig, apiKeyFromRequestString))
                {
                    context.Result = new UnauthorizedResult();
                    return Task.CompletedTask;
                  
                }
            }
            else
            { 
                context.Result = new UnauthorizedResult();
                return Task.CompletedTask;
            }
            return Task.CompletedTask;

        }

        // Implement your API key validation logic here
        private static bool IsValidApiKey(string apiKeyFromConfig, string apiKeyFromRequestString)
        {
            try
            {
                // Check if apiKey is valid (e.g., compare it with a stored value)
                // Return true if valid, false if not
                
                if (apiKeyFromRequestString == apiKeyFromConfig)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        //Implement the decryption logic here
        private static string DecryptApiKey(string apiKeyFromRequest)
        {

            var encryptionService = new EncryptionService();
            var decryptedApiKey = encryptionService.Decrypt(apiKeyFromRequest);
            return decryptedApiKey;
        }
    }
}
