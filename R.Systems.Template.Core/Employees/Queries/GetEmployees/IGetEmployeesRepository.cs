﻿using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Core.Common.Lists;

namespace R.Systems.Template.Core.Employees.Queries.GetEmployees;

public interface IGetEmployeesRepository
{
    Task<ListInfo<Employee>> GetEmployeesAsync(ListParameters listParameters);

    Task<ListInfo<Employee>> GetEmployeesAsync(ListParameters listParameters, int companyId);
}
