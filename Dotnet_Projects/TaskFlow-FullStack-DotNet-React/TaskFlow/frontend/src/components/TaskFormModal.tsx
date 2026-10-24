import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import type { TaskItem } from '../types';

// Schema-driven validation with Zod + react-hook-form — the modern, type-safe
// replacement for hand-rolled onChange validation.
const schema = z.object({
  title: z.string().min(1, 'Title is required').max(200),
  description: z.string().max(2000).optional(),
  status: z.enum(['ToDo', 'InProgress', 'InReview', 'Done']),
  priority: z.enum(['Low', 'Medium', 'High', 'Critical']),
  dueDateUtc: z.string().optional(),
});

export type TaskFormValues = z.infer<typeof schema>;

interface Props {
  initial?: TaskItem | null;
  onSubmit: (values: TaskFormValues) => Promise<void>;
  onClose: () => void;
}

export function TaskFormModal({ initial, onSubmit, onClose }: Props) {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<TaskFormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      title: initial?.title ?? '',
      description: initial?.description ?? '',
      status: initial?.status ?? 'ToDo',
      priority: initial?.priority ?? 'Medium',
      dueDateUtc: initial?.dueDateUtc ? initial.dueDateUtc.substring(0, 10) : '',
    },
  });

  const submit = async (values: TaskFormValues) => {
    await onSubmit(values);
    onClose();
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 p-4">
      <div className="w-full max-w-md rounded-lg bg-white p-6 shadow-xl">
        <h2 className="mb-4 text-lg font-semibold">{initial ? 'Edit Task' : 'New Task'}</h2>
        <form onSubmit={handleSubmit(submit)} className="space-y-4">
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">Title</label>
            <input {...register('title')} className="w-full rounded-md border px-3 py-2 text-sm" />
            {errors.title && <p className="mt-1 text-xs text-red-500">{errors.title.message}</p>}
          </div>

          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">Description</label>
            <textarea {...register('description')} rows={3} className="w-full rounded-md border px-3 py-2 text-sm" />
          </div>

          <div className="grid grid-cols-2 gap-3">
            <div>
              <label className="mb-1 block text-sm font-medium text-gray-700">Status</label>
              <select {...register('status')} className="w-full rounded-md border px-3 py-2 text-sm">
                <option value="ToDo">To Do</option>
                <option value="InProgress">In Progress</option>
                <option value="InReview">In Review</option>
                <option value="Done">Done</option>
              </select>
            </div>
            <div>
              <label className="mb-1 block text-sm font-medium text-gray-700">Priority</label>
              <select {...register('priority')} className="w-full rounded-md border px-3 py-2 text-sm">
                <option value="Low">Low</option>
                <option value="Medium">Medium</option>
                <option value="High">High</option>
                <option value="Critical">Critical</option>
              </select>
            </div>
          </div>

          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">Due date</label>
            <input type="date" {...register('dueDateUtc')} className="w-full rounded-md border px-3 py-2 text-sm" />
          </div>

          <div className="flex justify-end gap-2 pt-2">
            <button type="button" onClick={onClose} className="rounded-md border px-4 py-2 text-sm">
              Cancel
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="rounded-md bg-brand-500 px-4 py-2 text-sm text-white hover:bg-brand-600 disabled:opacity-50"
            >
              {isSubmitting ? 'Saving...' : 'Save'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
