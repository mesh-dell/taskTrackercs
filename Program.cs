using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;                        // For JsonConvert

namespace TaskTracker
{
    public class Task
    {
        public int Id { get; set; }
        public string? Description { get; set; }

        public enum Status
        {
            todo,
            inProgress,
            done
        }

        public Status TaskStatus { get; set; } = Status.todo;


        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    class Program
    {

        static void Main(string[] args)
        {
            int length = args.Length;

            if (length == 0)
            {
                return;
            }

            if (args[0] == "task-cli")
            {
                if (length == 1)
                {
                    PrintHelp();
                }

                if (length == 2 && args[1] == "help")
                {
                    PrintHelp();
                }

                if (length >= 2)
                {
                    switch (args[1])
                    {
                        case "add":
                            if (length != 3)
                            {
                                Console.WriteLine("Error: Add takes one argument");
                                return;
                            }
                            string task = args[2];
                            AddTask(task);
                            break;
                        case "delete":
                            if (length != 3)
                            {
                                Console.WriteLine("Error: Delete takes one argument");
                                return;
                            }
                            if (int.TryParse(args[2], out int deleteId))
                            {
                                if (DeleteTask(deleteId))
                                {
                                    Console.WriteLine($"Task {deleteId} deleted successfully");
                                }
                                else
                                {
                                    Console.WriteLine("Error: Task Id not found");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error: Enter an Integer Id");
                            }
                            break;
                        case "update":
                            if (length != 4)
                            {
                                Console.WriteLine("Error: Update takes two arguments");
                                return;
                            }
                            string newTaskDescription = args[3];
                            if (int.TryParse(args[2], out int updateId))
                            {
                                if (UpdateTask(updateId, newTaskDescription))
                                {
                                    Console.WriteLine("Task updated successfully");
                                }
                                else
                                {
                                    Console.WriteLine("Error: Task Id not found");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error: Enter an Integer Id");
                            }
                            break;
                        case "mark-in-progress":
                            if (length != 3)
                            {
                                Console.WriteLine("mark-in-progress takes one argument");
                                return;
                            }
                            if (int.TryParse(args[2], out int markInProgressId))
                            {
                                if (MarkProgress(markInProgressId, inProgress: true))
                                {
                                    Console.WriteLine($"Task {markInProgressId} marked as in progress");
                                }
                                else
                                {
                                    Console.WriteLine("Error: Task Id not found");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error: Enter an Integer Id");
                            }
                            break;

                        case "mark-done":
                            if (length != 3)
                            {
                                Console.WriteLine("mark-done takes one argument");
                                return;
                            }
                            if (int.TryParse(args[2], out int markDoneId))
                            {
                                if (MarkProgress(markDoneId, inProgress: false))
                                {
                                    Console.WriteLine($"Task {markDoneId} marked as done");
                                }
                                else
                                {
                                    Console.WriteLine("Error: Task Id not found");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error: Enter an Integer Id");
                            }
                            break;

                        case "list":

                            if (length == 2)
                            {
                                ListTasks(0);
                            }
                            else if (length == 3)
                            {
                                switch (args[2])
                                {
                                    case "done":
                                        ListTasks(1);
                                        break;
                                    case "todo":
                                        ListTasks(2);
                                        break;
                                    case "in-progress":
                                        ListTasks(3);
                                        break;
                                }
                            }
                            else
                            {
                                Console.WriteLine("List takes a maximum of one argument");
                            }
                            break;

                    }
                }
            }
            else
            {
                PrintHelp();
                return;
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine(@"
        task-cli help
Task CLI - A simple command-line task manager

Usage:
  task-cli <command> [arguments]

Commands:
  add <task_description>        Add a new task
  update <task_id> <new_description>  Update an existing task
  delete <task_id>                Delete a task
  mark-in-progress <task_id>       Mark a task as in progress
  mark-done <task_id>              Mark a task as done
  list                             List all tasks
  list done                        List completed tasks
  list todo                        List pending tasks
  list in-progress                 List tasks in progress

Examples:
  task-cli add Buy groceries
  task-cli update 1 Buy groceries and cook dinner
  task-cli delete 1
  task-cli mark-in-progress 1
  task-cli mark-done 1
  task-cli list
  task-cli list done
        ");
        }

        static List<Task> LoadTasks()
        {
            List<Task> tasks;
            if (File.Exists("data.json"))
            {
                try
                {
                    using StreamReader r = new("data.json");
                    string json = r.ReadToEnd();
                    tasks = JsonConvert.DeserializeObject<List<Task>>(json) ?? [];
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON {ex.Message}");
                    tasks = [];
                }
            }
            else
            {
                tasks = [];
            }
            return tasks;
        }
        static void SaveTasks(List<Task> tasks)
        {
            string updatedJson = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText("data.json", updatedJson);
        }
        static void AddTask(string task)
        {
            List<Task> tasks = LoadTasks();
            Task newTask = new()
            {
                Id = tasks.Count + 1,
                Description = task,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                TaskStatus = Task.Status.todo
            };
            tasks.Add(newTask);
            SaveTasks(tasks);
        }
        static bool DeleteTask(int id)
        {
            List<Task> tasks = LoadTasks();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == id)
                {
                    tasks.Remove(tasks[i]);
                    SaveTasks(tasks);
                    return true;
                }
            }
            return false;
        }

        static bool UpdateTask(int id, string newTaskDescription)
        {
            List<Task> tasks = LoadTasks();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == id)
                {
                    tasks[i].Description = newTaskDescription;
                    tasks[i].UpdatedAt = DateTime.Now;
                    SaveTasks(tasks);
                    return true;
                }
            }
            return false;
        }

        static bool MarkProgress(int id, bool inProgress = true)
        {
            List<Task> tasks = LoadTasks();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == id)
                {
                    if (inProgress)
                    {
                        tasks[i].TaskStatus = Task.Status.inProgress;
                        tasks[i].UpdatedAt = DateTime.Now;
                    }
                    else
                    {
                        tasks[i].TaskStatus = Task.Status.done;
                        tasks[i].UpdatedAt = DateTime.Now;
                    }
                    SaveTasks(tasks);
                    return true;
                }
            }
            return false;
        }

        static void ListTasks(int selector)
        {
            // 0 = all
            // 1 = done 
            // 2 = todo
            // 3 = inProgress 
            List<Task> tasks = LoadTasks();
            switch (selector)
            {
                case 0:
                    foreach (Task task in tasks)
                    {
                        Console.WriteLine($"ID: {task.Id}");
                        Console.WriteLine($"Description: {task.Description}");
                        Console.WriteLine($"Status: {task.TaskStatus}");
                        Console.WriteLine($"Created At: {task.CreatedAt.ToShortTimeString()}");
                        Console.WriteLine($"Updated At: {task.UpdatedAt.ToShortTimeString()}");
                        Console.WriteLine();
                    }
                    break;
                case 1:
                    foreach (Task task in tasks)
                    {
                        if (task.TaskStatus == Task.Status.done)
                        {
                            Console.WriteLine($"ID: {task.Id}");
                            Console.WriteLine($"Description: {task.Description}");
                            Console.WriteLine($"Status: {task.TaskStatus}");
                            Console.WriteLine($"Created At: {task.CreatedAt.ToShortTimeString()}");
                            Console.WriteLine($"Updated At: {task.UpdatedAt.ToShortTimeString()}");
                            Console.WriteLine();
                        }
                    }
                    break;

                case 2:
                    foreach (Task task in tasks)
                    {
                        if (task.TaskStatus == Task.Status.todo)
                        {
                            Console.WriteLine($"ID: {task.Id}");
                            Console.WriteLine($"Description: {task.Description}");
                            Console.WriteLine($"Status: {task.TaskStatus}");
                            Console.WriteLine($"Created At: {task.CreatedAt.ToShortTimeString()}");
                            Console.WriteLine($"Updated At: {task.UpdatedAt.ToShortTimeString()}");
                            Console.WriteLine();
                        }
                    }
                    break;


                case 3:
                    foreach (Task task in tasks)
                    {
                        if (task.TaskStatus == Task.Status.inProgress)
                        {
                            Console.WriteLine($"ID: {task.Id}");
                            Console.WriteLine($"Description: {task.Description}");
                            Console.WriteLine($"Status: {task.TaskStatus}");
                            Console.WriteLine($"Created At: {task.CreatedAt.ToShortTimeString()}");
                            Console.WriteLine($"Updated At: {task.UpdatedAt.ToShortTimeString()}");
                            Console.WriteLine();
                        }
                    }
                    break;

                default:
                    Console.WriteLine("invalid selector");
                    break;
            }
        }
    }
}