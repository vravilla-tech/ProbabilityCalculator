import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import { ResultDisplay } from '../components/ResultDisplay';

describe('ResultDisplay', () => {
  it('renders nothing when result and error are both null', () => {
    const { container } = render(<ResultDisplay result={null} error={null} isLoading={false} />);
    expect(container.firstChild).toBeNull();
  });

  it('shows the error message when error is provided', () => {
    render(<ResultDisplay result={null} error="Something went wrong" isLoading={false} />);
    expect(screen.getByText(/something went wrong/i)).toBeInTheDocument();
  });

  it('shows the result when calculation succeeds', () => {
    const result = { result: 0.25, operation: 'CombinedWith', probabilityA: 0.5, probabilityB: 0.5 };
    render(<ResultDisplay result={result} error={null} isLoading={false} />);
    expect(screen.getByText('0.25')).toBeInTheDocument();
    expect(screen.getByText('CombinedWith')).toBeInTheDocument();
  });

  it('shows calculating message when loading', () => {
    render(<ResultDisplay result={null} error={null} isLoading={true} />);
    expect(screen.getByText(/calculating/i)).toBeInTheDocument();
  });

  it('hides previous result while loading', () => {
    const result = { result: 0.25, operation: 'CombinedWith', probabilityA: 0.5, probabilityB: 0.5 };
    render(<ResultDisplay result={result} error={null} isLoading={true} />);
    expect(screen.queryByText('0.25')).not.toBeInTheDocument();
  });
});
