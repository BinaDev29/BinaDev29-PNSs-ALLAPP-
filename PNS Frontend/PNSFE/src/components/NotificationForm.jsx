// src/components/NotificationForm.jsx
import React, { useState, useEffect } from 'react';

const NotificationForm = ({ onNotificationSent }) => {
  const [title, setTitle] = useState('');
  const [body, setBody] = useState('');
  const [toEmail, setToEmail] = useState('');
  const [toPhoneNumber, setToPhoneNumber] = useState('');
  const [clientApplications, setClientApplications] = useState([]);
  const [selectedClientId, setSelectedClientId] = useState('');
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

    const notificationData = {
      title,
      body,
      toEmail,
      toPhoneNumber,
      clientApplicationId: selectedClientId,
    };

    try {
      const response = await fetch('https://localhost:7045/api/notification', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(notificationData),
      });

      if (response.ok) {
        setStatusMessage('Notification sent successfully!');
        setTitle('');
        setBody('');
        setToEmail('');
        setToPhoneNumber('');
        onNotificationSent();
      } else {
        const errorData = await response.json();
        setStatusMessage(`Notification failed to send! Error: ${JSON.stringify(errorData.errors)}`);
      }
    } catch (error) {
      console.error('Network Error:', error);
      setStatusMessage('Network error. Check your connection or API server.');
    }
  };

  return (
    <div className="card p-4 mb-4">
      <h2 className="mb-4">Create New Notification</h2>
      <form onSubmit={handleSubmit}>
        <div className="mb-3">
          <label className="form-label">Select Client Application:</label>
          <select className="form-select" value={selectedClientId} onChange={(e) => setSelectedClientId(e.target.value)} required>
            {clientApplications.map(client => (
              <option key={client.id} value={client.id}>{client.name}</option>
            ))}
          </select>
        </div>
        <div className="mb-3">
          <label className="form-label">Title:</label>
          <input type="text" className="form-control" value={title} onChange={(e) => setTitle(e.target.value)} required />
        </div>
        <div className="mb-3">
          <label className="form-label">Message:</label>
          <textarea className="form-control" rows="3" value={body} onChange={(e) => setBody(e.target.value)} required />
        </div>
        <div className="mb-3">
          <label className="form-label">Email Address:</label>
          <input type="email" className="form-control" value={toEmail} onChange={(e) => setToEmail(e.target.value)} />
        </div>
        <div className="mb-3">
          <label className="form-label">Phone Number:</label>
          <input type="tel" className="form-control" value={toPhoneNumber} onChange={(e) => setToPhoneNumber(e.target.value)} />
        </div>
        <button type="submit" className="btn btn-primary w-100">Send Notification</button>
      </form>
      {statusMessage && <div className="mt-3 text-center text-danger">{statusMessage}</div>}
    </div>
  );
};

export default NotificationForm;