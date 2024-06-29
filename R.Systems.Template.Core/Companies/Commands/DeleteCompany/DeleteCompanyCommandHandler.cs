using MediatR;
using R.Systems.Template.Core.Common.Infrastructure;

namespace R.Systems.Template.Core.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommand : IContextRequest, IRequest
{
    public long CompanyId { get; init; }
    public ApplicationContext AppContext { get; set; } = new();
}

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
{
    private readonly IVersionedRepositoryFactory<IDeleteCompanyRepository> _repositoryFactory;

    public DeleteCompanyCommandHandler(IVersionedRepositoryFactory<IDeleteCompanyRepository> repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
    {
        IDeleteCompanyRepository repository = _repositoryFactory.GetRepository(command.AppContext);
        await repository.DeleteAsync(command.CompanyId);
    }
}
