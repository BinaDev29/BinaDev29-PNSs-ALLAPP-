// PNS-FrontEnd/src/app/main.tsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import './styles/index.css'; // Global styles
import { BrowserRouter } from 'react-router-dom';
import { ThemeProvider } from '@mui/material/styles';
import { CssBaseline } from '@mui/material';
import theme from './styles/theme.ts'; // Our custom MUI theme
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'; // Correct Devtools import
import { AuthProvider } from './providers/AuthProvider.tsx'; // Authentication Context Provider

// Create a React Query client
const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false, // Prevents unnecessary data fetching when window is refocused
      staleTime: 1000 * 60 * 5, // Data is considered fresh for 5 minutes
    },
  },
});

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <BrowserRouter>
      <ThemeProvider theme={theme}>
        <CssBaseline /> {/* Resets CSS to a consistent baseline style */}
        <QueryClientProvider client={queryClient}>
          <AuthProvider> {/* Our AuthProvider */}
            <App />
          </AuthProvider>
          <ReactQueryDevtools initialIsOpen={false} /> {/* React Query Devtools (for inspection) */}
        </QueryClientProvider>
      </ThemeProvider>
    </BrowserRouter>
  </React.StrictMode>,
);