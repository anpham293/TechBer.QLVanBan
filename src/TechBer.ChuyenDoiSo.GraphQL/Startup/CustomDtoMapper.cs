using AutoMapper;
using TechBer.ChuyenDoiSo.Authorization.Users;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.Startup
{
    public static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<User, UserDto>()
                .ForMember(dto => dto.Roles, options => options.Ignore())
                .ForMember(dto => dto.OrganizationUnits, options => options.Ignore());
        }
    }
}