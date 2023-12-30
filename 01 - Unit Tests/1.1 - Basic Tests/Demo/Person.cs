namespace Demo;

public class Person
{
    public string Name { get; protected set; }
    public string Nickname { get; set; }
}

public class Employer : Person
{
    public double Salary { get; private set; }
    public ProfessionalLevel ProfessionalLevel { get; private set; }
    public IList<string> Skills { get; private set; }

    public Employer(string name, double salary)
    {
        Name = string.IsNullOrEmpty(name) ? "Random" : name;
        DefineSalary(salary);
        DefineSkills();
    }

    public void DefineSalary(double salary)
    {
        if (salary < 500) throw new Exception("Salary less than permitted");

        Salary = salary;

        ProfessionalLevel = salary switch
        {
            < 2000 => ProfessionalLevel = ProfessionalLevel.EntryLevel,
            >= 2000 and < 8000 => ProfessionalLevel = ProfessionalLevel.MidLevel,
            >= 8000 => ProfessionalLevel = ProfessionalLevel.Senior,
            _ => throw new Exception("Professional Level not defined")
        };
    }

    private void DefineSkills()
    {
        var basicSkills = new List<string>()
        {
            "Programming Logic",
            "OOP"
        };

        Skills = basicSkills;

        switch (ProfessionalLevel)
        {
            case ProfessionalLevel.MidLevel:
                Skills.Add("Tests");
                break;
            case ProfessionalLevel.Senior:
                Skills.Add("Tests");
                Skills.Add("Microservices");
                break;
        }
    }
}

public enum ProfessionalLevel
{
    EntryLevel,
    MidLevel,
    Senior
}

public class EmployerFactory
{
    public static Employer Create(string name, double salary) =>
        new(name, salary);
}