import type { CalculationRequest, CalculationResult } from './types';

const API_URL = import.meta.env.VITE_API_URL ?? 'http://localhost:5000';

export async function calculate(request: CalculationRequest): Promise<CalculationResult> {
  const response = await fetch(`${API_URL}/api/calculate`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    const error = await response.json().catch(() => ({}));
    throw new Error(error?.errors?.[0] ?? `HTTP ${response.status}`);
  }

  return response.json();
}
