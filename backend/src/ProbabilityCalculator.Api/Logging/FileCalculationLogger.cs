using ProbabilityCalculator.Api.Models;

namespace ProbabilityCalculator.Api.Logging;

public class FileCalculationLogger : ICalculationLogger
{
    private readonly string _logPath;
    private static readonly SemaphoreSlim _lock = new(1, 1);

    public FileCalculationLogger(IConfiguration configuration)
    {
        _logPath = configuration["CalculationLog:FilePath"] ?? "calculation-log.txt";
    }

    public void Log(CalculationResult result)
    {
        var entry = $"{DateTime.UtcNow:O} | {result.Operation} | A={result.ProbabilityA} | B={result.ProbabilityB} | Result={result.Result}{Environment.NewLine}";

        _lock.Wait();
        try
        {
            File.AppendAllText(_logPath, entry);
        }
        finally
        {
            _lock.Release();
        }
    }
}
