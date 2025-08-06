// src/features/client-applications/types/index.ts

export interface ClientApplication {
  id: string;
  name: string;
  description: string;
  logoUrl?: string;
  apiKey?: string;
  createdAt: string;
  updatedAt: string;
}

export interface CreateClientApplicationDto {
  name: string;
  description: string;
  logoUrl?: string;
}

export interface UpdateClientApplicationDto {
  id: string;
  name: string;
  description: string;
  logoUrl?: string;
}

export interface ApiResponse<T> {
  data: T;
  message?: string;
  success: boolean;
  statusCode: number;
}

export interface PaginatedApiResponse<T> {
  data: T[];
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
  message?: string;
  success: boolean;
  statusCode: number;
}