export function parseApiDate(value: string | null | undefined): Date | null {
  if (!value) {
    return null;
  }

  const parsed = new Date(value);

  return Number.isNaN(parsed.getTime()) ? null : parsed;
}

export function formatApiDateTime(value: string | null | undefined, locale = 'pt-BR'): string {
  const parsed = parseApiDate(value);
  return parsed ? parsed.toLocaleString(locale) : '—';
}

export function formatApiDate(value: string | null | undefined, locale = 'pt-BR'): string {
  const parsed = parseApiDate(value);
  return parsed ? parsed.toLocaleDateString(locale) : '—';
}
