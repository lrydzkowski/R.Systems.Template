﻿using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Services;
using R.Systems.Template.Core.Companies.Commands.CreateCompany;
using R.Systems.Template.Core.Companies.Commands.UpdateCompany;
using R.Systems.Template.Infrastructure.CosmosDb.Common.Items;

namespace R.Systems.Template.Infrastructure.CosmosDb.Common.Mappers;

internal interface ICompanyMapper
{
    CompanyItem Map(CompanyToCreate companyToCreate);
    CompanyItem Map(CompanyToUpdate companyToUpdate);
    List<Company> Map(List<CompanyItem> companyItems);
    Company Map(CompanyItem companyItem);
}

internal class CompanyMapper
    : ICompanyMapper
{
    private readonly IUniqueIdGenerator _uniqueIdGenerator;

    public CompanyMapper(IUniqueIdGenerator uniqueIdGenerator)
    {
        _uniqueIdGenerator = uniqueIdGenerator;
    }

    public CompanyItem Map(CompanyToCreate companyToCreate)
    {
        CompanyItem companyItem = new()
        {
            Id = _uniqueIdGenerator.Generate().ToString(),
            Name = companyToCreate.Name
        };

        return companyItem;
    }

    public CompanyItem Map(CompanyToUpdate companyToUpdate)
    {
        CompanyItem companyItem = new()
        {
            Id = companyToUpdate.CompanyId.ToString(),
            Name = companyToUpdate.Name
        };

        return companyItem;
    }

    public List<Company> Map(List<CompanyItem> companyItems)
    {
        return companyItems.Select(Map).ToList();
    }

    public Company Map(CompanyItem companyItem)
    {
        Company company = new()
        {
            CompanyId = long.Parse(companyItem.Id),
            Name = companyItem.Name
        };

        return company;
    }
}
