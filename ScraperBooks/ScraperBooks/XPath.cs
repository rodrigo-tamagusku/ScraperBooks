public static class XPath
{
    public static string PRODUTO_LIVRO { get => "//article[@class='product_pod']"; }
    public static string PRODUTO_LIVRO_PRECO { get => "div[@class='product_price']"; }
    public static string PRODUTO_LIVRO_TITULO { get => "h3/a[@title]"; }
    public static string PRODUTO_LIVRO_RATING { get => "p[contains(@class, 'star-rating')]"; }
}
