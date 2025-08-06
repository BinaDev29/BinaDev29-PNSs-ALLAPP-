// PNS-FrontEnd/src/app/providers/AuthProvider.tsx
import React, { useState, useEffect, ReactNode } from 'react';
// import { AuthContext, AuthContextType } from './AuthContext'; // Import from AuthContext.ts
import { AuthContext } from './AuthContext';

const API_KEY_STORAGE_KEY = 'pns_api_key';

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(() => {
    // Initialize authentication state based on whether an API key exists in localStorage
    return !!localStorage.getItem(API_KEY_STORAGE_KEY);
  });

  // Function to handle user login
  const login = (apiKey: string) => {
    localStorage.setItem(API_KEY_STORAGE_KEY, apiKey); // Store API key
    setIsAuthenticated(true); // Set authentication status to true
  };

  // Function to handle user logout
  const logout = () => {
    localStorage.removeItem(API_KEY_STORAGE_KEY); // Remove API key
    setIsAuthenticated(false); // Set authentication status to false
  };

  // Function to retrieve the stored API key
  const getApiKey = () => {
    return localStorage.getItem(API_KEY_STORAGE_KEY);
  };

  // Effect to listen for changes in localStorage across different tabs/windows
  useEffect(() => {
    const handleStorageChange = () => {
      setIsAuthenticated(!!localStorage.getItem(API_KEY_STORAGE_KEY));
    };

    window.addEventListener('storage', handleStorageChange); // Add event listener
    return () => {
      window.removeEventListener('storage', handleStorageChange); // Clean up event listener
    };
  }, []);

  return (
    // Provide the authentication context to its children components
    <AuthContext.Provider value={{ isAuthenticated, login, logout, getApiKey }}>
      {children}
    </AuthContext.Provider>
  );
};