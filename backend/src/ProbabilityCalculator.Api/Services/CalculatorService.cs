using ProbabilityCalculator.Api.Logging;
using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Operations;

namespace ProbabilityCalculator.Api.Services;

public class CalculatorService(IEnumerable<ICalculationOperation> operations, ICalculationLogger logger)
    : ICalculatorService
{
    public CalculationResult Calculate(CalculationRequest request)
    {
        var operation = operations.FirstOrDefault(o =>
            o.Name.Equals(request.Operation, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Unknown operation: {request.Operation}");

        var result = operation.Calculate(request.ProbabilityA, request.ProbabilityB);
        var calculationResult = new CalculationResult(request.Operation, request.ProbabilityA, request.ProbabilityB, result);

        logger.Log(calculationResult);

        return calculationResult;
    }
}
