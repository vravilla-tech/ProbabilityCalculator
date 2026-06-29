using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Operations;
using ProbabilityCalculator.Api.Services;

namespace ProbabilityCalculator.Tests.Services;

public class CalculatorServiceTests
{
    private readonly ILogger<CalculatorService> _logger = Substitute.For<ILogger<CalculatorService>>();
    private readonly CombinedWithOperation _combinedWith = new();
    private readonly EitherOperation _either = new();
    private readonly CalculatorService _sut;

    public CalculatorServiceTests()
    {
        _sut = new CalculatorService([_combinedWith, _either], _logger);
    }

    [Fact]
    public void Calculate_WithCombinedWith_ShouldReturnProduct()
    {
        var result = _sut.Calculate(new CalculationRequest(0.5, 0.5, "CombinedWith"));
        result.Result.Should().BeApproximately(0.25, 1e-10);
    }

    [Fact]
    public void Calculate_WithEither_ShouldReturnInclusionExclusion()
    {
        var result = _sut.Calculate(new CalculationRequest(0.5, 0.5, "Either"));
        result.Result.Should().BeApproximately(0.75, 1e-10);
    }

    [Fact]
    public void Calculate_ShouldCallLogger()
    {
        _sut.Calculate(new CalculationRequest(0.5, 0.5, "CombinedWith"));
        _logger.Received(1).Log(
            LogLevel.Information,
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception?>(),
            Arg.Any<Func<object, Exception?, string>>());
    }

    [Fact]
    public void Calculate_WithUnknownOperation_ShouldThrow()
    {
        var act = () => _sut.Calculate(new CalculationRequest(0.5, 0.5, "Unknown"));
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Calculate_ShouldReturnEchoedInputsInResult()
    {
        var result = _sut.Calculate(new CalculationRequest(0.3, 0.7, "CombinedWith"));
        result.ProbabilityA.Should().Be(0.3);
        result.ProbabilityB.Should().Be(0.7);
        result.Operation.Should().Be("CombinedWith");
    }

    [Fact]
    public void Calculate_CombinedWith_OneZeroProbability_ShouldReturnZero()
    {
        var result = _sut.Calculate(new CalculationRequest(0.0, 0.8, "CombinedWith"));
        result.Result.Should().Be(0.0);
    }

    [Fact]
    public void Calculate_Either_OneIsOne_ShouldReturnOne()
    {
        var result = _sut.Calculate(new CalculationRequest(1.0, 0.5, "Either"));
        result.Result.Should().Be(1.0);
    }

    [Fact]
    public void Calculate_LoggerReceivesCorrectOperation()
    {
        var result = _sut.Calculate(new CalculationRequest(0.4, 0.6, "Either"));
        result.Operation.Should().Be("Either");
    }
}
