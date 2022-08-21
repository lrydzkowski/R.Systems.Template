using AutoMapper;
using FluentValidation;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class CreateCompanyRepository : ICreateCompanyRepository
{
    public CreateCompanyRepository(IMapper mapper, IValidator<CompanyToCreate> validator, AppDbContext dbContext)
    {
        Mapper = mapper;
        Validator = validator;
        DbContext = dbContext;
    }

    private IMapper Mapper { get; }
    private IValidator<CompanyToCreate> Validator { get; }
    private AppDbContext DbContext { get; }

    public async Task<Company> CreateCompanyAsync(CompanyToCreate companyToCreate)
    {
        await Validator.ValidateAndThrowAsync(companyToCreate);

        CompanyEntity companyEntity = Mapper.Map<CompanyEntity>(companyToCreate);

        await DbContext.Companies.AddAsync(companyEntity);
        await DbContext.SaveChangesAsync();

        return Mapper.Map<Company>(companyEntity);
    }
}
