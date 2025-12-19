using CourseLib.Models;
using CourseLib.Services;
using CourseLib.Builders;

class Program
{
    private static readonly CourseManager manager = CourseManager.Instance;

    static void ShowMainMenu()
    {
        Console.WriteLine("Меню:");
        Console.WriteLine("1. Показать все курсы");
        Console.WriteLine("2. Добавить курс");
        Console.WriteLine("3. Удалить курс");
        Console.WriteLine("4. Добавить преподавателя");
        Console.WriteLine("5. Добавить студента");
        Console.WriteLine("6. Записать студента на курс");
        Console.WriteLine("7. Удалить студента с курса");
        Console.WriteLine("8. Удалить преподавателя с курса");
        Console.WriteLine("9. Получить курсы преподавателя");
        Console.WriteLine("10. Назначить преподавателя на курс");
        Console.WriteLine("0. Выход");
        Console.WriteLine();
    }

    static void ShowCourses()
    {
        var courses = manager.GetAllCourses().ToList();
        if (!courses.Any())
        {
            Console.WriteLine("Курсов пока нет :(");
            return;
        }

        Console.WriteLine("Курсы:");
        foreach (var c in courses)
        {
            Console.WriteLine($"- {c.Title}");
            Console.WriteLine($"  Тип: {(c is OnlineCourse ? "Онлайн" : "Очно")}");
            Console.WriteLine($"  Преподаватель: {c.Teacher?.Name ?? "нет"}");
            if (c is OfflineCourse off)
                Console.WriteLine($"  Аудитория: {off.Classroom}");
            if (c is OnlineCourse on)
                Console.WriteLine($"  Количество видео: {on.VideoCount}");
            Console.WriteLine($"  Студенты: {(c.Students.Any() ? string.Join(", ", c.Students.Select(s => s.Name)) : "нет")}");
            Console.WriteLine();
        }
    }

    static void AddCourse()
    {
        Console.Write("Введите название курса: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Тип курса (1 - онлайн, 2 - офлайн): ");
        string type = Console.ReadLine() ?? "1";

        Console.Write("Введите имя преподавателя: ");
        string teacherName = Console.ReadLine() ?? "";
        var teacher = new Teacher(teacherName);

        Course course;
        if (type == "2")
        {
            Console.Write("Введите аудиторию: ");
            string room = Console.ReadLine() ?? "Не указана";
            course = new CourseBuilder()
                .WithTitle(title)
                .WithTeacher(teacher)
                .AsOffline(room)
                .Build();
        }
        else
        {
            Console.Write("Введите количество видео: ");
            int videoCount = int.TryParse(Console.ReadLine(), out int count) ? count : 8;
            course = new CourseBuilder()
                .WithTitle(title)
                .WithTeacher(teacher)
                .AsOnline(videoCount)
                .Build();
        }
        try
        {
            manager.AddCourse(course);
            Console.WriteLine($"Курс '{title}' успешно добавлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void RemoveCourse()
    {
        Console.Write("Введите название курса для удаления: ");
        string title = Console.ReadLine() ?? "";

        try
        {
            manager.RemoveCourse(title);
            Console.WriteLine($"Курс '{title}' удалён.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void AddTeacher()
    {
        Console.Write("Введите имя преподавателя: ");
        string name = Console.ReadLine() ?? "";
        try
        {
            manager.AddTeacher(new Teacher(name));
            Console.WriteLine($"Преподаватель '{name}' добавлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        
    }

    static void AddStudent()
    {
        Console.Write("Введите имя студента: ");
        string name = Console.ReadLine() ?? "";
        try
        {
            manager.AddStudent(new Student(name));
            Console.WriteLine($"Студент '{name}' добавлен.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void EnrollStudent()
    {
        Console.Write("Введите название курса: ");
        string courseTitle = Console.ReadLine() ?? "";

        Console.Write("Введите имя студента: ");
        string studentName = Console.ReadLine() ?? "";

        var student = new Student(studentName);
        try
        {
            manager.EnrollStudent(courseTitle, student);
            Console.WriteLine($"Студент '{studentName}' записан на курс '{courseTitle}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void EnrollTeacher()
    {
        Console.Write("Введите название курса: ");
        string courseTitle = Console.ReadLine() ?? "";

        Console.Write("Введите имя преподавателя: ");
        string teacherName = Console.ReadLine() ?? "";

        var teacher = new Teacher(teacherName);
        try
        {
            manager.EnrollTeacher(courseTitle, teacher);
            Console.WriteLine($"Преподаватель '{teacherName}' приписан к курсу '{courseTitle}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void RemoveStudent()
    {
        Console.Write("Введите название курса: ");
        string courseTitle = Console.ReadLine() ?? "";

        Console.Write("Введите имя студента для удаления: ");
        string studentName = Console.ReadLine() ?? "";

        try
        {
            manager.RemoveStudent(courseTitle, studentName);
            Console.WriteLine($"Студент '{studentName}' удалён с курса '{courseTitle}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        
    }

    static void RemoveTeacherFromCourse()
    {
        Console.Write("Введите название курса: ");
        string courseTitle = Console.ReadLine() ?? "";

        try
        {
            manager.RemoveTeacher(courseTitle);
            Console.WriteLine($"Преподаватель удалён с курса '{courseTitle}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void CoursesByTeacher()
    {
        Console.Write("Введите имя преподавателя: ");
        string teacherName = Console.ReadLine() ?? "";

        var courses = manager.GetCoursesByTeacher(teacherName);

        if (courses.Any())
        {
            foreach (var c in courses)
            {
                Console.WriteLine($"- {c.Title}");
                Console.WriteLine($"  Тип: {(c is OnlineCourse ? "Онлайн" : "Очно")}");
                Console.WriteLine($"  Преподаватель: {c.Teacher?.Name ?? "нет"}");
                if (c is OfflineCourse off)
                    Console.WriteLine($"  Аудитория: {off.Classroom}");
                if (c is OnlineCourse on)
                    Console.WriteLine($"  Количество видео: {on.VideoCount}");
                Console.WriteLine($"  Студенты: {(c.Students.Any() ? string.Join(", ", c.Students.Select(s => s.Name)) : "нет")}");
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("У данного преподавателя нет курсов :(");
        }
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Система управления курсами ===\n");

        bool running = true;
        while (running)
        {
            ShowMainMenu();
            Console.Write("Выберите действие: ");
            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ShowCourses();
                    break;
                case "2":
                    AddCourse();
                    break;
                case "3":
                    RemoveCourse();
                    break;
                case "4":
                    AddTeacher();
                    break;
                case "5":
                    AddStudent();
                    break;
                case "6":
                    EnrollStudent();
                    break;
                case "7":
                    RemoveStudent();
                    break;
                case "8":
                    RemoveTeacherFromCourse();
                    break;
                case "9":
                    CoursesByTeacher();
                    break;
                case "10":
                    EnrollTeacher();
                    break;
                case "0":
                    running = false;
                    Console.WriteLine("Выход из программы...");
                    break;
                default:
                    Console.WriteLine("Неверный выбор, попробуйте снова.");
                    break;
            }

            Console.WriteLine();
        }
    }
}
