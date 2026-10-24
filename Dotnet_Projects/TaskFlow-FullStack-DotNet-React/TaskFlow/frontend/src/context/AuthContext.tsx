import { createContext, useCallback, useEffect, useMemo, useState, ReactNode } from 'react';
import { authApi } from '../api/authApi';
import type { AuthUser } from '../types';

interface AuthContextValue {
  user: AuthUser | null;
  isAuthenticated: boolean;
  isAdmin: boolean;
  loading: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (fullName: string, email: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
}

// eslint-disable-next-line react-refresh/only-export-components
export const AuthContext = createContext<AuthContextValue | undefined>(undefined);

// Classic React Context + Provider pattern for global auth state — the
// alternative to prop-drilling the current user through every component.
export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<AuthUser | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const raw = localStorage.getItem('authUser');
    if (raw) setUser(JSON.parse(raw));
    setLoading(false);
  }, []);

  const persistSession = (auth: {
    accessToken: string; refreshToken: string; userId: string; fullName: string; email: string; role: 'Admin' | 'User';
  }) => {
    const authUser: AuthUser = { userId: auth.userId, fullName: auth.fullName, email: auth.email, role: auth.role };
    localStorage.setItem('accessToken', auth.accessToken);
    localStorage.setItem('refreshToken', auth.refreshToken);
    localStorage.setItem('authUser', JSON.stringify(authUser));
    setUser(authUser);
  };

  const login = useCallback(async (email: string, password: string) => {
    const auth = await authApi.login(email, password);
    persistSession(auth);
  }, []);

  const register = useCallback(async (fullName: string, email: string, password: string) => {
    const auth = await authApi.register(fullName, email, password);
    persistSession(auth);
  }, []);

  const logout = useCallback(async () => {
    const refreshToken = localStorage.getItem('refreshToken');
    try {
      if (refreshToken) await authApi.logout(refreshToken);
    } finally {
      localStorage.removeItem('accessToken');
      localStorage.removeItem('refreshToken');
      localStorage.removeItem('authUser');
      setUser(null);
    }
  }, []);

  const value = useMemo<AuthContextValue>(
    () => ({
      user,
      isAuthenticated: !!user,
      isAdmin: user?.role === 'Admin',
      loading,
      login,
      register,
      logout,
    }),
    [user, loading, login, register, logout]
  );

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
