using R.Systems.Template.Infrastructure.Db.Common.Configurations;

namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

internal static class IdGenerator
{
    public static long GetCompanyId(long id)
    {
        return GetId(CompanyEntityTypeConfiguration.FirstAvailableId, id);
    }

    public static long GetEmployeeId(long id)
    {
        return GetId(EmployeeEntityTypeConfiguration.FirstAvailableId, id);
    }

    private static long GetId(long firstAvailableId, long id)
    {
        return firstAvailableId - 1 + id;
    }
}
