using HtmlAgilityPack;

public class GeradorHttp
{
    private HttpClient httpClient;
    private HtmlDocument documento;

    public GeradorHttp()
    {
        this.httpClient = new();
        this.documento = new();
        string html = this.httpClient.GetStringAsync(Constantes.URL_BOOKS).GetAwaiter().GetResult();
        this.documento.LoadHtml(html);
        this.Categorias = new();
    }
    public Dictionary<string, string> Categorias { get; set; }

    public bool LinkValido(string link)
    {
        if (link == null) return false;
        string resposta = this.httpClient.GetStringAsync(link).GetAwaiter().GetResult();
        return !string.IsNullOrEmpty(resposta);
    }

    public void CarregaCategorias()
    {
        HtmlNode? NodeQueTemCategoria = this.documento.DocumentNode.ChildNodes.Nodes()?.LastOrDefault(d => d.HasChildNodes);
        if (NodeQueTemCategoria is null)
        {
            throw new Exception("Ocorreu um erro ao começar o scraping");
        }
        while (true)
        {
            HtmlNode? categoria = NodeQueTemCategoria.ChildNodes.Nodes().FirstOrDefault(n => n.InnerText.Contains(Constantes.ALGUMA_CATEGORIA));
            if (categoria is null) //Tratar loop infinito
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
                string stringURL = categoria.InnerHtml.Split(Constantes.PRE_URL)[1];
                this.Categorias.Add(categoriaTexto, stringURL);
                Console.WriteLine(categoriaTexto);
            }
        }
    }

    public List<ProdutoLivro> CarregaUmaCategoria(int indiceCategoria)
    {
        List<ProdutoLivro> produtos = new();
        KeyValuePair<string, string>? categoria = this.Categorias.ElementAtOrDefault(indiceCategoria);
        if (categoria is null || !categoria.HasValue)
        {
            throw new Exception($"Não há categoria no índice {indiceCategoria}");
        }
        string resposta = this.httpClient.GetStringAsync(Constantes.URL_BOOKS+categoria.Value.Value).GetAwaiter().GetResult();
        HtmlDocument documento = new();
        documento.LoadHtml(resposta);
        HtmlNodeCollection livros = documento.DocumentNode.SelectNodes(Constantes.XPATH_LIVRO);
        foreach (HtmlNode livro in livros)
        {
            ProdutoLivro produtoLivro = new()
            {
                Titulo = "",
                Preco = 10,
                Rating = 10,
                Categoria = categoria.Value.Key,
                URL = Constantes.URL_BOOKS + categoria.Value.Value
            };
        }
        return produtos;
    }
}