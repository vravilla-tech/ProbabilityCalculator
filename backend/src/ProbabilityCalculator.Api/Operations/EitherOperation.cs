namespace ProbabilityCalculator.Api.Operations;

public class EitherOperation : ICalculationOperation
{
    public string Name => "Either";
    public double Calculate(double a, double b) => a + b - (a * b);
}
