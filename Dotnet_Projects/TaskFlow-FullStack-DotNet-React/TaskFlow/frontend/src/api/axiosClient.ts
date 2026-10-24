import axios, { AxiosError, InternalAxiosRequestConfig } from 'axios';
import type { AuthResponse } from '../types';

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const axiosClient = axios.create({
  baseURL: BASE_URL,
  headers: { 'Content-Type': 'application/json' },
});

// ---- Request interceptor: attach the access token to every outgoing call ----
axiosClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('accessToken');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// ---- Response interceptor: on a 401, transparently refresh the token once
// and replay the original request. This is the pattern every senior dev should
// know for JWT-based SPAs: silent refresh instead of forcing a re-login. ----
let isRefreshing = false;
let pendingQueue: Array<(token: string) => void> = [];

function onRefreshed(token: string) {
  pendingQueue.forEach((cb) => cb(token));
  pendingQueue = [];
}

axiosClient.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

    if (error.response?.status === 401 && !originalRequest._retry) {
      const refreshToken = localStorage.getItem('refreshToken');
      if (!refreshToken) {
        clearSession();
        return Promise.reject(error);
      }

      originalRequest._retry = true;

      if (isRefreshing) {
        // Queue this request until the in-flight refresh call resolves.
        return new Promise((resolve) => {
          pendingQueue.push((token: string) => {
            originalRequest.headers!.Authorization = `Bearer ${token}`;
            resolve(axiosClient(originalRequest));
          });
        });
      }

      isRefreshing = true;
      try {
        const { data } = await axios.post<{ data: AuthResponse }>(`${BASE_URL}/auth/refresh`, { refreshToken });
        const auth = data.data;
        localStorage.setItem('accessToken', auth.accessToken);
        localStorage.setItem('refreshToken', auth.refreshToken);
        isRefreshing = false;
        onRefreshed(auth.accessToken);

        originalRequest.headers!.Authorization = `Bearer ${auth.accessToken}`;
        return axiosClient(originalRequest);
      } catch (refreshError) {
        isRefreshing = false;
        clearSession();
        return Promise.reject(refreshError);
      }
    }

    return Promise.reject(error);
  }
);

function clearSession() {
  localStorage.removeItem('accessToken');
  localStorage.removeItem('refreshToken');
  localStorage.removeItem('authUser');
  window.location.href = '/login';
}
