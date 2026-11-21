const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getRegistrations({
  participantName = '',
  type = '',
  status = '',
  page = 1,
  pageSize = 10,
} = {}) {
  const params = new URLSearchParams({
    participantName,
    type,
    status,
    page: String(page),
    pageSize: String(pageSize),
  });

  const response = await fetch(
    `${API_BASE_URL}/api/registrations?${params.toString()}`
  );

  if (!response.ok) {
    throw new Error('Failed to load registrations.');
  }

  return await response.json();
}

export async function getSummary() {
  const response = await fetch(
    `${API_BASE_URL}/api/registrations/summary`
  );

  if (!response.ok) {
    throw new Error('Failed to load dashboard summary.');
  }

  return await response.json();
}