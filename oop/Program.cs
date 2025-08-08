using System;
using System.Collections.Generic;

// Interfaces
interface IPersonService
{
    int CalculateAge();
    decimal CalculateSalary();
    List<string> GetAddresses();
}

interface IStudentService : IPersonService
{
    double CalculateGPA();
}

interface IInstructorService : IPersonService
{
    int GetYearsOfExperience();
}

interface ICourseService
{
    void AddStudent(Student student);
    List<Student> GetEnrolledStudents();
}

interface IDepartmentService
{
    void AddCourse(Course course);
    List<Course> GetOfferedCourses();
}

// Person class
class Person : IPersonService
{
    private List<string> addresses = new List<string>();
    public string Name { get; set; }
    public DateTime DateOfBirth { get; set; }

    private decimal salary;
    public decimal Salary
    {
        get => salary;
        set
        {
            if (value < 0)
                throw new ArgumentException("Salary cannot be negative.");
            salary = value;
        }
    }

    public int CalculateAge()
    {
        var today = DateTime.Today;
        int age = today.Year - DateOfBirth.Year;
        if (DateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    public virtual decimal CalculateSalary()
    {
        return Salary;
    }

    public void AddAddress(string address)
    {
        addresses.Add(address);
    }

    public List<string> GetAddresses()
    {
        return addresses;
    }
}

// Instructor
class Instructor : Person, IInstructorService
{
    public Department Department { get; set; }
    public bool IsHeadOfDepartment { get; set; }
    public DateTime JoinDate { get; set; }

    public int GetYearsOfExperience()
    {
        int years = DateTime.Now.Year - JoinDate.Year;
        if (JoinDate > DateTime.Now.AddYears(-years)) years--;
        return years;
    }

    public override decimal CalculateSalary()
    {
        decimal bonus = GetYearsOfExperience() * 1000;
        return base.CalculateSalary() + bonus;
    }
}

// Student
class Student : Person, IStudentService
{
    private Dictionary<Course, char> courseGrades = new Dictionary<Course, char>();

    public void EnrollInCourse(Course course, char grade)
    {
        courseGrades[course] = grade;
        course.AddStudent(this);
    }

    public double CalculateGPA()
    {
        if (courseGrades.Count == 0) return 0.0;

        double totalPoints = 0;
        foreach (var grade in courseGrades.Values)
        {
            totalPoints += grade switch
            {
                'A' => 4.0,
                'B' => 3.0,
                'C' => 2.0,
                'D' => 1.0,
                'F' => 0.0,
                _ => 0.0
            };
        }

        return totalPoints / courseGrades.Count;
    }
}

// Course
class Course : ICourseService
{
    public string CourseName { get; set; }
    private List<Student> enrolledStudents = new List<Student>();

    public void AddStudent(Student student)
    {
        enrolledStudents.Add(student);
    }

    public List<Student> GetEnrolledStudents()
    {
        return enrolledStudents;
    }
}

// Department
class Department : IDepartmentService
{
    public Instructor Head { get; set; }
    public DateTime BudgetStartDate { get; set; }
    public DateTime BudgetEndDate { get; set; }
    private List<Course> offeredCourses = new List<Course>();

    public void AddCourse(Course course)
    {
        offeredCourses.Add(course);
    }

    public List<Course> GetOfferedCourses()
    {
        return offeredCourses;
    }
}

// Color class
class Color
{
    private int red, green, blue, alpha;

    public Color(int red, int green, int blue, int alpha)
    {
        this.red = red;
        this.green = green;
        this.blue = blue;
        this.alpha = alpha;
    }

    public Color(int red, int green, int blue) : this(red, green, blue, 255) { }

    public int Red { get => red; set => red = value; }
    public int Green { get => green; set => green = value; }
    public int Blue { get => blue; set => blue = value; }
    public int Alpha { get => alpha; set => alpha = value; }

    public int GetGrayscale()
    {
        return (red + green + blue) / 3;
    }
}

// Ball class
class Ball
{
    private int size;
    private Color color;
    private int throwCount;

    public Ball(int size, Color color)
    {
        this.size = size;
        this.color = color;
        this.throwCount = 0;
    }

    public void Pop()
    {
        size = 0;
    }

    public void Throw()
    {
        if (size > 0)
            throwCount++;
    }

    public int GetThrowCount()
    {
        return throwCount;
    }
}

// Program
class OOPDemo
{
    static void Main()
{
    List<Ball> balls = new List<Ball>();

    Console.WriteLine("🎯 Welcome to the Ball Simulator!");
    Console.Write("How many balls do you want to create? ");
    int ballCount = int.Parse(Console.ReadLine());

    for (int i = 0; i < ballCount; i++)
    {
        Console.WriteLine($"\nCreating Ball #{i + 1}");
        Console.Write("Enter size: ");
        int size = int.Parse(Console.ReadLine());

        Console.Write("Enter Red (0-255): ");
        int r = int.Parse(Console.ReadLine());

        Console.Write("Enter Green (0-255): ");
        int g = int.Parse(Console.ReadLine());

        Console.Write("Enter Blue (0-255): ");
        int b = int.Parse(Console.ReadLine());

        Ball ball = new Ball(size, new Color(r, g, b));
        balls.Add(ball);
    }

    // Ball interaction
    bool running = true;
    while (running)
    {
        Console.WriteLine("\n⚙️ What would you like to do?");
        Console.WriteLine("1. Throw a ball");
        Console.WriteLine("2. Pop a ball");
        Console.WriteLine("3. View ball throw counts");
        Console.WriteLine("4. Exit");
        Console.Write("Enter choice: ");
        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                Console.Write("Which ball to throw (1 to N)? ");
                int throwIndex = int.Parse(Console.ReadLine()) - 1;
                if (throwIndex >= 0 && throwIndex < balls.Count)
                {
                    balls[throwIndex].Throw();
                    Console.WriteLine("Ball thrown!");
                }
                else
                {
                    Console.WriteLine("Invalid ball number.");
                }
                break;

            case "2":
                Console.Write("Which ball to pop (1 to N)? ");
                int popIndex = int.Parse(Console.ReadLine()) - 1;
                if (popIndex >= 0 && popIndex < balls.Count)
                {
                    balls[popIndex].Pop();
                    Console.WriteLine("Ball popped.");
                }
                else
                {
                    Console.WriteLine("Invalid ball number.");
                }
                break;

            case "3":
                for (int i = 0; i < balls.Count; i++)
                {
                    Console.WriteLine($"Ball #{i + 1}: Thrown {balls[i].GetThrowCount()} times");
                }
                break;

            case "4":
                running = false;
                break;

            default:
                Console.WriteLine("Invalid option.");
                break;
        }
    }

    
}

}
