// frontend/lib/api.ts
import axios from 'axios';
import { CreateTaskRequest, Task } from '../types/task';

const API_URL = process.env.NEXT_PUBLIC_API_URL || 'https://localhost:5001/api';

// Create an axios instance with default config
const apiClient = axios.create({
  baseURL: API_URL + '/api',
  headers: {
    'Content-Type': 'application/json',
  },
});

// Add request interceptor to include JWT token in requests
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('auth_token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Add response interceptor to handle token expiration
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    // If we get a 401 Unauthorized, it might be due to expired token
    if (error.response && error.response.status === 401) {
      // Clear the token and redirect to login
      localStorage.removeItem('auth_token');
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

// Auth-related API functions
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  accessToken: string;
  expiresIn: number;
}

export async function login(credentials: LoginRequest): Promise<AuthResponse> {
  const response = await apiClient.post(
    API_URL + '/api/users/login',
    credentials
  );
  const authData = response.data;

  // Store the token for future requests
  localStorage.setItem('auth_token', authData.accessToken);

  return authData;
}

export async function register(
  userData: RegisterRequest
): Promise<AuthResponse> {
  const response = await apiClient.post(
    API_URL + '/api/users/register',
    userData
  );
  const authData = response.data;

  // Store the token for future requests
  localStorage.setItem('auth_token', authData.token);

  return authData;
}

export function logout(): void {
  localStorage.removeItem('auth_token');
  // Optionally redirect to login page
  window.location.href = '/login';
}

export function isAuthenticated(): boolean {
  return localStorage.getItem('auth_token') !== null;
}

// Task-related API functions
export async function getAllTasks(): Promise<Task[]> {
  try {
    const response = await apiClient.get(API_URL + '/api/tasks');
    return response.data;
  } catch (error) {
    console.error('Error fetching tasks:', error);
    throw new Error('Failed to fetch tasks');
  }
}

export async function createTask(task: CreateTaskRequest): Promise<void> {
  try {
    const response = await apiClient.post(API_URL + '/api/tasks', task);
    return response.data;
  } catch (error) {
    console.error('Error creating task:', error);
    throw new Error('Failed to create task');
  }
}

export async function completeTask(taskId: number): Promise<Task> {
  try {
    const response = await apiClient.put(`${API_URL}/api/tasks/${taskId}`);
    return response.data;
  } catch (error) {
    console.error('Error completing task:', error);
    throw new Error('Failed to complete task');
  }
}

export async function deleteTask(taskId: number): Promise<void> {
  try {
    await apiClient.delete(`${API_URL}/api/tasks/${taskId}`);
  } catch (error) {
    console.error('Error deleting task:', error);
    throw new Error('Failed to delete task');
  }
}
