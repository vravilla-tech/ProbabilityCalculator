namespace ProbabilityCalculator.Api.Operations;

public interface ICalculationOperation
{
    string Name { get; }
    double Calculate(double a, double b);
}
