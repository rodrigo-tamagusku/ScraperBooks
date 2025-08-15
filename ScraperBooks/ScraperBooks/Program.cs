internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"Início: {DateTime.Now}");
        GeradorHttp geradorHttp = new();
        geradorHttp.CarregaCategorias();
        List<ProdutoLivro> categoriaTravel = geradorHttp.CarregaUmaCategoria(0);
        List<ProdutoLivro> categoriaMystery = geradorHttp.CarregaUmaCategoria(1);
        List<ProdutoLivro> categoriaHistoricalFiction = geradorHttp.CarregaUmaCategoria(2);
        List<ProdutoLivro> todasCategorias = geradorHttp.CarregaTodosProdutos();
        geradorHttp.EnviaPost(Conversor.SeralizaJson(todasCategorias));
        Console.WriteLine($"Fim: {DateTime.Now}");
        Console.WriteLine("Fim da execução. Pressione qualquer tecla para sair");
        Console.ReadKey();
    }
}
