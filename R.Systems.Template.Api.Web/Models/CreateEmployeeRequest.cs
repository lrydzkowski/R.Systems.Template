namespace R.Systems.Template.Api.Web.Models;

public class CreateEmployeeRequest
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public long? CompanyId { get; init; }
}
