using AutoMapper;
using LW4._2_Kovalchuk.Models;
namespace LW4._2_Kovalchuk.Mapping
{
    public class BoardGameProfile : Profile
    {
        public BoardGameProfile()
        {
            CreateMap<BoardGameItem, BoardGameDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id ?? 0))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Difficulty, o => o.MapFrom(s => s.Difficulty.ToString()))
                .ForMember(d => d.MinPlayers, o => o.MapFrom(s => s.MinPlayers))
                .ForMember(d => d.MaxPlayers, o => o.MapFrom(s => s.MaxPlayers));
        }
    }
}