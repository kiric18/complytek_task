using EmployeeManagement.Application.Exceptions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EmployeeManagement.Api.Filters
{
    public class CustomExceptionResponseFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // 1. Define the standardized error response model for Swagger documentation
            var errorResponseSchema = new OpenApiSchema
            {
                Type = "object",
                Properties =
            {
                ["statusCode"] = new OpenApiSchema { Type = "integer", Format = "int32", Description = "The HTTP status code." },
                ["message"] = new OpenApiSchema { Type = "string", Description = "The detailed error message from the exception." }
            }
            };

            var errorContent = new Dictionary<string, OpenApiMediaType>
            {
                ["application/json"] = new OpenApiMediaType { Schema = errorResponseSchema }
            };

            // 2. Map custom exceptions to HTTP status codes
            var exceptionMappings = new Dictionary<Type, (int statusCode, string description)>
        {
            { typeof(NotFoundException), (404, "Not Found (Resource not found)") },
            { typeof(ValidationException), (400, "Bad Request (Input validation failed or Business rule violated)") },
            { typeof(Exception), (500, "Internal Server Error (Unhandled server exception)") }
        };

            // 3. Add responses to the Swagger documentation if they aren't already defined
            foreach (var mapping in exceptionMappings.Values.Distinct())
            {
                var statusCode = mapping.statusCode.ToString();

                // Only add if the response type hasn't been explicitly defined by [ProducesResponseType]
                if (!operation.Responses.ContainsKey(statusCode))
                {
                    operation.Responses.Add(statusCode, new OpenApiResponse
                    {
                        Description = mapping.description,
                        Content = errorContent
                    });
                }
            }
        }
    }
}
