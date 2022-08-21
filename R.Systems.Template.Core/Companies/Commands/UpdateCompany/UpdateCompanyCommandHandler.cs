using AutoMapper;
using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommand : IRequest<UpdateCompanyResult>
{
    public int CompanyId { get; init; }

    public string? Name { get; init; }
}

public class UpdateCompanyResult
{
    public Company Company { get; init; } = new();
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, UpdateCompanyResult>
{
    public UpdateCompanyCommandHandler(IMapper mapper, IUpdateCompanyRepository updateCompanyRepository)
    {
        Mapper = mapper;
        UpdateCompanyRepository = updateCompanyRepository;
    }

    private IMapper Mapper { get; }
    private IUpdateCompanyRepository UpdateCompanyRepository { get; }

    public async Task<UpdateCompanyResult> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        CompanyToUpdate companyToUpdate = Mapper.Map<CompanyToUpdate>(command);
        Company company = await UpdateCompanyRepository.UpdateCompanyAsync(companyToUpdate);

        return new UpdateCompanyResult
        {
            Company = company
        };
    }
}
