﻿using R.Systems.Template.Core.Common.Errors;
using Swashbuckle.AspNetCore.Filters;

namespace R.Systems.Template.Api.Web.Swagger.Examples.Companies;

public class GetCompanyNotFoundResponseExamples : IMultipleExamplesProvider<ErrorInfo>
{
    public IEnumerable<SwaggerExample<ErrorInfo>> GetExamples()
    {
        yield return SwaggerExample.Create(
            "Example 1",
            "Example 1",
            new ErrorInfo
            {
                PropertyName = "Company",
                ErrorMessage = "Company doesn't exist.",
                ErrorCode = "NotExist",
                AttemptedValue = new
                {
                    CompanyId = 5
                }
            }
        );
    }
}
