using ProbabilityCalculator.Api.Models;

namespace ProbabilityCalculator.Api.Services;

public interface ICalculatorService
{
    CalculationResult Calculate(CalculationRequest request);
}
