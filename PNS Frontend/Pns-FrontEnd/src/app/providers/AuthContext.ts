// PNS-FrontEnd/src/app/providers/AuthContext.ts
import { createContext } from 'react';

export interface AuthContextType {
  isAuthenticated: boolean;
  login: (apiKey: string) => void;
  logout: () => void;
  getApiKey: () => string | null;
}

export const AuthContext = createContext<AuthContextType | undefined>(undefined);