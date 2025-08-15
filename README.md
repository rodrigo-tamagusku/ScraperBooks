# ScraperBooks

Aplicação em C# que faz scraping do site https://books.toscrape.com/,
aplica filtros configuráveis e envia os dados filtrados para um endpoint simulado de API REST.

Features:

* Scraping
* Filtros configuráveis
* Normalização e exportação
* Integração com API REST
* Implementação em C#.
* Tratamento de erros e logging mínimo.
* Uso de biblioteca de parsing HTML
* Saída em JSON e XML.
* Documentação clara no README.md (passo a passo de configuração e execução)

Execução:

Inicialize as seguintes variáveis de ambiente:

	ALGUMA_CATEGORIA="Sequential Art"
	URL_BOOKS="https://books.toscrape.com/"

Compile e executa a solução "ScraperBooks" como console.
Exemplo de execução:

<img width="1052" height="592" alt="image" src="https://github.com/user-attachments/assets/9f893497-e173-4679-a58e-898d4313d935" />


Exemplo de books.xml gerado:

		<?xml version="1.0" encoding="utf-16"?>
		<ArrayOfProdutoLivro xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
		  <ProdutoLivro>
		    <Titulo>It&amp;#39;s Only the Himalayas</Titulo>
		    <Preco>45.17</Preco>
		    <Rating>2</Rating>
		    <Categoria>Travel</Categoria>
		    <URL>https://books.toscrape.com/catalogue/category/books/travel_2/index.html</URL>
		  </ProdutoLivro>
		  <ProdutoLivro>
		    <Titulo>Full Moon over Noah’s ...</Titulo>
		    <Preco>49.43</Preco>
		    <Rating>4</Rating>
		    <Categoria>Travel</Categoria>
		    <URL>https://books.toscrape.com/catalogue/category/books/travel_2/index.html</URL>
		  </ProdutoLivro>	

Exemplo de books.json gerado:

	  {
	    "Titulo": "Equal Is Unfair: America&#39;s ...",
	    "Preco": 56.86,
	    "Rating": 1,
	    "Categoria": "Politics",
	    "URL": "https://books.toscrape.com/catalogue/category/books/politics_48/index.html"
	  },
	  {
	    "Titulo": "Amid the Chaos",
	    "Preco": 36.58,
	    "Rating": 1,
	    "Categoria": "Cultural",
	    "URL": "https://books.toscrape.com/catalogue/category/books/cultural_49/index.html"
	  }
