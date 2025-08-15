internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine($"Início: {DateTime.Now}");
        Rotinas.FluxoCompleto();
        Console.WriteLine($"Fim: {DateTime.Now}");
        Console.WriteLine("Fim da execução. Pressione qualquer tecla para sair");
        Console.ReadKey();
    }
}
