// src/app/providers/QueryClientProvider.tsx
import React from 'react';
import { QueryClient, QueryClientProvider as ReactQueryProvider } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      refetchOnWindowFocus: false, // Don't refetch queries automatically when the window regains focus
      retry: false, // Don't retry failed queries automatically
    },
  },
});

interface QueryClientProviderProps {
  children: React.ReactNode;
}

const QueryClientProvider: React.FC<QueryClientProviderProps> = ({ children }) => {
  return (
    <ReactQueryProvider client={queryClient}>
      {children}
      {/* Show React Query Devtools only in development environment */}
      {process.env.NODE_ENV === 'development' && <ReactQueryDevtools initialIsOpen={false} />}
    </ReactQueryProvider>
  );
}; // Ensure the parenthesis here is correctly closed

export default QueryClientProvider;