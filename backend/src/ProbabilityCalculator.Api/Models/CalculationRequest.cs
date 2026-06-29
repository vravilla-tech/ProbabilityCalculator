namespace ProbabilityCalculator.Api.Models;

/// <summary>A probability calculation request.</summary>
/// <param name="ProbabilityA">First probability value. Must be between 0 and 1 inclusive.</param>
/// <param name="ProbabilityB">Second probability value. Must be between 0 and 1 inclusive.</param>
/// <param name="Operation">The calculation to perform. Valid values: CombinedWith, Either.</param>
public record CalculationRequest(double ProbabilityA, double ProbabilityB, string Operation);
