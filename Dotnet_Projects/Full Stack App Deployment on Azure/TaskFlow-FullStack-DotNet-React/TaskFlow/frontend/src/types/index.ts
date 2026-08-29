export type Role = 'Admin' | 'User';

export interface AuthUser {
  userId: string;
  fullName: string;
  email: string;
  role: Role;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  accessTokenExpiresAtUtc: string;
  userId: string;
  fullName: string;
  email: string;
  role: Role;
}

export type WorkStatus = 'ToDo' | 'InProgress' | 'InReview' | 'Done';
export type Priority = 'Low' | 'Medium' | 'High' | 'Critical';

export interface TaskItem {
  id: string;
  title: string;
  description?: string;
  status: WorkStatus;
  priority: Priority;
  dueDateUtc?: string;
  ownerId: string;
  ownerName?: string;
  assignedToUserId?: string;
  assignedToName?: string;
  createdAtUtc: string;
  updatedAtUtc?: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data: T;
}

export interface AppUser {
  id: string;
  fullName: string;
  email: string;
  role: Role;
  isActive: boolean;
  createdAtUtc: string;
}
