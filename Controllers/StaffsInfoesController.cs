using Azure.Core;
using HotelRoomManagementApp.Data;
using HotelRoomManagementApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Options;

namespace HotelRoomManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class StaffsInfoesController : ControllerBase
    {
        
        private readonly LEC2023DbContext _context;
        //private readonly MyConfiguration _myConfiguration;
        private readonly IConfiguration _configuration;

        //private readonly ApiKeyAuthenticationOptions _apiKeyAuthenticationOptions;
        public StaffsInfoesController(LEC2023DbContext context, /*MyConfiguration myConfiguration,*/ IConfiguration configuration/*, IOptions<ApiKeyAuthenticationOptions> apiKeyAuthenticationOptions*/)
        {

            _context = context;
            //_myConfiguration = myConfiguration;
            _configuration = configuration;
            //_apiKeyAuthenticationOptions = apiKeyAuthenticationOptions.Value; // Extract the options using .Value
        }

        
        [HttpPost("Login")]
        //[AllowAnonymous] // Allow unauthenticated access to the Login action
        [ApiKey]
        public async Task<ActionResult<StaffLoginResponse>> Login([FromBody] StaffLoginRequest request)
        {
            try
            {
                // Validate username and password (add your validation logic here)
                var isValidUser = await ValidateUserAsync(request.Username, request.Password);

                if (isValidUser)
                {
                    // Retrieve the user's ID from the database based on their username 
                    int userId = await GetUserIdAsync(request.Username);

                    var apiKey = _configuration["ApiKeySetting:ApiKey"]; // This is the api key as a string
                    string encryptedApiKey = EncryptApiKey(apiKey); // Convert the string to byte array


                    // Return the token to the client
                    return Ok("Authentication Successful.");
                    //return Ok("Authentication Successful. This is the Api-Key: " + encryptedApiKey);
                }
                else
                {
                    // Authentication failed
                    return Unauthorized("Authentication failed. Invalid username or password.");
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, and return an appropriate response
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        private async Task<bool> ValidateUserAsync(string username, string password)
        {
            try
            {
                // Create parameters for the stored procedure
                var usernameParam = new SqlParameter("@Username", username);
                var passwordParam = new SqlParameter("@Password", password);
                var resultCodeParam = new SqlParameter("@ResultCode", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                // Call the `USP_StaffsInfo_Login` stored procedure with parameters
                await _context.Database.ExecuteSqlRawAsync("EXEC USP_StaffsInfo_Login @Username, @Password, @ResultCode OUTPUT",
                    usernameParam,
                    passwordParam,
                    resultCodeParam);

                int resultCode = Convert.ToInt32(resultCodeParam.Value);

                if (resultCode == 0)
                {
                    return true;  // Return true if validation is successful
                }
                else
                {
                    return false; // Return false if validation fails
                }
            }
            catch
            {
                // Handle exceptions, log errors, and return false (validation failed)
                return false;
            }
        }

        private async Task<int> GetUserIdAsync(string username)
        {
            // Query your database to retrieve the user's ID based on their username
            var user = await _context.StaffsInfo.SingleOrDefaultAsync(x => x.Username == username);

            if (user != null)
            {
                return (int)user.Id; // Assuming the user's ID is stored in a property called "Id"
            }

            return -1; // Return -1 or an appropriate value if the user is not found
        }

        //Implement the encryption logic here
        private string EncryptApiKey(string apiKey)
        {
            var encryptionService = new EncryptionService(); // Create an instance of EncryptionService
            var encryptedApiKey = encryptionService.Encrypt(apiKey);
            return encryptedApiKey;
        }

    }
}
