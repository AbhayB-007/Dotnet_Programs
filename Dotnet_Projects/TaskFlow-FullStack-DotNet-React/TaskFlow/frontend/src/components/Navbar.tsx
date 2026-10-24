import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';

export function Navbar() {
  const { user, isAdmin, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = async () => {
    await logout();
    navigate('/login');
  };

  const linkClass = ({ isActive }: { isActive: boolean }) =>
    `rounded-md px-3 py-2 text-sm font-medium ${isActive ? 'bg-brand-500 text-white' : 'text-gray-600 hover:bg-gray-100'}`;

  return (
    <nav className="border-b bg-white px-6 py-3 shadow-sm">
      <div className="mx-auto flex max-w-6xl items-center justify-between">
        <div className="flex items-center gap-6">
          <span className="text-lg font-bold text-brand-600">TaskFlow</span>
          <NavLink to="/tasks" className={linkClass}>My Tasks</NavLink>
          {isAdmin && <NavLink to="/admin/users" className={linkClass}>Manage Users</NavLink>}
        </div>
        <div className="flex items-center gap-4">
          <span className="text-sm text-gray-500">
            {user?.fullName} <span className="ml-1 rounded bg-gray-100 px-2 py-0.5 text-xs font-medium">{user?.role}</span>
          </span>
          <button onClick={handleLogout} className="text-sm font-medium text-red-500 hover:text-red-700">
            Log out
          </button>
        </div>
      </div>
    </nav>
  );
}
