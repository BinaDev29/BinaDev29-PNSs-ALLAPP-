// src/components/NotificationsList.jsx
import React, { useState, useEffect } from 'react';
import NotificationCard from './NotificationCard.jsx';

const NotificationsList = ({ refreshTrigger }) => {
  const [notifications, setNotifications] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchNotifications = async () => {
      try {
        const response = await fetch('https://localhost:7045/api/notification');
        if (response.ok) {
          const data = await response.json();
          setNotifications(data);
        } else {
          console.error('Failed to fetch notifications');
        }
      } catch (error) {
        console.error('Network error fetching notifications:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchNotifications();
  }, [refreshTrigger]);

  if (loading) {
    return <div className="text-center text-muted">Loading notifications...</div>;
  }

  return (
    <div className="notifications-list card p-4">
      <h3 className="mb-3">Sent Notifications</h3>
      {notifications.length === 0 ? (
        <div className="alert alert-info bg-transparent text-muted border-0">No notifications have been sent yet.</div>
      ) : (
        notifications.map((notification) => (
          <NotificationCard key={notification.id} notification={notification} />
        ))
      )}
    </div>
  );
};

export default NotificationsList;