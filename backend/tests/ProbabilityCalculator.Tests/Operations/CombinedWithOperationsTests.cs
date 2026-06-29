using FluentAssertions;
using ProbabilityCalculator.Api.Operations;

namespace ProbabilityCalculator.Tests.Operations;

public class CombinedWithOperationTests
{
    private readonly CombinedWithOperation _sut = new();

    [Fact]
    public void Name_ShouldBe_CombinedWith()
    {
        _sut.Name.Should().Be("CombinedWith");
    }

    [Theory]
    [InlineData(0.5, 0.5, 0.25)]
    [InlineData(1.0, 1.0, 1.0)]
    [InlineData(0.0, 0.5, 0.0)]
    [InlineData(0.3, 0.7, 0.21)]
    public void Calculate_ShouldReturn_ProductOfProbabilities(double a, double b, double expected)
    {
        _sut.Calculate(a, b).Should().BeApproximately(expected, 1e-10);
    }
}
