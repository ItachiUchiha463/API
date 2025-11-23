using AutoMapper;
using LW4._2_Kovalchuk.Models;
namespace LW4._2_Kovalchuk.Mapping

{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserItem, UserDTO>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Username))
                .ForMember(d => d.Email, o => o.MapFrom(s => s.Email))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id ?? 0))
                .ForMember(d => d.Password, o => o.MapFrom(s => s.Password));
        }
    } }
