using R.Systems.Template.Infrastructure.Db.Common.Configurations;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

internal static class IdGenerator
{
    public static int GetCompanyId(int id)
    {
        return GetId(CompanyEntityTypeConfiguration.FirstAvailableId, id);
    }

    public static int GetEmployeeId(int id)
    {
        return GetId(EmployeeEntityTypeConfiguration.FirstAvailableId, id);
    }

    private static int GetId(int firstAvailableId, int id)
    {
        return firstAvailableId - 1 + id;
    }
}
