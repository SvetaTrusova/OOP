using CourseLib.Builders;
using CourseLib.Models;

namespace CourseTests;

public class CourseBuilderTests
{
    [Fact]
    public void Build_OnlineCourse_With_VideoCount()
    {
        // Arrange
        var teacher = new Teacher("Ольга Михайловна");
        var course = new CourseBuilder()
            .WithTitle("АИСД")
            .WithTeacher(teacher)
            .AsOnline(videoCount: 12)
            .Build();

        // Act
        var online = Assert.IsType<OnlineCourse>(course);

        // Assert
        Assert.Equal("АИСД", online.Title);
        Assert.Equal(12, online.VideoCount);
        Assert.Equal("Ольга Михайловна", online.Teacher!.Name);
    }

    [Fact]
    public void Build_OfflineCourse_With_Classroom()
    {
        // Arrange
        var teacher = new Teacher("Говорова Марина Михайловна");
        var course = new CourseBuilder()
            .WithTitle("ПиРБД")
            .WithTeacher(teacher)
            .AsOffline(classroom: "465")
            .Build();

        // Act
        var offline = Assert.IsType<OfflineCourse>(course);

        // Assert
        Assert.Equal("ПиРБД", offline.Title);
        Assert.Equal("465", offline.Classroom);
        Assert.Equal("Говорова Марина Михайловна", offline.Teacher!.Name);
    }

    [Fact]
    public void Build_Throws_If_Title_Or_Teacher_Missing()
    {
        // Arrange
        string name = "ПиРБД";
        var teacher = new Teacher("Ольга Михайловна");

        // Act
        var builder = new CourseBuilder();
        var b2 = new CourseBuilder().WithTitle(name);
        var b3 = new CourseBuilder().WithTeacher(teacher);

        // Assert
        Assert.Throws<InvalidOperationException>(() => builder.Build());
        Assert.Throws<InvalidOperationException>(() => b2.Build());
        Assert.Throws<InvalidOperationException>(() => b3.Build());
    }
}