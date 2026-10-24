import { axiosClient } from './axiosClient';
import type { ApiResponse, PagedResult, TaskItem } from '../types';

export interface TaskFilters {
  page?: number;
  pageSize?: number;
  search?: string;
  status?: string;
  priority?: string;
}

export interface TaskPayload {
  title: string;
  description?: string;
  status?: string;
  priority: string;
  dueDateUtc?: string | null;
  assignedToUserId?: string | null;
}

export const taskApi = {
  list: (filters: TaskFilters) =>
    axiosClient.get<ApiResponse<PagedResult<TaskItem>>>('/tasks', { params: filters }).then((r) => r.data.data),

  getById: (id: string) => axiosClient.get<ApiResponse<TaskItem>>(`/tasks/${id}`).then((r) => r.data.data),

  create: (payload: TaskPayload) => axiosClient.post<ApiResponse<TaskItem>>('/tasks', payload).then((r) => r.data.data),

  update: (id: string, payload: TaskPayload) =>
    axiosClient.put<ApiResponse<TaskItem>>(`/tasks/${id}`, payload).then((r) => r.data.data),

  remove: (id: string) => axiosClient.delete(`/tasks/${id}`),
};
