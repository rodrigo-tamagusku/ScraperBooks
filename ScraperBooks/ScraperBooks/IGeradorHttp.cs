using System.Net;

public interface IGeradorHttp
{
    Dictionary<string, string> Categorias { get; set; }

    void CarregaCategorias();
    List<ProdutoLivro> CarregaTodosProdutos();
    List<ProdutoLivro> CarregaUmaCategoria(int indiceCategoria);
    HttpStatusCode EnviaPost(string envio);
    bool LinkValido(string link);
}