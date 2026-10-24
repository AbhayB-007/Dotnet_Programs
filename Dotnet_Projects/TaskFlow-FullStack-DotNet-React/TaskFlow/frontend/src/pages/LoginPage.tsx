import { zodResolver } from '@hookform/resolvers/zod';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Link, useNavigate } from 'react-router-dom';
import toast from 'react-hot-toast';
import { z } from 'zod';
import { useAuth } from '../hooks/useAuth';

const schema = z.object({
  email: z.string().email('Enter a valid email'),
  password: z.string().min(1, 'Password is required'),
});
type FormValues = z.infer<typeof schema>;

export function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [serverError, setServerError] = useState<string | null>(null);

  const { register, handleSubmit, formState: { errors, isSubmitting } } = useForm<FormValues>({
    resolver: zodResolver(schema),
  });

  const onSubmit = async (values: FormValues) => {
    setServerError(null);
    try {
      await login(values.email, values.password);
      toast.success('Welcome back!');
      navigate('/tasks');
    } catch (err: any) {
      setServerError(err?.response?.data?.message ?? 'Login failed. Please try again.');
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-sm rounded-lg bg-white p-8 shadow-md">
        <h1 className="mb-1 text-2xl font-bold text-brand-600">TaskFlow</h1>
        <p className="mb-6 text-sm text-gray-500">Sign in to manage your tasks</p>

        {serverError && <p className="mb-4 rounded-md bg-red-50 p-2 text-sm text-red-600">{serverError}</p>}

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">Email</label>
            <input {...register('email')} className="w-full rounded-md border px-3 py-2 text-sm" placeholder="you@example.com" />
            {errors.email && <p className="mt-1 text-xs text-red-500">{errors.email.message}</p>}
          </div>
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">Password</label>
            <input type="password" {...register('password')} className="w-full rounded-md border px-3 py-2 text-sm" />
            {errors.password && <p className="mt-1 text-xs text-red-500">{errors.password.message}</p>}
          </div>
          <button
            type="submit"
            disabled={isSubmitting}
            className="w-full rounded-md bg-brand-500 py-2 text-sm font-medium text-white hover:bg-brand-600 disabled:opacity-50"
          >
            {isSubmitting ? 'Signing in...' : 'Sign in'}
          </button>
        </form>

        <p className="mt-4 text-center text-sm text-gray-500">
          No account? <Link to="/register" className="font-medium text-brand-600">Register</Link>
        </p>
        <p className="mt-2 text-center text-xs text-gray-400">
          Demo admin: admin@taskflow.local / Admin@12345
        </p>
      </div>
    </div>
  );
}
