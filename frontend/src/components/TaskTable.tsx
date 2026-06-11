import { useRef } from 'react';
import { Button } from 'primereact/button';
import { DataTable } from 'primereact/datatable';
import { Column } from 'primereact/column';
import { Menu } from 'primereact/menu';
import type { MenuItem } from 'primereact/menuitem';
import {
  type TaskItem,
  TaskItemStatus,
  TaskItemStatusLabel,
  TaskItemStatusSeverity,
  type TaskItemStatus as TaskStatusValue,
} from '../types/task';
import { formatApiDate } from '../utils/date';

interface Props {
  tasks: TaskItem[];
  loading: boolean;
  onEdit: (task: TaskItem) => void;
  onDelete: (task: TaskItem) => void;
  onStatusChange: (task: TaskItem, status: TaskStatusValue) => void;
  onViewDetails: (task: TaskItem) => void;
}

export function TaskTable({
  tasks,
  loading,
  onEdit,
  onDelete,
  onStatusChange,
  onViewDetails,
}: Props) {
  const menuRefs = useRef<Record<number, Menu | null>>({});

  const statusBody = (row: TaskItem) => {
    const items: MenuItem[] = [
      TaskItemStatus.Pending,
      TaskItemStatus.Running,
      TaskItemStatus.Completed,
    ]
      .filter(status => status !== row.status)
      .map(status => ({
        label: TaskItemStatusLabel[status],
        command: () => onStatusChange(row, status),
      }));

    return (
      <div className="task-status-chip-wrap">
        <Menu
          popup
          model={items}
          ref={element => {
            menuRefs.current[row.id] = element;
          }}
        />
        <Button
          type="button"
          label={TaskItemStatusLabel[row.status]}
          icon="pi pi-chevron-down"
          iconPos="right"
          text
          className={`task-status-chip status-${TaskItemStatusSeverity[row.status]}`}
          onClick={event => {
            event.stopPropagation();
            menuRefs.current[row.id]?.toggle(event);
          }}
        />
      </div>
    );
  };

  const dateBody = (row: TaskItem) => formatApiDate(row.endDate);

  const startBody = (row: TaskItem) => formatApiDate(row.startTime);

  const titleBody = (row: TaskItem) => {
    const text = row.title?.trim() || '—';

    return <span className="task-title-cell" title={text}>{text}</span>;
  };

  const descriptionBody = (row: TaskItem) => {
    const text = row.description?.trim() || '—';

    return <span className="task-description-cell" title={text}>{text}</span>;
  };

  const actionsBody = (row: TaskItem) => (
    <div className="task-row-actions">
      <Button
        icon="pi pi-pencil"
        rounded
        text
        severity="info"
        tooltip="Editar"
        tooltipOptions={{ position: 'top' }}
        onClick={event => {
          event.stopPropagation();
          onEdit(row);
        }}
      />
      <Button
        icon="pi pi-trash"
        rounded
        text
        severity="danger"
        tooltip="Excluir"
        tooltipOptions={{ position: 'top' }}
        onClick={event => {
          event.stopPropagation();
          onDelete(row);
        }}
      />
    </div>
  );

  return (
    <DataTable
      value={tasks}
      loading={loading}
      stripedRows
      paginator
      rows={10}
      scrollable
      scrollHeight="flex"
      emptyMessage="Nenhuma tarefa encontrada."
      className="task-table"
      rowHover
      onRowClick={event => onViewDetails(event.data as TaskItem)}
    >
      <Column field="id" header="ID" style={{ width: '52px' }} />
      <Column header="Título" body={titleBody} style={{ width: '180px' }} />
      <Column header="Descrição" body={descriptionBody} />
      <Column header="Início" body={startBody} style={{ width: '100px' }} />
      <Column header="Prazo" body={dateBody} style={{ width: '100px' }} />
      <Column
        header="Status"
        body={statusBody}
        style={{ width: '170px' }}
      />
      <Column
        header="Ações"
        body={actionsBody}
        style={{ width: '96px' }}
        frozen
        alignFrozen="right"
      />
    </DataTable>
  );
}
