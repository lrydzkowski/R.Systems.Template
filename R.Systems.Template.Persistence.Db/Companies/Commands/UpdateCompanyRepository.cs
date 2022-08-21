using AutoMapper;
using FluentValidation;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class UpdateCompanyRepository : IUpdateCompanyRepository
{
    public UpdateCompanyRepository(IMapper mapper, IValidator<CompanyToUpdate> validator, AppDbContext dbContext)
    {
        Mapper = mapper;
        Validator = validator;
        DbContext = dbContext;
    }

    private IMapper Mapper { get; }
    private IValidator<CompanyToUpdate> Validator { get; }
    private AppDbContext DbContext { get; }

    public async Task<Company> UpdateCompanyAsync(CompanyToUpdate companyToUpdate)
    {
        await Validator.ValidateAndThrowAsync(companyToUpdate);

        CompanyEntity companyEntity = new CompanyEntity { Id = companyToUpdate.CompanyId };
        DbContext.Companies.Attach(companyEntity);
        companyEntity.Name = companyToUpdate.Name;
        await DbContext.SaveChangesAsync();

        return Mapper.Map<Company>(companyEntity);
    }
}
