using HotelRoomManagementApp.Data;
using HotelRoomManagementApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HotelRoomManagementApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [ApiKey]
    public class ClientsInfoesController : ControllerBase
    {
        private readonly LEC2023DbContext _context;

        public ClientsInfoesController(LEC2023DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a list of clients
        /// </summary>
        //[Produces("application/json")]
        [HttpGet]
        [ApiKey]
        public async Task<ActionResult<IEnumerable<ClientsInfo>>> GetClients()
        {
            if (_context.ClientsInfo == null)
            {
                return NotFound("Clients not found.");
            }
            // Define the parameter value `status` and set it to `1`
            short status = 1;

            // Call the stored procedure using `FromSqlRaw` method to execute the stored procedure with parameter named `@Status` and value
            var sp_clientGetByStatus = await _context.ClientsInfo
                .FromSqlRaw("EXEC USP_ClientsInfo_GetByStatus @Status = {0}", status)
                .ToListAsync();  // retrieve results as a list of `ClientsInfo` entities using `ToListAsync()`
            /* use `{0}` as a placeholder for the parameter value, specify the parameter name `@Status` in the SQL query and assign the value `status` using the placeholder `{0}`.
                This way, you explicitly declared the parameter name and value in the SQL query, and Entity Framework Core will correctly pass the parameter to the stored procedure.*/

            //return await _context.ClientsInfo.ToListAsync();
            return sp_clientGetByStatus;

        }
           


        /// <summary>
        /// Gets client by Client ID
        /// </summary>
        /// <remarks>
        /// Enter Client ID
        /// </remarks>
        [HttpGet("{id}")]  // GET: api/ClientsInfoes/5  // GET: api/ClientsInfoes/{id}
        [ApiKey]
        //[Produces("application/json")]
        public async Task<ActionResult<ClientsInfo>> GetClientsInfo(long id)
        {
            if (_context.ClientsInfo == null)
            {
                return NotFound();
            }

            // Define the parameter values `status` and `clientId`
            short status = 1;
            long clientId = id;

            // Call the `USP_ClientsInfo_GetByStatus` stored procedure to filter clients by status and store the result in the `storProc_clientGetByStatus` list
            var sp_clientGetByStatus = await _context.ClientsInfo
                .FromSqlRaw("EXEC USP_ClientsInfo_GetByStatus @Status = {0}", status)
                .ToListAsync();

            // Find the client by ID in the filterd by status list - use `FirstOrDefault` to find the client by `Id` in the filtered list.
            var clientGetById = sp_clientGetByStatus.FirstOrDefault(c => c.Id == clientId);

            //var clientsInfo = await _context.ClientsInfo.FindAsync(id);

            if (clientGetById == null)
            {
                return NotFound("Client ID not found.");
            }

            return clientGetById;

            /* This `GetClientsInfo()` action method first filters clients by status using stored procedure, 
             * and then filters by ID from the filtered list. If a client with the specified `id` is found, it's returned;
               otherwise, a `NotFound` response is returned. */
        }


        /// <summary>
        /// Updates client by Client ID
        /// </summary>
        /// <remarks>
        /// Enter Client ID and Request Body
        /// </remarks>
        [HttpPut("{id}")]  // PUT: api/ClientsInfoes/5
        [ApiKey]
        //[Consumes("application/json")] // Specify that the endpoint consumes JSON data
        public async Task<ActionResult<PutClientsInfoResponse>> PutClientsInfo(long id, [FromBody] PutClientsInfoRequest request)  // Use the [FromBody] attribute to indicate the data comes from the request body)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

        
            // Disable change tracking for the existing client
            _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            // Get the existing client info
            var existingClient = await _context.ClientsInfo.FindAsync(id);

            if (existingClient == null)
            {
                return NotFound("Client ID not found.");
            }
      

            try
            {
                _context.Database.ExecuteSqlRaw(
                   "EXEC USP_ClientsInfo_UpdateById " +
                   "@ClientID, @ClientName, @ClientIcPassport, @ClientPhoneNumber, " +
                   "@ClientEmail, @ClientGender, @ClientBirthDate, @ClientAddress, @ModifiedBy",
                   new SqlParameter("@ClientID", id),
                   new SqlParameter("@ClientName", request.ClientName),
                   new SqlParameter("@ClientIcPassport", request.ClientIcPassport),
                   new SqlParameter("@ClientPhoneNumber", request.ClientPhoneNumber),
                   new SqlParameter("@ClientEmail", request.ClientEmail),
                   new SqlParameter("@ClientGender", request.ClientGender),
                   new SqlParameter("@ClientBirthDate", request.ClientBirthDate),
                   new SqlParameter("@ClientAddress", request.ClientAddress),
                   new SqlParameter("@ModifiedBy", request.ModifiedBy)
               );

                //_context.Entry(request).State = EntityState.Modified;

                // Save changes to the database
                await _context.SaveChangesAsync();

                // After the update, retrieve the full record with all columns
                var updatedRecord = await _context.ClientsInfo.FindAsync(id);

                // Create a PutClientsInfoResponse object with the full record
                var response = new PutClientsInfoResponse
                {
                    Message = "Client ID updated successfully.",
                    Id = updatedRecord.Id,
                    ClientName = updatedRecord.ClientName,
                    ClientIcPassport = updatedRecord.ClientIcPassport,
                    ClientPhoneNumber = updatedRecord.ClientPhoneNumber,
                    ClientEmail = updatedRecord.ClientEmail,
                    ClientGender = updatedRecord.ClientGender,
                    ClientBirthDate = updatedRecord.ClientBirthDate,
                    ClientAddress = updatedRecord.ClientAddress,
                    CreatedAt = updatedRecord.CreatedAt,
                    CreatedBy = updatedRecord.CreatedBy,
                    ModifiedAt = updatedRecord.ModifiedAt,
                    ModifiedBy = updatedRecord.ModifiedBy,
                    Status = updatedRecord.Status
                };

                //return Ok("Client ID updated successfully.");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ErrorResponse
                {
                    Message = "An error occurred while updating the client.",
                    ErrorDetails = ex.Message // You can include more details if needed
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);

            }
            finally
            {
                // Revert the change tracking behavior to the default
                _context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
            }
        
        }

        /// <summary>
        /// Create new client
        /// </summary>
        /// <remarks>
        /// Enter ClientName, ClientIcPassport, ClientPhoneNumber, ClientEmail, ClientGender, ClientBirthDate, ClientAddress, CreatedBy
        /// </remarks>
        //[Consumes("application/json")] // Specify that the endpoint consumes JSON data
        [HttpPost]  // POST: api/ClientsInfoes
        [ApiKey]
        public async Task<ActionResult<ClientsInfo>> PostClientsInfo([FromBody] PostClientsInfoRequest request)
        {
            if (request == null)
            {
                return Problem("Invalid request data.");
            }

            try
            {
                // Call the USP_ClientsInfo_Insert stored procedure
                _context.Database.ExecuteSqlRaw(
                    "EXEC USP_ClientsInfo_Insert " +
                    "@ClientName, @ClientIcPassport, @ClientPhoneNumber, " +
                    "@ClientEmail, @ClientGender, @ClientBirthDate, @ClientAddress, @CreatedBy",
                    new SqlParameter("@ClientName", request.ClientName),
                    new SqlParameter("@ClientIcPassport", request.ClientIcPassport),
                    new SqlParameter("@ClientPhoneNumber", request.ClientPhoneNumber),
                    new SqlParameter("@ClientEmail", request.ClientEmail),
                    new SqlParameter("@ClientGender", request.ClientGender),
                    new SqlParameter("@ClientBirthDate", request.ClientBirthDate),
                    new SqlParameter("@ClientAddress", request.ClientAddress),
                    new SqlParameter("@CreatedBy", request.CreatedBy)
                );
                // Save changes to the database
                await _context.SaveChangesAsync();

                // Retrieve the newly created ClientsInfo entity
                var newlyCreatedClient = await _context.ClientsInfo
                    .OrderByDescending(c => c.Id)
                    .FirstOrDefaultAsync();

                if (newlyCreatedClient == null)
                {
                    return NotFound("Failed to retrieve the newly created client.");
                }
                // You can return an appropriate response based on your application's requirements
                return CreatedAtAction("GetClientsInfo", new { id = newlyCreatedClient.Id }, newlyCreatedClient);
            }
            catch (Exception ex)
            {
                // Handle any exceptions here and return an appropriate error response
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Deletes client by Client ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedBy"></param>
        /// <remarks>
        /// Enter Client ID, ModifiedBy
        [HttpDelete("{id}")]  // DELETE: api/ClientsInfoes/5
        [ApiKey]
        public async Task<IActionResult> DeleteClientsInfo(long id, long modifiedBy)  
            // [FromBody] ClientsInfo clientsInfo - create a class model for the request body - only update the id and modifiedby - like the one i do for staffloginRequest.cs class
        {
            if (_context.ClientsInfo == null)
            {
                return NotFound("Client ID not found.");
            }

            try
            {
                    
                // Call the `USP_ClientsInfo_DeleteById` stored procedure
                _context.Database.ExecuteSqlRaw("EXEC USP_ClientsInfo_DeleteById @ClientID, @ModifiedBy",
                    new SqlParameter("@ClientID", id), new SqlParameter("@ModifiedBy", modifiedBy));

                // Save changes to the database
                await _context.SaveChangesAsync();

                return Ok("Client ID deleted successfully.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions here and return an appropriate error response
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
                     
        }
    }

