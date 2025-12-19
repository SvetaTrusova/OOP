namespace CourseLib.Models;

public abstract class Person
{
    public Guid Id { get; private set; }
    public string Name { get; set; }

    protected Person(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}