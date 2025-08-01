// src/App.jsx
import React, { useState } from 'react';
import ClientApplicationForm from './components/ClientApplicationForm.jsx';
import ClientApplicationsList from './components/ClientApplicationsList.jsx';
import DeviceSubscriptionForm from './components/DeviceSubscriptionForm.jsx';
import DeviceSubscriptionsList from './components/DeviceSubscriptionsList.jsx';
import NotificationForm from './components/NotificationForm.jsx';
import NotificationsList from './components/NotificationsList.jsx';
import './App.css';

function App() {
  const [activeTab, setActiveTab] = useState('notifications');
  const [refreshList, setRefreshList] = useState(0);

  const handleNotificationSent = () => {
    setRefreshList(prev => prev + 1);
  };

  return (
    <div className="App-container container mt-5">
      <header className="text-center mb-5">
        <h1>Push Notification Dashboard</h1>
      </header>

      <ul className="nav nav-tabs justify-content-center mb-4">
        <li className="nav-item">
          <button className={`nav-link ${activeTab === 'notifications' ? 'active' : ''}`} onClick={() => setActiveTab('notifications')}>Notifications</button>
        </li>
        <li className="nav-item">
          <button className={`nav-link ${activeTab === 'clients' ? 'active' : ''}`} onClick={() => setActiveTab('clients')}>Client Apps</button>
        </li>
        <li className="nav-item">
          <button className={`nav-link ${activeTab === 'devices' ? 'active' : ''}`} onClick={() => setActiveTab('devices')}>Device Subscriptions</button>
        </li>
      </ul>

      <div className="tab-content">
        {activeTab === 'notifications' && (
          <div className="row">
            <div className="col-lg-6 mb-4">
              <NotificationForm onNotificationSent={handleNotificationSent} />
            </div>
            <div className="col-lg-6 mb-4">
              <NotificationsList refreshTrigger={refreshList} />
            </div>
          </div>
        )}
        {activeTab === 'clients' && (
          <div className="row">
            <div className="col-lg-6 mb-4">
              <ClientApplicationForm />
            </div>
            <div className="col-lg-6 mb-4">
              <ClientApplicationsList />
            </div>
          </div>
        )}
        {activeTab === 'devices' && (
          <div className="row">
            <div className="col-lg-6 mb-4">
              <DeviceSubscriptionForm />
            </div>
            <div className="col-lg-6 mb-4">
              <DeviceSubscriptionsList />
            </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default App;