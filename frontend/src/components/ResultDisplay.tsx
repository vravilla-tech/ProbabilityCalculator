import type { CalculationResult } from '../types';

interface Props {
  result: CalculationResult | null;
  error: string | null;
  isLoading: boolean;
}

export function ResultDisplay({ result, error, isLoading }: Props) {
  if (isLoading) return <p style={{ marginTop: '24px', color: '#718096' }}>Calculating...</p>;
  if (error) {
    return (
      <div className="result result--error" role="alert">
        <strong>Error:</strong> {error}
      </div>
    );
  }

  if (!result) return null;

  return (
    <div className="result result--success" role="status">
      <h2>Result</h2>
      <table className="result-table">
        <tbody>
          <tr><th>Operation</th><td>{result.operation}</td></tr>
          <tr><th>P(A)</th><td>{result.probabilityA}</td></tr>
          <tr><th>P(B)</th><td>{result.probabilityB}</td></tr>
          <tr><th>Result</th><td><strong>{result.result}</strong></td></tr>
        </tbody>
      </table>
    </div>
  );
}
