using PublicApiRepo.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicApiRepo.Validators
{
    public class LogQueryValidator : AbstractValidator<LogQuery>
    {
        public LogQueryValidator()
        {
            RuleFor(x => x.From)
                .NotEmpty()
                .WithMessage("'From' date is required")
                .Must(x => DateTimeOffset.TryParseExact(x, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .WithMessage("'From' date must be a valid date in the format yyyy-MM-dd");

            RuleFor(x => x.To)
                .NotEmpty()
                .WithMessage("'To' date is required")
                .Must(x => DateTimeOffset.TryParseExact(x, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                .WithMessage("'To' date must be a valid date in the format yyyy-MM-dd")
                .GreaterThan(x => x.From)
                .WithMessage("'To' date must be later than 'From' date");
        }
    }
}
