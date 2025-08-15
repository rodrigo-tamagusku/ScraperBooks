using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

public class GeradorHttp
{
    private HttpClient httpClient;
    private HtmlDocument documento;

    public GeradorHttp()
    {
        this.httpClient = new();
        this.documento = new();
        string html = this.httpClient.GetStringAsync(Environment.GetEnvironmentVariable("URL_BOOKS")).GetAwaiter().GetResult();
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
            HtmlNode? categoria = NodeQueTemCategoria.ChildNodes.Nodes().FirstOrDefault(n => n.InnerText.Contains(Environment.GetEnvironmentVariable("ALGUMA_CATEGORIA") ?? "Travel"));
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
        string resposta = this.httpClient.GetStringAsync(Environment.GetEnvironmentVariable("URL_BOOKS") + categoria.Value.Value).GetAwaiter().GetResult();
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
                Preco = Conversor.ConvertePreco(preco),
                Rating = Conversor.ConverteRating(ratingEstrelas),
                Categoria = categoria.Value.Key,
                URL = Environment.GetEnvironmentVariable("URL_BOOKS") + categoria.Value.Value
            };
            produtos.Add(produtoLivro);
        }
        return produtos;
    }

    public HttpStatusCode EnviaPost(string envio)
    {
        HttpContent content = new StringContent(envio, Encoding.UTF8, "application/json");
        string url = Constantes.HTTP_BIN_POST;
        Console.WriteLine($"[{DateTime.Now}]POST solicitado para {url}");
        Console.WriteLine($"Conteúdo do POST: {envio}");
        HttpResponseMessage response = this.httpClient.PostAsync(url, content).Result;
        Console.WriteLine($"Resultado: {response.ToString()}");
        return response.StatusCode;
    }
}