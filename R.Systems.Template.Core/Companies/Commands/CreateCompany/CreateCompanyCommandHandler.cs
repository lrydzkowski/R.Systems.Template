using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

public class CreateCompanyCommand : IRequest<CreateCompanyResult>
{
    public string? Name { get; set; }
}

public class CreateCompanyResult
{
    public Company Company { get; init; } = new();
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResult>
{
    private readonly ICreateCompanyRepository _createCompanyRepository;

    public CreateCompanyCommandHandler(ICreateCompanyRepository createCompanyRepository)
    {
        _createCompanyRepository = createCompanyRepository;
    }

    public async Task<CreateCompanyResult> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        CreateCompanyCommandMapper mapper = new();
        CompanyToCreate companyToCreate = mapper.ToCompanyToCreate(command);
        Company companyCreated = await _createCompanyRepository.CreateCompanyAsync(companyToCreate);
        return new CreateCompanyResult
        {
            Company = companyCreated
        };
    }
}
