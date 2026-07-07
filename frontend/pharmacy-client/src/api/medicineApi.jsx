const BASE_URL = "/api";

async function handleResponse(res) {
  if (!res.ok) {
    let message = `Request failed with status ${res.status}`;
    try {
      const body = await res.json();
      message = body.title || body.message || JSON.stringify(body);
    } catch {
      // ignore parse errors, keep default message
    }
    throw new Error(message);
  }
  if (res.status === 204) return null;
  return res.json();
}

export async function fetchMedicines(search) {
  const query = search ? `?search=${encodeURIComponent(search)}` : "";
  const res = await fetch(`${BASE_URL}/medicines${query}`);
  return handleResponse(res);
}

export async function fetchMedicine(id) {
  const res = await fetch(`${BASE_URL}/medicines/${id}`);
  return handleResponse(res);
}

export async function createMedicine(payload) {
  const res = await fetch(`${BASE_URL}/medicines`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  return handleResponse(res);
}

export async function updateMedicine(id, payload) {
  const res = await fetch(`${BASE_URL}/medicines/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  return handleResponse(res);
}

export async function deleteMedicine(id) {
  const res = await fetch(`${BASE_URL}/medicines/${id}`, { method: "DELETE" });
  return handleResponse(res);
}

export async function fetchSales() {
  const res = await fetch(`${BASE_URL}/sales`);
  return handleResponse(res);
}

export async function recordSale(payload) {
  const res = await fetch(`${BASE_URL}/sales`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });
  return handleResponse(res);
}
