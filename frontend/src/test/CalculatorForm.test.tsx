import { describe, it, expect, vi } from 'vitest';
import { render, screen, waitFor } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { CalculatorForm } from '../components/CalculatorForm';

describe('CalculatorForm', () => {
  it('renders all form fields', () => {
    render(<CalculatorForm onSubmit={vi.fn()} onInvalid={vi.fn()} isLoading={false} />);
    expect(screen.getByLabelText('Probability A')).toBeInTheDocument();
    expect(screen.getByLabelText('Probability B')).toBeInTheDocument();
    expect(screen.getByLabelText('Operation')).toBeInTheDocument();
    expect(screen.getByRole('button', { name: /calculate/i })).toBeInTheDocument();
  });

  it('disables the button and shows loading text while loading', () => {
    render(<CalculatorForm onSubmit={vi.fn()} onInvalid={vi.fn()} isLoading={true} />);
    const button = screen.getByRole('button');
    expect(button).toBeDisabled();
    expect(button).toHaveTextContent(/calculating/i);
  });

  it('calls onSubmit with correct values when form is valid', async () => {
    const onSubmit = vi.fn();
    render(<CalculatorForm onSubmit={onSubmit} onInvalid={vi.fn()} isLoading={false} />);

    await userEvent.type(screen.getByLabelText('Probability A'), '0.5');
    await userEvent.type(screen.getByLabelText('Probability B'), '0.3');
    await userEvent.click(screen.getByRole('button', { name: /calculate/i }));

    await waitFor(() => {
      expect(onSubmit).toHaveBeenCalledWith(
        expect.objectContaining({ probabilityA: 0.5, probabilityB: 0.3, operation: 'CombinedWith' }),
        expect.anything()
      );
    });
  });

  it('shows validation error when probability is missing', async () => {
    render(<CalculatorForm onSubmit={vi.fn()} onInvalid={vi.fn()} isLoading={false} />);
    await userEvent.click(screen.getByRole('button', { name: /calculate/i }));
    await waitFor(() => {
      expect(screen.getAllByText(/required/i).length).toBeGreaterThan(0);
    });
  });

  it('calls onInvalid when form is submitted with invalid data', async () => {
    const onInvalid = vi.fn();
    render(<CalculatorForm onSubmit={vi.fn()} onInvalid={onInvalid} isLoading={false} />);
    await userEvent.click(screen.getByRole('button', { name: /calculate/i }));
    await waitFor(() => {
      expect(onInvalid).toHaveBeenCalled();
    });
  });

  it('shows error when probability has more than 15 decimal places', async () => {
    render(<CalculatorForm onSubmit={vi.fn()} onInvalid={vi.fn()} isLoading={false} />);
    await userEvent.type(screen.getByLabelText('Probability A'), '0.1234567890123456');
    await userEvent.type(screen.getByLabelText('Probability B'), '0.5');
    await userEvent.click(screen.getByRole('button', { name: /calculate/i }));
    await waitFor(() => {
      expect(screen.getByText(/maximum 15 decimal places/i)).toBeInTheDocument();
    });
  });
});
