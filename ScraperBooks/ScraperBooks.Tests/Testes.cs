namespace ScraperBooks.Tests
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            Console.WriteLine(DateTime.Now);
        }

        [Test]
        public void ScrapBooks()
        {
            GeradorHttp geradorHttp = new();
            geradorHttp.EscreveConsole();
        }
    }
}
