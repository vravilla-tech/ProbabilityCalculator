using ProbabilityCalculator.Api.Models;
using ProbabilityCalculator.Api.Operations;

namespace ProbabilityCalculator.Api.Services;

public class CalculatorService(IEnumerable<ICalculationOperation> operations, ILogger<CalculatorService> logger)
    : ICalculatorService
{
    public CalculationResult Calculate(CalculationRequest request)
    {
        var operation = operations.FirstOrDefault(o =>
            o.Name.Equals(request.Operation, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException($"Unknown operation: {request.Operation}");

        var result = operation.Calculate(request.ProbabilityA, request.ProbabilityB);
        var calculationResult = new CalculationResult(request.Operation, request.ProbabilityA, request.ProbabilityB, result);

        logger.LogInformation(
            "Calculation | Operation={Operation} | A={ProbabilityA} | B={ProbabilityB} | Result={Result}",
            calculationResult.Operation, calculationResult.ProbabilityA, calculationResult.ProbabilityB, calculationResult.Result);

        return calculationResult;
    }
}
