import { useState } from 'react';
import { calculate } from './api';
import { CalculatorForm } from './components/CalculatorForm';
import { ResultDisplay } from './components/ResultDisplay';
import type { CalculationRequest, CalculationResult } from './types';
import './App.css';

function App() {
  const [result, setResult] = useState<CalculationResult | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);

  const handleSubmit = async (data: CalculationRequest) => {
    setIsLoading(true);
    setError(null);
    setResult(null);
    try {
      const res = await calculate(data);
      setResult(res);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unexpected error');
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <main className="app">
      <header className="app-header">
        <h1>Probability Calculator</h1>
        <p>Enter two probabilities (0-1) and select a calculation.</p>
      </header>
      <section className="app-body">
        <CalculatorForm
          onSubmit={handleSubmit}
          onInvalid={() => { setResult(null); setError(null); }}
          isLoading={isLoading}
        />
        <ResultDisplay result={result} error={error} isLoading={isLoading} />
      </section>
    </main>
  );
}

export default App;
