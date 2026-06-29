import { describe, it, expect, vi, beforeEach } from 'vitest';
import { calculate } from '../api';

describe('calculate', () => {
  beforeEach(() => {
    vi.restoreAllMocks();
  });

  it('returns a result when the API call succeeds', async () => {
    const mockResult = { result: 0.25, operation: 'CombinedWith', probabilityA: 0.5, probabilityB: 0.5 };

    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      json: () => Promise.resolve(mockResult),
    }));

    const result = await calculate({ probabilityA: 0.5, probabilityB: 0.5, operation: 'CombinedWith' });

    expect(result).toEqual(mockResult);
  });

  it('calls the correct endpoint with the correct method and body', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      json: () => Promise.resolve({}),
    }));

    await calculate({ probabilityA: 0.3, probabilityB: 0.7, operation: 'Either' });

    expect(fetch).toHaveBeenCalledWith(
      expect.stringContaining('/api/calculate'),
      expect.objectContaining({
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ probabilityA: 0.3, probabilityB: 0.7, operation: 'Either' }),
      })
    );
  });

  it('throws an error when the API returns a non-ok response', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: false,
      status: 400,
      json: () => Promise.resolve({ errors: ['ProbabilityA must be between 0 and 1.'] }),
    }));

    await expect(
      calculate({ probabilityA: -1, probabilityB: 0.5, operation: 'CombinedWith' })
    ).rejects.toThrow('ProbabilityA must be between 0 and 1.');
  });

  it('throws a generic error when the error response has no message', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: false,
      status: 500,
      json: () => Promise.reject(new Error('not json')),
    }));

    await expect(
      calculate({ probabilityA: 0.5, probabilityB: 0.5, operation: 'CombinedWith' })
    ).rejects.toThrow('HTTP 500');
  });
});
