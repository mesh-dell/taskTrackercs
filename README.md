# Task CLI

A simple command-line task manager written in C#.

## Overview

Task CLI allows you to manage your tasks directly from the command line. You can add, update, delete, and track the status of your tasks with simple commands.

## Features

- Add new tasks
- Update existing tasks
- Delete tasks
- Mark tasks as "in progress" or "done"
- List all tasks or filter by status (todo, in progress, done)

## Installation

1. Ensure you have .NET installed on your system
2. Clone this repository
3. Build the project:
   ```
   dotnet build
   ```

## Usage

```
task-cli <command> [arguments]
```

### Commands

| Command | Description |
|---------|-------------|
| `add <task_description>` | Add a new task |
| `update <task_id> <new_description>` | Update an existing task |
| `delete <task_id>` | Delete a task |
| `mark-in-progress <task_id>` | Mark a task as in progress |
| `mark-done <task_id>` | Mark a task as done |
| `list` | List all tasks |
| `list done` | List completed tasks |
| `list todo` | List pending tasks |
| `list in-progress` | List tasks in progress |
| `help` | Display help information |

### Examples

```
task-cli add "Buy groceries"
task-cli update 1 "Buy groceries and cook dinner"
task-cli delete 1
task-cli mark-in-progress 1
task-cli mark-done 1
task-cli list
task-cli list done
```

## Data Storage

Tasks are saved in a local `data.json` file in the same directory as the application.

## Dependencies

- Newtonsoft.Json for JSON serialization/deserialization

[https://roadmap.sh/projects/task-tracker]
