// src/features/client-applications/hooks/useCreateClientApplication.ts
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { clientApplicationService } from '../api/clientApplicationService';
import { CreateClientApplicationDto, ClientApplication, ApiResponse } from '../types';
import { CLIENT_APPLICATIONS_QUERY_KEY } from './useClientApplications'; // ከሌላው hook የመጣውን key እንጠቀማለን

export const useCreateClientApplication = () => {
  const queryClient = useQueryClient();

  return useMutation<ApiResponse<ClientApplication>, Error, CreateClientApplicationDto>({
    mutationFn: clientApplicationService.createClientApplication,
    onSuccess: (data) => {
      // Invalidate the cache for client applications to refetch the list
      // This ensures the new application appears in the list immediately
      queryClient.invalidateQueries({ queryKey: CLIENT_APPLICATIONS_QUERY_KEY });
      console.log('Client application created successfully:', data.data.name);
      // Optionally, show a success toast/notification
    },
    onError: (error) => {
      console.error('Failed to create client application:', error.message);
      // Optionally, show an error toast/notification
    },
  });
};