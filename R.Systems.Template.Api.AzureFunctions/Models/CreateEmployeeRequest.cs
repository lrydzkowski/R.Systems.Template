namespace R.Systems.Template.Api.AzureFunctions.Models;

public class CreateEmployeeRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public long? CompanyId { get; init; }
}
