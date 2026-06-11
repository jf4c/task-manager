import { useState, useEffect, useCallback, useRef } from 'react';
import { Button } from 'primereact/button';
import { Toast } from 'primereact/toast';
import { InputText } from 'primereact/inputtext';
import { Dialog } from 'primereact/dialog';
import { TaskTable } from './components/TaskTable';
import { TaskFormDialog, type TaskFormValues } from './components/TaskFormDialog';
import { DeleteConfirmDialog, showDeleteConfirm } from './components/DeleteConfirmDialog';
import { StatusFilter } from './components/StatusFilter';
import { taskApi } from './api/taskApi';
import { type TaskItem, TaskItemStatus, TaskItemStatusLabel } from './types/task';
import { parseApiDate, formatApiDateTime } from './utils/date';
import './App.css';

export default function App() {
  const toast = useRef<Toast>(null);

  const [tasks, setTasks] = useState<TaskItem[]>([]);
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');
  const [statusFilter, setStatusFilter] = useState<TaskItemStatus | null>(null);

  const [dialogVisible, setDialogVisible] = useState(false);
  const [editingTask, setEditingTask] = useState<TaskItem | null>(null);
  const [detailsVisible, setDetailsVisible] = useState(false);
  const [selectedTask, setSelectedTask] = useState<TaskItem | null>(null);

  const notify = (severity: 'success' | 'error', summary: string, detail: string) =>
    toast.current?.show({ severity, summary, detail, life: 3500 });

  const loadTasks = useCallback(async () => {
    setLoading(true);
    try {
      const data = await taskApi.getAll();
      setTasks(data);
    } catch {
      notify('error', 'Erro', 'Nao foi possivel carregar as tarefas.');
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => { loadTasks(); }, [loadTasks]);

  const filteredTasks = tasks.filter(t => {
    const matchesStatus = statusFilter === null || t.status === statusFilter;
    const q = search.toLowerCase();
    const matchesSearch =
      !q ||
      t.title.toLowerCase().includes(q) ||
      t.description?.toLowerCase().includes(q);
    return matchesStatus && matchesSearch;
  }).sort((a, b) => {
    const dateA = parseApiDate(a.startTime)?.getTime() ?? 0;
    const dateB = parseApiDate(b.startTime)?.getTime() ?? 0;
    return dateB - dateA;
  });

  const openCreate = () => {
    setEditingTask(null);
    setDialogVisible(true);
  };

  const openEdit = (task: TaskItem) => {
    setEditingTask(task);
    setDialogVisible(true);
  };

  const openDetails = (task: TaskItem) => {
    setSelectedTask(task);
    setDetailsVisible(true);
  };

  const toggleStatusFilter = (nextStatus: TaskItemStatus | null) => {
    setStatusFilter(current => (current === nextStatus ? null : nextStatus));
  };

  const handleQuickStatusChange = async (task: TaskItem, status: TaskItemStatus) => {
    if (task.status === status) {
      return;
    }

    if (!task.endDate) {
      notify('error', 'Erro', 'Nao foi possivel alterar o status sem prazo definido.');
      return;
    }

    try {
      await taskApi.update(task.id, {
        title: task.title,
        description: task.description,
        endDate: task.endDate,
        status,
      });

      setTasks(current => current.map(item => (
        item.id === task.id ? { ...item, status } : item
      )));

      notify('success', 'Status alterado', 'O status da tarefa foi atualizado.');
    } catch {
      notify('error', 'Erro', 'Nao foi possivel alterar o status da tarefa.');
    }
  };

  const handleSave = async (values: TaskFormValues) => {
    const payload = {
      title: values.title,
      description: values.description,
      endDate: values.endDate!.toISOString(),
      status: editingTask ? values.status : TaskItemStatus.Pending,
    };

    try {
      if (editingTask) {
        await taskApi.update(editingTask.id, payload);
        notify('success', 'Atualizada', 'Tarefa atualizada com sucesso.');
      } else {
        await taskApi.create(payload);
        notify('success', 'Criada', 'Tarefa criada com sucesso.');
      }
      setDialogVisible(false);
      loadTasks();
    } catch {
      notify('error', 'Erro', 'Nao foi possivel salvar a tarefa.');
      throw new Error('save failed');
    }
  };

  const handleDelete = (task: TaskItem) => {
    showDeleteConfirm(task, async () => {
      try {
        await taskApi.remove(task.id);
        notify('success', 'Excluida', 'Tarefa removida com sucesso.');
        loadTasks();
      } catch {
        notify('error', 'Erro', 'Nao foi possivel excluir a tarefa.');
      }
    });
  };

  const pendingCount = tasks.filter(t => t.status === TaskItemStatus.Pending).length;
  const runningCount = tasks.filter(t => t.status === TaskItemStatus.Running).length;
  const completedCount = tasks.filter(t => t.status === TaskItemStatus.Completed).length;

  return (
    <div className="app-wrapper">
      <Toast ref={toast} />
      <DeleteConfirmDialog />

      <header className="app-header">
        <div className="header-brand">
          <i className="pi pi-check-square" style={{ fontSize: '1.6rem' }} />
          <span>Task Manager</span>
        </div>
        <Button
          label="Nova tarefa"
          icon="pi pi-plus"
          onClick={openCreate}
          className="p-button-rounded"
        />
      </header>

      <section className="stats-row">
        <button
          type="button"
          className={`stat-card stat-total ${statusFilter === null ? 'is-active' : ''}`}
          onClick={() => toggleStatusFilter(null)}
        >
          <span className="stat-value">{tasks.length}</span>
          <span className="stat-label">Total</span>
        </button>
        <button
          type="button"
          className={`stat-card stat-pending ${statusFilter === TaskItemStatus.Pending ? 'is-active' : ''}`}
          onClick={() => toggleStatusFilter(TaskItemStatus.Pending)}
        >
          <span className="stat-value">{pendingCount}</span>
          <span className="stat-label">Pendentes</span>
        </button>
        <button
          type="button"
          className={`stat-card stat-running ${statusFilter === TaskItemStatus.Running ? 'is-active' : ''}`}
          onClick={() => toggleStatusFilter(TaskItemStatus.Running)}
        >
          <span className="stat-value">{runningCount}</span>
          <span className="stat-label">Em andamento</span>
        </button>
        <button
          type="button"
          className={`stat-card stat-completed ${statusFilter === TaskItemStatus.Completed ? 'is-active' : ''}`}
          onClick={() => toggleStatusFilter(TaskItemStatus.Completed)}
        >
          <span className="stat-value">{completedCount}</span>
          <span className="stat-label">Concluidas</span>
        </button>
      </section>

      <section className="filters-row">
        <span className="search-wrap">
          <i className="pi pi-search" />
          <InputText
            value={search}
            onChange={e => setSearch(e.target.value)}
            placeholder="Buscar tarefa..."
            className="search-input"
          />
        </span>
        <StatusFilter value={statusFilter} onChange={setStatusFilter} />
      </section>

      <section className="table-section">
        <div className="table-shell">
          <TaskTable
            tasks={filteredTasks}
            loading={loading}
            onEdit={openEdit}
            onDelete={handleDelete}
            onStatusChange={handleQuickStatusChange}
            onViewDetails={openDetails}
          />
        </div>
      </section>

      <Dialog
        header="Detalhes da tarefa"
        visible={detailsVisible}
        onHide={() => setDetailsVisible(false)}
        style={{ width: 'min(560px, 92vw)' }}
        modal
        dismissableMask
        draggable={false}
      >
        {selectedTask && (
          <div className="task-details-grid">
            <div className="task-details-row">
              <span className="task-details-label">ID</span>
              <span className="task-details-value">#{selectedTask.id}</span>
            </div>
            <div className="task-details-row">
              <span className="task-details-label">Título</span>
              <span className="task-details-value">{selectedTask.title}</span>
            </div>
            <div className="task-details-row">
              <span className="task-details-label">Descrição</span>
              <span className="task-details-value">
                {selectedTask.description || 'Sem descrição'}
              </span>
            </div>
            <div className="task-details-row">
              <span className="task-details-label">Status</span>
              <span className="task-details-value">
                {TaskItemStatusLabel[selectedTask.status]}
              </span>
            </div>
            <div className="task-details-row">
              <span className="task-details-label">Início</span>
              <span className="task-details-value">
                {formatApiDateTime(selectedTask.startTime)}
              </span>
            </div>
            <div className="task-details-row">
              <span className="task-details-label">Prazo</span>
              <span className="task-details-value">
                {selectedTask.endDate ? formatApiDateTime(selectedTask.endDate) : 'Sem prazo'}
              </span>
            </div>
          </div>
        )}
      </Dialog>

      <TaskFormDialog
        visible={dialogVisible}
        task={editingTask}
        onHide={() => setDialogVisible(false)}
        onSave={handleSave}
      />
    </div>
  );
}
