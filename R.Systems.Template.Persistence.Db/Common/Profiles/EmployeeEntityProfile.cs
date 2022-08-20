using AutoMapper;
using R.Systems.Template.Core.Common.Domain;
using R.Systems.Template.Persistence.Db.Common.Entities;

namespace R.Systems.Template.Persistence.Db.Common.Profiles;

internal class EmployeeEntityProfile : Profile
{
    public EmployeeEntityProfile()
    {
        CreateMap<EmployeeEntity, Employee>();
    }
}
