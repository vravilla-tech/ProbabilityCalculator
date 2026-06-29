import { useForm } from 'react-hook-form';
import type { CalculationRequest } from '../types';

interface Props {
  onSubmit: (data: CalculationRequest) => void;
  onInvalid: () => void;
  isLoading: boolean;
}

export function CalculatorForm({ onSubmit, onInvalid, isLoading }: Props) {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CalculationRequest>({ defaultValues: { operation: 'CombinedWith' } });

  const probabilityRules = {
    required: 'This field is required.',
    min: { value: 0, message: 'Must be >= 0.' },
    max: { value: 1, message: 'Must be <= 1.' },
    valueAsNumber: true,
  };

  return (
    <form className="calc-form" onSubmit={handleSubmit(onSubmit, onInvalid)} noValidate>
      <div className="field-group">
        <label htmlFor="probabilityA">Probability A</label>
        <input
          id="probabilityA"
          type="number"
          step="any"
          placeholder="e.g. 0.5"
          {...register('probabilityA', probabilityRules)}
        />
        {errors.probabilityA && <span className="error">{errors.probabilityA.message}</span>}
      </div>

      <div className="field-group">
        <label htmlFor="probabilityB">Probability B</label>
        <input
          id="probabilityB"
          type="number"
          step="any"
          placeholder="e.g. 0.5"
          {...register('probabilityB', probabilityRules)}
        />
        {errors.probabilityB && <span className="error">{errors.probabilityB.message}</span>}
      </div>

      <div className="field-group">
        <label htmlFor="operation">Operation</label>
        <select id="operation" {...register('operation', { required: 'Select an operation.' })}>
          <option value="CombinedWith">Combined With - P(A) x P(B)</option>
          <option value="Either">Either - P(A) + P(B) - P(A)P(B)</option>
        </select>
        {errors.operation && <span className="error">{errors.operation.message}</span>}
      </div>

      <button type="submit" disabled={isLoading}>
        {isLoading ? 'Calculating...' : 'Calculate'}
      </button>
    </form>
  );
}
