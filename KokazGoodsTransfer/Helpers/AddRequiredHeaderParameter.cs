using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Web.Http.Description;

namespace KokazGoodsTransfer.Helpers
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "branchId",
                In = ParameterLocation.Header,
                AllowEmptyValue=true,
                Description = "Branch Id",
                Required = true,
                
            });
        }
    }
}

