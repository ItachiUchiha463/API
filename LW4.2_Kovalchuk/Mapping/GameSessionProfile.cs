using AutoMapper;
using LW4._2_Kovalchuk.Models;
namespace LW4._2_Kovalchuk.Mapping
{
    public class GameSessionProfile : Profile
    {
        public GameSessionProfile()
        {

            CreateMap<GameSessionItem, GameSessionDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id ?? 0))
                .ForMember(d => d.NumberOfPlayers, o => o.MapFrom(s => s.NumberOfPlayers))
                .ForMember(d => d.BoardGameId, o => o.MapFrom(s => s.BoardGameId))
                .ForMember(d => d.InProgress, o => o.MapFrom(s => s.InProgress))
                .ForMember(d => d.UserIds, o => o.MapFrom(s => s.UserIds));
        }
    }
}
