// src/components/NotificationCard.jsx
import React from 'react';

const NotificationCard = ({ notification }) => {
  return (
    <div className="card shadow-sm mb-3">
      <div className="card-body">
        <h5 className="card-title text-start">{notification.title}</h5>
        <p className="card-text text-white">{notification.body}</p>
        <div className="card-footer bg-transparent border-top-0 pt-0">
          {notification.toEmail && (
            <small className="text-muted d-block">
              Email: {notification.toEmail}
            </small>
          )}
          {notification.toPhoneNumber && (
            <small className="text-muted d-block">
              Phone: {notification.toPhoneNumber}
            </small>
          )}
        </div>
      </div>
    </div>
  );
};

export default NotificationCard;