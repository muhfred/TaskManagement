import React from 'react';
import { Task } from '../types/task';
import TaskItem from './TaskItem';

interface TaskListProps {
  tasks: Task[];
  onCompleteTask: (taskId: number) => Promise<void>;
  onDeleteTask: (taskId: number) => Promise<void>;
}

const TaskList: React.FC<TaskListProps> = ({
  tasks,
  onCompleteTask,
  onDeleteTask,
}) => {
  const activeTasks = Object.values(tasks).filter((task) => !task.isCompleted);
  const completedTasks = Object.values(tasks).filter(
    (task) => task.isCompleted
  );

  return (
    <div className="space-y-6">
      <div>
        <h2 className="text-xl font-semibold text-gray-800 mb-4">
          Active Tasks ({activeTasks.length})
        </h2>
        {activeTasks.length === 0 ? (
          <p className="text-gray-500 italic">No active tasks</p>
        ) : (
          activeTasks.map((task, index) => (
            <TaskItem
              key={task.id || index}
              task={task}
              onComplete={onCompleteTask}
              onDelete={onDeleteTask}
            />
          ))
        )}
      </div>

      {completedTasks.length > 0 && (
        <div>
          <h2 className="text-xl font-semibold text-gray-800 mb-4">
            Completed Tasks ({completedTasks.length})
          </h2>
          {completedTasks.map((task, index) => (
            <TaskItem
              key={task.id || index}
              task={task}
              onComplete={onCompleteTask}
              onDelete={onDeleteTask}
            />
          ))}
        </div>
      )}
    </div>
  );
};

export default TaskList;
