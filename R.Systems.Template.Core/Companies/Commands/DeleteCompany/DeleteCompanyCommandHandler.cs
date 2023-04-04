using MediatR;

namespace R.Systems.Template.Core.Companies.Commands.DeleteCompany;

public class DeleteCompanyCommand : IRequest
{
    public int CompanyId { get; init; }
}

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
{
    private readonly IDeleteCompanyRepository _deleteCompanyRepository;

    public DeleteCompanyCommandHandler(IDeleteCompanyRepository deleteCompanyRepository)
    {
        _deleteCompanyRepository = deleteCompanyRepository;
    }

    public async Task Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
    {
        await _deleteCompanyRepository.DeleteAsync(command.CompanyId);
    }
}
