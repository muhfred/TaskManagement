import React from 'react';
import { Priority } from '../types/task';

interface PriorityBadgeProps {
  priority: Priority;
}

const PriorityBadge: React.FC<PriorityBadgeProps> = ({ priority }) => {
  let bgColor = 'bg-gray-200';
  let textColor = 'text-gray-800';

  switch (priority) {
    case 'None':
      bgColor = 'bg-gray-200';
      textColor = 'text-gray-800';
      break;
    case 'Low':
      bgColor = 'bg-green-100';
      textColor = 'text-green-800';
      break;
    case 'Medium':
      bgColor = 'bg-yellow-100';
      textColor = 'text-yellow-800';
      break;
    case 'High':
      bgColor = 'bg-red-100';
      textColor = 'text-red-800';
      break;
  }

  return (
    <span
      className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${bgColor} ${textColor}`}
    >
      {priority}
    </span>
  );
};

export default PriorityBadge;
