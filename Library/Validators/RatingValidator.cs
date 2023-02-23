using Database.Entities;
using FluentValidation;

namespace Validators
{
    public class RatingValidator: AbstractValidator<Rating>
    {
        public RatingValidator()
        {
            RuleFor(book => book.Score).ExclusiveBetween(-1, 6);
        }
    }
}