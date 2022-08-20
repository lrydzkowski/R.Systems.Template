using AutoMapper;
using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

public class CreateCompanyCommand : IRequest<CreateCompanyResult>
{
    public string? Name { get; init; }
}

public class CreateCompanyResult
{
    public Company Company { get; init; } = new();
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResult>
{
    public CreateCompanyCommandHandler(IMapper mapper, ICreateCompanyRepository createCompanyRepository)
    {
        Mapper = mapper;
        CreateCompanyRepository = createCompanyRepository;
    }

    private IMapper Mapper { get; }
    private ICreateCompanyRepository CreateCompanyRepository { get; }

    public async Task<CreateCompanyResult> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        CompanyToCreate companyToCreate = Mapper.Map<CompanyToCreate>(command);
        Company companyCreated = await CreateCompanyRepository.CreateCompanyAsync(companyToCreate);

        return new CreateCompanyResult
        {
            Company = companyCreated
        };
    }
}
