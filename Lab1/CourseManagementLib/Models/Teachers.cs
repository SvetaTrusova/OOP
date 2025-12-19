namespace CourseLib.Models;

public class Teacher : Person
{
    public List<Course> Courses = new();

    public Teacher(string name) : base(name) {}
}
