using AutoMapper;
using FluentValidation;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Companies.Commands;

internal class CreateCompanyRepository : ICreateCompanyRepository
{
    public CreateCompanyRepository(AppDbContext dbContext, IMapper mapper, IValidator<CompanyEntity> validator)
    {
        DbContext = dbContext;
        Mapper = mapper;
        Validator = validator;
    }

    private AppDbContext DbContext { get; }
    private IMapper Mapper { get; }
    private IValidator<CompanyEntity> Validator { get; }

    public async Task<Company> CreateCompanyAsync(CompanyToCreate companyToCreate)
    {
        CompanyEntity companyEntity = Mapper.Map<CompanyEntity>(companyToCreate);
        await Validator.ValidateAndThrowAsync(companyEntity);

        await DbContext.Companies.AddAsync(companyEntity);
        await DbContext.SaveChangesAsync();

        return Mapper.Map<Company>(companyEntity);
    }
}
