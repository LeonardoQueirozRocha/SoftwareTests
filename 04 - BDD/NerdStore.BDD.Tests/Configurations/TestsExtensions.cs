namespace NerdStore.BDD.Tests.Configurations;

public static class TestsExtensions
{
    public static int OnlyNumbers(this string value) =>
        Convert.ToInt16(new string(value.Where(char.IsDigit).ToArray()));
}
