// PNS-FrontEnd/src/app/api/axiosInstance.ts
import axios from 'axios';

// Ensure this matches your .env variable
// Make sure you have a .env file in your project root folder with VITE_API_BASE_URL=https://localhost:7198 (or http://localhost:5217)
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor to add API Key from localStorage before sending the request
apiClient.interceptors.request.use(
  (config) => {
    const apiKey = localStorage.getItem('X-API-KEY'); // Retrieves API Key from localStorage

    if (apiKey) {
      config.headers['X-API-KEY'] = apiKey; // Adds it to the request header
    }
    // There's no need for console.error here, not all requests require an API Key (e.g., login itself)
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to globally handle 401/403 errors
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response && (error.response.status === 401 || error.response.status === 403)) {
      console.error('Authentication or Authorization Error:', error.response.data);
      // When 401/403 occurs, remove the API Key and redirect to the login page
      // This requires accessing AuthContext outside of a React component.
      // A more robust solution might involve using a custom event or a global state manager.
      // For now, we'll just remove the key and redirect.
      localStorage.removeItem('X-API-KEY');
      window.location.href = '/login'; // Redirect to the login page
    }
    return Promise.reject(error);
  }
);

export default apiClient;