internal class Program
{
    private static void Main(string[] args)
    {
        GeradorHttp geradorHttp = new();
        geradorHttp.CarregaCategorias();
        List<ProdutoLivro> categoriaTravel = geradorHttp.CarregaUmaCategoria(0);
        List<ProdutoLivro> categoriaMystery = geradorHttp.CarregaUmaCategoria(11);
        List<ProdutoLivro> categoriaHistoricalFiction = geradorHttp.CarregaUmaCategoria(11);
    }
}
