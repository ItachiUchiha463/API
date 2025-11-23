using FluentValidation;
using LW4._2_Kovalchuk.Models;
using LW4._2_Kovalchuk.Interfaces;
using System.Threading.Tasks;

namespace LW4._2_Kovalchuk.Validators
{
    public class GameSessionValidator : AbstractValidator<GameSessionItem>
    {
        public GameSessionValidator()
        {
            RuleFor(x => x.NumberOfPlayers)
                .InclusiveBetween(2, 8)
                .WithMessage("Кількість гравців повинна бути від 2 до 8.");

            RuleFor(x => x.BoardGameId)
                .GreaterThan(0)
                .WithMessage("BoardGameId має бути більшим за 0.");

            RuleFor(x => x.UserIds)
                .NotNull()
                .WithMessage("Список UserIds не може бути null.")
                .NotEmpty()
                .WithMessage("Список UserIds не може бути порожнім.");

            RuleFor(x => x.UserIds)
                .Must((session, userIds) => userIds.Count == session.NumberOfPlayers)
                .WithMessage("Кількість UserIds повинна дорівнювати NumberOfPlayers.");

            RuleForEach(x => x.UserIds)
                .GreaterThan(0)
                .WithMessage("Кожен UserId має бути більшим за 0.");
        }
    }
}