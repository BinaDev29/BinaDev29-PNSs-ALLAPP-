// src/components/DeviceSubscriptionForm.jsx
import React, { useState, useEffect } from 'react';

const DeviceSubscriptionForm = () => {
  const [clientApplications, setClientApplications] = useState([]);
  const [selectedClientId, setSelectedClientId] = useState('');
  const [deviceToken, setDeviceToken] = useState('');
  const [statusMessage, setStatusMessage] = useState('');

  useEffect(() => {
    const fetchClients = async () => {
      try {
        const response = await fetch('https://localhost:7045/api/client-applications');
        const data = await response.json();
        setClientApplications(data);
        if (data.length > 0) {
          setSelectedClientId(data[0].id);
        }
      } catch (error) {
        console.error('Failed to fetch client applications:', error);
      }
    };
    fetchClients();
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!selectedClientId) {
      setStatusMessage('Please select a Client Application.');
      return;
    }
    const data = { clientApplicationId: selectedClientId, deviceToken };

    try {
      const response = await fetch('https://localhost:7045/api/device-subscriptions', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        setStatusMessage('Device Subscription registered successfully!');
        setDeviceToken('');
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
      <h2 className="mb-4">Create New Device Subscription</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label className="form-label">Select Client Application:</label>
          <select className="form-select" value={selectedClientId} onChange={(e) => setSelectedClientId(e.target.value)}>
            {clientApplications.map(client => (
              <option key={client.id} value={client.id}>{client.name}</option>
            ))}
          </select>
        </div>
        <div className="mb-3">
          <label className="form-label">Device Token:</label>
          <input type="text" className="form-control" value={deviceToken} onChange={(e) => setDeviceToken(e.target.value)} required />
        </div>
        <button type="submit" className="btn btn-primary w-100">Subscribe Device</button>
      </form>
      {statusMessage && <div className="mt-3 text-center text-info">{statusMessage}</div>}
    </div>
  );
};

export default DeviceSubscriptionForm;