namespace CourseLib.Models;

public abstract class Course
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; set; }
    public Teacher? Teacher { get; set; }
    public List<Student> Students { get; } = new();

    protected Course(string title, Teacher? teacher = null)
    {
        Title = title;
        Teacher = teacher;
    }

    public abstract string GetCourseInfo();
}