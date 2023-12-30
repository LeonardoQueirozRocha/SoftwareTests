namespace Demo.Tests;

public class AssertingCollectionsTests
{
    [Fact]
    public void Employer_Skills_ShouldNotPossessEmptySkills()
    {
        // Arrange and Act
        var employer = EmployerFactory.Create("Leonardo", 10000);

        // Assert
        Assert.All(employer.Skills, skill => Assert.False(string.IsNullOrWhiteSpace(skill)));
    }

    [Fact]
    public void Employer_Skills_EntryLevelShouldPossessBasicSkill()
    {
        // Arrange and Act
        var employer = EmployerFactory.Create("Leonardo", 10000);

        // Assert
        Assert.Contains("OOP", employer.Skills);
    }

    [Fact]
    public void Employer_Skills_EntryLevelShouldNotPossessAdvancedSkill()
    {
        // Arrange and Act
        var employer = EmployerFactory.Create("Leonardo", 1000);

        // Assert
        Assert.DoesNotContain("Microservices", employer.Skills);
    }

    [Fact]
    public void Employer_Skills_SeniorShouldPossessAllSkills()
    {
        // Arrange and Act
        var employer = EmployerFactory.Create("Leonardo", 10000);
        var skills = new[]
        {
            "Programming Logic",
            "OOP",
            "Tests",
            "Microservices"
        };

        // Assert
        Assert.Equal(skills, employer.Skills);
    }
}