﻿using MediatR;
using R.Systems.Template.Core.Common.Domain;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

public class UpdateCompanyCommand : IRequest<UpdateCompanyResult>
{
    public int CompanyId { get; set; }

    public string? Name { get; set; }
}

public class UpdateCompanyResult
{
    public Company Company { get; init; } = new();
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, UpdateCompanyResult>
{
    public UpdateCompanyCommandHandler(IUpdateCompanyRepository updateCompanyRepository)
    {
        UpdateCompanyRepository = updateCompanyRepository;
    }

    private IUpdateCompanyRepository UpdateCompanyRepository { get; }

    public async Task<UpdateCompanyResult> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
    {
        UpdateCompanyCommandMapper mapper = new();
        CompanyToUpdate companyToUpdate = mapper.ToCompanyToUpdate(command);
        Company company = await UpdateCompanyRepository.UpdateCompanyAsync(companyToUpdate);

        return new UpdateCompanyResult
        {
            Company = company
        };
    }
}
