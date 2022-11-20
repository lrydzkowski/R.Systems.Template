using R.Systems.Template.Persistence.Db.Common.Configurations;

namespace R.Systems.Template.Tests.Integration.Common.Db.SampleData;

internal static class IdGenerator
{
    public static int GetCompanyId(int id)
    {
        return GetId(CompanyConfiguration.FirstAvailableId, id);
    }

    public static int GetEmployeeId(int id)
    {
        return GetId(EmployeeConfiguration.FirstAvailableId, id);
    }

    private static int GetId(int firstAvailableId, int id)
    {
        return (firstAvailableId - 1) + id;
    }
}
