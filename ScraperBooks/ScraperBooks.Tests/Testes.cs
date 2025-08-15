using System.Net;

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
            string? teste = Environment.GetEnvironmentVariable("URL_BOOKS");
            string? ALGUMA_CATEGORIA = Environment.GetEnvironmentVariable("ALGUMA_CATEGORIA");
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
                    string link = Environment.GetEnvironmentVariable("URL_BOOKS") + linkCategoria;
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
            List<ProdutoLivro>? categoriaTravel = null;
            string json = "";
            Assert.Multiple(() =>
            {
                categoriaTravel = this.geradorHttp.CarregaUmaCategoria(0);
                Assert.That(categoriaTravel.Count, Is.AtLeast(10), "Quantidade baixa");
                Assert.That(categoriaTravel[0].Categoria, Is.EqualTo("Travel"), "Categoria Travel incorreta");
            });
            Assert.Multiple(() =>
            {
                List<ProdutoLivro> categoriaMystery = this.geradorHttp.CarregaUmaCategoria(1);
                Assert.That(categoriaMystery.Count, Is.AtLeast(10), "Quantidade baixa");
                Assert.That(categoriaMystery[0].Categoria, Is.EqualTo("Mystery"), "Categoria Mystery incorreta");
            });
            Assert.Multiple(() =>
            {
                List<ProdutoLivro> categoriaHistoricalFiction = this.geradorHttp.CarregaUmaCategoria(2);
                Assert.That(categoriaHistoricalFiction.Count, Is.AtLeast(10), "Quantidade baixa");
                Assert.That(categoriaHistoricalFiction[0].Categoria, Is.EqualTo("Historical Fiction"), "Categoria Historical Fiction incorreta");
            });
            Assert.Multiple(() =>
            {
                string xml = Conversor.SeralizaXML(categoriaTravel);
                json = Conversor.SeralizaJson(categoriaTravel);
                Assert.That(xml.Length, Is.AtLeast(10), "Provavelmente não converteu");
                Assert.That(xml.StartsWith("<?xml version="), "Não possui o início de xml");
                Assert.That(json.Length, Is.AtLeast(10), "Provavelmente não converteu");
                Assert.That(json.StartsWith("[{\"Titulo\":\""), "Não possui o início de json");
            });
            HttpStatusCode resultado = geradorHttp.EnviaPost(json);
            Assert.That(resultado, Is.EqualTo(HttpStatusCode.OK), "Ocorreu algum erro no Post");
        }
    }
}
