using MediatR.Pipeline;

namespace R.Systems.Template.Core.Employees.Commands.UpdateEmployee;

internal class UpdateEmployeeCommandPreProcessor : IRequestPreProcessor<UpdateEmployeeCommand>
{
    public Task Process(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        request.EmployeeId = request.EmployeeId.Trim();
        request.FirstName = request.FirstName?.Trim();
        request.LastName = request.LastName?.Trim();
        request.CompanyId = request.CompanyId?.Trim();

        return Task.CompletedTask;
    }
}
