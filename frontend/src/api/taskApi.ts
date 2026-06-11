import axios from 'axios';
import type { TaskItem, CreateTaskPayload, UpdateTaskPayload } from '../types/task';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL ?? 'http://localhost:5030',
  headers: { 'Content-Type': 'application/json' },
});

export const taskApi = {
  getAll: () => api.get<TaskItem[]>('/api/taskitem').then(r => r.data),

  getById: (id: number) => api.get<TaskItem>(`/api/taskitem/${id}`).then(r => r.data),

  create: (payload: CreateTaskPayload) => api.post('/api/taskitem', payload),

  update: (id: number, payload: UpdateTaskPayload) =>
    api.put(`/api/taskitem/${id}`, payload),

  remove: (id: number) => api.delete(`/api/taskitem/${id}`),
};
