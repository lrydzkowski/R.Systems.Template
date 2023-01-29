using MediatR.Pipeline;

namespace R.Systems.Template.Core.Companies.Commands.CreateCompany;

internal class CreateCompanyCommandPreProcessor : IRequestPreProcessor<CreateCompanyCommand>
{
    public Task Process(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        request.Name = request.Name?.Trim();

        return Task.CompletedTask;
    }
}
