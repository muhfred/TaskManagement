'use client';

import React, { useEffect, useState } from 'react';
import { useRouter } from 'next/navigation';
import TaskForm from './components/TaskForm';
import TaskList from './components/TaskList';
import { CreateTaskRequest, Task } from './types/task';
import {
  getAllTasks,
  createTask,
  completeTask,
  deleteTask,
  logout,
  isAuthenticated,
} from './lib/api';
import signalRService from './lib/signalr';

export default function Home() {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    // Start SignalR connection and register event handlers
    const setupSignalR = async () => {
      try {
        await signalRService.startConnection();

        signalRService.onTaskCreated((task) => {
          console.log('Task created event received:', task);
          setTasks((prevTasks) => {
            // Check if the task already exists to avoid duplicates
            if (!prevTasks.some((t) => t.id === task.id)) {
              return [task, ...prevTasks];
            }
            return prevTasks;
          });
        });

        signalRService.onTaskCompleted((task) => {
          console.log('Task completed event received:', task);
          setTasks((prevTasks) =>
            prevTasks.map((t) => (t.id === task.id ? task : t))
          );
        });

        signalRService.onTaskPriorityUpdated((task) => {
          console.log('Task priority updated event received:', task);
          setTasks((prevTasks) =>
            prevTasks.map((t) => (t.id === task.id ? task : t))
          );
        });
      } catch (error) {
        console.error('Failed to connect to SignalR:', error);
        setError(
          'Failed to establish real-time connection. Some features may be limited.'
        );
      }
    };

    // Fetch initial tasks data
    const fetchTasks = async () => {
      try {
        const data = await getAllTasks();
        setTasks(data);
      } catch (error) {
        console.error('Failed to fetch tasks:', error);
        setError('Failed to load tasks. Please try again later.');
      } finally {
        setIsLoading(false);
      }
    };

    setupSignalR();
    fetchTasks();
  }, []);

  const handleCreateTask = async (newTask: CreateTaskRequest) => {
    try {
      await createTask(newTask);
      // No need to update state here as the SignalR event will handle it
    } catch (error) {
      console.error('Error creating task:', error);
      setError('Failed to create task. Please try again.');
    }
  };

  const handleCompleteTask = async (taskId: number) => {
    try {
      await completeTask(taskId);
      // No need to update state here as the SignalR event will handle it
    } catch (error) {
      console.error('Error completing task:', error);
      setError('Failed to complete task. Please try again.');
    }
  };

  const handleDeleteTask = async (taskId: number) => {
    try {
      await deleteTask(taskId);
      // SignalR doesn't handle deletions, so we update state manually
      setTasks((prevTasks) => prevTasks.filter((task) => task.id !== taskId));
    } catch (error) {
      console.error('Error deleting task:', error);
      setError('Failed to delete task. Please try again.');
    }
  };

  const router = useRouter();

  useEffect(() => {
    // Check if the user is authenticated
    if (!isAuthenticated()) {
      router.push('/login');
    }
  }, [router]);

  const handleLogout = () => {
    logout();
  };

  return (
    <div className="px-4 py-6">
      <div className="flex justify-end mb-6">
        <button
          onClick={handleLogout}
          className="px-4 py-2 bg-gray-200 text-gray-800 rounded-md hover:bg-gray-300 focus:outline-none focus:ring-2 focus:ring-gray-500"
        >
          Logout
        </button>
      </div>

      {error && (
        <div className="mb-6 p-4 bg-red-100 text-red-700 rounded-md">
          <p>{error}</p>
          <button
            onClick={() => setError(null)}
            className="mt-2 text-sm underline"
          >
            Dismiss
          </button>
        </div>
      )}

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="md:col-span-1">
          <TaskForm onSubmit={handleCreateTask} />
        </div>

        <div className="md:col-span-2">
          {isLoading ? (
            <div className="flex justify-center items-center h-64">
              <p className="text-gray-500">Loading tasks...</p>
            </div>
          ) : (
            <TaskList
              tasks={tasks}
              onCompleteTask={handleCompleteTask}
              onDeleteTask={handleDeleteTask}
            />
          )}
        </div>
      </div>
    </div>
  );
}
