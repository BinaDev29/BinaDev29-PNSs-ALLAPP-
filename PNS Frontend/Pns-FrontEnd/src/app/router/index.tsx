// PNS-FrontEnd/src/app/router/index.tsx
import React from 'react';
import { Routes, Route, Navigate } from 'react-router-dom';
import { useAuth } from '../providers/useAuth'; // <<<<<<< Ensure this line is correct!

// Public Pages
import HomePage from '../../pages/HomePage';
import AboutPage from '../../pages/AboutPage';
import ContactPage from '../../pages/ContactPage';
import LoginPage from '../../features/auth/pages/LoginPage';

// Admin Dashboard Pages
import ClientApplicationsDashboard from '../../features/client-applications/pages/ClientApplicationsDashboard';
import EmailRecipientsDashboard from '../../features/email-recipients/pages/EmailRecipientsDashboard';
import NotificationDashboard from '../../features/notifications/pages/NotificationDashboard';

// Layout Components
import PublicLayout from '../components/PublicLayout';
import AdminLayout from '../components/AdminLayout';

// PrivateRoute component to protect our routes
const PrivateRoute: React.FC<{ children: React.ReactElement }> = ({ children }) => { // <<<<<<< Changed JSX.Element to React.ReactElement
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" replace />;
};

const AppRoutes: React.FC = () => {
  return (
    <Routes>
      {/* Public Routes with PublicLayout */}
      <Route path="/" element={<PublicLayout />}>
        <Route index element={<HomePage />} />
        <Route path="about" element={<AboutPage />} />
        <Route path="contact" element={<ContactPage />} />
        <Route path="login" element={<LoginPage />} />
      </Route>

      {/* Protected Admin Routes with AdminLayout */}
      <Route
        path="/admin"
        element={
          <PrivateRoute>
            <AdminLayout />
          </PrivateRoute>
        }
      >
        <Route index element={<Navigate to="/admin/client-applications" replace />} />
        <Route path="client-applications" element={<ClientApplicationsDashboard />} />
        <Route path="email-recipients" element={<EmailRecipientsDashboard />} />
        <Route path="notifications" element={<NotificationDashboard />} />
      </Route>

      {/* Catch-all for undefined routes */}
      <Route path="*" element={<div>404 - Page Not Found</div>} />
    </Routes>
  );
};

export default AppRoutes;