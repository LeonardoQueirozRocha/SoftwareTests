namespace Demo.Tests;

public class AssertStringsTests
{
    [Fact]
    public void StringTools_JoinNames_ReturnFullName()
    {
        // Arrange
        var tools = new StringTools();

        // Act
        var fullName = tools.Join("Leonardo", "Rocha");

        Assert.Equal("Leonardo Rocha", fullName);
    }

    [Fact]
    public void StringTools_JoinNames_ShouldIgnoreCase()
    {
        // Arrange
        var tools = new StringTools();

        // Act
        var fullName = tools.Join("Leonardo", "Rocha");

        // Assert
        Assert.Equal("LEONARDO ROCHA", fullName, true);
    }

    [Fact]
    public void StringTools_JoinNames_ShouldContainsWith()
    {
        // Arrange
        var tools = new StringTools();

        // Act
        var fullName = tools.Join("Leonardo", "Rocha");

        // Assert
        Assert.Contains("ardo", fullName);
    }

    [Fact]
    public void StringTools_JoinNames_ShouldStartWith()
    {
        // Arrange
        var tools = new StringTools();

        // Act
        var fullName = tools.Join("Leonardo", "Rocha");

        // Assert
        Assert.StartsWith("Leo", fullName);
    }

    [Fact]
    public void StringTools_JoinNames_ShouldEndsWith()
    {
        // Arrange
        var tools = new StringTools();

        // Act
        var fullName = tools.Join("Leonardo", "Rocha");

        // Assert
        Assert.EndsWith("cha", fullName);
    }

    [Fact]
    public void StringTools_JoinNames_ValidateRegularExpression()
    {
        // Arrange
        var tools = new StringTools();

        // Act
        var fullName = tools.Join("Leonardo", "Rocha");

        // Assert
        Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", fullName);
    }
}