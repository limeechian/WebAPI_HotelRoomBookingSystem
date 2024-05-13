using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HotelRoomManagementApp
{
    public class ApiKeyDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "API-Key" // Reference the same ID used when defining the security scheme
                        }
                    },
                    new List<string>()
                }
            };

            // Apply the security requirement to all paths
            foreach (var path in swaggerDoc.Paths)
            {
                foreach (var operation in path.Value.Operations)
                {
                    operation.Value.Security = new List<OpenApiSecurityRequirement> { securityRequirement };
                }
            }

            //swaggerDoc.SecurityRequirements.Add(securityRequirement);
        }
    }
}