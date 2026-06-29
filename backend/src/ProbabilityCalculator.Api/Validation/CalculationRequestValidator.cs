using FluentValidation;
using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Operations;

namespace ProbabilityCalculator.Api.Validation;

public class CalculationRequestValidator : AbstractValidator<CalculationRequest>
{
    public CalculationRequestValidator(IEnumerable<ICalculationOperation> operations)
    {
        var validOperations = operations.Select(o => o.Name).ToList();

        RuleFor(x => x.ProbabilityA)
            .InclusiveBetween(0, 1)
            .WithMessage("ProbabilityA must be between 0 and 1 inclusive.");

        RuleFor(x => x.ProbabilityB)
            .InclusiveBetween(0, 1)
            .WithMessage("ProbabilityB must be between 0 and 1 inclusive.");

        RuleFor(x => x.Operation)
            .NotEmpty()
            .Must(op => validOperations.Contains(op, StringComparer.OrdinalIgnoreCase))
            .WithMessage(x => $"Operation '{x.Operation}' is not supported. Valid operations: {string.Join(", ", validOperations)}.");
    }
}
