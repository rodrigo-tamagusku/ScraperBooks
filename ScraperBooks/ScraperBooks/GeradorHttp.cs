using HtmlAgilityPack;

public class GeradorHttp
{
    const string URL_BOOKS = "https://books.toscrape.com/";
    private HttpClient httpClient;
    private HtmlDocument documento;

    public void Carrega()
    {
        this.httpClient = new();
        this.documento = new();
        string html = this.httpClient.GetStringAsync(URL_BOOKS).GetAwaiter().GetResult();
        this.documento.LoadHtml(html);
    }
    public void EscreveConsole()
    {
        Console.WriteLine(this.documento.Text);
    }
}
