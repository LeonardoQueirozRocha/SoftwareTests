namespace Demo.Tests;

public class AssertNullBoolTests
{
    [Fact]
    public void Employer_Name_ShouldNotBeNullOrEmpty()
    {
        // Arrange and Act
        var employer = new Employer(string.Empty, 1000);

        // Assert
        Assert.False(string.IsNullOrEmpty(employer.Name));
    }

    [Fact]
    public void Employer_Nickname_ShouldNotHaveNickname()
    {
        // Arrange and Act
        var employer = new Employer("Leonardo", 1000);

        // Assert
        Assert.Null(employer.Nickname);

        // Assert Bool
        Assert.True(string.IsNullOrEmpty(employer.Nickname));
        Assert.False(employer.Nickname?.Length > 0);
    }
}