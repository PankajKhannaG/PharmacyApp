import { useCallback, useEffect, useState } from "react";
import MedicineGrid from "./components/MedicineGrid";
import AddMedicineForm from "./components/AddMedicineForm";
import SaleForm from "./components/SaleForm";
import SearchBar from "./components/SearchBar";
import {
  fetchMedicines,
  createMedicine,
  deleteMedicine,
  recordSale,
} from "./api/medicineApi";
import "./App.css";

export default function App() {
  const [medicines, setMedicines] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [search, setSearch] = useState("");
  const [showAddForm, setShowAddForm] = useState(false);
  const [saleTarget, setSaleTarget] = useState(null);
  const [submitting, setSubmitting] = useState(false);
  const [toast, setToast] = useState("");

  const loadMedicines = useCallback(async (searchTerm) => {
    setLoading(true);
    setError("");
    try {
      const data = await fetchMedicines(searchTerm);
      setMedicines(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      loadMedicines(search);
    }, 300); // debounce search input
    return () => clearTimeout(timer);
  }, [search, loadMedicines]);

  useEffect(() => {
    if (!toast) return;
    const t = setTimeout(() => setToast(""), 3000);
    return () => clearTimeout(t);
  }, [toast]);

  async function handleAddMedicine(payload) {
    setSubmitting(true);
    try {
      await createMedicine(payload);
      setShowAddForm(false);
      setToast("Medicine added.");
      await loadMedicines(search);
    } catch (err) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  }

  async function handleDeleteMedicine(medicine) {
    if (!window.confirm(`Delete "${medicine.fullName}"? This cannot be undone.`)) return;
    try {
      await deleteMedicine(medicine.id);
      setToast("Medicine deleted.");
      await loadMedicines(search);
    } catch (err) {
      setError(err.message);
    }
  }

  async function handleRecordSale(payload) {
    setSubmitting(true);
    try {
      await recordSale(payload);
      setSaleTarget(null);
      setToast("Sale recorded.");
      await loadMedicines(search);
    } catch (err) {
      setError(err.message);
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <div className="app">
      <header className="app-header">
        <div>
          <h1>ABC Pharmacy</h1>
          <p className="subtitle">Medicine inventory &amp; sales tracker</p>
        </div>
        <button className="btn btn-primary" onClick={() => setShowAddForm(true)}>
          + Add medicine
        </button>
      </header>

      <main className="app-main">
        <SearchBar value={search} onChange={setSearch} />

        <MedicineGrid
          medicines={medicines}
          loading={loading}
          error={error}
          onSell={setSaleTarget}
          onDelete={handleDeleteMedicine}
        />
      </main>

      {showAddForm && (
        <AddMedicineForm
          onSubmit={handleAddMedicine}
          onCancel={() => setShowAddForm(false)}
          submitting={submitting}
        />
      )}

      {saleTarget && (
        <SaleForm
          medicine={saleTarget}
          onSubmit={handleRecordSale}
          onCancel={() => setSaleTarget(null)}
          submitting={submitting}
        />
      )}

      {toast && <div className="toast">{toast}</div>}
    </div>
  );
}