using HtmlAgilityPack;

internal class Program
{
    private static void Main(string[] args)
    {
        const string URL_BOOKS = "https://books.toscrape.com/";
        HttpClient httpClient = new();
        string html = httpClient.GetStringAsync(URL_BOOKS).GetAwaiter().GetResult();
        Console.WriteLine(html);
        HtmlDocument documento = new();
        documento.LoadHtml(html);
        Console.WriteLine(documento.ToString());
    }
}
