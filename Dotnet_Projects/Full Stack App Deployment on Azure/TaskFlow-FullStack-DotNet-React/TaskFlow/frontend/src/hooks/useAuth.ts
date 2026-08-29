import { useContext } from 'react';
import { AuthContext } from '../context/AuthContext';

// Thin wrapper hook so components do `useAuth()` instead of importing the
// context directly everywhere — also gives us a single place to guard against
// using the hook outside the provider.
export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within an AuthProvider');
  return ctx;
}
