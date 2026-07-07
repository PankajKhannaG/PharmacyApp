import React from "react";

export default function SearchBar({ value, onChange }) {
  return (
    <div className="search-bar">
      <input
        type="text"
        placeholder="Search medicines by name…"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        aria-label="Search medicines by name"
      />
    </div>
  );
}
