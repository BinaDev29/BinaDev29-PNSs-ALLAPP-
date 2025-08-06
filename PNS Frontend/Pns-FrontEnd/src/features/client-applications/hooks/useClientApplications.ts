// src/features/client-applications/hooks/useClientApplications.ts
import { useQuery } from '@tanstack/react-query';
import { clientApplicationService } from '../api/clientApplicationService';
import { ClientApplication, PaginatedApiResponse } from '../types';

// Query Key for Client Applications
// This is used by React Query to cache and identify the data
export const CLIENT_APPLICATIONS_QUERY_KEY = ['clientApplications'];

export const useClientApplications = () => {
  return useQuery<PaginatedApiResponse<ClientApplication>, Error>({
    queryKey: CLIENT_APPLICATIONS_QUERY_KEY, // The unique key for this query's cached data
    queryFn: clientApplicationService.getAllClientApplications, // The function that fetches the data
    // Optional: Add more options here like staleTime, cacheTime, etc.
    // staleTime: 5 * 60 * 1000, // Data is considered fresh for 5 minutes (in milliseconds)
    // cacheTime: 10 * 60 * 1000, // Data stays in cache for 10 minutes (in milliseconds)
  });
};