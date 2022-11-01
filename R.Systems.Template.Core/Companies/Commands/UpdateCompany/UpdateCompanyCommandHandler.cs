using AutoMapper;
using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommand : IRequest<Result<Company>>
{
    public int CompanyId { get; init; }

    public string? Name { get; init; }
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Result<Company>>
{
    public UpdateCompanyCommandHandler(IMapper mapper, IUpdateCompanyRepository updateCompanyRepository)
    {
        Mapper = mapper;
        UpdateCompanyRepository = updateCompanyRepository;
    }

    private IMapper Mapper { get; }
    private IUpdateCompanyRepository UpdateCompanyRepository { get; }

    public async Task<Result<Company>> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        CompanyToUpdate companyToUpdate = Mapper.Map<CompanyToUpdate>(command);

        return await UpdateCompanyRepository.UpdateCompanyAsync(companyToUpdate);
    }
}
