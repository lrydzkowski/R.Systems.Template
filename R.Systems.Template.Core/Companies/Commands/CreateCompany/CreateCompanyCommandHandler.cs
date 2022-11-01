using AutoMapper;
using MediatR;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Validation;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

public class CreateCompanyCommand : IRequest<Result<Company>>
{
    public string? Name { get; init; }
}

public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Result<Company>>
{
    public CreateCompanyCommandHandler(IMapper mapper, ICreateCompanyRepository createCompanyRepository)
    {
        Mapper = mapper;
        CreateCompanyRepository = createCompanyRepository;
    }

    private IMapper Mapper { get; }
    private ICreateCompanyRepository CreateCompanyRepository { get; }

    public async Task<Result<Company>> Handle(CreateCompanyCommand command, CancellationToken cancellationToken)
    {
        CompanyToCreate companyToCreate = Mapper.Map<CompanyToCreate>(command);

        return await CreateCompanyRepository.CreateCompanyAsync(companyToCreate);
    }
}
