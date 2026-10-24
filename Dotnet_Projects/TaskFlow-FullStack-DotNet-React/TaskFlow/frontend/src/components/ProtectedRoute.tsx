import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { Loader } from './Loader';

interface Props {
  requireAdmin?: boolean;
}

// Route guard: redirects unauthenticated users to /login, and non-admins away
// from admin-only screens. Wraps <Outlet /> so it can protect a whole subtree
// of nested routes declared in routes/AppRoutes.tsx.
export function ProtectedRoute({ requireAdmin = false }: Props) {
  const { isAuthenticated, isAdmin, loading } = useAuth();

  if (loading) return <Loader />;
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  if (requireAdmin && !isAdmin) return <Navigate to="/tasks" replace />;

  return <Outlet />;
}
