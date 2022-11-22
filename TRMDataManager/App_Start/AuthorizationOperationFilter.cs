using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace TRMDataManager.App_Start
{
    public class AuthorizationOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
            {
                operation.parameters = new List<Parameter>();
            }
            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                description = "access token",
                // False because some methods don't need authorization
                required = false,
                type = "string"
            });
        }
    }
}