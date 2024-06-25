using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommand : IContextRequest, IRequest<UpdateCompanyResult>
{
    public int CompanyId { get; set; }
    public string? Name { get; set; }
    public ApplicationContext AppContext { get; set; } = new();
}

public class UpdateCompanyResult
{
    public Company Company { get; init; } = new();
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, UpdateCompanyResult>
{
    private readonly IVersionedRepositoryFactory<IUpdateCompanyRepository> _repositoryFactory;

    public UpdateCompanyCommandHandler(IVersionedRepositoryFactory<IUpdateCompanyRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<UpdateCompanyResult> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        IUpdateCompanyRepository repository = _repositoryFactory.GetRepository(command.AppContext);
        UpdateCompanyCommandMapper mapper = new();
        CompanyToUpdate companyToUpdate = mapper.ToCompanyToUpdate(command);
        Company company = await repository.UpdateCompanyAsync(companyToUpdate);
        return new UpdateCompanyResult
        {
            Company = company
        };
    }
}
