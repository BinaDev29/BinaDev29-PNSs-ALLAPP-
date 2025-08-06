// src/features/client-applications/api/clientApplicationService.ts

import apiClient from '../../../app/api/axiosInstance';
import type { // You should use the 'type' keyword
  ClientApplication,
  CreateClientApplicationDto,
  UpdateClientApplicationDto,
  PaginatedApiResponse,
  ApiResponse
} from '../types';

const BASE_URL = '/ClientApplications'; // Your backend base URL is already set in axiosInstance

export const clientApplicationService = {
  /**
   * Fetches all client applications.
   * @returns A promise that resolves to a paginated list of client applications.
   */
  getAllClientApplications: async (): Promise<PaginatedApiResponse<ClientApplication>> => {
    const response = await apiClient.get<PaginatedApiResponse<ClientApplication>>(BASE_URL);
    return response.data;
  },

  /**
   * Fetches a single client application by its ID.
   * @param id The ID of the client application.
   * @returns A promise that resolves to a single client application.
   */
  getClientApplicationById: async (id: string): Promise<ApiResponse<ClientApplication>> => {
    const response = await apiClient.get<ApiResponse<ClientApplication>>(`${BASE_URL}/${id}`);
    return response.data;
  },

  /**
   * Creates a new client application.
   * @param data The data for the new client application.
   * @returns A promise that resolves to the created client application.
   */
  createClientApplication: async (data: CreateClientApplicationDto): Promise<ApiResponse<ClientApplication>> => {
    const response = await apiClient.post<ApiResponse<ClientApplication>>(BASE_URL, data);
    return response.data;
  },

  /**
   * Updates an existing client application.
   * @param id The ID of the client application to update.
   * @param data The updated data for the client application.
   * @returns A promise that resolves to the updated client application.
   */
  updateClientApplication: async (id: string, data: UpdateClientApplicationDto): Promise<ApiResponse<ClientApplication>> => {
    const response = await apiClient.put<ApiResponse<ClientApplication>>(`${BASE_URL}/${id}`, data);
    return response.data;
  },

  /**
   * Deletes a client application by its ID.
   * @param id The ID of the client application to delete.
   * @returns A promise that resolves when the deletion is successful.
   */
  deleteClientApplication: async (id: string): Promise<ApiResponse<null>> => {
    const response = await apiClient.delete<ApiResponse<null>>(`${BASE_URL}/${id}`);
    return response.data;
  },
};