using FluentAssertions;
using FluentValidation;
using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Operations;
using ProbabilityCalculator.Api.Validation;

namespace ProbabilityCalculator.Tests.Validation;

public class CalculationRequestValidatorTests
{
    private readonly IValidator<CalculationRequest> _validator = new CalculationRequestValidator(
        [new CombinedWithOperation(), new EitherOperation()]);

    [Theory]
    [InlineData(0.5, 0.5, "CombinedWith")]
    [InlineData(0.0, 1.0, "Either")]
    [InlineData(1.0, 0.0, "CombinedWith")]
    public void ValidRequest_ShouldPassValidation(double a, double b, string op)
    {
        var result = _validator.Validate(new CalculationRequest(a, b, op));
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(-0.1, 0.5, "CombinedWith")]
    [InlineData(0.5, 1.1, "Either")]
    public void OutOfRangeProbability_ShouldFailValidation(double a, double b, string op)
    {
        var result = _validator.Validate(new CalculationRequest(a, b, op));
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void UnknownOperation_ShouldFailValidation()
    {
        var result = _validator.Validate(new CalculationRequest(0.5, 0.5, "Multiply"));
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Operation");
    }
}
