namespace CourseLib.Models;

public class OfflineCourse : Course
{
    public string Classroom { get; set; }

    public OfflineCourse(string title, Teacher teacher, string classroom) : base(title, teacher)
    {
        Classroom = classroom;
    }

    public override string GetCourseInfo()
    {
        return $"Очный курс: {Title}, Преподаватель: {Teacher?.Name}, Аудитория: {Classroom}, Id: {Id}";
    }
}
