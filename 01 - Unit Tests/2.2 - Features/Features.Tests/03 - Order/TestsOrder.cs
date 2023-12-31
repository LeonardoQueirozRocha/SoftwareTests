namespace Features.Tests.Order;

[TestCaseOrderer("Features.Tests.Order.PriorityOrderer", "Features.Tests")]
public class TestsOrder
{
    public static bool Test1Called;
    public static bool Test2Called;
    public static bool Test3Called;
    public static bool Test4Called;

    [Fact(DisplayName = "Test 01")]
    [Trait("Category", "Tests Order"), TestPriority(2)]
    public void Test1()
    {
        Test1Called = true;

        Assert.True(Test3Called);
        Assert.False(Test4Called);
        Assert.False(Test2Called);
    }

    [Fact(DisplayName = "Test 02"), TestPriority(4)]
    [Trait("Category", "Tests Order")]
    public void Test02()
    {
        Test2Called = true;

        Assert.True(Test3Called);
        Assert.True(Test4Called);
        Assert.True(Test1Called);
    }

    [Fact(DisplayName = "Test 03")]
    [Trait("Category", "Tests Order"), TestPriority(1)]
    public void Teste03()
    {
        Test3Called = true;

        Assert.False(Test1Called);
        Assert.False(Test2Called);
        Assert.False(Test4Called);
    }

    [Fact(DisplayName = "Test 04"), TestPriority(3)]
    [Trait("Category", "Tests Order")]
    public void Test4()
    {
        Test4Called = true;

        Assert.True(Test3Called);
        Assert.True(Test1Called);
        Assert.False(Test2Called);
    }
}
