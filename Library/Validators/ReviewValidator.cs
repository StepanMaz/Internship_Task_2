using Database.Entities;
using FluentValidation;

namespace Validators
{
    public class ReviewValidator: AbstractValidator<Review>
    {
        public ReviewValidator()
        {
            RuleFor(book => book.Message).NotEmpty();
            RuleFor(book => book.Reviewer).NotEmpty();
        }
    }
}