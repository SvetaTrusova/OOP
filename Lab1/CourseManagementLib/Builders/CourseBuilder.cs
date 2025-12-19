using CourseLib.Models;

namespace CourseLib.Builders;

public class CourseBuilder
{
    private string? title;
    private Teacher? teacher;
    private int? videoCount;
    private string? classroom;
    private bool isOnline;

    public CourseBuilder WithTitle(string title)
    {
        this.title = title;
        return this;
    }

    public CourseBuilder WithTeacher(Teacher teacher)
    {
        this.teacher = teacher;
        return this;
    }

    public CourseBuilder AsOnline(int videoCount)
    {
        this.videoCount = videoCount;
        isOnline = true;
        return this;
    }

    public CourseBuilder AsOffline(string classroom)
    {
        isOnline = false;
        this.classroom = classroom;
        return this;
    }

    public Course Build()
    {
        if (title == null || teacher == null)
        {
            throw new InvalidOperationException("Не введены название и преподаватель");
        }

        if (isOnline)
        {
            return new OnlineCourse(title, teacher, videoCount ?? 0);
        }
        else
        {
            return new OfflineCourse(title, teacher, classroom ?? "Не указана");
        }
    }
}