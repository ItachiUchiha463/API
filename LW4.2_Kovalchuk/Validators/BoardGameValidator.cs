using FluentValidation;
using LW4._2_Kovalchuk.Models;

namespace LW4._2_Kovalchuk.Validators
{
    public class BoardGameItemValidator : AbstractValidator<BoardGameItem>
    {
        public BoardGameItemValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Поле 'Name' є обов'язковим.")
                .MinimumLength(3).WithMessage("Назва гри повинна містити щонайменше 3 символи.")
                .MaximumLength(100).WithMessage("Назва гри не може перевищувати 100 символів.");

            RuleFor(x => x.MinPlayers)
                .GreaterThan(0).WithMessage("Мінімальна кількість гравців повинна бути більше 0.");

            RuleFor(x => x.MaxPlayers)
                .GreaterThanOrEqualTo(x => x.MinPlayers)
                .WithMessage("Максимальна кількість гравців повинна бути більшою або рівною мінімальній.");

            RuleFor(x => x.Difficulty)
                .IsInEnum().WithMessage("Поле 'Difficulty' повинно містити коректне значення (Easy, Medium, Hard)(1,2,3).");
        }
    }
}
