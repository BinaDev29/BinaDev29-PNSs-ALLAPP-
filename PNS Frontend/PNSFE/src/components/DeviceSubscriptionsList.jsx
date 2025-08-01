// src/components/DeviceSubscriptionsList.jsx
import React, { useState, useEffect } from 'react';

const DeviceSubscriptionsList = () => {
  const [subscriptions, setSubscriptions] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchSubscriptions = async () => {
      try {
        const response = await fetch('https://localhost:7045/api/device-subscriptions');
        const data = await response.json();
        setSubscriptions(data);
      } catch (error) {
        console.error('Failed to fetch device subscriptions:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchSubscriptions();
  }, []);

  if (loading) return <div className="text-center text-muted">Loading...</div>;

  return (
    <div className="card p-4">
      <h3 className="mb-3">Device Subscriptions</h3>
      {subscriptions.length === 0 ? (
        <div className="alert alert-info bg-transparent text-muted border-0">No Device Subscriptions found.</div>
      ) : (
        <ul className="list-group list-group-flush">
          {subscriptions.map(sub => (
            <li key={sub.id} className="list-group-item bg-transparent text-white border-bottom-0" style={{borderBottom: '1px solid var(--border-color-dark)'}}>
              <p className="mb-1">Device Token: {sub.deviceToken}</p>
              <small className="text-muted d-block">Client ID: {sub.clientApplicationId}</small>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default DeviceSubscriptionsList;