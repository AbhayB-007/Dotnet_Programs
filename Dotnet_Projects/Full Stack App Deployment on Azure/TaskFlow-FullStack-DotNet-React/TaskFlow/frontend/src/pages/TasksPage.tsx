import { useCallback, useEffect, useState } from 'react';
import toast from 'react-hot-toast';
import { taskApi } from '../api/taskApi';
import { Loader } from '../components/Loader';
import { Pagination } from '../components/Pagination';
import { TaskFormModal, TaskFormValues } from '../components/TaskFormModal';
import { useDebounce } from '../hooks/useDebounce';
import type { TaskItem } from '../types';

const statusColors: Record<string, string> = {
  ToDo: 'bg-gray-100 text-gray-700',
  InProgress: 'bg-blue-100 text-blue-700',
  InReview: 'bg-amber-100 text-amber-700',
  Done: 'bg-green-100 text-green-700',
};

const priorityColors: Record<string, string> = {
  Low: 'bg-gray-100 text-gray-600',
  Medium: 'bg-blue-50 text-blue-600',
  High: 'bg-orange-100 text-orange-700',
  Critical: 'bg-red-100 text-red-700',
};

export function TasksPage() {
  const [tasks, setTasks] = useState<TaskItem[]>([]);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [search, setSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState('');
  const [modalOpen, setModalOpen] = useState(false);
  const [editingTask, setEditingTask] = useState<TaskItem | null>(null);

  const debouncedSearch = useDebounce(search);

  const loadTasks = useCallback(async () => {
    setLoading(true);
    try {
      const result = await taskApi.list({
        page,
        pageSize: 8,
        search: debouncedSearch || undefined,
        status: statusFilter || undefined,
      });
      setTasks(result.items);
      setTotalPages(result.totalPages || 1);
    } catch {
      toast.error('Failed to load tasks.');
    } finally {
      setLoading(false);
    }
  }, [page, debouncedSearch, statusFilter]);

  // Effect + dependency array: the bread-and-butter pattern for syncing
  // component state with an external data source (the API) in React.
  useEffect(() => {
    loadTasks();
  }, [loadTasks]);

  const handleCreateOrUpdate = async (values: TaskFormValues) => {
    try {
      if (editingTask) {
        await taskApi.update(editingTask.id, { ...values, dueDateUtc: values.dueDateUtc || null });
        toast.success('Task updated.');
      } else {
        await taskApi.create({ ...values, dueDateUtc: values.dueDateUtc || null });
        toast.success('Task created.');
      }
      await loadTasks();
    } catch {
      toast.error('Something went wrong saving the task.');
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Delete this task?')) return;
    try {
      await taskApi.remove(id);
      toast.success('Task deleted.');
      await loadTasks();
    } catch {
      toast.error('Failed to delete task.');
    }
  };

  return (
    <div className="mx-auto max-w-6xl px-6 py-8">
      <div className="mb-6 flex flex-wrap items-center justify-between gap-3">
        <h1 className="text-2xl font-bold text-gray-800">My Tasks</h1>
        <button
          onClick={() => { setEditingTask(null); setModalOpen(true); }}
          className="rounded-md bg-brand-500 px-4 py-2 text-sm font-medium text-white hover:bg-brand-600"
        >
          + New Task
        </button>
      </div>

      <div className="mb-4 flex flex-wrap gap-3">
        <input
          value={search}
          onChange={(e) => { setSearch(e.target.value); setPage(1); }}
          placeholder="Search tasks..."
          className="w-64 rounded-md border px-3 py-2 text-sm"
        />
        <select
          value={statusFilter}
          onChange={(e) => { setStatusFilter(e.target.value); setPage(1); }}
          className="rounded-md border px-3 py-2 text-sm"
        >
          <option value="">All statuses</option>
          <option value="ToDo">To Do</option>
          <option value="InProgress">In Progress</option>
          <option value="InReview">In Review</option>
          <option value="Done">Done</option>
        </select>
      </div>

      {loading ? (
        <Loader />
      ) : tasks.length === 0 ? (
        <p className="py-12 text-center text-gray-400">No tasks found. Create your first one!</p>
      ) : (
        <div className="overflow-hidden rounded-lg border bg-white">
          <table className="w-full text-left text-sm">
            <thead className="bg-gray-50 text-xs uppercase text-gray-500">
              <tr>
                <th className="px-4 py-3">Title</th>
                <th className="px-4 py-3">Status</th>
                <th className="px-4 py-3">Priority</th>
                <th className="px-4 py-3">Due</th>
                <th className="px-4 py-3">Owner</th>
                <th className="px-4 py-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody className="divide-y">
              {tasks.map((task) => (
                <tr key={task.id} className="hover:bg-gray-50">
                  <td className="px-4 py-3 font-medium text-gray-800">{task.title}</td>
                  <td className="px-4 py-3">
                    <span className={`rounded-full px-2 py-1 text-xs font-medium ${statusColors[task.status]}`}>
                      {task.status}
                    </span>
                  </td>
                  <td className="px-4 py-3">
                    <span className={`rounded-full px-2 py-1 text-xs font-medium ${priorityColors[task.priority]}`}>
                      {task.priority}
                    </span>
                  </td>
                  <td className="px-4 py-3 text-gray-500">
                    {task.dueDateUtc ? new Date(task.dueDateUtc).toLocaleDateString() : '—'}
                  </td>
                  <td className="px-4 py-3 text-gray-500">{task.ownerName ?? '—'}</td>
                  <td className="px-4 py-3 text-right">
                    <button
                      onClick={() => { setEditingTask(task); setModalOpen(true); }}
                      className="mr-3 text-brand-600 hover:underline"
                    >
                      Edit
                    </button>
                    <button onClick={() => handleDelete(task.id)} className="text-red-500 hover:underline">
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <Pagination page={page} totalPages={totalPages} onChange={setPage} />

      {modalOpen && (
        <TaskFormModal
          initial={editingTask}
          onSubmit={handleCreateOrUpdate}
          onClose={() => setModalOpen(false)}
        />
      )}
    </div>
  );
}
