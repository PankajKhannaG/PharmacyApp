import React, { useState } from "react";

const emptyForm = {
  fullName: "",
  notes: "",
  expiryDate: "",
  quantity: "",
  price: "",
  brand: "",
};

export default function AddMedicineForm({ onSubmit, onCancel, submitting }) {
  const [form, setForm] = useState(emptyForm);
  const [validationError, setValidationError] = useState("");

  function handleChange(e) {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  }

  function handleSubmit(e) {
    e.preventDefault();
    setValidationError("");

    if (!form.fullName.trim() || !form.brand.trim() || !form.expiryDate) {
      setValidationError("Full name, brand and expiry date are required.");
      return;
    }
    if (Number(form.quantity) < 0 || Number.isNaN(Number(form.quantity))) {
      setValidationError("Quantity must be a valid non-negative number.");
      return;
    }
    if (Number(form.price) <= 0 || Number.isNaN(Number(form.price))) {
      setValidationError("Price must be a valid positive number.");
      return;
    }

    onSubmit({
      fullName: form.fullName.trim(),
      notes: form.notes.trim() || null,
      expiryDate: form.expiryDate,
      quantity: Number(form.quantity),
      price: Number(Number(form.price).toFixed(2)),
      brand: form.brand.trim(),
    });
  }

  return (
    <div className="modal-backdrop">
      <div className="modal">
        <h2>Add medicine</h2>
        <form onSubmit={handleSubmit} className="form">
          <label>
            Full name
            <input name="fullName" value={form.fullName} onChange={handleChange} placeholder="e.g. Paracetamol 500mg" />
          </label>

          <label>
            Brand
            <input name="brand" value={form.brand} onChange={handleChange} placeholder="e.g. Cipla" />
          </label>

          <div className="form-row">
            <label>
              Expiry date
              <input type="date" name="expiryDate" value={form.expiryDate} onChange={handleChange} />
            </label>
            <label>
              Quantity
              <input type="number" name="quantity" min="0" value={form.quantity} onChange={handleChange} />
            </label>
            <label>
              Price
              <input type="number" name="price" min="0" step="0.01" value={form.price} onChange={handleChange} />
            </label>
          </div>

          <label>
            Notes
            <textarea name="notes" rows={3} value={form.notes} onChange={handleChange} placeholder="Optional notes" />
          </label>

          {validationError && <div className="form-error">{validationError}</div>}

          <div className="form-actions">
            <button type="button" className="btn btn-ghost" onClick={onCancel} disabled={submitting}>
              Cancel
            </button>
            <button type="submit" className="btn btn-primary" disabled={submitting}>
              {submitting ? "Saving…" : "Save medicine"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
