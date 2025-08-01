// src/components/ClientApplicationForm.jsx
import React, { useState } from 'react';

const ClientApplicationForm = () => {
  const [name, setName] = useState('');
  const [statusMessage, setStatusMessage] = useState('');

  const handleSubmit = async (e) => {
    e.preventDefault();
    const data = { name };
    try {
      const response = await fetch('https://localhost:7045/api/client-applications', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        setStatusMessage('Client Application registered successfully!');
        setName('');
      } else {
        const errorData = await response.json();
        setStatusMessage(`Registration failed. Error: ${JSON.stringify(errorData.errors)}`);
      }
    } catch (error) {
      setStatusMessage('Network error. Please check your API server.');
    }
  };

  return (
    <div className="card p-4 mb-4">
      <h2 className="mb-4">Create New Client Application</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label className="form-label">Name:</label>
          <input type="text" className="form-control" value={name} onChange={(e) => setName(e.target.value)} required />
        </div>
        <button type="submit" className="btn btn-primary w-100">Submit Client Application</button>
      </form>
      {statusMessage && <div className="mt-3 text-center text-info">{statusMessage}</div>}
    </div>
  );
};

export default ClientApplicationForm;