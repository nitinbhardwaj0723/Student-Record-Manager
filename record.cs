class Student
{
    public int RollNo;
    public string Name;
    public DateTime DOB;
    public double Marks;

    public int GetAge()
    {
        int age = DateTime.Now.Year - DOB.Year;

        if (DOB.Date > DateTime.Now.AddYears(-age).Date)
            age--;

        return age;
    }

    public override string ToString()
    {
        return $"{RollNo},{Name},{DOB:yyyy-MM-dd},{Marks}";
    }
}

class Program
{
    static List<Student> students = new List<Student>();
    static string filePath = "Students.txt";

    static void Main()
    {
        LoadRecords();

        while (true)
        {
            Console.WriteLine("\n===== STUDENT RECORD MANAGER =====");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View Students");
            Console.WriteLine("3. Search Student");
            Console.WriteLine("4. Delete Student");
            Console.WriteLine("5. Save Records");
            Console.WriteLine("6. Exit");

            Console.Write("Enter your choice: ");
            int choice;

            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid choice!");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddStudent();
                    break;

                case 2:
                    ViewStudents();
                    break;

                case 3:
                    SearchStudent();
                    break;

                case 4:
                    DeleteStudent();
                    break;

                case 5:
                    SaveRecords();
                    break;

                case 6:
                    SaveRecords();
                    Console.WriteLine("Thank you!");
                    return;

                default:
                    Console.WriteLine("Invalid option!");
                    break;
            }
        }
    }

    static void AddStudent()
    {
        Student s = new Student();

        Console.Write("Enter Roll Number: ");
        if (!int.TryParse(Console.ReadLine(), out s.RollNo))
        {
            Console.WriteLine("Invalid roll number!");
            return;
        }

        if (students.Any(x => x.RollNo == s.RollNo))
        {
            Console.WriteLine("Roll number already exists!");
            return;
        }

        Console.Write("Enter Name: ");
        s.Name = (Console.ReadLine() ?? "").Trim();

        if (string.IsNullOrWhiteSpace(s.Name))
        {
            Console.WriteLine("Name cannot be empty!");
            return;
        }

        Console.Write("Enter DOB (yyyy-MM-dd): ");
        if (!DateTime.TryParse(Console.ReadLine(), out s.DOB) ||
            s.DOB.Date > DateTime.Today)
        {
            Console.WriteLine("Invalid date of birth!");
            return;
        }

        Console.Write("Enter Marks: ");
        if (!double.TryParse(Console.ReadLine(), out s.Marks) ||
            s.Marks < 0 || s.Marks > 100)
        {
            Console.WriteLine("Marks must be between 0 and 100!");
            return;
        }

        students.Add(s);
        Console.WriteLine("Student added successfully!");
    }

    static void ViewStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No records found!");
            return;
        }

        Console.WriteLine("\n--- Student Records ---");

        foreach (Student s in students)
        {
            Console.WriteLine(
                $"Roll No: {s.RollNo} | Name: {s.Name} | " +
                $"DOB: {s.DOB:dd-MM-yyyy} | Age: {s.GetAge()} | Marks: {s.Marks}"
            );
        }
    }

    static void SearchStudent()
    {
        Console.Write("Enter student name to search: ");
        string search = (Console.ReadLine() ?? "").Trim().ToUpper();

        var result = students.Where(s =>
            s.Name.ToUpper().Contains(search)).ToList();

        if (result.Count == 0)
        {
            Console.WriteLine("Student not found!");
            return;
        }

        foreach (Student s in result)
        {
            Console.WriteLine(
                $"Roll No: {s.RollNo}, Name: {s.Name}, " +
                $"Age: {s.GetAge()}, Marks: {s.Marks}"
            );
        }
    }

    static void DeleteStudent()
    {
        Console.Write("Enter Roll Number to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int roll))
        {
            Console.WriteLine("Invalid roll number!");
            return;
        }

        Student s = students.FirstOrDefault(x => x.RollNo == roll);

        if (s != null)
        {
            students.Remove(s);
            Console.WriteLine("Student deleted successfully!");
        }
        else
        {
            Console.WriteLine("Student not found!");
        }
    }

    static void SaveRecords()
    {
        File.WriteAllLines(filePath,
            students.Select(s => s.ToString()));

        Console.WriteLine("Records saved successfully!");
    }

    static void LoadRecords()
    {
        if (!File.Exists(filePath))
            return;

        foreach (string line in File.ReadAllLines(filePath))
        {
            string[] data = line.Split(',');

            if (data.Length == 4 &&
                int.TryParse(data[0], out int roll) &&
                DateTime.TryParse(data[2], out DateTime dob) &&
                double.TryParse(data[3], out double marks))
            {
                students.Add(new Student
                {
                    RollNo = roll,
                    Name = data[1],
                    DOB = dob,
                    Marks = marks
                });
            }
        }
    }
}
