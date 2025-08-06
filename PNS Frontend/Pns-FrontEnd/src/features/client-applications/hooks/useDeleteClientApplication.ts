// src/features/client-applications/hooks/useDeleteClientApplication.ts
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { clientApplicationService } from '../api/clientApplicationService';
import { ApiResponse } from '../types';
import { CLIENT_APPLICATIONS_QUERY_KEY } from './useClientApplications';

export const useDeleteClientApplication = () => {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<null>, Error, string>({ // string is the ID of the item to delete
    mutationFn: clientApplicationService.deleteClientApplication,
    // 'data' የሚለውን parameter ወደ '_data' ቀይር
    onSuccess: (_data, deletedId) => {
      queryClient.invalidateQueries({ queryKey: CLIENT_APPLICATIONS_QUERY_KEY }); // Refetch the list
      console.log(`Client application with ID ${deletedId} deleted successfully.`);
    },
    onError: (error) => {
      console.error('Failed to delete client application:', error.message);
    },
  });
};