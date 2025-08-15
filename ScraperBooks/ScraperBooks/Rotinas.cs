using System.Xml;

public class Rotinas
{
    public static void FluxoCompleto()
    {
        #region declaração
        GeradorHttp geradorHttp;
        List<ProdutoLivro> todasCategorias;
        string produtosJson;
        XmlDocument xmlDocument;
        #endregion
        CarregamentoTudo(out geradorHttp, out todasCategorias);
        SerializacaoTudo(todasCategorias, out produtosJson, out xmlDocument);
        SalvamentoTudo(produtosJson, xmlDocument);
        ExportacaoTudo(geradorHttp, produtosJson);
    }

    private static void ExportacaoTudo(GeradorHttp geradorHttp, string produtosJson)
    {
        geradorHttp.EnviaPost(produtosJson);
    }

    private static void SalvamentoTudo(string produtosJson, XmlDocument xmlDocument)
    {
        Conversor.SalvaTexto(produtosJson, "books.json");
        Conversor.SalvaXmlDocument(xmlDocument);
    }

    private static void SerializacaoTudo(List<ProdutoLivro> todasCategorias, out string produtosJson, out XmlDocument xmlDocument)
    {
        produtosJson = Conversor.SeralizaJson(todasCategorias);
        string produtosXML = Conversor.SeralizaXML(todasCategorias);
        xmlDocument = Conversor.GeraXmlDocument(produtosXML);
    }

    private static void CarregamentoTudo(out GeradorHttp geradorHttp, out List<ProdutoLivro> todasCategorias)
    {
        geradorHttp = new();
        geradorHttp.CarregaCategorias();
        List<ProdutoLivro> categoriaTravel = geradorHttp.CarregaUmaCategoria(0);
        List<ProdutoLivro> categoriaMystery = geradorHttp.CarregaUmaCategoria(1);
        List<ProdutoLivro> categoriaHistoricalFiction = geradorHttp.CarregaUmaCategoria(2);
        todasCategorias = geradorHttp.CarregaTodosProdutos();
    }
}
