namespace Features.Tests.Skip;

public class TestNotWorkingForASpecificReason
{
    [Fact(DisplayName = "New Customer 2.0", Skip = "New version 2.0 is broken")]
    [Trait("Category", "Skipping tests")]
    public void Test_NotWorking_NewVersionNotCompatible()
    {
        Assert.True(false);
    }
}