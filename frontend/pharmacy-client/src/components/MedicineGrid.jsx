import React from "react";

function formatDate(dateStr) {
  const d = new Date(dateStr);
  return d.toLocaleDateString(undefined, { year: "numeric", month: "short", day: "numeric" });
}

function formatPrice(value) {
  return `₹${Number(value).toFixed(2)}`;
}

export default function MedicineGrid({ medicines, loading, error, onSell, onDelete }) {
  if (loading) {
    return <div className="state-message">Loading medicines…</div>;
  }

  if (error) {
    return <div className="state-message state-error">Couldn't load medicines: {error}</div>;
  }

  if (!medicines.length) {
    return <div className="state-message">No medicines found. Add one to get started.</div>;
  }

  return (
    <div className="grid-wrapper">
      <table className="medicine-grid">
        <thead>
          <tr>
            <th>Full Name</th>
            <th>Brand</th>
            <th>Expiry Date</th>
            <th>Quantity</th>
            <th>Price</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {medicines.map((m) => {
            const rowClass = m.isNearExpiry
              ? "row-expiring"
              : m.isLowStock
              ? "row-low-stock"
              : "";
            return (
              <tr key={m.id} className={rowClass}>
                <td>{m.fullName}</td>
                <td>{m.brand}</td>
                <td>
                  {formatDate(m.expiryDate)}
                  {m.isNearExpiry && <span className="badge badge-danger">Expiring soon</span>}
                </td>
                <td>
                  {m.quantity}
                  {m.isLowStock && <span className="badge badge-warning">Low stock</span>}
                </td>
                <td>{formatPrice(m.price)}</td>
                <td className="grid-actions">
                  <button className="btn btn-small" onClick={() => onSell(m)}>
                    Record sale
                  </button>
                  <button className="btn btn-small btn-ghost" onClick={() => onDelete(m)}>
                    Delete
                  </button>
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
      <div className="legend">
        <span className="legend-item"><span className="swatch swatch-danger" /> Expiry within 30 days</span>
        <span className="legend-item"><span className="swatch swatch-warning" /> Quantity below 10</span>
      </div>
    </div>
  );
}
