namespace R.Systems.Template.Core.Companies.Commands.DeleteCompany;

public interface IDeleteCompanyRepository
{
    Task DeleteAsync(int companyId);
}
