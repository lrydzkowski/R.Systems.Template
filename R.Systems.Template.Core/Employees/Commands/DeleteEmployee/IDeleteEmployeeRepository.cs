namespace R.Systems.Template.Core.Employees.Commands.DeleteEmployee;

public interface IDeleteEmployeeRepository
{
    Task DeleteEmployeeAsync(int employeeId);
}
