using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

public static class Conversor
{
    public static string SeralizaJson(object objeto)
    {
        return JsonConvert.SerializeObject(objeto, Formatting.Indented);
    }

    public static XmlDocument GeraXmlDocument(string xmlTexto)
    {
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlTexto);
        return xml;
    }

    public static void SalvaXmlDocument(XmlDocument xml)
    {
        xml.Save("books.xml");
    }

    public static string SeralizaXML<T>(T value)
    {
        if (value == null)
        {
            return string.Empty;
        }
        try
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ", // Two spaces for indentation
                NewLineHandling = NewLineHandling.Replace,
                NewLineChars = "\r\n"
            };

            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new StringWriter();
            using (var writer = XmlWriter.Create(stringWriter, settings))
            {
                xmlserializer.Serialize(writer, value);
                return stringWriter.ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred", ex);
        }
    }
    public static decimal ConvertePreco(string preco)
    {
        string precoLimpo = preco.Split("£")[1].Split("\n")[0];
        if (decimal.TryParse(precoLimpo, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out decimal resultDecimal))
        {
            return resultDecimal;
        }
        throw new Exception($"Falha ao converter o preço {preco}");
    }

    public static int ConverteRating(string ratingEstrelas)
    {
        string[] ratingArray = ratingEstrelas.Split("star-rating ");
        string rating = ratingArray.Count() > 0 ? ratingArray[1] : ratingArray[0];
        switch (rating.ToUpper())
        {
            case "ONE":
                return 1;
            case "TWO":
                return 2;
            case "THREE":
                return 3;
            case "FOUR":
                return 4;
            case "FIVE":
                return 5;
            default:
                Console.WriteLine($"Falha ao converter {rating} como rating");
                return 0;
        }
    }
}