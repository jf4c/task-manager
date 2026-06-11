export const TaskItemStatus = {
  Pending: 0,
  Running: 1,
  Completed: 2,
} as const;

export type TaskItemStatus = (typeof TaskItemStatus)[keyof typeof TaskItemStatus];

export const TaskItemStatusLabel: Record<TaskItemStatus, string> = {
  [TaskItemStatus.Pending]: 'Pendente',
  [TaskItemStatus.Running]: 'Em andamento',
  [TaskItemStatus.Completed]: 'Concluída',
};

export const TaskItemStatusSeverity: Record<TaskItemStatus, string> = {
  [TaskItemStatus.Pending]: 'warning',
  [TaskItemStatus.Running]: 'info',
  [TaskItemStatus.Completed]: 'success',
};

export interface TaskItem {
  id: number;
  title: string;
  description?: string | null;
  startTime: string;
  endDate: string | null;
  status: TaskItemStatus;
}

export interface CreateTaskPayload {
  title: string;
  description?: string | null;
  endDate: string;
  status: TaskItemStatus;
}

export interface UpdateTaskPayload {
  title: string;
  description?: string | null;
  endDate: string;
  status: TaskItemStatus;
}
