using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

public class CreateCompanyCommand : IContextRequest, IRequest<CreateCompanyResult>
{
    public string? Name { get; set; }
    public ApplicationContext AppContext { get; set; } = new();
}

public class CreateCompanyResult
{
    public Company Company { get; init; } = new();
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, CreateCompanyResult>
{
    private readonly IVersionedRepositoryFactory<ICreateCompanyRepository> _repositoryFactory;

    public CreateCompanyCommandHandler(IVersionedRepositoryFactory<ICreateCompanyRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<CreateCompanyResult> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        ICreateCompanyRepository repository = _repositoryFactory.GetRepository(command.AppContext);
        CreateCompanyCommandMapper mapper = new();
        CompanyToCreate companyToCreate = mapper.ToCompanyToCreate(command);
        Company companyCreated = await repository.CreateCompanyAsync(companyToCreate);
        return new CreateCompanyResult
        {
            Company = companyCreated
        };
    }
}
