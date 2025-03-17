using System.Diagnostics;
using System.Text.Json;
class Program
{

    static Dictionary<int, Task> Tasks = LoadTasks();
    static void Main(string[] args)
    {
        if (args == null)
        {
            Console.WriteLine("Комманда не обнаружена\nHelp выведет список команд");
            return;
        }
        string command = args[0];


        switch (command)
        {
            case "Create":
                if (args.Length < 2) // Должно быть хотя бы 2 аргумента: команда и статус
                {
                    Console.WriteLine("Ошибка: статус задачи не указан\nHelp для деталей команды");
                    break;
                }
                Task Newtask = new Task();
                string commandStatus = args[1];
                if (!Enum.TryParse<TaskState>(args[1], out TaskState state))
                {
                    Console.WriteLine("Ошибка: статус задачи не найден.");
                    break;
                }
                Newtask._TaskState = state;

                Newtask.Title = args.Length > 2 && !string.IsNullOrWhiteSpace(args[2]) ? args[2] : "Без названия";
                Newtask.Description = args.Length > 3 && !string.IsNullOrWhiteSpace(args[3]) ? args[3] : "Без описания";

                int newId = Tasks.Any() ? Tasks.Keys.Max() + 1 : 0;
                Tasks.Add(newId, Newtask);
                UpdateFile();
                Console.WriteLine($"Была успешно созданна запись под названием {Newtask.Title}, ключ {newId}");
                break;

            case "Help":
                {
                    Console.WriteLine("Доступные команды:");
                    Console.WriteLine("Create [Status] (Title) (Description) - создать задачу");
                    Console.WriteLine("Statuses : [OnGoing] [Completed] [Pause] [Canceled]");
                    Console.WriteLine("View - показать все задачи");
                    Console.WriteLine("View [ID] - показать задачу по ID");
                    Console.WriteLine("Delete [ID] - удалить задачу");
                    Console.WriteLine("Update [ID] [TaskStatus] (Title) (Description) - удалить задачу");
                    break;
                }
            case "View":
                {
                    if (args.Length < 2)
                    {
                        foreach (var (key, val) in Tasks)
                        {
                            Console.WriteLine($"Ключ:{key}, Название:{val.Title}");
                        }
                    }
                    else
                    {
                        if (int.TryParse(args[1], out int taskId) && Tasks.TryGetValue(taskId, out Task? mytask))
                        {
                            Console.WriteLine($"Ключ: {taskId}");
                            Console.WriteLine($"Название: {mytask.Title}");
                            Console.WriteLine($"Описание: {mytask.Description}");
                            Console.WriteLine($"Статус: {mytask._TaskState}");
                        }
                        else if (Enum.GetNames<TaskState>().Contains(args[1]))
                        {
                            TaskState filterState = Enum.Parse<TaskState>(args[1]);
                            var filteredTasks = Tasks.Where(t => t.Value._TaskState == filterState);

                            if (!filteredTasks.Any())
                                Console.WriteLine($"Нет задач со статусом {filteredTasks}");
                            else
                            {
                                foreach (var (key, val) in filteredTasks)
                                {
                                    Console.WriteLine($"Ключ: {key}, Название: {val.Title},Описание: {val.Description}, Статус: {val._TaskState}");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Задачи нету или Неверный код");
                        }
                    }
                    break;
                }
            case "Delete":
                {
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Неправильная команнда");
                        break;
                    }

                    if (int.TryParse(args[1], out int id))
                    {
                        Tasks.Remove(id, out Task? myTask);
                        Console.WriteLine($@"Задача с ключом {id} и названием ""{myTask?.Title}"" Удалена успешно");
                    }
                    else
                    {
                        Console.WriteLine("Такой задачи не существует");
                    }
                    UpdateFile();
                    break;
                }
            case "Update":
                string title = string.Empty;
                string description = string.Empty;

                if (args.Length < 3)
                {
                    Console.WriteLine("Неверный формат команды, Help для справки");
                }
                title = args.Length >= 3 ? args[2] : "Нет названия";
                description = args.Length >= 4 ? args[3] : "Нет названия";
                if (int.TryParse(args[1], out int updatedTaskId) && Enum.TryParse<TaskState>(args[2], out TaskState updatestate))
                {
                    if (Tasks.TryGetValue(updatedTaskId, out Task? taskToUpdate) && taskToUpdate != null)
                    {
                        taskToUpdate._TaskState = updatestate;
                        taskToUpdate.Title = title;
                        taskToUpdate.Description = description;
                        UpdateFile();
                        Console.WriteLine($@"Задача {taskToUpdate.Title} обновлена");
                    }
                    else
                    {
                        Console.WriteLine("Задача не найдена");
                    }
                }
                else
                {
                    Console.WriteLine("Ошибка, неверный формат");
                }

                break;
            default:
                Console.WriteLine("Неизвестная команда. Введите Help для справки.");
                break;
        }
    }

    public class Task
    {
        public TaskState _TaskState { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

    }
    static void UpdateFile()
    {
        File.WriteAllText("tasks.json", JsonSerializer.Serialize(Tasks, new JsonSerializerOptions { WriteIndented = true }));
    }
    static Dictionary<int, Task> LoadTasks()
    {
        if (File.Exists("tasks.json"))
        {
            string json = File.ReadAllText("tasks.json");
            return JsonSerializer.Deserialize<Dictionary<int, Task>>(json) ?? new Dictionary<int, Task>();
        }
        return new Dictionary<int, Task>();
    }
}
public enum TaskState
{
    OnGoing,
    Completed,
    Pause,
    Canceled
}
