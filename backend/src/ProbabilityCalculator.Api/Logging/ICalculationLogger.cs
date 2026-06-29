using ProbabilityCalculator.Api.Models;

namespace ProbabilityCalculator.Api.Logging;

public interface ICalculationLogger
{
    void Log(CalculationResult result);
}
