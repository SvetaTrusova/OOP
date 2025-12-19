using CourseLib.Models;
using CourseLib.Services;

namespace CourseTests;

public class CourseManagerTests
{

    [Fact]
    public void AddCourse_Adds_And_Populates_TeacherCourses()
    {
        // Arrange
        var manager = CourseManager.Instance;
        var teacher = new Teacher("Ольга Михайловна");
        var course = new OnlineCourse("АИСД", teacher, 8);

        // Act
        manager.AddCourse(course);

        // Assert
        Assert.Contains(course, manager.GetAllCourses());
        Assert.Contains(course, teacher.Courses);
    }

    [Fact]
    public void AddCourse_Throws_On_Duplicate_Id()
    {
        // Arrange
        var manager = CourseManager.Instance;
        var teacher = new Teacher("Ольга Михайловна");
        var course = new OnlineCourse("АИСД", teacher, 8);

        // Act
        manager.AddCourse(course);

        // Assert
        Assert.Throws<InvalidOperationException>(() => manager.AddCourse(course));
    }

    [Fact]
    public void GetCoursesByTeacher_Returns_Only_Teacher_Courses()
    {
        // Arrange
        var manager = CourseManager.Instance;
        var t1 = new Teacher("Терещенко Владислав");
        var t2 = new Teacher("Слюсаренко Сергей");

        var c1 = new OnlineCourse("Программирование", t1, 10);
        var c2 = new OfflineCourse("ООП", t2, "465");
        var c3 = new OfflineCourse("ОС Linux", t1, "301");

        manager.AddCourse(c1);
        manager.AddCourse(c2);
        manager.AddCourse(c3);

        // Act
        var adaCourses = manager.GetCoursesByTeacher("Терещенко Владислав").ToList();

        // Assert
        Assert.Equal(2, adaCourses.Count);
        Assert.All(adaCourses, c => Assert.Equal("Терещенко Владислав", c.Teacher!.Name));
    }
}