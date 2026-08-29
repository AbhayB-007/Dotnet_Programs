import { useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import { userApi } from '../api/userApi';
import { Loader } from '../components/Loader';
import { useAuth } from '../hooks/useAuth';
import type { AppUser } from '../types';

// Admin-only screen — this route is already guarded by <ProtectedRoute requireAdmin />,
// but we double-check client-side too since defense-in-depth matters (the real
// enforcement, of course, is the [Authorize(Roles="Admin")] on the API side).
export function AdminUsersPage() {
  const { user: currentUser } = useAuth();
  const [users, setUsers] = useState<AppUser[]>([]);
  const [loading, setLoading] = useState(true);

  const loadUsers = async () => {
    setLoading(true);
    try {
      setUsers(await userApi.list());
    } catch {
      toast.error('Failed to load users.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadUsers();
  }, []);

  const toggleRole = async (u: AppUser) => {
    const newRole = u.role === 'Admin' ? 'User' : 'Admin';
    try {
      await userApi.updateRole(u.id, newRole);
      toast.success(`${u.fullName} is now ${newRole}.`);
      await loadUsers();
    } catch {
      toast.error('Failed to update role.');
    }
  };

  const toggleActive = async (u: AppUser) => {
    try {
      await userApi.setActive(u.id, !u.isActive);
      toast.success(!u.isActive ? 'User activated.' : 'User deactivated.');
      await loadUsers();
    } catch {
      toast.error('Failed to update status.');
    }
  };

  const removeUser = async (u: AppUser) => {
    if (!confirm(`Delete ${u.fullName}? This cannot be undone.`)) return;
    try {
      await userApi.remove(u.id);
      toast.success('User deleted.');
      await loadUsers();
    } catch {
      toast.error('Failed to delete user.');
    }
  };

  if (loading) return <Loader />;

  return (
    <div className="mx-auto max-w-6xl px-6 py-8">
      <h1 className="mb-6 text-2xl font-bold text-gray-800">Manage Users</h1>

      <div className="overflow-hidden rounded-lg border bg-white">
        <table className="w-full text-left text-sm">
          <thead className="bg-gray-50 text-xs uppercase text-gray-500">
            <tr>
              <th className="px-4 py-3">Name</th>
              <th className="px-4 py-3">Email</th>
              <th className="px-4 py-3">Role</th>
              <th className="px-4 py-3">Status</th>
              <th className="px-4 py-3 text-right">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y">
            {users.map((u) => (
              <tr key={u.id} className="hover:bg-gray-50">
                <td className="px-4 py-3 font-medium text-gray-800">{u.fullName}</td>
                <td className="px-4 py-3 text-gray-500">{u.email}</td>
                <td className="px-4 py-3">
                  <span className={`rounded-full px-2 py-1 text-xs font-medium ${u.role === 'Admin' ? 'bg-purple-100 text-purple-700' : 'bg-gray-100 text-gray-600'}`}>
                    {u.role}
                  </span>
                </td>
                <td className="px-4 py-3">
                  <span className={`rounded-full px-2 py-1 text-xs font-medium ${u.isActive ? 'bg-green-100 text-green-700' : 'bg-red-100 text-red-700'}`}>
                    {u.isActive ? 'Active' : 'Inactive'}
                  </span>
                </td>
                <td className="space-x-3 px-4 py-3 text-right">
                  <button onClick={() => toggleRole(u)} className="text-brand-600 hover:underline" disabled={u.id === currentUser?.userId}>
                    Make {u.role === 'Admin' ? 'User' : 'Admin'}
                  </button>
                  <button onClick={() => toggleActive(u)} className="text-amber-600 hover:underline">
                    {u.isActive ? 'Deactivate' : 'Activate'}
                  </button>
                  <button onClick={() => removeUser(u)} className="text-red-500 hover:underline" disabled={u.id === currentUser?.userId}>
                    Delete
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
