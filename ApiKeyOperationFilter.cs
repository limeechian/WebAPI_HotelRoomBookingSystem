using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Microsoft.OpenApi.Models;
using IOperationFilter = Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter;
using Microsoft.OpenApi.Any;

namespace HotelRoomManagementApp
{
    public class ApiKeyOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var apiKeyAttribute = context.MethodInfo.GetCustomAttribute<ApiKeyAttribute>();

            if (apiKeyAttribute != null)
            {
                // Add the API key parameter to the operation
                operation.Parameters ??= new List<OpenApiParameter>();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "API-Key",
                    In = ParameterLocation.Header,
                    Description = "Access Token",
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string",
                        Default = new OpenApiString("") // Specify the default value here
                    },
                });
            }
        }

        
    }

}

