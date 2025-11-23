using FluentValidation;
using LW4._2_Kovalchuk.Models;

namespace LW4._2_Kovalchuk.Validators
{
    public class UserItemValidator : AbstractValidator<UserItem>
    {
        public UserItemValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Поле 'Username' є обов'язковим.")
                .MinimumLength(3).WithMessage("Ім'я користувача повинно містити щонайменше 3 символи.")
                .MaximumLength(50).WithMessage("Ім'я користувача не може перевищувати 50 символів.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Поле 'Email' є обов'язковим.")
                .Matches(@"^[\w\.-]+@[\w\.-]+\.\w{2,}$")
                .WithMessage("Невірний формат електронної пошти. Приклад: name@example.com");
        }
    }
}
