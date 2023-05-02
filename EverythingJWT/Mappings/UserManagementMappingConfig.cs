using AutoMapper;
using EverythingJWT.Data;
using EverythingJWT.Models;

namespace EverythingJWT.Mappings
{
    public class UserManagementMappingConfig: Profile
    {
        public UserManagementMappingConfig()
        {

            CreateMap <ApiUser, RegisterModel>().ReverseMap();
            CreateMap <ApiUser, LoginModel>().ReverseMap();
            CreateMap <ApiUser, UserModel>().ReverseMap();

        }
    }
}
