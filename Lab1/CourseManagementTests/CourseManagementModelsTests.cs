using CourseLib.Models;

namespace CourseTests;

public class CourseModelsTests
{
    [Fact]
    public void OnlineCourse_GetCourseInfo_Contains_Core_Data()
    {
        // Arrange
        var course = new OnlineCourse("Программирование", new Teacher("Терещенко Владислав"), 5);

        // Act
        var info = course.GetCourseInfo();

        // Assert
        Assert.Contains("Онлайн-курс", info);
        Assert.Contains("Программирование", info);
        Assert.Contains("Терещенко Владислав", info);
        Assert.Contains("Видео: 5", info);
    }

    [Fact]
    public void OfflineCourse_GetCourseInfo_Contains_Core_Data()
    {
        // Arrange
        var course = new OfflineCourse("ООП", new Teacher("Слюсаренко Сергей"), "244");
        
        // Act
        var info = course.GetCourseInfo();

        // Assert
        Assert.Contains("Очный курс", info);
        Assert.Contains("ООП", info);
        Assert.Contains("Слюсаренко Сергей", info);
        Assert.Contains("Аудитория: 244", info);
    }
}