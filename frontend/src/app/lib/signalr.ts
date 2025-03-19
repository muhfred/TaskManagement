import * as signalR from '@microsoft/signalr';
import { Task } from '../types/task';

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private taskCreatedCallbacks: ((task: Task) => void)[] = [];
  private taskCompletedCallbacks: ((task: Task) => void)[] = [];
  private taskPriorityUpdatedCallbacks: ((task: Task) => void)[] = [];

  public async startConnection(): Promise<void> {
    if (this.connection) {
      return;
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(
        `${process.env.NEXT_PUBLIC_API_URL || 'https://localhost:5001'}/taskhub`
      )
      .withAutomaticReconnect()
      .build();

    try {
      await this.connection.start();
      console.log('SignalR connected');
      this.registerHandlers();
    } catch (error) {
      console.error('Error starting SignalR connection:', error);
      throw error;
    }
  }

  public async stopConnection(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
      console.log('SignalR disconnected');
    }
  }

  private registerHandlers(): void {
    if (!this.connection) {
      return;
    }

    this.connection.on('TaskCreated', (task: Task) => {
      console.log('Task created:', task);
      this.taskCreatedCallbacks.forEach((callback) => callback(task));
    });

    this.connection.on('TaskCompleted', (task: Task) => {
      console.log('Task completed:', task);
      this.taskCompletedCallbacks.forEach((callback) => callback(task));
    });

    this.connection.on('TaskPriorityUpdated', (task: Task) => {
      console.log('Task priority updated:', task);
      this.taskPriorityUpdatedCallbacks.forEach((callback) => callback(task));
    });
  }

  public onTaskCreated(callback: (task: Task) => void): void {
    this.taskCreatedCallbacks.push(callback);
  }

  public onTaskCompleted(callback: (task: Task) => void): void {
    this.taskCompletedCallbacks.push(callback);
  }

  public onTaskPriorityUpdated(callback: (task: Task) => void): void {
    this.taskPriorityUpdatedCallbacks.push(callback);
  }
}

// Export a singleton instance
const signalRService = new SignalRService();
export default signalRService;
