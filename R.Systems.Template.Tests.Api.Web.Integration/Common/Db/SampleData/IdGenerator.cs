namespace R.Systems.Template.Tests.Api.Web.Integration.Common.Db.SampleData;

internal static class IdGenerator
{
    private static readonly IReadOnlyList<Guid> CompanyIds =
    [
        new Guid("01910F22-5E78-AD9B-99EF-0017853EE4B8"),
        new Guid("01910F22-77A7-C134-4F08-50D511AE9C4C"),
        new Guid("01910F22-9101-2679-6AF5-4C431C8193E0"),
        new Guid("01910F22-A6DC-B909-E3DF-1AADD024A3ED"),
        new Guid("01910F22-B611-6B99-8B30-63BA5CEF1B06"),
        new Guid("01910F22-C78D-9DF3-43D8-0536D00D666F")
    ];

    private static readonly IReadOnlyList<Guid> EmployeeIds =
    [
        new Guid("01910F22-DC95-18CD-6856-F3434ABA24CB"),
        new Guid("01910F22-EDAF-8B84-09B6-FE9A772E0B31"),
        new Guid("01910F22-FC4A-BA84-13D6-51CE5D914574"),
        new Guid("01910F23-10A3-C77B-DD3F-7A411BB51D5B"),
        new Guid("01910F23-235D-5A5D-FF6E-B28D73141D0B"),
        new Guid("01910F23-3915-FCE3-AD75-3DBA16CFD3ED"),
        new Guid("01910F23-4A9B-1BEA-0870-BEAFA83A163B"),
        new Guid("01910F23-5D1D-AE66-098A-73AFD05382D6"),
        new Guid("01910F23-6BBD-E582-C2EB-0FA8B79C2358")
    ];

    public static Guid GetCompanyId(int id)
    {
        return CompanyIds[id - 1];
    }

    public static Guid GetEmployeeId(int id)
    {
        return EmployeeIds[id - 1];
    }
}
