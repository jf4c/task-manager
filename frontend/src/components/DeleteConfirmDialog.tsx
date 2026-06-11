import { ConfirmDialog, confirmDialog } from 'primereact/confirmdialog';
import type { TaskItem } from '../types/task';

export function showDeleteConfirm(task: TaskItem, onConfirm: () => void) {
  confirmDialog({
    message: `Deseja excluir a tarefa "${task.title}"? Esta ação não pode ser desfeita.`,
    header: 'Confirmar exclusão',
    icon: 'pi pi-exclamation-triangle',
    acceptLabel: 'Excluir',
    rejectLabel: 'Cancelar',
    acceptClassName: 'p-button-danger',
    accept: onConfirm,
  });
}

export function DeleteConfirmDialog() {
  return <ConfirmDialog />;
}
