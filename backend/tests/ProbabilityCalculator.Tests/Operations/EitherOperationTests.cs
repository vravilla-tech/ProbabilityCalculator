using FluentAssertions;
using ProbabilityCalculator.Api.Operations;

namespace ProbabilityCalculator.Tests.Operations;

public class EitherOperationTests
{
    private readonly EitherOperation _sut = new();

    [Fact]
    public void Name_ShouldBe_Either()
    {
        _sut.Name.Should().Be("Either");
    }

    [Theory]
    [InlineData(0.5, 0.5, 0.75)]
    [InlineData(1.0, 1.0, 1.0)]
    [InlineData(0.0, 0.5, 0.5)]
    [InlineData(0.3, 0.7, 0.79)]
    public void Calculate_ShouldReturn_InclusionExclusionResult(double a, double b, double expected)
    {
        _sut.Calculate(a, b).Should().BeApproximately(expected, 1e-10);
    }
}
