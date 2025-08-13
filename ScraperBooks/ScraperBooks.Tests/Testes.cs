namespace ScraperBooks.Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ScrapBooks()
        {
            GeradorHttp geradorHttp = new();
            geradorHttp.Carrega();
            geradorHttp.EscreveConsole();
        }
    }
}
