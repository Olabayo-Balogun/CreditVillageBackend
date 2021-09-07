using AutoMapper;

namespace CreditVillageBackend
{
    public class AppMapping : Profile
    {
        public AppMapping()
        {
            CreateMap<UserRegisterResponse, UserRegisterMapping>();

            CreateMap<AdminRegisterResponse, AdminRegisterMapping>();

            CreateMap<UserLoginResponse, UserLoginMapping>()
                    .ForMember(dest => dest.Data, opt =>
                        opt.MapFrom(x => new UserLoginConfirm { Token = x.Token,
                                                                Expiration = x.Expiration
                                                              }));
        }
    }
}