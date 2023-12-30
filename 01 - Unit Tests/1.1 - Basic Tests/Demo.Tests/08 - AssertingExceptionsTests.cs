namespace Demo.Tests;

public class AssertingExceptionsTests
{
    [Fact]
    public void Calculator_Divide_ShouldReturnDivideByZeroError()
    {
        // Arrange
        var calculator = new Calculator();

        // Act and Assert
        Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
    }

    [Fact]
    public void Employer_Salary_ShouldReturnSalaryLessThanPermittedError()
    {
        // Arrange and Act and Assert
        var exception = Assert.Throws<Exception>(() => EmployerFactory.Create("Leonardo", 250));

        Assert.Equal("Salary less than permitted", exception.Message);
    }
}