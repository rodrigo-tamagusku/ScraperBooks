using NUnit;

namespace ScraperBooks.Tests
{
    [TestFixture]
    public class Tests
    {
        private GeradorHttp geradorHttp;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Console.WriteLine(DateTime.Now);
            this.geradorHttp = new();
            this.geradorHttp.CarregaCategorias();
        }

        [Test]
        public void CarregaCategorias()
        {
            Assert.That(this.geradorHttp.Categorias.Count >= 50, "Não achou a quantidade esperada de categorias");
            Assert.Multiple(() =>
            {
                foreach (string linkCategoria in this.geradorHttp.Categorias.Values)
                {
                    if (string.IsNullOrEmpty(linkCategoria))
                    {
                        Assert.Fail("Link vazio");

                    }
                    string link = Constantes.URL_BOOKS + linkCategoria;
                    if (!geradorHttp.LinkValido(link))
                    {
                        Assert.Fail($"Não foi possível abrir o link {link}");
                    }
                }
            });
        }

        [Test]
        public void CarregaUmaCategoria()
        {
            Assert.Multiple(() =>
            {
                this.geradorHttp.CarregaUmaCategoria(1);
                this.geradorHttp.CarregaUmaCategoria(2);
                this.geradorHttp.CarregaUmaCategoria(3);
            });
        }
    }
}
