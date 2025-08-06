// PNS-FrontEnd/src/app/providers/useAuth.ts
import { useContext } from 'react';
import { AuthContext, AuthContextType } from './AuthContext'; // Import from AuthContext.ts

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext); // Get the current value of AuthContext
  if (context === undefined) {
    // If the context is undefined, it means useAuth was called outside of an AuthProvider
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context; // Return the context value (which will be of type AuthContextType)
};