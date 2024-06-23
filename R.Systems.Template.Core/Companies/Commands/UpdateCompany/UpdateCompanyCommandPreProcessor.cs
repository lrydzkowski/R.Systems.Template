using MediatR.Pipeline;

namespace R.Systems.Template.Core.Companies.Commands.UpdateCompany;

internal class UpdateCompanyCommandPreProcessor : IRequestPreProcessor<UpdateCompanyCommand>
{
    public Task Process(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        request.Name = request.Name?.Trim();
        return Task.CompletedTask;
    }
}
