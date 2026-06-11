import { useEffect, useState } from 'react';
import { Dialog } from 'primereact/dialog';
import { InputText } from 'primereact/inputtext';
import { InputTextarea } from 'primereact/inputtextarea';
import { Calendar } from 'primereact/calendar';
import { Dropdown } from 'primereact/dropdown';
import { Button } from 'primereact/button';
import { type TaskItem, TaskItemStatus, TaskItemStatusLabel } from '../types/task';
import { parseApiDate } from '../utils/date';

const STATUS_OPTIONS = [
  { label: TaskItemStatusLabel[TaskItemStatus.Pending], value: TaskItemStatus.Pending },
  { label: TaskItemStatusLabel[TaskItemStatus.Running], value: TaskItemStatus.Running },
  { label: TaskItemStatusLabel[TaskItemStatus.Completed], value: TaskItemStatus.Completed },
];

export interface TaskFormValues {
  title: string;
  description: string;
  endDate: Date | null;
  status: TaskItemStatus;
}

interface Props {
  visible: boolean;
  task: TaskItem | null;
  onHide: () => void;
  onSave: (values: TaskFormValues) => Promise<void>;
}

const defaultValues = (): TaskFormValues => ({
  title: '',
  description: '',
  endDate: null,
  status: TaskItemStatus.Pending,
});

const nowDateTime = () => new Date();

const isPastDateTime = (date: Date) => {
  const candidate = new Date(date);
  return candidate.getTime() < nowDateTime().getTime();
};

export function TaskFormDialog({ visible, task, onHide, onSave }: Props) {
  const [values, setValues] = useState<TaskFormValues>(defaultValues());
  const [saving, setSaving] = useState(false);
  const [errors, setErrors] = useState<Partial<Record<keyof TaskFormValues, string>>>({});

  useEffect(() => {
    if (task) {
      setValues({
        title: task.title,
        description: task.description ?? '',
        endDate: parseApiDate(task.endDate),
        status: task.status,
      });
    } else {
      setValues(defaultValues());
    }
    setErrors({});
  }, [task, visible]);

  const validate = (): boolean => {
    const e: typeof errors = {};
    if (!values.title.trim()) e.title = 'Título é obrigatório.';
    if (values.title.trim().length > 100) e.title = 'Título não pode ter mais de 100 caracteres.';
    if (!values.endDate) e.endDate = 'Prazo é obrigatório.';
    if (values.endDate && isPastDateTime(values.endDate)) {
      e.endDate = 'Prazo não pode ser anterior ao horário atual.';
    }
    setErrors(e);
    return Object.keys(e).length === 0;
  };

  const handleSave = async () => {
    if (!validate()) return;
    setSaving(true);
    try {
      await onSave(values);
    } finally {
      setSaving(false);
    }
  };

  const set = <K extends keyof TaskFormValues>(key: K, val: TaskFormValues[K]) =>
    setValues(prev => ({ ...prev, [key]: val }));

  const footer = (
    <div className="task-form-footer">
      <Button label="Cancelar" icon="pi pi-times" outlined onClick={onHide} disabled={saving} />
      <Button label="Salvar" icon="pi pi-check" onClick={handleSave} loading={saving} />
    </div>
  );

  return (
    <Dialog
      header={task ? 'Editar Tarefa' : 'Nova Tarefa'}
      visible={visible}
      onHide={onHide}
      footer={footer}
      style={{ width: 'min(560px, 92vw)' }}
      modal
      draggable={false}
    >
      <div className="task-form-grid">
        <div className="task-field">
          <label htmlFor="title" className="font-semibold text-sm">Título *</label>
          <InputText
            id="title"
            value={values.title}
            onChange={e => set('title', e.target.value)}
            placeholder="Nome da tarefa"
            maxLength={100}
            className={errors.title ? 'p-invalid w-full' : 'w-full'}
          />
          {errors.title && <small className="p-error">{errors.title}</small>}
        </div>

        <div className="task-field">
          <label htmlFor="description" className="font-semibold text-sm">Descrição</label>
          <InputTextarea
            id="description"
            value={values.description}
            onChange={e => set('description', e.target.value)}
            rows={3}
            placeholder="Descrição opcional"
            autoResize
            className="w-full"
          />
        </div>

        <div className="task-field">
          <label htmlFor="endDate" className="font-semibold text-sm">Prazo *</label>
          <Calendar
            id="endDate"
            value={values.endDate}
            onChange={e => set('endDate', e.value ?? null)}
            dateFormat="dd/mm/yy"
            placeholder="Selecione data e hora"
            showIcon
            showTime
            hourFormat="24"
            minDate={nowDateTime()}
            readOnlyInput
            className={errors.endDate ? 'p-invalid w-full' : 'w-full'}
          />
          {errors.endDate && <small className="p-error">{errors.endDate}</small>}
        </div>

        {task && (
          <div className="task-field">
            <label htmlFor="status" className="font-semibold text-sm">Status</label>
            <Dropdown
              id="status"
              value={values.status}
              options={STATUS_OPTIONS}
              onChange={e => set('status', e.value)}
              placeholder="Selecione o status"
              className="w-full"
            />
          </div>
        )}
      </div>
    </Dialog>
  );
}
