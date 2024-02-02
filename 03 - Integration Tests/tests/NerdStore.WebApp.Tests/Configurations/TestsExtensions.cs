using System.Net.Http.Headers;

namespace NerdStore.WebApp.Tests.Configurations;

public static class TestsExtensions
{
    public static decimal OnlyNumbers(this string value)
    {
        return Convert.ToDecimal(new string(value.Where(char.IsDigit).ToArray()));
    }

    public static void AssignToken(this HttpClient client, string token)
    {
        client.AssignJsonMediaType();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static void AssignJsonMediaType(this HttpClient client)
    {
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}