export type Priority = 'Low' | 'Medium' | 'High' | 'None';

export interface Task {
  id?: number;
  title?: string;
  description?: string;
  priority?: Priority;
  isCompleted?: boolean;
}

export interface CreateTaskRequest {
  id: number;
  title: string;
  description: string;
}
