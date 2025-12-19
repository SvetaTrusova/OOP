namespace CourseLib.Models;

public class OnlineCourse : Course
{
    public int VideoCount { get; set; }

    public OnlineCourse(string title, Teacher teacher, int videoCount) : base(title, teacher)
    {
        VideoCount = videoCount;
    }

    public override string GetCourseInfo()
    {
        return $"Онлайн-курс: {Title}, Преподаватель: {Teacher?.Name}, Видео: {VideoCount}, Id: {Id}";
    }
}
