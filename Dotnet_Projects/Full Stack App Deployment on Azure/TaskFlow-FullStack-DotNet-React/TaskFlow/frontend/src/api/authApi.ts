import { axiosClient } from './axiosClient';
import type { ApiResponse, AuthResponse } from '../types';

export const authApi = {
  login: (email: string, password: string) =>
    axiosClient.post<ApiResponse<AuthResponse>>('/auth/login', { email, password }).then((r) => r.data.data),

  register: (fullName: string, email: string, password: string) =>
    axiosClient.post<ApiResponse<AuthResponse>>('/auth/register', { fullName, email, password }).then((r) => r.data.data),

  logout: (refreshToken: string) => axiosClient.post('/auth/logout', { refreshToken }),
};
