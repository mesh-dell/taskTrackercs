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
                            if (length < 3)
                            {
                                Console.WriteLine("Error: Add takes one argument");
                                return;
                            }
                            string task = args[2];
                            AddTask(task);
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
        static void SaveTask(List<Task> tasks)
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
            SaveTask(tasks);
        }
    }
}