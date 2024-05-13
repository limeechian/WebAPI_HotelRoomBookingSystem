using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

/* Create a simple API controller in ASP.NET Core that accepts two values, 
   sums them together, and returns the result in JSON format. */


namespace HotelRoomManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("add")]  // This attribute specifies that this action can be accessed via an HTTP GET request to the URL `/api/Values/add`.
        public IActionResult AddValues(int value1, int value2)  /* The `AddValues` action method takes two integer parameters, `value1` and `value2`, which represent the values you want to add together. */
        {
        /* Inside the `AddValues` action method, it performs the addition operation and creates an anonymous object to structure the response data. */
            
            // Perform the addition operation
            int result = value1 + value2;

            // Create an anonymous object with the result
            var response = new
            {
                Value1 = value1,
                Value2 = value2,
                Sum = result
            };

            // Return the result in JSON format
            return Ok(response);  /* The `Ok` method is used to return an HTTP 200 OK response with the result in JSON format. */
        }
    }
}

/* When you run your ASP.NET Core application, and access the Swagger UI, you should see an endpoint for the `AddValues` action where you can input two values,
   and it will return the sum in JSON format. */
