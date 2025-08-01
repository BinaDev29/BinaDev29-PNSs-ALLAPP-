// src/components/ClientApplicationsList.jsx
import React, { useState, useEffect } from 'react';

const ClientApplicationsList = () => {
  const [clients, setClients] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchClients = async () => {
      try {
        const response = await fetch('https://localhost:7045/api/client-applications');
        const data = await response.json();
        setClients(data);
      } catch (error) {
        console.error('Failed to fetch client applications:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchClients();
  }, []);

  if (loading) return <div className="text-center text-muted">Loading...</div>;

  return (
    <div className="card p-4">
      <h3 className="mb-3">Client Applications</h3>
      {clients.length === 0 ? (
        <div className="alert alert-info bg-transparent text-muted border-0">No Client Applications found.</div>
      ) : (
        <ul className="list-group list-group-flush">
          {clients.map(client => (
            <li key={client.id} className="list-group-item d-flex justify-content-between align-items-center bg-transparent text-white border-bottom-0" style={{borderBottom: '1px solid var(--border-color-dark)'}}>
              {client.name}
              <span className="badge rounded-pill" style={{ backgroundColor: 'var(--primary-color)' }}>{client.id}</span>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default ClientApplicationsList;