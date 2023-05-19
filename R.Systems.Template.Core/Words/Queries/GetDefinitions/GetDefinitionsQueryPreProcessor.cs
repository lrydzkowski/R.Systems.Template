using MediatR.Pipeline;

namespace R.Systems.Template.Core.Words.Queries.GetDefinitions;

internal class GetDefinitionsQueryPreProcessor : IRequestPreProcessor<GetDefinitionsQuery>
{
    public Task Process(GetDefinitionsQuery request, CancellationToken cancellationToken)
    {
        request.Word = request.Word?.Trim();

        return Task.CompletedTask;
    }
}
