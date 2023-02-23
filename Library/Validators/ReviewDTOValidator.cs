using DTO;
using FluentValidation;

namespace Validators
{
    public class ReviewDTOValidator: AbstractValidator<ReviewDTO>
    {
        public ReviewDTOValidator()
        {
            RuleFor(book => book.Message).NotEmpty();
            RuleFor(book => book.Reviewer).NotEmpty();
        }
    }
}