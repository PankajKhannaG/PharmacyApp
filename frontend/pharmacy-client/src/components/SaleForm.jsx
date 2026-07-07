import React, { useState } from "react";

export default function SaleForm({ medicine, onSubmit, onCancel, submitting }) {
  const [quantitySold, setQuantitySold] = useState(1);
  const [buyer, setBuyer] = useState("");
  const [validationError, setValidationError] = useState("");

  function handleSubmit(e) {
    e.preventDefault();
    setValidationError("");

    const qty = Number(quantitySold);
    if (!qty || qty <= 0) {
      setValidationError("Enter a quantity greater than zero.");
      return;
    }
    if (qty > medicine.quantity) {
      setValidationError(`Only ${medicine.quantity} units in stock.`);
      return;
    }

    onSubmit({
      medicineId: medicine.id,
      quantitySold: qty,
      buyer: buyer.trim() || null,
    });
  }

  const total = (medicine.price * Number(quantitySold || 0)).toFixed(2);

  return (
    <div className="modal-backdrop">
      <div className="modal">
        <h2>Record sale</h2>
        <p className="modal-subtitle">
          {medicine.fullName} · {medicine.quantity} in stock · ₹{Number(medicine.price).toFixed(2)} each
        </p>
        <form onSubmit={handleSubmit} className="form">
          <label>
            Quantity sold
            <input
              type="number"
              min="1"
              max={medicine.quantity}
              value={quantitySold}
              onChange={(e) => setQuantitySold(e.target.value)}
            />
          </label>
          <label>
            Buyer (optional)
            <input value={buyer} onChange={(e) => setBuyer(e.target.value)} placeholder="Walk-in customer" />
          </label>

          <div className="sale-total">Total: ₹{total}</div>

          {validationError && <div className="form-error">{validationError}</div>}

          <div className="form-actions">
            <button type="button" className="btn btn-ghost" onClick={onCancel} disabled={submitting}>
              Cancel
            </button>
            <button type="submit" className="btn btn-primary" disabled={submitting}>
              {submitting ? "Recording…" : "Confirm sale"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
