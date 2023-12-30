namespace Demo.Tests;

public class AssertingObjectTypesTests
{
    [Fact]
    public void EmployerFactory_Create_ShouldReturnEmployerType()
    {
        // Arrange and Act
        var employer = EmployerFactory.Create("Leonardo", 10000);

        // Assert
        Assert.IsType<Employer>(employer);
    }

    [Fact]
    public void EmployerFactory_Create_ShouldReturnDerivativePersonType()
    {
        // Arrange and Act
        var employer = EmployerFactory.Create("Leonardo", 10000);

        // Assert
        Assert.IsAssignableFrom<Person>(employer);
    }
}