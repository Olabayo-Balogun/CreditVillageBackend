using AutoMapper;

namespace CreditVillageBackend
{
    public class AppMapping : Profile
    {
        public AppMapping()
        {
            CreateMap<UserRegisterResponse, UserRegisterMapping>();

            CreateMap<AdminRegisterResponse, AdminRegisterMapping>();

            CreateMap<UserResendCodeResponse, UserResendCodeMapping>();

            CreateMap<UserLoginResponse, UserLoginMapping>()
                .ForMember(dest => dest.Data, opt =>
                    opt.MapFrom(x => new UserLoginConfirm { Token = x.Token,
                                                            Expiration = x.Expiration
                                                        }));

            CreateMap<UserForgetPasswordResponse, UserForgetPasswordMapping>();                            

            CreateMap<AppUser, GetUserMapping>()
                .ForMember(dest => dest.Nationality, opt =>
                      opt.MapFrom(x => x.Nationality.Name))
                .ForMember(dest => dest.State, opt =>
                      opt.MapFrom(x => x.State.Name));   

            CreateMap<UserVerifyResponse, UserVerifyMapping>();

            CreateMap<ChangePasswordResponse, ChangePasswordMapping>();

            CreateMap<UserUpdateResponse, UserUpdateMapping>();

                                     
        }
    }
}