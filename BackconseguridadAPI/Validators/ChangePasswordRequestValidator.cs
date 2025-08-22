using FluentValidation;
using TiendaCarritoPasarelaSmeall.DTOs;

namespace TiendaCarritoPasarelaSmeall.Validators
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(10);
        }
    }
}
