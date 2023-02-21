using Database.Entities;
using FluentValidation;

namespace Validators
{
    public class BookValidator: AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(book => book.Title).Length(1, 30);
            RuleFor(book => book.Cover).NotEmpty();
            RuleFor(book => book.Content).NotEmpty();
            RuleFor(book => book.Author).NotEmpty();
            RuleFor(book => book.Genere).Length(1, 20);
        }
    }
}