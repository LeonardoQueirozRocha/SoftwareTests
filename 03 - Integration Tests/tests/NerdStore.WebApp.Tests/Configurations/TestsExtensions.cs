namespace NerdStore.WebApp.Tests.Configurations;

public static class TestsExtensions
{
    public static decimal OnlyNumbers(this string value)
    {
        return Convert.ToDecimal(new string(value.Where(char.IsDigit).ToArray()));
    }
}