using MediatR.Pipeline;

namespace R.Systems.Template.Core.Employees.Commands.CreateEmployee;

internal class CreateEmployeeCommandPreProcessor : IRequestPreProcessor<CreateEmployeeCommand>
{
    public Task Process(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        request.FirstName = request.FirstName?.Trim();
        request.LastName = request.LastName?.Trim();
        request.CompanyId = request.CompanyId?.Trim();

        return Task.CompletedTask;
    }
}
