namespace R.Systems.Template.Api.AzureFunctions.Models;

public class UpdateEmployeeRequest
{
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public int CompanyId { get; init; }
}
