import { axiosClient } from './axiosClient';
import type { ApiResponse, AppUser } from '../types';

export const userApi = {
  list: () => axiosClient.get<ApiResponse<AppUser[]>>('/users').then((r) => r.data.data),
  updateRole: (id: string, role: string) => axiosClient.put(`/users/${id}/role`, { role }),
  setActive: (id: string, isActive: boolean) => axiosClient.put(`/users/${id}/status`, { isActive }),
  remove: (id: string) => axiosClient.delete(`/users/${id}`),
};
