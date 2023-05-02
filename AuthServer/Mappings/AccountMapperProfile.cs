using AuthServer.Data;
using AuthServer.Models;
using AutoMapper;

namespace AuthServer.Mappings
{
    public class AccountMapperProfile: Profile
    {
        public AccountMapperProfile()
        {
            CreateMap <AccountUser, RegisterModel> ().ReverseMap();
            CreateMap <AccountUser, LoginModel> ().ReverseMap();
            CreateMap <AccountUser, UserModel> ().ReverseMap();
        }
    }
}
