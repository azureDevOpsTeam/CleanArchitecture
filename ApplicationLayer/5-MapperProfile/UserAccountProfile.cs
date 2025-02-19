using AutoMapper;

namespace ApplicationLayer.MapperProfile
{
    public class UserAccountProfile : Profile
    {
        public UserAccountProfile()
        {
            //CreateMap<CreateUserViewModel, User>().ReverseMap();

            //CreateMap<UserViewModel, User>()
            //    .ForMember(dest => dest.UserAccessKinds,
            //               opt => opt.MapFrom(src => src.AccessKind.Select(id => new UserAccessKind { AccessKind = id })));
        }
    }
}