# **Task Tracker**  
A simple command-line application for managing tasks. It allows users to create, update, delete, and view tasks.  

## **Installation**  
1. Make sure you have [.NET SDK](https://dotnet.microsoft.com/download) installed.  
2. Clone the repository:  
   ```sh
   git clone https://github.com/RunasTheAweken/TaskHolder.git
   cd task-tracker
   ```
3. Build and run the application:  
   ```sh
   dotnet run -- help
   ```

## **Usage**  
This application works via the command line.  

### **Available Commands:**  
- **Create a task:**  
  ```sh
  dotnet run Create [Status] (Title) (Description)
  ```
  Example:  
  ```sh
  dotnet run Create OnGoing "Fix a bug" "Issue in logic"
  ```  
- **Update a task:**  
  ```sh
  dotnet run Update [ID] [Status] (Title) (Description)
  ```
- **Delete a task:**  
  ```sh
  dotnet run Delete [ID]
  ```
- **View all tasks:**  
  ```sh
  dotnet run View
  ```
- **View a task by ID:**  
  ```sh
  dotnet run View [ID]
  ```
- **Help:**  
  ```sh
  dotnet run Help
  ```

## **Task Storage Format**  
Tasks are stored in a `tasks.json` file in the current directory.  

Example JSON structure:  
```json
{
  "0": {
    "_TaskState": "OnGoing",
    "Title": "Example Task",
    "Description": "Task description"
  }
}
```

https://roadmap.sh/projects/task-tracker
