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
        string resposta = this.httpClient.GetStringAsync(Constantes.URL_BOOKS + categoria.Value.Value).GetAwaiter().GetResult();
        HtmlDocument documento = new();
        documento.LoadHtml(resposta);
        HtmlNodeCollection livros = documento.DocumentNode.SelectNodes(XPath.PRODUTO_LIVRO);
        foreach (HtmlNode livro in livros)
        {
            string preco = livro.SelectNodes(XPath.PRODUTO_LIVRO_PRECO)[0].InnerText;
            string titulo = livro.SelectNodes(XPath.PRODUTO_LIVRO_TITULO)[0].InnerText.Trim();
            string ratingEstrelas = livro.SelectNodes(XPath.PRODUTO_LIVRO_RATING)[0].GetAttributeValue("class", "");
            ProdutoLivro produtoLivro = new()
            {
                Titulo = titulo,
                Preco = ConvertePreco(preco),
                Rating = ConverteRating(ratingEstrelas),
                Categoria = categoria.Value.Key,
                URL = Constantes.URL_BOOKS + categoria.Value.Value
            };
            produtos.Add(produtoLivro);
        }
        return produtos;
    }

    private decimal ConvertePreco(string preco)
    {
        string precoLimpo = preco.Split("£")[1].Split("\n")[0];
        if (decimal.TryParse(precoLimpo, out decimal resultDecimal))
        {
            return resultDecimal;
        }
        throw new Exception($"Falha ao converter o preço {preco}");
    }

    private int ConverteRating(string ratingEstrelas)
    {
        string[] ratingArray = ratingEstrelas.Split("star-rating ");
        string rating = ratingArray.Count() > 0 ? ratingArray[1] : ratingArray[0];
        switch (rating.ToUpper())
        {
            case "ONE":
                return 1;
            case "TWO":
                return 2;
            case "THREE":
                return 3;
            case "FOUR":
                return 4;
            case "FIVE":
                return 5;
            default:
                Console.WriteLine($"Falha ao converter {rating} como rating");
                return 0;
        }
    }
}