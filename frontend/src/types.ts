export interface CalculationRequest {
  probabilityA: number;
  probabilityB: number;
  operation: 'CombinedWith' | 'Either';
}

export interface CalculationResult {
  result: number;
  operation: string;
  probabilityA: number;
  probabilityB: number;
}
