using CourseLib.Models;

namespace CourseLib.Services;

public class CourseManager
{
    private static readonly Lazy<CourseManager> instance = new(() => new CourseManager());
    public static CourseManager Instance => instance.Value;

    private readonly List<Course> courses = new();
    private readonly List<Teacher> teachers = new();
    private readonly List<Student> students = new();

    private CourseManager() { }

    public void AddTeacher(Teacher newTeacher)
    {
        if (newTeacher == null)
            throw new ArgumentNullException(nameof(newTeacher));

        var existing = teachers.FirstOrDefault(t => t.Name == newTeacher.Name);
        if (existing != null)
        {
            throw new InvalidOperationException($"Преподаватель '{newTeacher.Name}' уже существует.");
        }
        else
        {
            teachers.Add(newTeacher);
        }
    }
    public void AddStudent(Student newStudent)
    {
        if (newStudent == null)
            throw new ArgumentNullException(nameof(newStudent));

        var existing = students.FirstOrDefault(s => s.Name == newStudent.Name);
        if (existing != null)
        {
            throw new InvalidOperationException($"Студент '{newStudent.Name}' уже существует.");
        }
        else
        {
            students.Add(newStudent);
        }
    }

    public void AddCourse(Course course)
    {
        if (course == null)
            throw new ArgumentNullException(nameof(course));
        if (courses.Any(c => c.Id == course.Id))
        {
            throw new InvalidOperationException($"Курс '{course.Title}' уже существует.");
        }
        else
        {
            courses.Add(course);
            course.Teacher?.Courses.Add(course);
        }
    }

    public void RemoveCourse(string courseTitle)
    {
        var course = courses.FirstOrDefault(c => c.Title == courseTitle);
        if (course == null)
            throw new InvalidOperationException($"Курс '{courseTitle}' не найден.");

        if (course.Teacher != null)
        {
            course.Teacher.Courses.Remove(course);
        }

        courses.Remove(course);
    }

    public void EnrollStudent(string courseTitle, Student student)
    {
        if (string.IsNullOrWhiteSpace(courseTitle))
        {
            throw new ArgumentException("Название курса не может быть пустым.");
        }
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        var course = courses.FirstOrDefault(c => c.Title == courseTitle);
        if (course == null)
        {
            throw new InvalidOperationException($"Курс '{courseTitle}' не найден.");
        }
        if (!students.Any(s => s.Name == student.Name))
        {
            AddStudent(student);
        }

        if (!course.Students.Any(s => s.Name == student.Name))
        {
            course.Students.Add(student);
        }
        else
        {
            throw new InvalidOperationException("На данном курсе уже имеется такой студент.");
        }
    }

    public void EnrollTeacher(string courseTitle, Teacher teacher)
    {
        if (string.IsNullOrWhiteSpace(courseTitle))
        {
            throw new ArgumentException("Название курса не может быть пустым.");
        }
        if (teacher == null)
        {
            throw new ArgumentNullException(nameof(teacher));
        }

        var course = courses.FirstOrDefault(c => c.Title == courseTitle);
        if (course == null)
        {
            throw new InvalidOperationException($"Курс '{courseTitle}' не найден.");
        }
        if (!teachers.Any(t => t.Name == teacher.Name))
        {
            AddTeacher(teacher);
        }

        course.Teacher = teacher;
        course.Teacher?.Courses.Add(course);

    }

    public void RemoveStudent(string courseTitle, string studentName)
    {
        var course = courses.FirstOrDefault(c => c.Title == courseTitle);
        if (course == null)
            throw new InvalidOperationException($"Курс '{courseTitle}' не найден.");

        var student = course.Students.FirstOrDefault(s => s.Name == studentName);
        if (student == null)
            throw new InvalidOperationException($"Студент с именем {studentName} не найден на курсе.");

        course.Students.Remove(student);
    }
    
    public void RemoveTeacher(string courseTitle)
    {
        var course = courses.FirstOrDefault(c => c.Title == courseTitle);
        if (course == null)
            throw new InvalidOperationException($"Курс '{courseTitle}' не найден.");

        if (course.Teacher == null)
            throw new InvalidOperationException("На курсе нет преподавателя.");

        var teacher = course.Teacher;
        course.Teacher = null;

        teacher.Courses.Remove(course);
    }

    public IEnumerable<Course> GetAllCourses() => courses;
    public IEnumerable<Course> GetCoursesByTeacher(string teacherName) =>
        courses.Where(c => c.Teacher?.Name == teacherName);

    internal void Clear()
    {
        courses.Clear();
        teachers.Clear();
        students.Clear();
    }
}