using DTO;
using FluentValidation;

namespace Validators
{
    public class RatingDTOValidator: AbstractValidator<RatingDTO>
    {
        public RatingDTOValidator()
        {
            RuleFor(book => book.Score).InclusiveBetween(1, 5);
        }
    }
}