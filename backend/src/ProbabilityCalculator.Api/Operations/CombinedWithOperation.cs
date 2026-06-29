namespace ProbabilityCalculator.Api.Operations;

public class CombinedWithOperation : ICalculationOperation
{
    public string Name => "CombinedWith";
    public double Calculate(double a, double b) => a * b;
}
