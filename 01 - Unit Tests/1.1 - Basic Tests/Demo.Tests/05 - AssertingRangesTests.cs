namespace Demo.Tests;

public class AssertingRangesTests
{
    [Theory]
    [InlineData(700)]
    [InlineData(1500)]
    [InlineData(2000)]
    [InlineData(7500)]
    [InlineData(8000)]
    [InlineData(15000)]
    public void Employer_Salary_SalariesRangesShouldRespectProfessionalLevel(double salary)
    {
        // Arrange and Act
        var employer = new Employer("Leonardo", salary);

        // Assert
        if (employer.ProfessionalLevel is ProfessionalLevel.EntryLevel)
            Assert.InRange(employer.Salary, 500, 1999);

        if (employer.ProfessionalLevel is ProfessionalLevel.MidLevel)
            Assert.InRange(employer.Salary, 2000, 7999);

        if (employer.ProfessionalLevel is ProfessionalLevel.Senior)
            Assert.InRange(employer.Salary, 8000, double.MaxValue);

        Assert.NotInRange(employer.Salary, 0, 499);
    }
}