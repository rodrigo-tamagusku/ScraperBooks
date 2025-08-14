using HtmlAgilityPack;

public class GeradorHttp
{
    const string URL_BOOKS = "https://books.toscrape.com/";
    const string ALGUMA_CATEGORIA = "Sequential Art";
    private HttpClient httpClient;
    private HtmlDocument documento;
    private List<string> categorias;

    public GeradorHttp()
    {
        this.httpClient = new();
        this.documento = new();
        string html = this.httpClient.GetStringAsync(URL_BOOKS).GetAwaiter().GetResult();
        this.documento.LoadHtml(html);
        this.categorias = new List<string>();
    }
    public void EscreveConsole()
    {
        HtmlNode? NodeQueTemCategoria = this.documento.DocumentNode.ChildNodes.Nodes()?.LastOrDefault(d => d.HasChildNodes);
        if (NodeQueTemCategoria is null)
        {
            throw new Exception("Ocorreu um erro ao começar o scraping");
        }
        while (true)
        {
            HtmlNode? categoria = NodeQueTemCategoria.ChildNodes.Nodes().FirstOrDefault(n => n.InnerText.Contains(ALGUMA_CATEGORIA));
            if (categoria is null)
            {
                break;
            }
            NodeQueTemCategoria = categoria;
        }
        HtmlNode categorias = NodeQueTemCategoria.ParentNode.ParentNode;
        foreach (HtmlNode? categoria in categorias.ChildNodes.ToList())
        {
            string categoriaTexto = categoria.InnerText.Trim();
            if (!string.IsNullOrEmpty(categoriaTexto))
            {
                this.categorias.Add(categoriaTexto);
                Console.WriteLine(categoriaTexto);
            }
        }
    }
}