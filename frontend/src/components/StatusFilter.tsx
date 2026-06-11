import { SelectButton } from 'primereact/selectbutton';
import { TaskItemStatus, TaskItemStatusLabel } from '../types/task';

const OPTIONS = [
  { label: 'Todas', value: null },
  { label: TaskItemStatusLabel[TaskItemStatus.Pending], value: TaskItemStatus.Pending },
  { label: TaskItemStatusLabel[TaskItemStatus.Running], value: TaskItemStatus.Running },
  { label: TaskItemStatusLabel[TaskItemStatus.Completed], value: TaskItemStatus.Completed },
];

interface Props {
  value: TaskItemStatus | null;
  onChange: (status: TaskItemStatus | null) => void;
}

export function StatusFilter({ value, onChange }: Props) {
  return (
    <SelectButton
      value={value}
      options={OPTIONS}
      onChange={e => onChange(e.value)}
      className="status-filter"
    />
  );
}
