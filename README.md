# Task Management Application with Event-Driven Architecture

This project demonstrates a full-stack task management application using ASP.NET Core for the backend and Next.js for the frontend, implementing event-driven architecture with real-time updates and AI-powered task prioritization.
![architecture_overview](https://github.com/muhfred/TaskManagement/blob/main/Untitled-2024-09-15-0839.excalidraw.png)
## Features

- **Event-Driven Architecture**: Using SignalR for real-time communication between backend and frontend
- **AI-Powered Priority Suggestions**: Automatically analyzes task descriptions to suggest priority levels
- **Real-Time Updates**: Tasks are updated in real-time across all connected clients
- **JWT Authentication**: Secure API endpoints and SignalR connections with JWT tokens
- **Axios for API Requests**: Leveraging Axios for more powerful HTTP requests with interceptors

## Architecture Overview

### Event Flow

1. **Task Creation**:
   - User creates a task on the frontend
   - Frontend sends a POST request to the backend API
   - Backend saves the task in the database
   - Backend triggers a `TaskCreatedEvent`
   - The event handler sends the task description to the AI service
   - AI service suggests a priority level
   - Backend updates the task with the suggested priority
   - Backend triggers a `TaskPriorityUpdatedEvent`
   - SignalR sends real-time updates to all connected clients

2. **Task Completion**:
   - User marks a task as completed on the frontend
   - Frontend sends a PUT request to the backend API
   - Backend updates the task status in the database
   - Backend triggers a `TaskCompletedEvent`
   - SignalR sends real-time updates to all connected clients

### AI Integration

The application integrates with OpenAI's API to analyze task descriptions and suggest appropriate priority levels:

1. When a new task is created, its description is sent to the OpenAI API
2. A prompt instructs the AI to analyze the text and determine if it's a Low, Medium, or High priority task
3. The AI's response is processed and used to update the task's priority
4. The updated priority is broadcast to all connected clients via SignalR

## Technical Stack

### Backend (.NET Core)
- ASP.NET Core Web API
- Entity Framework Core with PostgreSQL
- SignalR for real-time communication
- Event-driven architecture using publisher/subscriber pattern
- OpenAI integration for AI task analysis
- ASP.NET Core Identity for user management
- JWT authentication for secure API access

### Frontend (Next.js)
- Next.js 13 App Router with TypeScript
- Tailwind CSS for styling
- SignalR client for receiving real-time updates
- React hooks for state management
- Axios for API requests with interceptors
- JWT authentication with local storage
- Protected routes with middleware

## Getting Started

### Prerequisites
- .NET 7.0 SDK or later
- Node.js 16 or later
- PostgreSQL database
- GeminiAI API key

### Backend Setup

1. Navigate to the `backend` folder:
   ```bash
   cd backend
   ```

2. Update the connection string in `appsettings.json` with your PostgreSQL credentials:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=TaskManagementDb;Username=your-username;Password=your-password"
   }
   ```

3. Add your OpenAI API key to `appsettings.json`:
   ```json
   "AiService": {
     "ApiKey": "your-openai-api-key",
     "ApiUrl": "https://api.openai.com/v1/chat/completions"
   }
   ```

4. Run the migrations to create the database:
   ```bash
   dotnet ef migrations add "SampleMigration" --project src\Infrastructure --startup-project src\Web --output-dir Data\Migrations
   ```

5. Start the backend server:
   ```bash
   dotnet run
   ```

### Frontend Setup

1. Navigate to the `frontend` folder:
   ```bash
   cd frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Update the `.env.local` file with your backend API URL:
   ```
   NEXT_PUBLIC_API_URL=https://localhost:7181
   ```

4. Start the development server:
   ```bash
   npm run dev
   ```

5. Open your browser and navigate to `http://localhost:3000`

## Bonus Features Implementation

### JWT Authentication
To enhance security, JWT authentication has been implemented:
- The backend uses ASP.NET Core Identity for user management
- JWT tokens are issued upon successful login
- API endpoints are protected with the `[Authorize]` attribute
- The frontend stores the JWT token in local storage and includes it in API requests

### Retry Mechanism for AI Service
A retry mechanism has been implemented to handle potential failures in the AI service:
- Uses Polly for configurable retry policies
- Implements exponential backoff to avoid overwhelming the service
- Falls back to a default priority (Medium) after a certain number of failed attempts

## Future Enhancements

- **Advanced Task Categorization**: Add support for task categories and tags
- **Task Dependencies**: Allow tasks to depend on other tasks
- **Deadline Management**: Add support for task deadlines and reminders
- **User Collaboration**: Enable multiple users to collaborate on tasks
- **Offline Support**: Implement offline-first approach with synchronization
