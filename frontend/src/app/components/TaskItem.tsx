import React from 'react';
import { Task } from '../types/task';
import PriorityBadge from './PriorityBadge';

interface TaskItemProps {
  task: Task;
  onComplete: (taskId: number, task: Task) => Promise<void>;
  onDelete: (taskId: number) => Promise<void>;
}

const TaskItem: React.FC<TaskItemProps> = ({ task, onComplete, onDelete }) => {
  return (
    <div
      className={`p-4 mb-4 rounded-lg shadow ${
        task.isCompleted ? 'bg-gray-50' : 'bg-white'
      }`}
    >
      <div key={task.id} className="flex justify-between items-start">
        <div className="flex-1">
          <h3
            className={`text-lg font-medium ${
              task.isCompleted ? 'text-gray-500 line-through' : 'text-gray-900'
            }`}
          >
            {task.title}
          </h3>
          {task.description && (
            <p
              className={`mt-1 text-sm ${
                task.isCompleted ? 'text-gray-400' : 'text-gray-600'
              }`}
            >
              {task.description}
            </p>
          )}
          <div className="mt-2 flex items-center space-x-2">
            {task.priority !== undefined && (
              <PriorityBadge priority={task.priority} />
            )}
          </div>
        </div>
        <div className="flex space-x-2">
          {!task.isCompleted && (
            <button
              onClick={() => task.id !== undefined && onComplete(task.id, task)}
              className="inline-flex items-center px-2.5 py-1.5 border border-transparent text-xs font-medium rounded text-white bg-green-600 hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-green-500"
            >
              Complete
            </button>
          )}
          <button
            onClick={() => task.id !== undefined && onDelete(task.id)}
            className="inline-flex items-center px-2.5 py-1.5 border border-transparent text-xs font-medium rounded text-white bg-red-600 hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-red-500"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
};

export default TaskItem;
